#include <iostream>
#include <string>
#include <limits>
#include <cstdlib>
#include <unordered_map>
#include <functional>
#include <windows.h>
#include "matrix.h"
#include "algorithms.h"
#include "measurements.h"

bool CheckPowerOfTwo(int value)
{
    return (value & (value - 1)) == 0 && value > 0;
}

int DisplayMenu()
{
    std::cout << "\nAvailable Options\n"
        << "1. Standard Matrix Multiplication\n"
        << "2. Winograd Algorithm Multiplication\n"
        << "3. Optimized Winograd Algorithm Multiplication\n"
        << "4. Time Measurement for Implemented Algorithms\n"
        << "0. Exit\n\n"
        << "Choose an option: ";

    int selection;
    std::cin >> selection;
    std::cout << std::endl;

    return selection;
}

void MultiplyUsingSimpleMethod(const Matrix& mat1, const Matrix& mat2)
{
    if (mat1.rowCount() == 0 || mat2.rowCount() == 0)
    {
        std::cout << "Matrix size is invalid!\n";
        return;
    }
    else if (mat1.colCount() != mat2.rowCount())
    {
        std::cout << "The number of columns in the first matrix (" << mat1.colCount()
            << ") does not match the number of rows in the second matrix ("
            << mat2.rowCount() << ")\n";
        return;
    }

    std::cout << Simple(mat1, mat2);
}

void MultiplyUsingWinogradMethod(const Matrix& mat1, const Matrix& mat2, Matrix(*winogradFunc)(const Matrix&, const Matrix&))
{
    if (mat1.rowCount() == 0 || mat2.rowCount() == 0)
    {
        std::cout << "Invalid matrix dimensions.\n";
        return;
    }
    else if (mat1.rowCount() != mat1.colCount() || mat2.rowCount() != mat2.colCount())
    {
        std::cout << "Both matrices must be square.\n";
        return;
    }
    else if (mat1.rowCount() != mat2.rowCount())
    {
        std::cout << "Matrix dimensions do not match: First matrix is "
            << mat1.rowCount() << "x" << mat1.colCount()
            << " and second matrix is "
            << mat2.rowCount() << "x" << mat2.colCount() << ".\n";
        return;
    }

    std::cout << winogradFunc(mat1, mat2);
}

int main()
{
    SetConsoleCP(1251);
    SetConsoleOutputCP(1251);

    const std::string matrixFilePath1 = "txt/matrix_1.txt";
    const std::string matrixFilePath2 = "txt/matrix_2.txt";

    std::unordered_map<int, std::function<void()>> actions = {
        {1, [&]() {
            MultiplyUsingSimpleMethod(Matrix::loadFromFile(matrixFilePath1), Matrix::loadFromFile(matrixFilePath2));
        }},
        {2, [&]() {
            MultiplyUsingWinogradMethod(Matrix::loadFromFile(matrixFilePath1), Matrix::loadFromFile(matrixFilePath2), Winograd);
        }},
        {3, [&]() {
            MultiplyUsingWinogradMethod(Matrix::loadFromFile(matrixFilePath1), Matrix::loadFromFile(matrixFilePath2), WinogradOpt);
        }},
        {4, [&]() {
            MeasureAndOutput(1, 100, 100);
        }}
    };

    int userChoice = 0;
    while ((userChoice = DisplayMenu()) != 0)
    {
        if (actions.find(userChoice) != actions.end())
        {
            actions[userChoice]();
        }
        else
        {
            std::cout << "Invalid option, please try again.\n";
        }
    }

    return 0;
}
