# NeKanban - система управления проектами

### Стeк проекта

- Angular v13
- .Net6 + preview features
- PostgreSql
- EF Core
  
## Запуск проекта
В корневой папке запустить docker-compose;
В папке ```/NeKanban/nekanban``` выполнить команду ```npm start``` предварительно скачав все пакеты (```npm i```)
Запустить проект ```NeKanban```;
При желании выполнить сид базы данных запустив проект ```NeKanban.Seeder```;

### Генерация миграции 
```dotnet ef migrations add [Migration Name] --verbose --project ..\NeKanban.Data``` из папки ```NeKanban/NeKanban.Api```
### Применение миграций
Запустить проект ```NeKanban.Migrator``` или ```dotnet ef datebase udapte --verbose --project ..\NeKanban.Data``` из папки ```NeKanban/NeKanban.Api```

## Правила работы на проекте
[Проект в youtrack](https://nekwebteam.youtrack.cloud/agiles/141-2/)
[Проект в github](https://github.com/nikitakozlovcoder/NeKanban)
- После начала работы над задачей создается feature ветка из dev ветки, пример наименования ```NK-5```;
- Пример именовая коммита ```NK-5 add task modal window```
- Задача переносится в колонку ```Разработка```
- После заверешения работы над задачей dev ветка вливается в feature ветку и открывается merge request в dev ветку
- Задача переносится в колонку Ревью

# Рабочие окружения
## Локальное окружение
### Frontend
- url: http://localhost:4200
### Backend
- url: https://localhost:7146
### Database
- драйвер: postgres
- connectionstring: "Host=localhost; Port=5432;Database=nekanban; User Id=postgres; Password=password;"
## Staging окружение
### Frontend
- url: https://nikitakozlovcoder.github.io/NeKanban



