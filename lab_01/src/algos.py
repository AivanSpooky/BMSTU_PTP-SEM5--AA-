# I/O

def create_lev_matrix(n, m):
    matrix = [[0] * m for _ in range(n)]
    for i in range(n):
        matrix[i][0] = i
    for j in range(m):
        matrix[0][j] = j
    return matrix
    

def print_lev_matrix(matrix, str1, str2):
    print("\n0  ∅  " + "  ".join([i for i in str2]))

    for i in range(len(str1) + 1):
        
        print(str1[i - 1] if (i != 0) else "∅", end = "")

        for j in range(len(str2) + 1):

            print("  " + str(matrix[i][j]), end = "")

        print("")


# Algos

def levenstein_recursive(str1, str2, output = True):
    n = len(str1)
    m = len(str2)

    if ((n == 0) or (m == 0)):
        return abs(n - m)

    if (str1[0] == str2[0]):
        return levenstein_recursive(str1[1:], str2[1:])

    min_distance = 1 + min(levenstein_recursive(str1[1:], str2),
                    levenstein_recursive(str1, str2[1:]),
                    levenstein_recursive(str1[1:], str2[1:]))

    return min_distance

def levenstein_dynamic(str1, str2, output = True):
    n = len(str1)
    m = len(str2)

    matrix = create_lev_matrix(n + 1, m + 1)

    for i in range(1, n + 1):
        for j in range(1, m + 1):
            add = matrix[i - 1][j] + 1
            delete = matrix[i][j - 1] + 1
            change = matrix[i - 1][j - 1]
            
            if (str1[i - 1] != str2[j - 1]):
                change += 1

            matrix[i][j] = min(add, delete, change)

    if (output):
        print("\nМатрица, с помощью которой происходило вычисление расстояния Левенштейна:")
        print_lev_matrix(matrix, str1, str2)

    return matrix[n][m]

def damerau_levenstein_dynamic(str1, str2, output=True):
    n = len(str1)
    m = len(str2)

    matrix = create_lev_matrix(n + 1, m + 1)

    for i in range(1, n + 1):
        for j in range(1, m + 1):
            add = matrix[i - 1][j] + 1    
            delete = matrix[i][j - 1] + 1 
            change = matrix[i - 1][j - 1]

            if str1[i - 1] != str2[j - 1]:
                change += 1

            matrix[i][j] = min(add, delete, change)

            if i > 1 and j > 1 and str1[i - 1] == str2[j - 2] and str1[i - 2] == str2[j - 1]:
                transposition = matrix[i - 2][j - 2] + 1
                matrix[i][j] = min(matrix[i][j], transposition)

    if output:
        print("\nМатрица, с помощью которой происходило вычисление расстояния Дамерау-Левенштейна:")
        print_lev_matrix(matrix, str1, str2)

    return matrix[n][m]