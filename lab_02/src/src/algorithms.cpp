#include "algorithms.h"

Matrix Simple(const Matrix& A, const Matrix& B)
{
    size_t rowsA = A.rowCount();
    size_t colsA = A.colCount();
    size_t colsB = B.colCount();

    Matrix result(rowsA, colsB, 0);

    for (size_t i = 0; i < rowsA; ++i)
        for (size_t j = 0; j < colsB; ++j)
            for (size_t k = 0; k < colsA; ++k)
                result[i][j] += A[i][k] * B[k][j];

    return result;
}

Matrix Winograd(const Matrix& A, const Matrix& B)
{
    size_t rowsA = A.rowCount();
    size_t colsA = A.colCount();
    size_t colsB = B.colCount();

    Matrix result(rowsA, colsB);

    std::vector<int> rowFactors(rowsA, 0);
    std::vector<int> colFactors(colsB, 0);

    for (size_t i = 0; i < rowsA; ++i)
        for (size_t j = 0; j < colsA / 2; ++j)
            rowFactors[i] += A[i][2 * j] * A[i][2 * j + 1];

    for (size_t i = 0; i < colsB; ++i)
        for (size_t j = 0; j < colsA / 2; ++j)
            colFactors[i] += B[2 * j][i] * B[2 * j + 1][i];

    for (size_t i = 0; i < rowsA; ++i)
        for (size_t j = 0; j < colsB; ++j)
        {
            result[i][j] = -rowFactors[i] - colFactors[j];
            for (size_t k = 0; k < colsA / 2; ++k)
                result[i][j] += (A[i][2 * k] + B[2 * k + 1][j]) * (A[i][2 * k + 1] + B[2 * k][j]);
        }

    if (colsA % 2)
        for (size_t i = 0; i < rowsA; ++i)
            for (size_t j = 0; j < colsB; ++j)
                result[i][j] += A[i][colsA - 1] * B[colsA - 1][j];

    return result;
}

Matrix WinogradOpt(const Matrix& A, const Matrix& B)
{
    size_t rowsA = A.rowCount();
    size_t colsA = A.colCount();
    size_t colsB = B.colCount();

    Matrix result(rowsA, colsB);

    std::vector<int> rowFactors(rowsA, 0);
    std::vector<int> colFactors(colsB, 0);

    for (size_t i = 0; i < rowsA; ++i)
        for (size_t j = 0; j < colsA / 2; ++j)
            rowFactors[i] += A[i][2 * j] * A[i][2 * j + 1];

    for (size_t i = 0; i < colsB; ++i)
        for (size_t j = 0; j < colsA / 2; ++j)
            colFactors[i] += B[2 * j][i] * B[2 * j + 1][i];

    for (size_t i = 0; i < rowsA; ++i)
    {
        for (size_t j = 0; j < colsB; ++j)
        {
            result[i][j] = -rowFactors[i] - colFactors[j];
            for (size_t k = 0; k < colsA - 1; k += 2)
            {
                result[i][j] += (A[i][k] + B[k + 1][j]) * (A[i][k + 1] + B[k][j]);
            }

            if (colsA % 2)
            {
                result[i][j] += A[i][colsA - 1] * B[colsA - 1][j];
            }
        }
    }

    return result;
}