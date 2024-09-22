#include "algorithms.h"

Matrix Simple(const Matrix& m1, const Matrix& m2)
{
    size_t rows1 = m1.rows();
    size_t cols1 = m1.columns();
    size_t cols2 = m2.columns();

    Matrix res(rows1, cols2, 0);

    for (size_t i = 0; i < rows1; ++i)
        for (size_t j = 0; j < cols2; ++j)
            for (size_t k = 0; k < cols1; ++k)
                res[i][j] = res[i][j] + m1[i][k] * m2[k][j];

    return res;
}

Matrix Winograd(const Matrix& m1, const Matrix& m2)
{
    size_t rows1 = m1.rows();
    size_t cols1 = m1.columns();
    size_t cols2 = m2.columns();

    Matrix res(rows1, rows1);

    std::vector<int> row_factors(rows1, 0);
    std::vector<int> col_factors(cols2, 0);

    for (size_t i = 0; i < rows1; ++i)
        for (size_t j = 0; j < cols1 / 2; ++j)
            row_factors[i] = row_factors[i] + m1[i][2 * j] * m1[i][2 * j + 1];

    for (size_t i = 0; i < cols2; ++i)
        for (size_t j = 0; j < cols1 / 2; ++j)
            col_factors[i] = col_factors[i] + m2[2 * j][i] * m2[2 * j + 1][i];

    for (size_t i = 0; i < rows1; ++i)
    {
        for (size_t j = 0; j < cols2; ++j)
        {
            res[i][j] = -row_factors[i] - col_factors[j];
            for (size_t k = 0; k < cols1 / 2; ++k)
            {
                res[i][j] = res[i][j] + (m1[i][2 * k] + m2[2 * k + 1][j]) *
                    (m1[i][2 * k + 1] + m2[2 * k][j]);
            }
        }
    }

    if (cols1 % 2)
    {
        for (size_t i = 0; i < rows1; ++i)
            for (size_t j = 0; j < cols2; ++j)
                res[i][j] = res[i][j] + m1[i][cols1 - 1] *
                m2[cols1 - 1][j];
    }

    return res;
}

Matrix WinogradOpt(const Matrix& m1, const Matrix& m2)
{
    size_t rows1 = m1.rows();
    size_t cols1 = m1.columns();
    size_t cols2 = m2.columns();

    Matrix res(rows1, rows1);

    std::vector<int> row_factors(rows1, 0);
    std::vector<int> col_factors(cols2, 0);

    size_t half_cols1 = cols1 / 2;

    for (size_t i = 0; i < rows1; ++i)
    {
        for (size_t j = 0; j < half_cols1; ++j)
        {
            size_t j_mul = j << 1;
            row_factors[i] += m1[i][j_mul] * m1[i][j_mul + 1];
        }
    }

    for (size_t i = 0; i < cols2; ++i)
    {
        for (size_t j = 0; j < half_cols1; ++j)
        {
            size_t j_mul = j << 1;
            col_factors[i] += m2[j_mul][i] * m2[j_mul + 1][i];
        }
    }

    for (size_t i = 0; i < rows1; ++i)
    {
        for (size_t j = 0; j < cols2; ++j)
        {
            res[i][j] = -row_factors[i] - col_factors[j];
            for (size_t k = 0; k < half_cols1; ++k)
            {
                size_t k_mul = k << 1;
                res[i][j] += (m1[i][k_mul] + m2[k_mul + 1][j]) *
                    (m1[i][k_mul + 1] + m2[k_mul][j]);
            }
        }
    }

    if (cols1 % 2)
    {
        for (size_t i = 0; i < rows1; ++i)
            for (size_t j = 0; j < cols2; ++j)
                res[i][j] += m1[i][cols1 - 1] *
                m2[cols1 - 1][j];
    }

    return res;
}