# Knowledge Base Management System

A web application for centralized storage, structuring, and intelligent search of information. Built on the modern .NET 8 stack using clean architecture and AI integration for smart content processing.


## Architecture and Tech Stack

|Category|Technologies|
|---|---|
|Backend|`C#`, `.NET 8` |
|Architecture|`Repository`,`DTO`,`MVC`, `Middleware Pipeline`|
|Database|`PostgreSQL`|
|Frontend|`JavaScript`, `HTML5`, `CSS3`|
|AI|`Ollama API`|

## 🚀 Running the Application 

**Prerequisites**

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) 
---

1. Clone the repository: 
```
git clone https://github.com/vinwap07/knowledge-base
``` 
2. Configure the project. Enter the data into the existing config.json file.

``` json
{
  "DatabaseConnectionString": "Server=localhost;Database=KnowledgeBase;Trusted_Connection=true;",
  "ApiKey": "Ключ от Ollama API",
  "StaticFilesPath": "Директория статических файлов, в проекте - public/", 
  "Port": "Порт для хостинга сайта"
} 
```
2. Execute the command from the project's root directory.
```
dotnet run
```
3. After successful launch, the web application will be available at: http://localhost:`your port number`

## 📞 Contacts
* **Email:** adelinazakirova24@gmail.com
* **GitHub:** [vinwap07](https://github.com/vinwap07)
* **Telegram:** [@vinwap](https://t.me/vinwap)
