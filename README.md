Веб-сервис для управления клиентами со стороны учредителей.  
Перед запуском настроить строку подключения в appsettings.json, заменив на свою строку.
Применить миграции.
Пример авторизации:
POST /api/auth/login
Content-Type: application/json

{
  "username": "test123",
  "password": "pass123"
}
