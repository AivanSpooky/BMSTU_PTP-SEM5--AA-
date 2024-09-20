import matplotlib.pyplot as plt 

from time import perf_counter
from random import choice

import string

from algos import levenstein_recursive, \
                  levenstein_dynamic, \
                  damerau_levenstein_dynamic


MSG = "\n\n      Меню \n\n \
    1. Расстояние Левенштейна (рекурсивно) \n \
    2. Расстояние Левенштейна (динамика) \n \
    3. Расстояние Дамерау-Левенштейна (динамика) \n \
    4. Замерить времени \n \
    0. Выход \n\n \
    \
    Выбор: \
    "


START = -1
EXIT = 0
LEV_REC = 1
LEV_DYN = 2
DAM_LEV_DYN = 3
TEST =  4
TIMES = 50
TO_MILISECONDS = 1000


def input_data():
    str1 = input("\nВведите 1-ую строку:\t")
    str2 = input("Введите 2-ую строку:\t")

    return str1, str2


def get_random_string(size):
    letters = string.ascii_lowercase

    return "".join(choice(letters) for _ in range(size))


def get_process_time(func, size):
    
    time_res = 0

    for _ in range(TIMES):
        str1 = get_random_string(size)
        str2 = get_random_string(size)

        time_start = perf_counter()
        func(str1, str2, output = False)
        time_end = perf_counter()

        time_res += (time_end - time_start)


    return time_res / TIMES

# Вывести график сравнения Лев(рек) и Лев(дин)
def graph_lev_rec_and_lev_dyn(time_lev_rec, time_lev_dyn):

    sizes = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]

    fig = plt.figure(figsize=(10, 7))
    plot = fig.add_subplot()
    plot.plot(sizes, time_lev_rec, label = "Левенштейн (рекурсия)")
    plot.plot(sizes, time_lev_dyn, label="Левенштейн (динамика)")

    plt.legend()
    plt.grid()
    plt.title("Временные характеристики")
    plt.ylabel("Затраченное время (с)")
    plt.xlabel("Длина (букв)")
    
    plt.show()

# Вывести график сравнения Лев(дин) и ДамЛев(дин)
def graph_lev_dyn_and_dam_lev_dyn(time_lev_dyn, time_dam_lev_dyn):

    sizes = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]

    fig = plt.figure(figsize=(10, 7))
    plot = fig.add_subplot()
    plot.plot(sizes, time_lev_dyn, label = "Левенштейн (динамика)", linestyle='--', color='g')
    plot.plot(sizes, time_dam_lev_dyn, label="Дамерау-Левенштейн (динамика)", linestyle=':', color='r')

    plt.legend()
    plt.grid()
    plt.title("Временные характеристики")
    plt.ylabel("Затраченное время (с)")
    plt.xlabel("Длина (букв)")
    
    plt.show()

# Функция для вывода графиков всех трёх алгоритмов
def graph_all_algorithms(time_lev_rec, time_lev_dyn, time_dam_lev_dyn):

    sizes = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]

    fig = plt.figure(figsize=(10, 7))
    plot = fig.add_subplot()

    plot.plot(sizes, time_lev_rec, label="Левенштейн (рекурсия)", linestyle='-', color='b')
    plot.plot(sizes, time_lev_dyn, label="Левенштейн (динамика)", linestyle='--', color='g')
    plot.plot(sizes, time_dam_lev_dyn, label="Дамерау-Левенштейн (динамика)", linestyle=':', color='r')

    plt.legend()
    plt.grid()
    plt.title("Временные характеристики всех алгоритмов")
    plt.ylabel("Затраченное время (с)")
    plt.xlabel("Длина (букв)")
    
    plt.show()


def test_algos():
    time_LEV_REC = []
    time_LEV_DYN = []
    time_DAM_LEV_DYN = []

    for num in range(10):

        print("Progress:\t", num * 10, "%")

        time_LEV_REC.append(get_process_time(levenstein_recursive, num))
        time_LEV_DYN.append(get_process_time(levenstein_dynamic, num))
        time_DAM_LEV_DYN.append(get_process_time(damerau_levenstein_dynamic, num))

    print("\n\nЗамер времени для алгоритмов: \n")

    ind = 0

    for num in range(10):
        print(" %8d & %.2e & %.2e & %.2e \\\\ \n \\hline" %(num, \
            time_LEV_REC[ind], \
            time_LEV_DYN[ind], \
            time_DAM_LEV_DYN[ind]))

        ind += 1

    graph_lev_rec_and_lev_dyn(time_LEV_REC, time_LEV_DYN)
    graph_lev_dyn_and_dam_lev_dyn(time_LEV_DYN, time_DAM_LEV_DYN)
    graph_all_algorithms(time_LEV_REC, time_LEV_DYN, time_DAM_LEV_DYN)


def main():
    option = START

    while (option != EXIT):
        option = int(input(MSG))

        if (option == LEV_REC):

            str1, str2 = input_data()
            print("\nРезультат: ", levenstein_recursive(str1, str2))

        elif (option == LEV_DYN):

            str1, str2 = input_data()
            print("\nРезультат: ", levenstein_dynamic(str1, str2))

        elif (option == DAM_LEV_DYN):

            str1, str2 = input_data()
            print("\nРезультат: ", damerau_levenstein_dynamic(str1, str2))

        elif (option == TEST):
            
            test_algos()

        else:
            print("\nПовторите ввод\n")


if __name__ == "__main__":
    main()