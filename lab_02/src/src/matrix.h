#pragma once
#include <vector>
#include <string>
#include <iostream>

class Matrix
{
private:
    std::vector<std::vector<int>> data;
    size_t _rows, _cols;

public:
    Matrix();
    Matrix(size_t rows, size_t cols, int initial = 0);

    size_t rowCount() const { return _rows; }
    size_t colCount() const { return _cols; }

    std::vector<int>& operator[](size_t row);
    const std::vector<int>& operator[](size_t row) const;

    void randomFill(int upperLimit = 100);

    static Matrix loadFromFile(const std::string& path);

    Matrix operator+(const Matrix& rhs) const;
    Matrix operator-(const Matrix& rhs) const;

    friend std::ostream& operator<<(std::ostream& out, const Matrix& mtx);
};
