#include "Matrix.h"
#include <fstream>
#include <sstream>
#include <iomanip>
#include <stdexcept>
#include <cstdlib>
#include <ctime>

Matrix::Matrix() : _rows(0), _cols(0) {}

Matrix::Matrix(size_t rows, size_t cols, int initial)
    : _rows(rows), _cols(cols), data(rows, std::vector<int>(cols, initial))
{
    std::srand(static_cast<unsigned int>(std::time(nullptr)));
}

void Matrix::randomFill(int upperLimit)
{
    for (size_t row = 0; row < rowCount(); ++row)
        for (size_t col = 0; col < colCount(); ++col)
            data[row][col] = std::rand() % upperLimit;
}

Matrix Matrix::loadFromFile(const std::string& path)
{
    std::ifstream file(path);
    if (!file.is_open())
        throw std::runtime_error("Ошибка открытия файла: " + path);

    std::vector<std::vector<int>> loadedData;
    std::string line;

    while (std::getline(file, line))
    {
        std::istringstream stream(line);
        std::vector<int> rowValues;
        int value;
        while (stream >> value)
        {
            rowValues.push_back(value);
        }
        loadedData.push_back(rowValues);
    }

    size_t rows = loadedData.size();
    size_t cols = !loadedData.empty() ? loadedData[0].size() : 0;

    Matrix resultMatrix(rows, cols);
    resultMatrix.data = std::move(loadedData);

    return resultMatrix;
}

std::vector<int>& Matrix::operator[](size_t row)
{
    if (row >= data.size())
        throw std::out_of_range("Row index is out of bounds.");
    return data[row];
}

const std::vector<int>& Matrix::operator[](size_t row) const
{
    if (row >= data.size())
        throw std::out_of_range("Row index is out of bounds.");
    return data[row];
}


Matrix Matrix::operator+(const Matrix& rhs) const
{
    if (rowCount() != rhs.rowCount() || colCount() != rhs.colCount())
        throw std::invalid_argument("Матрицы имеют разные размеры!");

    Matrix sumMatrix(rowCount(), colCount());
    for (size_t row = 0; row < rowCount(); ++row)
        for (size_t col = 0; col < colCount(); ++col)
            sumMatrix[row][col] = data[row][col] + rhs[row][col];
    return sumMatrix;
}

Matrix Matrix::operator-(const Matrix& rhs) const
{
    if (rowCount() != rhs.rowCount() || colCount() != rhs.colCount())
        throw std::invalid_argument("Матрицы имеют разные размеры!");

    Matrix diffMatrix(rowCount(), colCount());
    for (size_t row = 0; row < rowCount(); ++row)
        for (size_t col = 0; col < colCount(); ++col)
            diffMatrix[row][col] = data[row][col] - rhs[row][col];
    return diffMatrix;
}

std::ostream& operator<<(std::ostream& out, const Matrix& mtx)
{
    for (const auto& row : mtx.data)
    {
        out << "[ ";
        for (const auto& value : row)
            out << std::setw(6) << value;
        out << " ]\n";
    }
    return out;
}
