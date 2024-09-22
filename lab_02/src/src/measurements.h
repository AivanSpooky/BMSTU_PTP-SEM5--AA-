#pragma once
#include "algorithms.h"
#include <functional>
#include <cstddef>

#define RESET   "\033[0m"           // Сброс цветов
#define RED     "\033[31m"          // Красный цвет
#define BLUE    "\033[34m"          // Синий цвет

typedef std::function<Matrix(const Matrix&, const Matrix&)> MatrixFunction;

double MeasureTime(const Matrix& m1, const Matrix& m2, MatrixFunction fn);

void OutputTimeMeasurements(int start, int maxLen, int iters);