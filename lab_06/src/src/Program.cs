using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace src
{
    class Program
    {
        static int[,] graph;
        static int[,] originalGraph;
        static List<string> cities;
        static int n;

        static void Main(string[] args)
        {
            Console.WriteLine("Задача коммивояжёра\n");
            InitializeGraphs();

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1 - Выбрать граф");
                Console.WriteLine("2 - Выбрать алгоритм");
                Console.WriteLine("3 - Вывести информацию о выбранном графе");
                Console.WriteLine("4 - Запустить параметризацию");
                Console.WriteLine("5 - Запустить исследование производительности");
                Console.WriteLine("6 - Выход");
                Console.WriteLine("7 - Дополнительная параметризация (a=2, r=0.1, t=100, варьирование % элитных муравьев)");
                Console.Write("Ваш выбор: ");
                int choice;
                bool isValidChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!isValidChoice)
                {
                    Console.WriteLine("Неверный ввод. Пожалуйста, введите число от 1 до 7.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        SelectGraph();
                        break;
                    case 2:
                        SelectAlgorithm();
                        break;
                    case 3:
                        DisplayGraphInfo();
                        break;
                    case 4:
                        RunParameterization();
                        break;
                    case 5:
                        RunPerformanceStudy();
                        break;
                    case 6:
                        return;
                    case 7:
                        RunAdditionalParameterization();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static Dictionary<int, int[,]> graphs = new Dictionary<int, int[,]>();
        /*
         * Результаты параметризации:
            Elite% | mxG1    | avG1    | mdG1    | mxG2    | avG2    | mdG2    | mxG3    | avG3    | mdG3
            -----------------------------------------------------------------------------------------------
            0%     | 2,00    | 0,60    | 0,00    | 9,00    | 2,90    | 2,50    | 8,00    | 6,80    | 6,00
            5%     | 1,00    | 0,30    | 0,00    | 4,00    | 2,60    | 2,00    | 8,00    | 7,20    | 8,00
            10%    | 0,00    | 0,00    | 0,00    | 12,00   | 4,50    | 5,00    | 8,00    | 4,20    | 6,00
            15%    | 0,00    | 0,00    | 0,00    | 13,00   | 1,90    | 0,00    | 8,00    | 7,60    | 8,00
            20%    | 0,00    | 0,00    | 0,00    | 13,00   | 9,40    | 9,00    | 10,00   | 7,40    | 7,00
            25%    | 2,00    | 1,30    | 1,00    | 12,00   | 3,60    | 0,00    | 10,00   | 8,40    | 10,00
            30%    | 1,00    | 0,30    | 0,00    | 14,00   | 7,20    | 10,00   | 6,00    | 6,00    | 6,00
            35%    | 2,00    | 0,60    | 0,50    | 18,00   | 6,00    | 0,00    | 10,00   | 6,20    | 5,00
            40%    | 2,00    | 0,70    | 0,00    | 7,00    | 4,60    | 5,00    | 10,00   | 6,60    | 5,00
            45%    | 3,00    | 1,70    | 1,00    | 10,00   | 9,40    | 10,00   | 4,00    | 4,00    | 4,00
            50%    | 2,00    | 0,60    | 0,00    | 13,00   | 10,10   | 9,00    | 10,00   | 7,20    | 8,00
            55%    | 3,00    | 0,90    | 0,50    | 14,00   | 5,70    | 3,00    | 8,00    | 6,60    | 6,00
            60%    | 2,00    | 1,40    | 1,50    | 18,00   | 7,00    | 8,00    | 10,00   | 7,40    | 6,00
            65%    | 3,00    | 1,70    | 2,00    | 12,00   | 7,60    | 8,00    | 10,00   | 6,00    | 6,00
            70%    | 1,00    | 0,10    | 0,00    | 10,00   | 6,00    | 4,00    | 6,00    | 4,20    | 6,00
            75%    | 1,00    | 0,60    | 1,00    | 12,00   | 7,50    | 11,00   | 6,00    | 5,40    | 6,00
            80%    | 3,00    | 1,20    | 1,00    | 12,00   | 5,80    | 4,00    | 8,00    | 5,60    | 4,00
            85%    | 0,00    | 0,00    | 0,00    | 5,00    | 1,50    | 0,00    | 12,00   | 8,00    | 8,00
            90%    | 3,00    | 1,80    | 2,00    | 16,00   | 12,60   | 15,00   | 10,00   | 9,20    | 10,00
            95%    | 1,00    | 0,70    | 1,00    | 15,00   | 8,90    | 8,00    | 8,00    | 7,20    | 8,00
            100%   | 2,00    | 0,80    | 0,50    | 11,00   | 6,60    | 7,00    | 6,00    | 5,20    | 6,00
        Лучшие = 5%, 15%, 40%
         */
        static void RunAdditionalParameterization()
        {
            double alpha = 2.0;
            double rho = 0.1;
            int tmax = 100;
            double beta = 2.0;

            int numAnts = n; // Количество муравьёв

            List<int> elitePercentages = new List<int>();
            for (int p = 0; p <= 100; p += 5)
            {
                elitePercentages.Add(p);
            }
            int runsPerPercentage = 10;

            Dictionary<int, double> optimalLengths = new Dictionary<int, double>();
            for (int g = 1; g <= 3; g++)
            {
                int[,] localGraph = graphs[g];
                double optimalLength = RunBruteForceForOptimalLength(localGraph);
                optimalLengths[g] = optimalLength;
            }

            List<ParameterResult> parameterResults = new List<ParameterResult>();

            foreach (var percent in elitePercentages)
            {
                int numEliteAnts = (int)(numAnts * percent / 100.0);

                ParameterResult result = new ParameterResult();
                result.Alpha = alpha;
                result.Rho = rho;
                result.Tmax = tmax;
                result.ElitePercentage = percent;

                for (int g = 1; g <= 3; g++)
                {
                    int[,] localGraph = (int[,])graphs[g].Clone();
                    int[,] localOriginalGraph = (int[,])localGraph.Clone();

                    double optimalLength = optimalLengths[g];

                    List<double> deviations = new List<double>();

                    for (int run = 0; run < runsPerPercentage; run++)
                    {
                        AntColonyOptimization aco = new AntColonyOptimization(
                            (int[,])localGraph.Clone(),
                            (int[,])localOriginalGraph.Clone(),
                            alpha,
                            beta,
                            rho,
                            numAnts,
                            tmax,
                            numEliteAnts
                        );

                        List<int> bestTour = aco.Run();

                        if (bestTour != null)
                        {
                            double bestLength = aco.BestLength;
                            double deviation = bestLength - optimalLength;
                            deviations.Add(deviation);
                        }
                        else
                        {
                            deviations.Add(double.PositiveInfinity);
                        }
                    }

                    var validDeviations = deviations.Where(d => !double.IsInfinity(d)).ToList();

                    if (validDeviations.Count > 0)
                    {
                        double maxDeviation = validDeviations.Max();
                        double avgDeviation = validDeviations.Average();
                        double medianDeviation = GetMedian(validDeviations);

                        if (g == 1)
                        {
                            result.DeviationsG1.Add(maxDeviation);
                            result.DeviationsG1.Add(avgDeviation);
                            result.DeviationsG1.Add(medianDeviation);
                        }
                        else if (g == 2)
                        {
                            result.DeviationsG2.Add(maxDeviation);
                            result.DeviationsG2.Add(avgDeviation);
                            result.DeviationsG2.Add(medianDeviation);
                        }
                        else if (g == 3)
                        {
                            result.DeviationsG3.Add(maxDeviation);
                            result.DeviationsG3.Add(avgDeviation);
                            result.DeviationsG3.Add(medianDeviation);
                        }
                    }
                    else
                    {
                        if (g == 1)
                        {
                            result.DeviationsG1.Add(double.NaN);
                            result.DeviationsG1.Add(double.NaN);
                            result.DeviationsG1.Add(double.NaN);
                        }
                        else if (g == 2)
                        {
                            result.DeviationsG2.Add(double.NaN);
                            result.DeviationsG2.Add(double.NaN);
                            result.DeviationsG2.Add(double.NaN);
                        }
                        else if (g == 3)
                        {
                            result.DeviationsG3.Add(double.NaN);
                            result.DeviationsG3.Add(double.NaN);
                            result.DeviationsG3.Add(double.NaN);
                        }
                    }
                }

                parameterResults.Add(result);
            }

            // Вывод результатов в консоль в формате таблицы
            Console.WriteLine("\nРезультаты параметризации:");
            Console.WriteLine("Elite% | mxG1    | avG1    | mdG1    | mxG2    | avG2    | mdG2    | mxG3    | avG3    | mdG3");
            Console.WriteLine("-----------------------------------------------------------------------------------------------");

            foreach (var result in parameterResults)
            {
                string eliteStr = $"{result.ElitePercentage}%";

                string mxG1 = double.IsNaN(result.DeviationsG1.ElementAtOrDefault(0)) ? "N/A" : result.DeviationsG1[0].ToString("F2");
                string avG1 = double.IsNaN(result.DeviationsG1.ElementAtOrDefault(1)) ? "N/A" : result.DeviationsG1[1].ToString("F2");
                string mdG1 = double.IsNaN(result.DeviationsG1.ElementAtOrDefault(2)) ? "N/A" : result.DeviationsG1[2].ToString("F2");

                string mxG2 = double.IsNaN(result.DeviationsG2.ElementAtOrDefault(0)) ? "N/A" : result.DeviationsG2[0].ToString("F2");
                string avG2 = double.IsNaN(result.DeviationsG2.ElementAtOrDefault(1)) ? "N/A" : result.DeviationsG2[1].ToString("F2");
                string mdG2 = double.IsNaN(result.DeviationsG2.ElementAtOrDefault(2)) ? "N/A" : result.DeviationsG2[2].ToString("F2");

                string mxG3 = double.IsNaN(result.DeviationsG3.ElementAtOrDefault(0)) ? "N/A" : result.DeviationsG3[0].ToString("F2");
                string avG3 = double.IsNaN(result.DeviationsG3.ElementAtOrDefault(1)) ? "N/A" : result.DeviationsG3[1].ToString("F2");
                string mdG3 = double.IsNaN(result.DeviationsG3.ElementAtOrDefault(2)) ? "N/A" : result.DeviationsG3[2].ToString("F2");

                Console.WriteLine($"{eliteStr,-7}| {mxG1,-8}| {avG1,-8}| {mdG1,-8}| {mxG2,-8}| {avG2,-8}| {mdG2,-8}| {mxG3,-8}| {avG3,-8}| {mdG3,-8}");
            }
        }
        static void InitializeGraphs()
        {
            cities = new List<string>
            {
                "Москва",
                "Санкт-Петербург",
                "Новгород",
                "Псков",
                "Смоленск",
                "Казань",
                "Астрахань",
                "Владимир",
                "Тверь"
            };

            n = cities.Count;

            int[,] graph1 = new int[9, 9]
            {
                {  0, 10, 15, 20, 25, 30, 35, 40, 45 },
                { 12,  0, 37, 27, 19, 26, 33, 18, 17 },
                { 14, 35,  0, 32, 22, 23, 28, 24, 26 },
                { 16, 29, 30,  0, 33, 17, 27, 28, 31 },
                { 18, 21, 24, 35,  0, 37, 32, 20, 23 },
                { 20, 31, 29, 19, 31,  0, 22, 26, 29 },
                { 22, 33, 34, 25, 28, 24,  0, 22, 25 },
                { 24, 23, 26, 27, 22, 29, 26,  0, 13 },
                { 26, 25, 28, 29, 25, 31, 29, 15,  0 }
            };

            int[,] graph2 = new int[9, 9]
            {
                {  0, 12, 18, 24, 30, 36, 42, 48, 54 },
                { 14,  0, 23, 29, 35, 41, 47, 53, 59 },
                { 16, 25,  0, 38, 44, 50, 56, 62, 68 },
                { 18, 27, 40,  0, 53, 59, 65, 71, 77 },
                { 20, 29, 42, 55,  0, 68, 74, 80, 86 },
                { 22, 31, 44, 57, 70,  0, 83, 89, 95 },
                { 24, 33, 46, 59, 72, 85,  0, 98,104 },
                { 26, 35, 48, 61, 74, 87,100,  0,110 },
                { 28, 37, 50, 63, 76, 89,102,115,  0 }
            };

            int[,] graph3 = new int[9, 9]
            {
                {  0, 14, 20, 18, 24, 32, 40, 26, 22 },
                { 16,  0, 24, 30, 36, 40, 44, 28, 20 },
                { 18, 26,  0, 28, 32, 36, 40, 30, 22 },
                { 20, 28, 30,  0, 38, 42, 46, 32, 24 },
                { 22, 30, 34, 40,  0, 48, 52, 34, 26 },
                { 24, 32, 36, 42, 48,  0, 56, 36, 28 },
                { 26, 34, 38, 44, 50, 56,  0, 38, 30 },
                { 28, 36, 40, 46, 52, 58, 64,  0, 18 },
                { 30, 38, 42, 48, 54, 60, 66, 18,  0 }
            };

            graphs[1] = graph1;
            graphs[2] = graph2;
            graphs[3] = graph3;
        }


        static int[,] GenerateRandomGraph(int size)
        {
            int[,] tempGraph = new int[size, size];
            Random rand = new Random();

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (i != j)
                        tempGraph[i, j] = rand.Next(5, 15);
                    else
                        tempGraph[i, j] = 0;

            return tempGraph;
        }

        static void SelectGraph()
        {
            Console.WriteLine("\nДоступные графы:");
            Console.WriteLine("1 - Граф 1");
            Console.WriteLine("2 - Граф 2");
            Console.WriteLine("3 - Граф 3");
            Console.Write("Выберите граф: ");
            int graphChoice;
            bool isValidChoice = int.TryParse(Console.ReadLine(), out graphChoice);

            if (isValidChoice && graphs.ContainsKey(graphChoice))
            {
                graph = graphs[graphChoice];
                originalGraph = (int[,])graph.Clone();
                Console.WriteLine($"Граф {graphChoice} выбран.");
            }
            else
                Console.WriteLine("Неверный выбор графа.");
        }

        static void DisplayGraphInfo()
        {
            if (graph == null)
            {
                Console.WriteLine("Сначала выберите граф.");
                return;
            }

            Console.WriteLine("\nСписок городов:");
            for (int i = 0; i < cities.Count; i++)
                Console.WriteLine($"{i}: {cities[i]}");

            Console.WriteLine("\nМатрица графа (количество дней перехода между городами):");
            Console.Write("     ");
            for (int i = 0; i < n; i++)
                Console.Write($"{i,4}");
            Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                Console.Write($"{i,4}:");
                for (int j = 0; j < n; j++)
                    Console.Write($"{graph[i, j],4}");
                Console.WriteLine();
            }
        }

        static void SelectAlgorithm()
        {
            if (graph == null)
            {
                Console.WriteLine("Сначала выберите граф.");
                return;
            }

            Console.WriteLine("\nВыберите алгоритм:");
            Console.WriteLine("1 - Полный перебор");
            Console.WriteLine("2 - Муравьиный алгоритм");
            Console.Write("Ваш выбор: ");
            int algorithmChoice;
            bool isValidChoice = int.TryParse(Console.ReadLine(), out algorithmChoice);

            if (!isValidChoice)
            {
                Console.WriteLine("Неверный ввод. Пожалуйста, введите 1 или 2.");
                return;
            }

            switch (algorithmChoice)
            {
                case 1:
                    RunBruteForce();
                    break;
                case 2:
                    RunAntAlgorithm();
                    break;
                default:
                    Console.WriteLine("Неверный выбор алгоритма.");
                    break;
            }
        }

        static void RunBruteForce()
        {
            Console.WriteLine("\nЗапуск метода полного перебора...");

            List<int> bestRoute = null;
            int minCost = int.MaxValue;

            foreach (var permutation in GetPermutations(Enumerable.Range(0, n), n))
            {
                int cost = 0;
                bool validRoute = true;

                for (int i = 0; i < permutation.Count() - 1; i++)
                {
                    int from = permutation.ElementAt(i);
                    int to = permutation.ElementAt(i + 1);

                    if (graph[from, to] == 0)
                    {
                        validRoute = false;
                        break;
                    }

                    cost += graph[from, to];
                }

                if (validRoute && cost < minCost)
                {
                    minCost = cost;
                    bestRoute = permutation.ToList();
                }
            }

            if (bestRoute != null)
            {
                Console.WriteLine("Минимальная стоимость: " + minCost);
                Console.WriteLine("Лучший маршрут: " +
                    string.Join(" -> ", bestRoute.Select(i => cities[i])));
            }
            else
                Console.WriteLine("Маршрут не найден.");
        }

        // Генерация всех перестановок
        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1)
                return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(o => !t.Contains(o)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        static void RunAntAlgorithm()
        {
            Console.WriteLine("\nЗапуск муравьиного алгоритма...");

            Console.Write("Введите значение альфа (влияние феромона): ");
            double alpha;
            bool isValidAlpha = double.TryParse(Console.ReadLine(), out alpha);
            if (!isValidAlpha)
            {
                Console.WriteLine("Неверный ввод для альфа.");
                return;
            }

            Console.Write("Введите значение бета (влияние эвристики): ");
            double beta;
            bool isValidBeta = double.TryParse(Console.ReadLine(), out beta);
            if (!isValidBeta)
            {
                Console.WriteLine("Неверный ввод для бета.");
                return;
            }

            Console.Write("Введите значение ро (коэффициент испарения феромона): ");
            double rho;
            bool isValidRho = double.TryParse(Console.ReadLine(), out rho);
            if (!isValidRho)
            {
                Console.WriteLine("Неверный ввод для ро.");
                return;
            }

            Console.Write("Введите количество итераций tmax: ");
            int tmax;
            bool isValidTmax = int.TryParse(Console.ReadLine(), out tmax);
            if (!isValidTmax)
            {
                Console.WriteLine("Неверный ввод для tmax.");
                return;
            }

            Console.Write("Введите количество муравьёв: ");
            int numAnts;
            bool isValidAnts = int.TryParse(Console.ReadLine(), out numAnts);
            if (!isValidAnts || numAnts <= 0)
            {
                Console.WriteLine("Неверный ввод для количества муравьёв.");
                return;
            }

            Console.Write("Введите количество элитных муравьёв: ");
            int numEliteAnts;
            bool isValidElite = int.TryParse(Console.ReadLine(), out numEliteAnts);
            if (!isValidElite || numEliteAnts < 0 || numEliteAnts > numAnts)
            {
                Console.WriteLine("Неверный ввод для количества элитных муравьёв.");
                return;
            }

            AntColonyOptimization aco = new AntColonyOptimization(
                (int[,])graph.Clone(), (int[,])originalGraph.Clone(), alpha, beta, rho, numAnts, tmax, numEliteAnts);

            List<int> bestTour = aco.Run();

            if (bestTour != null)
            {
                Console.WriteLine("Лучшая длина маршрута: " + aco.BestLength);
                Console.WriteLine("Лучший маршрут: " +
                    string.Join(" -> ", bestTour.Select(i => cities[i])));
            }
            else
                Console.WriteLine("Маршрут не найден.");
        }

        // Параметризация
        static void RunParameterization()
        {
            Console.WriteLine("\nЗапуск параметризации...");
            double[] alphas = { 0.5, 1.0, 1.5, 2.0, 2.5 };
            double[] rhos = { 0.1, 0.3, 0.5, 0.7, 0.9 };
            int[] tmaxValues = { 50, 100, 150, 200, 250 };

            List<ParameterResult> parameterResults = new List<ParameterResult>();

            var parameterCombinations = from tmax in tmaxValues
                                        from rho in rhos
                                        from alpha in alphas
                                        select new { alpha, rho, tmax };

            // Предварительное вычисление оптимальных длин маршрутов
            Dictionary<int, double> optimalLengths = new Dictionary<int, double>();
            for (int g = 1; g <= 3; g++)
            {
                int[,] localGraph = graphs[g];
                double optimalLength = RunBruteForceForOptimalLength(localGraph);
                optimalLengths[g] = optimalLength;
            }

            Parallel.ForEach(parameterCombinations, param =>
            {
                ParameterResult result = new ParameterResult
                {
                    Alpha = param.alpha,
                    Rho = param.rho,
                    Tmax = param.tmax
                };

                for (int g = 1; g <= 3; g++)
                {
                    int[,] localGraph = (int[,])graphs[g].Clone();
                    int[,] localOriginalGraph = (int[,])localGraph.Clone();

                    double optimalLength = optimalLengths[g];

                    List<double> deviations = new List<double>();

                    for (int run = 0; run < 10; run++)
                    {
                        AntColonyOptimization aco = new AntColonyOptimization(
                            (int[,])localGraph.Clone(), (int[,])localOriginalGraph.Clone(),
                            param.alpha, 2.0, param.rho, n, param.tmax, n / 2);

                        List<int> tour = aco.Run();
                        double length = aco.BestLength;
                        double deviation = length - optimalLength;
                        deviations.Add(deviation);
                    }

                    double maxDeviation = deviations.Max();
                    double avgDeviation = deviations.Average();
                    double medianDeviation = GetMedian(deviations);

                    if (g == 1)
                    {
                        result.DeviationsG1.Add(maxDeviation);
                        result.DeviationsG1.Add(avgDeviation);
                        result.DeviationsG1.Add(medianDeviation);
                    }
                    else if (g == 2)
                    {
                        result.DeviationsG2.Add(maxDeviation);
                        result.DeviationsG2.Add(avgDeviation);
                        result.DeviationsG2.Add(medianDeviation);
                    }
                    else if (g == 3)
                    {
                        result.DeviationsG3.Add(maxDeviation);
                        result.DeviationsG3.Add(avgDeviation);
                        result.DeviationsG3.Add(medianDeviation);
                    }
                }

                lock (parameterResults)
                {
                    parameterResults.Add(result);
                }
            });

            var sortedResults = parameterResults
                .OrderBy(r => r.Tmax)
                .ThenBy(r => r.Rho)
                .ThenBy(r => r.Alpha)
                .ToList();

            GenerateLaTeXTable(sortedResults);

            Console.WriteLine("Параметризация завершена. Таблица результатов сохранена в файле 'table.tex'.");
        }

        static double RunBruteForceForOptimalLength(int[,] localGraph)
        {
            List<int> bestRoute = null;
            int minCost = int.MaxValue;
            int localN = localGraph.GetLength(0);

            // Генерация всех возможных незамкнутых маршрутов
            foreach (var permutation in GetPermutations(Enumerable.Range(0, localN), localN))
            {
                int cost = 0;
                bool validRoute = true;

                for (int i = 0; i < permutation.Count() - 1; i++)
                {
                    int from = permutation.ElementAt(i);
                    int to = permutation.ElementAt(i + 1);

                    if (localGraph[from, to] == 0)
                    {
                        validRoute = false;
                        break;
                    }

                    cost += localGraph[from, to];
                }

                if (validRoute && cost < minCost)
                {
                    minCost = cost;
                    bestRoute = permutation.ToList();
                }
            }

            return minCost;
        }

        static double GetMedian(List<double> numbers)
        {
            int numberCount = numbers.Count;
            int halfIndex = numbers.Count / 2;
            var sortedNumbers = numbers.OrderBy(n => n).ToList();
            double median;

            if ((numberCount % 2) == 0)
                median = ((sortedNumbers[halfIndex - 1] +
                           sortedNumbers[halfIndex]) / 2);
            else
                median = sortedNumbers[halfIndex];

            return median;
        }

        class ParameterResult
        {
            public double Alpha { get; set; }
            public double Rho { get; set; }
            public int Tmax { get; set; }
            public int ElitePercentage { get; set; }
            public List<double> DeviationsG1 { get; set; }
            public List<double> DeviationsG2 { get; set; }
            public List<double> DeviationsG3 { get; set; }

            public ParameterResult()
            {
                DeviationsG1 = new List<double>();
                DeviationsG2 = new List<double>();
                DeviationsG3 = new List<double>();
            }
        }
        static void GenerateLaTeXTable(List<ParameterResult> parameterResults)
        {
            using (StreamWriter writer = new StreamWriter("table.tex"))
            {
                writer.WriteLine("\\begin{table}[h]");
                writer.WriteLine("\\centering");
                writer.WriteLine("\\caption{Результаты параметризации}");
                writer.WriteLine("\\begin{tabular}{|r|r|r|r|r|r|r|r|r|r|r|r|}");
                writer.WriteLine("\\hline");
                writer.WriteLine("α & ρ & t & mxG1 & avG1 & " +
                                 "mdG1 & mxG2 & avG2 & " +
                                 "mdG2 & mxG3 & avG3 & " +
                                 "mdG3 \\\\");
                writer.WriteLine("\\hline");

                foreach (var result in parameterResults)
                {
                    string row = $"{result.Alpha} & {result.Rho} & {result.Tmax} & " +
                        $"{result.DeviationsG1[0]:F2} & {result.DeviationsG1[1]:F2} & {result.DeviationsG1[2]:F2} & " +
                        $"{result.DeviationsG2[0]:F2} & {result.DeviationsG2[1]:F2} & {result.DeviationsG2[2]:F2} & " +
                        $"{result.DeviationsG3[0]:F2} & {result.DeviationsG3[1]:F2} & {result.DeviationsG3[2]:F2} \\\\";
                    writer.WriteLine(row);
                }

                writer.WriteLine("\\hline");
                writer.WriteLine("\\end{tabular}");
                writer.WriteLine("\\end{table}");
            }
        }

        static void RunPerformanceStudy()
        {
            Console.WriteLine("\nЗапуск исследования производительности...");

            int tests = 50; // Количество замеров для усреднения
            List<int> sizes = Enumerable.Range(1, 8).ToList();

            List<(int size, double time)> bruteForceTimes = new List<(int, double)>();
            List<(int size, double time)> antAlgorithmTimes = new List<(int, double)>();

            foreach (int size in sizes)
            {
                Console.WriteLine($"Тестирование для размера матрицы: {size}x{size}");

                double totalBruteForceTime = 0;
                double totalAntAlgorithmTime = 0;

                for (int i = 0; i < tests; i++)
                {
                    int[,] testGraph = GenerateRandomGraph(size);
                    graph = testGraph;
                    originalGraph = (int[,])graph.Clone();

                    // Замер времени для полного перебора
                    Stopwatch swBruteForce = Stopwatch.StartNew();
                    RunBruteForceForPerformance();
                    swBruteForce.Stop();
                    totalBruteForceTime += swBruteForce.Elapsed.TotalMilliseconds;

                    // Замер времени для муравьиного алгоритма
                    Stopwatch swAntAlgorithm = Stopwatch.StartNew();
                    RunAntAlgorithmForPerformance();
                    swAntAlgorithm.Stop();
                    totalAntAlgorithmTime += swAntAlgorithm.Elapsed.TotalMilliseconds;
                }

                double avgBruteForceTime = totalBruteForceTime / tests;
                double avgAntAlgorithmTime = totalAntAlgorithmTime / tests;

                bruteForceTimes.Add((size, avgBruteForceTime));
                antAlgorithmTimes.Add((size, avgAntAlgorithmTime));

                SaveTimesToFile("bruteforce_times.txt", size, avgBruteForceTime);
                SaveTimesToFile("antalgorithm_times.txt", size, avgAntAlgorithmTime);
            }
            ShowPerformanceChart(bruteForceTimes, antAlgorithmTimes);
        }

        static void SaveTimesToFile(string filename, int size, double time)
        {
            lock (filename)
            {
                using (StreamWriter writer = new StreamWriter(filename, true))
                {
                    writer.WriteLine($"{size} {time}");
                }
            }
        }

        static void RunBruteForceForPerformance()
        {
            List<int> bestRoute = null;
            int minCost = int.MaxValue;
            int localN = graph.GetLength(0);

            foreach (var permutation in GetPermutations(Enumerable.Range(0, localN), localN))
            {
                int cost = 0;
                bool validRoute = true;

                for (int i = 0; i < permutation.Count() - 1; i++)
                {
                    int from = permutation.ElementAt(i);
                    int to = permutation.ElementAt(i + 1);

                    if (graph[from, to] == 0)
                    {
                        validRoute = false;
                        break;
                    }

                    cost += graph[from, to];
                }

                if (validRoute && cost < minCost)
                {
                    minCost = cost;
                    bestRoute = permutation.ToList();
                }
            }
        }

        static void RunAntAlgorithmForPerformance()
        {
            double alpha = 1.0;
            double beta = 2.0;
            double rho = 0.5;
            int tmax = 100;
            int numAnts = n;
            int numEliteAnts = n / 2;

            AntColonyOptimization aco = new AntColonyOptimization(
                (int[,])graph.Clone(), (int[,])originalGraph.Clone(), alpha, beta, rho, numAnts, tmax, numEliteAnts);

            aco.Run();
        }

        static void ShowPerformanceChart(List<(int size, double time)> bruteForceTimes,
                                 List<(int size, double time)> antAlgorithmTimes)
        {
            Thread thread = new Thread(() =>
            {
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                Form chartForm = new Form();
                chartForm.Text = "График производительности алгоритмов";
                chartForm.Size = new System.Drawing.Size(800, 600);

                Chart chart = new Chart();
                chart.Dock = DockStyle.Fill;

                ChartArea chartArea = new ChartArea();
                chart.ChartAreas.Add(chartArea);

                chartArea.AxisX.Minimum = 1;
                chartArea.AxisX.Maximum = 8;
                chartArea.AxisX.Interval = 1;
                chartArea.AxisX.Title = "Размер матрицы";

                chartArea.AxisY.Title = "Время (мс)";

                Series seriesBruteForce = new Series
                {
                    Name = "Полный перебор",
                    ChartType = SeriesChartType.Line,
                    Color = System.Drawing.Color.Red,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 8
                };

                Series seriesAntAlgorithm = new Series
                {
                    Name = "Муравьиный алгоритм",
                    ChartType = SeriesChartType.Line,
                    Color = System.Drawing.Color.Blue,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 8
                };

                chart.Series.Add(seriesBruteForce);
                chart.Series.Add(seriesAntAlgorithm);

                foreach (var dataPoint in bruteForceTimes)
                {
                    seriesBruteForce.Points.AddXY(dataPoint.size, dataPoint.time);
                }

                foreach (var dataPoint in antAlgorithmTimes)
                {
                    seriesAntAlgorithm.Points.AddXY(dataPoint.size, dataPoint.time);
                }

                chart.Legends.Add(new Legend());

                chartForm.Controls.Add(chart);

                System.Windows.Forms.Application.Run(chartForm);
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        // Класс муравьиного алгоритма
        class AntColonyOptimization
        {
            int n;        // Количество городов
            int[,] graph; // Матрица расстояний
            int[,] originalGraph; // Исходная матрица для смены сезона
            double[,] pheromone; // Матрица феромонов
            double alpha; // Влияние феромона
            double beta;  // Влияние эвристической информации
            double rho;   // Коэффициент испарения
            int numAnts;  // Количество муравьёв
            int tMax;     // Максимальное количество итераций
            int numEliteAnts; // Количество элитных муравьёв
            Random rand = new Random();
            public double BestLength { get; private set; } = double.MaxValue;
            List<int> BestTour;

            bool isSummer = true; // Начальный сезон - лето
            int seasonCounter = 0; // Счетчик дней для смены сезона

            public AntColonyOptimization(int[,] graph, int[,] originalGraph, double alpha, double beta,
                                         double rho, int numAnts, int tMax,
                                         int numEliteAnts)
            {
                this.graph = graph;
                this.originalGraph = originalGraph;
                n = graph.GetLength(0);
                this.alpha = alpha;
                this.beta = beta;
                this.rho = rho;
                this.numAnts = numAnts;
                this.tMax = tMax;
                this.numEliteAnts = numEliteAnts;
                pheromone = new double[n, n];

                // Инициализация феромона
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        pheromone[i, j] = 1.0;
            }

            public List<int> Run()
            {
                for (int t = 0; t < tMax; t++)
                {
                    List<List<int>> tours = new List<List<int>>();
                    List<double> lengths = new List<double>();

                    for (int k = 0; k < numAnts; k++)
                    {
                        List<int> tour = ConstructSolution();
                        double length = CalculateTourLength(tour);
                        tours.Add(tour);
                        lengths.Add(length);

                        if (length < BestLength)
                        {
                            BestLength = length;
                            BestTour = new List<int>(tour);
                        }
                    }

                    UpdatePheromones(tours, lengths);

                    seasonCounter++;
                    if (seasonCounter % 60 == 0)
                    {
                        UpdateSeason();
                        isSummer = !isSummer;
                    }
                }

                return BestTour;
            }

            List<int> ConstructSolution()
            {
                List<int> tour = new List<int>();
                bool[] visited = new bool[n];
                int currentCity = rand.Next(n);
                tour.Add(currentCity);
                visited[currentCity] = true;

                while (tour.Count < n)
                {
                    int nextCity = SelectNextCity(currentCity, visited);
                    if (nextCity == -1)
                        break;
                    tour.Add(nextCity);
                    visited[nextCity] = true;
                    currentCity = nextCity;
                }

                return tour;
            }

            int SelectNextCity(int currentCity, bool[] visited)
            {
                double[] probabilities = new double[n];
                double sum = 0.0;

                for (int i = 0; i < n; i++)
                {
                    if (!visited[i] && graph[currentCity, i] > 0)
                    {
                        probabilities[i] = Math.Pow(pheromone[currentCity, i], alpha) *
                                           Math.Pow(1.0 / graph[currentCity, i], beta);
                        sum += probabilities[i];
                    }
                    else
                        probabilities[i] = 0.0;
                }

                // Обеспечить ненулевую вероятность
                if (sum == 0.0)
                    return -1;

                double r = rand.NextDouble() * sum;
                double total = 0.0;

                for (int i = 0; i < n; i++)
                {
                    total += probabilities[i];
                    if (total >= r)
                        return i;
                }

                return -1;
            }

            void UpdatePheromones(List<List<int>> tours, List<double> lengths)
            {
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        pheromone[i, j] *= (1.0 - rho);

                for (int k = 0; k < tours.Count; k++)
                {
                    double delta = 1.0 / lengths[k];
                    List<int> tour = tours[k];

                    for (int i = 0; i < tour.Count - 1; i++)
                    {
                        int from = tour[i];
                        int to = tour[i + 1];
                        pheromone[from, to] += delta;
                    }
                }

                var eliteAnts = lengths.Select((length, index) => new { length, index })
                                        .OrderBy(l => l.length)
                                        .Take(numEliteAnts)
                                        .Select(l => tours[l.index]);
                foreach (var tour in eliteAnts)
                {
                    double delta = 1.0 / CalculateTourLength(tour);
                    for (int i = 0; i < tour.Count - 1; i++)
                    {
                        int from = tour[i];
                        int to = tour[i + 1];
                        pheromone[from, to] += delta;
                    }
                }
            }

            double CalculateTourLength(List<int> tour)
            {
                double length = 0.0;
                for (int i = 0; i < tour.Count - 1; i++)
                    length += graph[tour[i], tour[i + 1]];
                return length;
            }

            void UpdateSeason()
            {
                if (isSummer)
                {
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            graph[i, j] = originalGraph[i, j];
                }
                else
                {
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            if (i != j)
                                if ((i + j) % 2 == 0)
                                    graph[i, j] = originalGraph[i, j] / 2; // По течению
                                else
                                    graph[i, j] = originalGraph[i, j] * 4; // Против течения
                }
            }
        }
    }
}
