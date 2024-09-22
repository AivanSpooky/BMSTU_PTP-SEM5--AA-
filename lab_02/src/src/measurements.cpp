#include "measurements.h"

#include <vector>
#include <iomanip>
#include <fstream>
#include <chrono>
#include <time.h>

double MeasureTime(const Matrix& m1, const Matrix& m2, MatrixFunction fn)
{
    auto start = std::chrono::high_resolution_clock::now();
    Matrix res = fn(m1, m2);
    auto end = std::chrono::high_resolution_clock::now();
    auto duration = std::chrono::duration_cast<std::chrono::nanoseconds>(end - start);

    return static_cast<double>(duration.count());
}

void WriteToFile(const std::string& filename, double data)
{
    std::ofstream file("times/" + filename, std::ios_base::app);
    if (file.is_open())
    {
        file << std::fixed << std::setprecision(2) << data << "\n";
        file.close();
    }
    else
        std::cerr << "Не удалось открыть файл: " << filename << "\n";
}

void OutputResults(int i, double times_0, double times_1, double times_2)
{
    int fieldWidth = 35;

    std::cout << "Размер матрицы: " << i << "\n\n";

    std::cout << std::setw(fieldWidth) << "Обычный метод: "
        << std::fixed << std::setprecision(2) << times_0 << " нс" << "\n";

    std::cout << std::setw(fieldWidth) << "Виноград неоптимизированный: "
        << BLUE << std::fixed << std::setprecision(2) << times_1 << " нс" << RESET << "\n";

    std::cout << std::setw(fieldWidth) << "Виноград оптимизированный: "
        << RED << std::fixed << std::setprecision(2) << times_2 << " нс" << RESET << "\n";

    WriteToFile("i.txt", i);
    WriteToFile("times_0.txt", times_0);
    WriteToFile("times_1.txt", times_1);
    WriteToFile("times_2.txt", times_2);
}

void OutputTimeMeasurements(int start, int maxLen, int iters)
{
    std::srand(std::time(nullptr));
    const int step = 10;
    start = start == 0 ? step : start;
    for (int i = start; i <= maxLen + start; )
    {
        std::vector<long long> times(4, 0);

        Matrix m1(i, i);
        Matrix m2(i, i);
        m1.fillRandom();
        m2.fillRandom();

        for (int j = 0; j < iters; ++j)
        {
            times[0] += MeasureTime(m1, m2, Simple);
            times[1] += MeasureTime(m1, m2, Winograd);
            times[2] += MeasureTime(m1, m2, WinogradOpt);
        }

        double times_0 = times[0] / (double)iters;
        double times_1 = times[1] / (double)iters;
        double times_2 = times[2] / (double)iters;

        OutputResults(i, times_0, times_1, times_2);

        if (start == 2)
            i *= 2;
        else
        {
            i += step;
            std::cout << "-------------------------------" << RESET << "\n";
        }
    }
}