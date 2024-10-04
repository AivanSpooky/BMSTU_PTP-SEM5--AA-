#include "measurements.h"
#include "algorithms.h"
#include <vector>
#include <iomanip>
#include <fstream>
#include <chrono>
#include <ctime>

double MeasureTime(const Matrix& A, const Matrix& B, MatrixFunction operation)
{
    auto startTime = std::clock();
    Matrix result = operation(A, B);
    auto endTime = std::clock();
    return 1000.0 * (endTime - startTime) / CLOCKS_PER_SEC;
}

void SaveToFile(const std::string& filename, double value)
{
    std::ofstream outputFile("times/" + filename, std::ios_base::app);
    if (outputFile.is_open())
    {
        outputFile << std::fixed << std::setprecision(2) << value << "\n";
        outputFile.close();
    }
    else
        std::cerr << "Failed to open file: " << filename << "\n";
}

void DisplayResults(int matrixSize, double naiveTime, double winogradTime, double optWinogradTime)
{
    int fieldWidth = 35;

    std::cout << "Matrix Size: " << matrixSize << "\n\n";

    std::cout << std::setw(fieldWidth) << "Naive Method: "
        << std::fixed << std::setprecision(2) << naiveTime << " ms\n";

    std::cout << std::setw(fieldWidth) << "Unoptimized Winograd: "
        << BLUE << std::fixed << std::setprecision(2) << winogradTime << " ms" << RESET << "\n";

    std::cout << std::setw(fieldWidth) << "Optimized Winograd: "
        << RED << std::fixed << std::setprecision(2) << optWinogradTime << " ms" << RESET << "\n";

    SaveToFile("i.txt", matrixSize);
    SaveToFile("times_0.txt", naiveTime);
    SaveToFile("times_1.txt", winogradTime);
    SaveToFile("times_2.txt", optWinogradTime);
}

void MeasureAndOutput(int startSize, int maxSize, int iterations)
{
    std::srand(std::time(nullptr));
    const int step = 10;
    startSize = startSize == 0 ? step : startSize;

    bool useSmallStep = true;
    for (int size = startSize; size <= maxSize + startSize; )
    {
        std::vector<long long> times(3, 0);

        Matrix A(size, size);
        Matrix B(size, size);
        A.randomFill();
        B.randomFill();

        for (int i = 0; i < iterations; ++i)
        {
            times[0] += MeasureTime(A, B, Simple);
            times[1] += MeasureTime(A, B, Winograd);
            times[2] += MeasureTime(A, B, WinogradOpt);
        }

        double avgNaiveTime = times[0] / static_cast<double>(iterations);
        double avgWinogradTime = times[1] / static_cast<double>(iterations);
        double avgOptWinogradTime = times[2] / static_cast<double>(iterations);

        DisplayResults(size, avgNaiveTime, avgWinogradTime, avgOptWinogradTime);

        size += (useSmallStep ? 1 : step);
        if (size >= 10)
            useSmallStep = !useSmallStep;

        std::cout << "---------------------------------\n";
    }
}