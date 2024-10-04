#pragma once

#include "algorithms.h"
#include <functional>
#include <string>

#define RESET   "\033[0m"           // Reset colors
#define RED     "\033[31m"          // Red color
#define BLUE    "\033[34m"          // Blue color


typedef std::function<Matrix(const Matrix&, const Matrix&)> MatrixFunction;


double MeasureTime(const Matrix& m1, const Matrix& m2, MatrixFunction fn);

void SaveToFile(const std::string& filename, double value);

void DisplayResults(int matrixSize, double naiveTime, double winogradTime, double optWinogradTime);

void MeasureAndOutput(int startSize, int maxSize, int iterations);
