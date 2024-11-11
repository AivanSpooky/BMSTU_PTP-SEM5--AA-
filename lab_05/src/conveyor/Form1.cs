using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace conveyor
{
    public partial class Form1 : Form
    {
        private string baseUrl;
        private int pageLimit;

        // BlockingCollection для конвейера
        private BlockingCollection<TaskItem> queueStage1 = new BlockingCollection<TaskItem>();
        private BlockingCollection<TaskItem> queueStage2 = new BlockingCollection<TaskItem>();
        private BlockingCollection<TaskItem> queueStage3 = new BlockingCollection<TaskItem>();

        private int taskIdCounter = 0;
        private readonly object idLock = new object();

        private List<TaskItem> completedTasks = new List<TaskItem>();
        private readonly object completedTasksLock = new object();

        private string databasePath;
        private Logger logger;
        private Logger loggerSequential;
        private System.Threading.Timer collectorTimer;

        // Потоки для каждой стадии
        private Thread stage1Thread;
        private Thread stage2Thread;
        private Thread stage3Thread;
        private Thread collectorThread;

        public Form1()
        {
            InitializeComponent();
            // Устанавливаем абсолютные пути относительно базовой директории
            string appBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            databasePath = Path.Combine(appBaseDir, "recipes.db");
            logger = new Logger("output");
            loggerSequential = new Logger(Path.Combine(appBaseDir, "output"), "sequential_log");
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);
                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();
                    string createTableQuery = @"
                        CREATE TABLE Recipes (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            issue_id INTEGER,
                            url TEXT,
                            title TEXT,
                            ingredients TEXT,
                            steps TEXT,
                            image_url TEXT
                        )";
                    SQLiteCommand command = new SQLiteCommand(createTableQuery, connection);
                    command.ExecuteNonQuery();
                }
                logger.LogCollector("База данных создана.");
            }
            else
            {
                logger.LogCollector("База данных уже существует.");
            }
        }

        private void StartPipeline()
        {
            // Поток для стадии 1: Чтение и парсинг HTML
            stage1Thread = new Thread(Stage1_Process)
            {
                IsBackground = true
            };
            stage1Thread.Start();
            logger.LogCollector("Поток стадии 1 запущен.");

            // Поток для стадии 2: Дополнительная обработка
            stage2Thread = new Thread(Stage2_Process)
            {
                IsBackground = true
            };
            stage2Thread.Start();
            logger.LogCollector("Поток стадии 2 запущен.");

            // Поток для стадии 3: Запись в SQLite
            stage3Thread = new Thread(Stage3_Process)
            {
                IsBackground = true
            };
            stage3Thread.Start();
            logger.LogCollector("Поток стадии 3 запущен.");

            // Поток-накопитель: Логирование завершенных задач
            collectorThread = new Thread(Collector_Process)
            {
                IsBackground = true
            };
            collectorThread.Start();
            logger.LogCollector("Поток накопителя запущен.");
        }

        private void GenerateTasks()
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputfile.txt");
            if (!File.Exists(inputFilePath))
            {
                MessageBox.Show($"Файл {inputFilePath} не найден.");
                logger.LogCollector($"Файл {inputFilePath} не найден.");
                return;
            }

            var lines = File.ReadAllLines(inputFilePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
            if (lines.Count == 0)
            {
                logger.LogCollector("Файл inputfile.txt пуст.");
                return;
            }

            foreach (var line in lines)
            {
                var task = new TaskItem
                {
                    Id = GetNextTaskId(),
                    Url = line.Trim(),
                    CreationTime = DateTime.Now
                };
                queueStage1.Add(task);
                task.QueueStage1Time = DateTime.Now;
                logger.LogStage1($"Задача {task.Id} добавлена в очередь стадии 1: {task.Url}");
            }

            // Завершаем добавление задач в очередь стадии 1
            queueStage1.CompleteAdding();
            logger.LogStage1("Добавление задач в очередь стадии 1 завершено.");
        }

        private int GetNextTaskId()
        {
            lock (idLock)
                return ++taskIdCounter;
        }

        private void Stage1_Process()
        {
            logger.LogStage1("Поток стадии 1 начал работу.");

            foreach (var task in queueStage1.GetConsumingEnumerable())
            {
                task.StartStage1Time = DateTime.Now;
                logger.LogStage1($"Задача {task.Id} начала обработку на стадии 1: {task.Url}");

                // Чтение данных из URL
                try
                {
                    var web = new HtmlWeb();
                    web.PreRequest += request =>
                    {
                        request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                                            "AppleWebKit/537.36 (KHTML, like Gecko) " +
                                            "Chrome/114.0.0.0 Safari/537.36";
                        return true;
                    };
                    task.Doc = web.Load(task.Url);

                    logger.LogStage1($"Задача {task.Id} успешно обработана на стадии 1.");
                }
                catch (Exception ex)
                {
                    logger.LogStage1($"Ошибка при обработке задачи {task.Id} на стадии 1: {ex.Message}");
                    continue;
                }

                // Постановка задачи в очередь стадии 2
                task.QueueStage2Time = DateTime.Now;
                queueStage2.Add(task);
                logger.LogStage1($"Задача {task.Id} поставлена в очередь стадии 2.");
            }

            // Завершаем добавление задач в очередь стадии 2
            queueStage2.CompleteAdding();
            logger.LogStage1("Добавление задач в очередь стадии 2 завершено.");
            logger.LogStage1("Поток стадии 1 завершил работу.");
        }

        private void Stage2_Process()
        {
            logger.LogStage2("Поток стадии 2 начал работу.");

            foreach (var task in queueStage2.GetConsumingEnumerable())
            {
                task.StartStage2Time = DateTime.Now;
                logger.LogStage2($"Задача {task.Id} начала обработку на стадии 2.");

                // Извлечение данных
                try
                {
                    task.Title = ExtractTitle(task.Doc);
                    task.ImageUrl = ExtractImageUrl(task.Doc);
                    task.Ingredients = ExtractIngredients(task.Doc);
                    task.Steps = ExtractSteps(task.Doc);
                    logger.LogStage2($"Задача {task.Id} успешно обработана на стадии 2.");
                }
                catch (Exception ex)
                {
                    logger.LogStage2($"Ошибка при обработке задачи {task.Id} на стадии 2: {ex.Message}");
                    continue;
                }

                // Постановка задачи в очередь стадии 3
                task.QueueStage3Time = DateTime.Now;
                queueStage3.Add(task);
                logger.LogStage2($"Задача {task.Id} поставлена в очередь стадии 3.");
            }

            // Завершаем добавление задач в очередь стадии 3
            queueStage3.CompleteAdding();
            logger.LogStage2("Добавление задач в очередь стадии 3 завершено.");
            logger.LogStage2("Поток стадии 2 завершил работу.");
        }

        private void Stage3_Process()
        {
            logger.LogStage3("Поток стадии 3 начал работу.");

            foreach (var task in queueStage3.GetConsumingEnumerable())
            {
                task.StartStage3Time = DateTime.Now;
                logger.LogStage3($"Задача {task.Id} начала обработку на стадии 3.");

                // Запись данных в SQLite
                try
                {
                    using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                    {
                        connection.Open();
                        string insertQuery = @"
                            INSERT INTO Recipes (issue_id, url, title, ingredients, steps, image_url)
                            VALUES (@issue_id, @url, @title, @ingredients, @steps, @image_url)";
                        SQLiteCommand command = new SQLiteCommand(insertQuery, connection);
                        command.Parameters.AddWithValue("@issue_id", task.IssueId);
                        command.Parameters.AddWithValue("@url", task.Url);
                        command.Parameters.AddWithValue("@title", task.Title);
                        command.Parameters.AddWithValue("@ingredients", task.Ingredients);
                        command.Parameters.AddWithValue("@steps", task.Steps);
                        command.Parameters.AddWithValue("@image_url", task.ImageUrl);
                        int s;
                        s = command.ExecuteNonQuery();
                        Console.WriteLine(s);
                    }
                    
                    logger.LogStage3($"Задача {task.Id} успешно записана в базу данных.");
                }
                catch (Exception ex)
                {
                    logger.LogStage3($"Ошибка записи в базу данных для задачи {task.Id}: {ex.Message}");
                    continue;
                }

                task.DestructionTime = DateTime.Now;
                lock (completedTasksLock)
                    completedTasks.Add(task);

                logger.LogStage3($"Задача {task.Id} завершена и записана в базу данных.");
            }

            logger.LogStage3("Добавление задач в базу данных завершено.");
            logger.LogStage3("Поток стадии 3 завершил работу.");
        }

        private void Collector_Process()
        {
            // Используем Timer для периодического расчета статистики
            collectorTimer = new System.Threading.Timer(_ =>
            {
                List<TaskItem> tasksCopy;
                lock (completedTasksLock)
                {
                    tasksCopy = new List<TaskItem>(completedTasks);
                    completedTasks.Clear();
                }

                Console.WriteLine($"TASKS: {tasksCopy.Count}");

                if (tasksCopy.Count > 0)
                {
                    try
                    {
                        double averageLifetime = tasksCopy.Average(t => (t.DestructionTime - t.CreationTime).TotalMilliseconds);
                        double averageWaitStage1 = tasksCopy.Average(t => (t.StartStage1Time - t.CreationTime).TotalMilliseconds);
                        double averageWaitStage2 = tasksCopy.Average(t => (t.StartStage2Time - t.QueueStage2Time).TotalMilliseconds);
                        double averageWaitStage3 = tasksCopy.Average(t => (t.StartStage3Time - t.QueueStage3Time).TotalMilliseconds);
                        double averageProcessingStage1 = tasksCopy.Average(t => (t.StartStage1Time - t.QueueStage1Time).TotalMilliseconds);
                        double averageProcessingStage2 = tasksCopy.Average(t => (t.StartStage3Time - t.StartStage2Time).TotalMilliseconds);
                        double averageProcessingStage3 = tasksCopy.Average(t => (t.DestructionTime - t.StartStage3Time).TotalMilliseconds);

                        logger.LogCollector($"Среднее время существования задачи: {averageLifetime:F6} мс");
                        logger.LogCollector($"Среднее время ожидания в очереди стадии 1: {averageWaitStage1:F6} мс");
                        logger.LogCollector($"Среднее время ожидания в очереди стадии 2: {averageWaitStage2:F6} мс");
                        logger.LogCollector($"Среднее время ожидания в очереди стадии 3: {averageWaitStage3:F6} мс");
                        logger.LogCollector($"Среднее время обработки на стадии 1: {averageProcessingStage1:F6} мс");
                        logger.LogCollector($"Среднее время обработки на стадии 2: {averageProcessingStage2:F6} мс");
                        logger.LogCollector($"Среднее время обработки на стадии 3: {averageProcessingStage3:F6} мс");
                    }
                    catch (Exception ex)
                    {
                        logger.LogCollector($"Ошибка при расчете статистики: {ex.Message}");
                    }
                }

            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(120)); 

            logger.LogCollector("Поток накопителя запущен и готов к логированию статистики.");
        }
        #region Parsing Methods
        // HARD
        private string ExtractTitle(HtmlAgilityPack.HtmlDocument doc)
        {
            var titleNode = doc.DocumentNode.SelectSingleNode("//h1");
            return titleNode != null ? CleanText(titleNode.InnerText) : "Без названия";
        }

        private string ExtractImageUrl(HtmlAgilityPack.HtmlDocument doc)
        {
            var imgNode = doc.DocumentNode.SelectSingleNode("//img[@alt]");
            return imgNode != null ? imgNode.GetAttributeValue("src", "") : "";
        }

        private string ExtractIngredients(HtmlAgilityPack.HtmlDocument doc)
        {
            var ingredientsNodes = doc.DocumentNode.SelectNodes("//tr[@itemprop='recipeIngredient']");
            if (ingredientsNodes == null)
                return "";

            List<string> ingredients = new List<string>();
            foreach (var node in ingredientsNodes)
            {
                var nameNode = node.SelectSingleNode(".//span[@class='name']");
                var quantityNode = node.SelectSingleNode(".//span[@class='value']");
                var unitNode = node.SelectSingleNode(".//span[@class='type']");

                string name = nameNode != null ? CleanText(nameNode.InnerText) : "Неизвестный ингредиент";
                string quantity = quantityNode != null ? CleanText(quantityNode.InnerText) : "0";
                string unit = unitNode != null ? CleanText(unitNode.InnerText) : "";

                ingredients.Add($"{name}: {quantity} {unit}");
            }

            return string.Join("; ", ingredients);
        }

        private string ExtractSteps(HtmlAgilityPack.HtmlDocument doc)
        {
            var stepsNodes = doc.DocumentNode.SelectNodes("//div[@itemprop='recipeInstructions']//span[contains(@class, 'markup_text')]");
            if (stepsNodes == null)
                return "";

            List<string> steps = new List<string>();
            foreach (var node in stepsNodes)
            {
                string step = CleanText(node.InnerText);
                if (!string.IsNullOrWhiteSpace(step))
                    steps.Add(step);
            }

            return string.Join(" | ", steps);
        }

        private string CleanText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Удаление HTML-тегов и нежелательных символов
            string cleaned = Regex.Replace(input, "<.*?>", string.Empty);
            cleaned = Regex.Replace(cleaned, @"[\!\@\#\$\%\^\&\*\(\)\+\=\[\]\{\};:'"",<>\\|/]", " ");
            cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();
            return cleaned;
        }
        #endregion
        #region Input funcs
        private void InputURL()
        {
            baseUrl = tb_url.Text.Trim();
            if (string.IsNullOrEmpty(baseUrl))
            {
                MessageBox.Show("Введите корректный базовый URL.");
                return;
            }
            if (!Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
            {
                MessageBox.Show("Введите корректный абсолютный базовый URL (начинающийся с http:// или https://).");
                return;
            }

            if (!int.TryParse(tb_pages.Text, out pageLimit) || pageLimit <= 0)
            {
                MessageBox.Show("Введите корректное количество страниц (целое положительное число).");
                return;
            }

            // Генерация inputfile с ссылками на рецепты
            logger.LogCollector("Начата генерация inputfile.txt с ссылками на рецепты.");
            GenerateInputFile();
        }

        private void GenerateInputFile()
        {
            string inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inputfile.txt");
            using (var writer = new StreamWriter(inputFilePath, false))
            {
                for (int i = 2; i <= pageLimit + 1; i++) // первой страницы нет ахахахах
                {
                    string pageUrl = $"{baseUrl}?page={i}";

                    try
                    {
                        var doc = LoadHtmlDocument(pageUrl);
                        if (doc == null)
                        {
                            logger.LogCollector($"Не удалось загрузить страницу {pageUrl}");
                            continue;
                        }

                        var recipeLinks = ExtractRecipeLinks(doc);

                        Console.WriteLine($"{pageUrl} - {recipeLinks.Count}");
                        logger.LogCollector($"Страница {i} обработана, собрано ссылок: {recipeLinks.Count} из {recipeLinks.Count}");

                        if (recipeLinks.Count == 0)
                            logger.LogCollector($"На странице {i} не найдено ни одной ссылки.");
                        else
                        {
                            foreach (var link in recipeLinks)
                                writer.WriteLine(link);

                            logger.LogCollector($"Страница {i} обработана, собрано ссылок: {recipeLinks.Count}");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogCollector($"Ошибка при обработке страницы {pageUrl}: {ex.Message}");
                    }
                }
            }

            logger.LogCollector($"Файл inputfile.txt с ссылками успешно создан.");
        }

        private HtmlAgilityPack.HtmlDocument LoadHtmlDocument(string url)
        {
            var web = new HtmlWeb();
            web.PreRequest += request =>
            {
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                                    "Chrome/114.0.0.0 Safari/537.36";
                return true;
            };

            try
            {
                var doc = web.Load(url);
                return doc;
            }
            catch (Exception ex)
            {
                logger.LogCollector($"Ошибка при загрузке URL {url}: {ex.Message}");
                return null;
            }
        }

        private List<string> ExtractRecipeLinks(HtmlAgilityPack.HtmlDocument doc)
        {
            List<string> recipeLinks = new List<string>();

            // XPath для выбора <a> тегов с конкретными атрибутами
            var linkNodes = doc.DocumentNode.SelectNodes("//a[@class='card_card__YG0I9' and @itemprop='url' and @target='_self' and starts-with(@href, '/recipes/')]");

            if (linkNodes == null)
            {
                // жöпа
                logger.LogCollector("Не найдено ни одной ссылки на рецепт в текущей странице.");
                return recipeLinks;
            }
            logger.LogCollector($"Найдено {linkNodes.Count} ссылок на рецепты.");
            foreach (var node in linkNodes)
            {
                string href = node.GetAttributeValue("href", "").Trim();
                if (string.IsNullOrEmpty(href))
                    continue;

                // Преобразование относительных URL в абсолютные
                if (!Uri.IsWellFormedUriString(href, UriKind.Absolute))
                {
                    try
                    {
                        Uri baseUri = new Uri(baseUrl);
                        Uri absoluteUri = new Uri(baseUri, href);
                        href = absoluteUri.ToString();
                    }
                    catch (Exception ex)
                    {
                        logger.LogCollector($"Некорректный URL: {href}. Ошибка: {ex.Message}");
                        continue;
                    }
                }

                // Добавление уникальных ссылок
                if (!recipeLinks.Contains(href))
                {
                    recipeLinks.Add(href);
                    logger.LogCollector($"Найдена ссылка на рецепт: {href}");
                }
            }
            logger.LogCollector($"Всего собрано {recipeLinks.Count} уникальных ссылок на рецепты.");
            return recipeLinks;
        }
        #endregion
        #region Form funcs
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Завершаем добавление задач, если это ещё не сделано
            if (!queueStage1.IsAddingCompleted)
                queueStage1.CompleteAdding();
            if (!queueStage2.IsAddingCompleted)
                queueStage2.CompleteAdding();
            if (!queueStage3.IsAddingCompleted)
                queueStage3.CompleteAdding();

            // Дожидаемся завершения потоков
            if (stage1Thread != null && stage1Thread.IsAlive)
                stage1Thread.Join();
            if (stage2Thread != null && stage2Thread.IsAlive)
                stage2Thread.Join();
            if (stage3Thread != null && stage3Thread.IsAlive)
                stage3Thread.Join();
            if (collectorThread != null && collectorThread.IsAlive)
                collectorThread.Join();

            collectorTimer?.Dispose();

            logger.LogCollector("Все потоки завершены. Приложение закрывается.");
            loggerSequential.LogCollector("Все задачи последовательной обработки завершены. Приложение закрывается.");
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Получение ссылки
                InputURL();
                // Запуск потоков конвейера
                StartPipeline();
                // Генерация задач (чтение из inputfile)
                GenerateTasks();
                logger.LogCollector("Генерация задач завершена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
                logger.LogCollector($"Ошибка: {ex.Message}");
            }
        }
        #endregion
    }
}
