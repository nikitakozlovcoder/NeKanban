# NeKanban - система управления проектами

### Стeк проекта

- Angular v14
- .Net7
- PostgreSql
- EF Core
  
## Запуск проекта
В корневой папки запустить docker-compose ```docker compose up api```;
В папке ```/NeKanban/nekanban``` выполнить команду ```npm start``` предварительно скачав все пакеты (```npm i```)

### Применение миграций
```docker compose up migrator``` или ```dotnet ef datebase udapte --verbose --project ..\NeKanban.Data``` из папки ```NeKanban/NeKanban.Api```

### Генерация миграции 
```dotnet ef migrations add [Migration Name] --verbose --project ..\NeKanban.Data``` из папки ```NeKanban/NeKanban.Api```

### Применение сидера
```docker compose up seeder``` из папки ```NeKanban/NeKanban.Seeder```

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
- url: http://localhost:7146
### Database
- драйвер: postgres
- connectionstring: "Host=localhost; Port=5432;Database=nekanban; User Id=postgres; Password=password;"
## Staging окружение
### Frontend
- url: https://nikitakozlovcoder.github.io/NeKanban



