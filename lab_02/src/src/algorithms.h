#pragma once
#include <iostream>
#include "matrix.h"

Matrix Simple(const Matrix& m1, const Matrix& m2);
Matrix Winograd(const Matrix& m1, const Matrix& m2);
Matrix WinogradOpt(const Matrix& m1, const Matrix& m2);