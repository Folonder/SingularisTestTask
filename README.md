# SingularisTestTask
## Консольное приложение для резервного копирования файлов.
## Реализованные основные требования
 - Платформа .NET 8.
 - Тип приложения – консольное.
 - Поддержка windows и linux-style путей.
 - Приложение должно корректно обрабатывать ошибки, возникающие в
процессе работы (некорректная конфигурация, ошибки ввода/вывода
итп), выводя понятные сообщения пользователю. Если произошла
ошибка во время считывания настроек приложения, программа после
обработки и вывода ошибок должна завершить выполнение. Если
ошибка происходит во время резервного копирования, программа
должна обработать и вывести ошибку, после чего дожидаться
следующей попытки согласно расписания.
 - В случае если предыдущее копирование не было завершено к моменту
начала нового копирования приложение должно выдать
соответствующее предупреждение, а новое копирование не должно
выполняться (т.е. пропущено целиком).
 - Для упрощения можно считать, что файловая система неизменна с
момента начала копирования до его окончания (т.е. в момент
выполнения задания файловая система не меняется, но может меняться
между сеансами копирования).
 - Для упрощения процесса приложение обрабатывает только новые и
измененные файлы. Удаленные файлы игнорируются.
## Реализованные дополнительные требования
 - Возможность изменения расписания без перезапуска приложения.
 - Реализовать логирование в приложении с помощью стандартного механизма.
ILogger: Логирование ошибок; Логирование прогресса; Логирование информации о шаге (первичное / инкрементальное копирование).
 - Реализовать сравнение файлов не на основе даты модификации, а наоснове подсчета хэша файлов с исползованием MD5;
 - Завернуть решение в docker-контейнер.
## Для запуска проекта требуется:
- .NET 8.0
- .NET SDK 8.0
- MongoDB 7.0
## Установка
- Клонировать проект:
```
git clone https://github.com/Folonder/SingularisTestTask.git
```
- Установка зависимостей:
```
dotnet restore "SingularisTestTask\SingularisTestTask.csproj"
```
- Компиляция проекта:
```
dotnet build "SingularisTestTask\SingularisTestTask.csproj"
```
## Запуск
- Запуск проекта:
```
cd .\SingularisTestTask\
```
```
dotnet run "SingularisTestTask.csproj"
```
## Как работать с приложением
### Настройка
 - Откройте appsettings.json;
 - Укажите в виде Unix-like Cron выражения как часто вы хотите делать резервное копирование в поле SchedulerWorkerOptions.JobCronModels.CronExpression;
 - Укажите путь к папке-источнику в поле IncrementCopyServiceOptions.SourceFolder;
 - Укажите путь к папке-назначение в поле IncrementCopyServiceOptions.DestinationFolder;
 - Укажите ссылку для подключения к базе MongoDB в поле IncrementCopyRepositoryOptions.ConnectionString;
#### Для запуска в докере
 - Проделайте все шаги выше;
 - Откройте docker-compose.yml
 - Задайте порты для MongoDB в поле mongodb.ports
 - Задайте в app.volumes пути ваших папки-источника и папки-незначение
 - Задайте временную зону, в которой запускаете проект в поле app.environment.TZ
 - Комманда для запуска
```
docker-compose up --build
```

## Использованные технологии
- MongoDB.Driver - драйвер для MongoDB
- Quartz - библиотека для выполнения задач по расписанию
