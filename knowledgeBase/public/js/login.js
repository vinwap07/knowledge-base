document.getElementById('login-form').addEventListener('submit', async function(event) {
    event.preventDefault();
    console.log('start registration');
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value;

    clearErrors();

    let isValid = true;

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        document.getElementById('emailError').textContent = 'Введите корректный email';
        isValid = false;
    }

    if (password.length < 2 || password.length > 15) {
        document.getElementById('passwordError').textContent = 'Пароль должен содержать от 2 до 15 символов';
        isValid = false;
    }

    if (isValid) {
        const userData = {
            Email: email,
            Name: name,
            Password: password
        };

        try {
            let response = await fetch(`http://localhost:5000/user/login`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json;charset=utf-8'
                },
                body: JSON.stringify(userData)
            });

            if (!response.ok) {
                return response.text().then(html => {
                    // Заменяем текущую страницу на HTML-ответ
                    document.open();
                    document.write(html);
                    document.close();
                });
            }
            
            let contentType = response.headers.get('content-type');

            let responseText = await response.text();
            
            if (contentType === 'text/plain; charset=utf-8') {
                document.getElementById('wrong-data-text').textContent = responseText;
            } else {
                showNotification('wrong-data-text','Вход выполнен успешно!');
                window.location.href = 'http://localhost:5000/profile.html';
            }

        } catch (error) {
            console.error("Ошибка:", error);
            document.getElementById("result").textContent = `Ошибка: ${error.message}`;
        }

        this.reset();
    }
});

function showError(elementId, message) {
    document.getElementById(elementId).textContent = message;
}

function clearErrors() {
    const errorElements = document.querySelectorAll('.error');
    errorElements.forEach(element => {
        element.textContent = '';
    });
}