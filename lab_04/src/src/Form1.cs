using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace src
{
    public partial class Form1 : Form
    {
        private HashSet<string> visitedUrls = new HashSet<string>();
        private int pageLimit;
        private int threadCount;
        private string baseUrl;
        private string outputDir;
        private readonly object consoleLock = new object();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                InputURL();

                if (radioButton1.Checked)
                {
                    if (!int.TryParse(tb_pages.Text, out pageLimit) || pageLimit <= 0)
                    {
                        MessageBox.Show("Введите корректное количество страниц (целое положительное число) для последовательного режима.");
                        return;
                    }

                    outputDir = "../../../seqfiles";
                    PrepareDirectory(outputDir);
                    ProcessSequential();
                }
                else if (radioButton2.Checked)
                {
                    if (!int.TryParse(tb_threads.Text, out threadCount) || threadCount <= 0 || threadCount > 16)
                    {
                        MessageBox.Show("Введите корректное количество потоков (целое число от 1 до 16) для параллельного режима.");
                        return;
                    }

                    pageLimit = threadCount;

                    outputDir = "../../../parfiles";
                    PrepareDirectory(outputDir);
                    ProcessParallel();
                }
                else
                {
                    MessageBox.Show("Выберите режим работы: последовательный или параллельный.");
                }
                lock (consoleLock)
                    Console.WriteLine($"Парсинг страниц закончен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
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
        }
        #endregion
        #region Processing funcs
        private void ProcessSequential()
        {
            for (int i = 2; i <= pageLimit+1; i++)
            {
                string url = BuildPageUrl(i);
                ProcessPage(url);
            }
        }
        private void ProcessSequential(List<long> times)
        {
            for (int i = 2; i <= pageLimit+1; i++)
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();
                string url = BuildPageUrl(i);
                ProcessPage(url);
                sw.Stop();
                times.Add(sw.ElapsedMilliseconds);
            }
        }
        private void ProcessParallel()
        {
            List<Thread> threads = new List<Thread>();

            for (int i = 2; i <= threadCount+1; i++)
            {
                int threadId = i;
                Thread thread = new Thread(() => ProcessPageInThread(threadId));
                thread.Start();
                threads.Add(thread);
            }
            foreach (var thread in threads)
                thread.Join();
        }
        private void ProcessParallel(List<long> times)
        {
            List<Thread> threads = new List<Thread>();

            for (int i = 2; i <= threadCount+1; i++)
            {
                int threadId = i;
                Thread thread = new Thread(() =>
                {
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    ProcessPageInThread(threadId);
                    sw.Stop();
                    lock (times)
                        times.Add(sw.ElapsedMilliseconds);
                });
                thread.Start();
                threads.Add(thread);
            }
            foreach (var thread in threads)
                thread.Join();
        }
        #endregion
        #region Page funcs
        private string BuildPageUrl(int pageNumber)
        {
            return $"{baseUrl}?page={pageNumber}";
        }
        private void ProcessPageInThread(int threadId)
        {
            string url = BuildPageUrl(threadId);
            ProcessPage(url);
        }
        private void ProcessPage(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                string html = client.GetStringAsync(url).Result;
                visitedUrls.Add(url);

                List<string> recipeLinks = ExtractRecipeLinks(html);
                SaveRecipeLinks(url, recipeLinks);
            }
            catch (Exception ex)
            {
                lock (consoleLock)
                    Console.WriteLine($"Ошибка при обработке {url}: {ex.Message}");
            }
        }
        #endregion
        #region Dir/File funcs
        private void PrepareDirectory(string directory)
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
            Directory.CreateDirectory(directory);
        }
        private void SaveRecipeLinks(string url, List<string> links)
        {
            string fileName = $"links_page_{visitedUrls.Count}.txt";
            string filePath = Path.Combine(outputDir, fileName);

            Directory.CreateDirectory(outputDir);

            File.WriteAllLines(filePath, links);

            lock (consoleLock)
                Console.WriteLine($"Сохранены ссылки со страницы: {url}");
        }
        #endregion
        #region Regex funcs
        private List<string> ExtractRecipeLinks(string html) // NO LIBRARY #!&*@>
        {
            List<string> recipeLinks = new List<string>();
            string itemPattern = @"<div\s+[^>]*role\s*=\s*[""']listitem[""'][^>]*>(?<itemContent>.+?)</div>";
            MatchCollection itemMatches = Regex.Matches(html, itemPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            foreach (Match itemMatch in itemMatches)
            {
                string itemContent = itemMatch.Groups["itemContent"].Value;

                string linkPattern = @"href\s*=\s*[""'](?<link>/recipes/[^""'#\s]+)[""']";
                Match linkMatch = Regex.Match(itemContent, linkPattern, RegexOptions.IgnoreCase);

                if (linkMatch.Success)
                {
                    string link = linkMatch.Groups["link"].Value;
                    if (!link.StartsWith("http"))
                    {
                        Uri baseUri = new Uri(baseUrl);
                        Uri absoluteUri = new Uri(baseUri, link);
                        link = absoluteUri.ToString();
                    }
                    if (!recipeLinks.Contains(link))
                    {
                        recipeLinks.Add(link);
                    }
                }
            }

            return recipeLinks;
        }
        #endregion
        #region Research funcs
        private void btn_research_Click(object sender, EventArgs e)
        {
            InputURL();
            StartResearch();
        }
        private void StartResearch()
        {
            var seriesSequential = chartResults.Series["Последовательно"];
            var seriesParallel = chartResults.Series["Параллельно"];
            seriesSequential.Points.Clear();
            seriesParallel.Points.Clear();

            int maxPages = 16;

            for (int pages = 1; pages <= maxPages; pages++)
            {
                #region Последовательно
                pageLimit = pages;
                outputDir = "../../../seqfiles";
                PrepareDirectory(outputDir);
                List<long> SeqTimes = new List<long>();
                var seq = System.Diagnostics.Stopwatch.StartNew();
                ProcessSequential(SeqTimes);
                seq.Stop();
                long totalSequentialTime = seq.ElapsedMilliseconds;
                #endregion
                #region Параллельно
                threadCount = pages;
                outputDir = "../../../parfiles";
                PrepareDirectory(outputDir);
                List<long> ParTimes = new List<long>();
                ProcessParallel(ParTimes);
                long maxParallelTime = ParTimes.Max();
                #endregion

                seriesSequential.Points.AddXY(pages, totalSequentialTime);
                seriesParallel.Points.AddXY(pages, maxParallelTime);
                lock (consoleLock)
                    Console.WriteLine($"Страницы: {pages}, Последовательный: {totalSequentialTime} мс, Параллельный: {maxParallelTime} мс");
            }
            chartResults.Invalidate();
        }
        #endregion
    }
}
