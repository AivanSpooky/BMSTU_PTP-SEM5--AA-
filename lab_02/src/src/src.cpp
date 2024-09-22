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

bool IsPowerOfTwo(int val)
{
    while ((val & 1) != 1)
        val >>= 1;
    return val == 1;
}

int menu()
{
    std::cout << "\nМеню\n"
        "1. Стандартное умножение матриц.\n"
        "2. Умножение алгоритмом Винограда.\n"
        "3. Умножение оптимизированным алгоритмом Винограда.\n"
        "4. Замерить время для реализованных алгоритмов.\n"
        "5. Редактировать матрицы\n"
        "0. Выход.\n\n"
        "Выберите опцию (0-6): ";

    int choice;
    std::cin >> choice;
    std::cout << std::endl;

    return choice;
}

void EditMatrix(const std::string& s_FirstMatrixPath, const std::string& s_SecondMatrixPath)
{
    for (int c = 0;;)
    {
        std::cout << "Файл какой матрицы необходимо отредактировать? (1 или 2): ";
        std::cin >> c;
        if (c != 1 && c != 2) break;
        std::system((std::string("nano ") + (c == 1 ? s_FirstMatrixPath : s_SecondMatrixPath)).c_str());
    }
}

void MultiplyCommon(const Matrix& m1, const Matrix& m2)
{
    if (m1.rows() == 0)
    {
        std::cout << "Первая матрица имеет неправильный размер!\n";
        return;
    }
    else if (m2.rows() == 0)
    {
        std::cout << "Вторая матрица имеет неправильный размер!\n";
        return;
    }
    else if (m1.columns() != m2.rows())
    {
        std::cout << "Количество столбцов первой матрицы (" << m1.columns() <<
            ") не совпадает с количеством строк второй матрицы (" << m2.rows() << ")\n";
        return;
    }

    std::cout << Simple(m1, m2);
}

void MultiplyWinograd(const Matrix& m1, const Matrix& m2, Matrix(*Winograd)(const Matrix& m1, const Matrix& m2))
{
    if (m1.rows() == 0)
    {
        std::cout << "Первая матрица имеет недопустимый размер\n";
        return;
    }
    else if (m2.rows() == 0)
    {
        std::cout << "Вторая матрица имеет недопустимый размер\n";
        return;
    }
    else if (m1.rows() != m1.columns() or m2.rows() != m2.columns())
    {
        std::cout << "Матрицы должны быть квадратными\n";
        return;
    }
    else if (m1.rows() != m2.rows())
    {
        std::cout << "Размер первой матрицы (" << m1.rows() << "x" << m1.rows() <<
            ") не совпадает с размером второй (" << m2.rows() << "x" << m2.columns() << ")\n";
        return;
    }
    std::cout << Winograd(m1, m2);
}

int main()
{
    SetConsoleCP(1251);
    SetConsoleOutputCP(1251);
    std::string matrix_1_path = "txt/matrix_1.txt";
    std::string matrix_2_path = "txt/matrix_2.txt";

    // Создаём map, где ключ - выбор пользователя, а значение - соответствующая операция
    std::unordered_map<int, std::function<void()>> operations = {
        {1, [&]() {
            MultiplyCommon(Matrix::fromFile(matrix_1_path), Matrix::fromFile(matrix_2_path));
        }},
        {2, [&]() {
            MultiplyWinograd(Matrix::fromFile(matrix_1_path), Matrix::fromFile(matrix_2_path), Winograd);
        }},
        {3, [&]() {
            MultiplyWinograd(Matrix::fromFile(matrix_1_path), Matrix::fromFile(matrix_2_path), WinogradOpt);
        }},
        {4, [&]() {
            OutputTimeMeasurements(0, 100, 100);
            OutputTimeMeasurements(1, 100, 100);
            OutputTimeMeasurements(2, 128, 100);
        }},
        {5, [&]() {
            EditMatrix(matrix_1_path, matrix_2_path);
        }}
    };

    int choice = 0;
    while ((choice = menu()))
    {
        if (choice == 0)
            break;

        if (operations.find(choice) != operations.end())
            operations[choice]();
        else
            std::cout << "Неверный выбор, попробуйте снова.\n";
    }

    return 0;
}
