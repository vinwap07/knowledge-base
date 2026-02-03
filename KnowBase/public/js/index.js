const joinBtn = document.getElementById("join");
joinBtn.addEventListener("click", () => {
    const sessionCookie = getCookie('SessionID');
    if (sessionCookie) {
        window.location.href = "http://localhost:5000/profile.html";
    } else {
        window.location.href = "http://localhost:5000/register.html";
    }
});

const allCookies = document.cookie;

// Получить конкретное значение куки
function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        const [cookieName, cookieValue] = cookie.trim().split('=');
        if (cookieName === name) {
            return cookieValue;
        }
    }
    return null;
}



