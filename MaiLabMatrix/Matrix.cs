public class Matrix
{


    //  |------------------------------------|
    //  | РАЗДЕЛ 1.1                         |
    //  | LU алгоритм и вычисление СЛАУ      |
    //  |------------------------------------|

    /// <summary>
    /// Выводит в файл выполнение 1.1
    /// </summary>
    /// <param name="n">Размерность массива</param>
    /// <param name="L">L Матрица</param>
    /// <param name="U">U Матрица</param>
    /// <param name="lu">lu Матрица</param>
    /// <param name="x">Вектор решения</param>
    private void OutputFile(int n, double[,] L, double[,] U, double[,] lu, double[] x)
    {   // Вывод в файлы

        if (!File.Exists("Result_1_1.txt"))
        {
            File.Create("Result_1_1.txt").Close();
        }

        using (StreamWriter outputFile = new StreamWriter("Result_1_1.txt"))
        {
            outputFile.WriteLine("L:");
            PrintTable(outputFile, n, L);
            outputFile.WriteLine();

            outputFile.WriteLine("U:");
            PrintTable(outputFile, n, U);
            outputFile.WriteLine();

            outputFile.WriteLine("X:");
            foreach (double i in x)
                outputFile.WriteLine(i);
            outputFile.WriteLine();

            outputFile.WriteLine("Check:");
            PrintTable(outputFile, n, lu);
        }
    }

    /// <summary>
    /// Перемножает матрицы
    /// </summary>
    /// <param name="matrixA">Первая матрица</param>
    /// <param name="matrixB">Вторая матрица</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private double[,] Mult(double[,] matrixA, double[,] matrixB)
    {
        if (matrixA.GetLength(1) != matrixB.GetLength(0))
            throw new Exception("Матрицы нельзя перемножить");

        double[,] r = new double[matrixA.GetLength(0), matrixB.GetLength(1)];

        for (int i = 0; i < matrixA.GetLength(0); i++)
        {
            for (int j = 0; j < matrixB.GetLength(1); j++)
            {
                for (int k = 0; k < matrixB.GetLength(0); k++)
                {
                    r[i, j] += matrixA[i, k] * matrixB[k, j];
                }
            }
        }
        return r;
    }

    /// <summary>
    /// Перемножает матрицу и вектор
    /// </summary>
    /// <param name="matrixA">Матрица</param>
    /// <param name="Vector">Вектор</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private double[] Mult(double[,] matrixA, double[] Vector)
    {
        if (matrixA.GetLength(1) != Vector.GetLength(0))
            throw new Exception("Матрицы нельзя перемножить");

        double[] r = new double[Vector.GetLength(0)];

        for (int i = 0; i < matrixA.GetLength(0); i++)
        {
            for (int j = 0; j < Vector.GetLength(0); j++)
            {
                r[i] += matrixA[i, j] * Vector[j];
            }
        }
        return r;
    }

    /// <summary>
    /// Функция которая печатает в файл матрицу
    /// </summary>
    /// <param name="outputFile">Файл в который записываем</param>
    /// <param name="n">Размерность матрицы</param>
    /// <param name="matrix">Сама матрица</param>
    private void PrintTable(StreamWriter outputFile, int n, double[,] matrix)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                outputFile.Write(matrix[i, j] + " ");
            }
            outputFile.WriteLine();
        }
    }

    /// <summary>
    /// Транспонирование матрицы
    /// </summary>
    /// <param name="matrix">Матрица которую транспонируем</param>
    /// <returns>Транпонированная матрица</returns>
    private double[,] Transposition(double[,] matrix)
    {
        double[,] r = new double[matrix.GetLength(0), matrix.GetLength(1)];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                r[i, j] = matrix[j, i];
            }
        }
        return r;
    }

    /// <summary>
    /// Функция которая меняет местами строки
    /// </summary>
    /// <param name="matrix">Матрица</param>
    /// <param name="pivot">Номер первой строки</param>
    /// <param name="i">Номер второй строки</param>
    /// <param name="n">Размерность матрицы</param>
    /// <returns>Матрица с поменянными строками</returns>
    private double[,] SwapRows(double[,] matrix, int pivot, int i, int n)
    {   // Свап строк
        for (int j = 0; j < n; j++)
        {
            double temp;
            temp = matrix[pivot, j];
            matrix[pivot, j] = matrix[i, j];
            matrix[i, j] = temp;
        }
        return matrix;
    }

    /// <summary>
    /// Основа Алгоритма 1.1
    /// </summary>
    /// <param name="matrix">Матрица</param>
    /// <param name="rightPart">Правая часть со значениями</param>
    public void LUP(double[,] matrix, double[] rightPart)
    {
        int n = matrix.GetLength(0);

        double[,] C = new double[n, n];
        double[,] P = new double[n, n];

        C = matrix;

        /* Пусть матрица P - единичная матрица:
         * [1 0 0 0]
         * [0 1 0 0]
         * [0 0 1 0]
         * [0 0 0 1]
         */

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                    P[i, j] = 1;
                else
                    P[i, j] = 0;
            }
        }

        // Найдём C = L + U - E
        for (int i = 0; i < n; i++)
        {   //поиск опорного элемента
            double pivotValue = 0;
            int pivot = -1;
            for (int row = i; row < n; row++)
            {
                if (Math.Abs(C[row, i]) > pivotValue)
                {
                    pivotValue = Math.Abs(C[row, i]);
                    pivot = row;
                }
            }
            if (pivotValue != 0)
            {   //меняем местами i-ю строку и строку с опорным элементом
                P = SwapRows(P, pivot, i, n);
                C = SwapRows(C, pivot, i, n);

                for (int j = i + 1; j < n; j++)
                {
                    C[j, i] /= C[i, i];
                    for (int k = i + 1; k < n; k++)
                        C[j, k] -= C[j, i] * C[i, k];
                }
            }
        }

        // Создаём L, U
        double[,] L = new double[n, n];
        double[,] U = new double[n, n];


        for (int i = 0; i < n; i++)
        {   //раскладываем в матрицы L, U
            for (int j = 0; j < n; j++)
            {
                if (i == j)
                {
                    L[i, j] = 1;
                    U[i, j] = C[i, j];
                }
                if (i < j)
                {
                    L[i, j] = 0;
                    U[i, j] = C[i, j];
                }
                if (i > j)
                {
                    U[i, j] = 0;
                    L[i, j] = C[i, j];
                }
            }
        }

        //Создаём 2 одномерных массива x, z
        double[] x = new double[n];
        double[] z = new double[n];

        //Находим P*b Где b - rightPart
        double[] Pb = Mult(P, rightPart);


        // Само решение ЛАУ
        for (int i = 0; i < n; i++)
        {
            if (i == 0)
                z[0] = Pb[0];
            else
            {
                double sum = 0;
                for (int j = 0; j <= i - 1; j++)
                {
                    sum += L[i, j] * z[j];
                }
                z[i] = Pb[i] - sum;
            }
        }
        for (int i = n - 1; i >= 0; i--)
        {
            if (i == n - 1)
                x[n - 1] = z[n - 1] / U[n - 1, n - 1];
            else
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                {
                    sum += U[i, j] * x[j];
                }
                x[i] = 1 / U[i, i] * (z[i] - sum);
            }
        }

        // LU
        double[,] lu = new double[n, n];
        lu = Mult(Transposition(P), Mult(L, U));

        //вывод
        OutputFile(n, L, U, lu, x);
    }


    //  |------------------------------------|
    //  | РАЗДЕЛ 1.2                         |
    //  | ПРОГОНКА И ТРЁХДИАГОНАЛЬНАЯ МАТРИЦА|
    //  |------------------------------------|

    /// <summary>
    /// Сам основной Алгоритм 1.2
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    /// <param name="size">Размерность</param>
    public void Progonka(double[] a, double[] b, double[] c, double[] d, int size)
    {
        double[] P = new double[size];
        double[] Q = new double[size];

        double[] X = new double[size];

        for (int i = 0; i < size; i++)
        {
            /*
            //проверка достаточного условия

            if ((a[i] == 0 && i != 0) || (c[i] == 0 && i != size - 1) || (Math.Abs(b[i]) < Math.Abs(a[i]) + Math.Abs(c[i])))
            {
              Console.WriteLine("Warning: exception - sufficient convergence condition is not satisfied.");
            Environment.Exit(-1);
            }

            */

            if (i == 0)
            {
                P[i] = -c[i] / b[i];
                Q[i] = d[i] / b[i];
            }
            else
            {
                P[i] = -c[i] / (b[i] + a[i] * P[i - 1]);
                Q[i] = (d[i] - a[i] * Q[i - 1]) / (b[i] + a[i] * P[i - 1]);
            }
        }

        for (int i = size - 1; i >= 0; i--)
        {
            if (i == size - 1)
            {
                X[i] = Q[i];
            }
            else
            {
                X[i] = P[i] * X[i + 1] + Q[i];
            }

        }
        OutputFile(X);
    }

    /// <summary>
    /// Запись результатов в файл
    /// </summary>
    /// <param name="X">Вектор ответов</param>
    private void OutputFile(double[] X)
    {
        //Проверка на существование файла с ответами
        if (!File.Exists("Result_1_2.txt"))
        {
            File.Create("Result_1_2.txt").Close();
        }

        using (StreamWriter outputFile = new StreamWriter("Result_1_2.txt"))
        {
            foreach (double x in X)
                outputFile.WriteLine(x);
        }
    }


    //  |------------------------------------|
    //  | РАЗДЕЛ 1.3                         |
    //  | Итерации и Зейдель                 |
    //  |------------------------------------|

    /* В РЕШЕНИИ ИСПОЛЬЗУЮТСЯ:
     * ФУНКЦИЯ Mult ИЗ РАЗДЕЛА 1.1
     * ;
     */

    /// <summary>
    /// Нормирование матрицы
    /// </summary>
    /// <param name="matrix">Матрица</param>
    /// <param name="size">Размерность</param>
    /// <returns></returns>
    private double Norm(double[,] matrix, int size)
    {
        double[] sum = new double[size];
        double norm = 0;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0, n = 0; n < size; n++, j++)
            {
                sum[i] += Math.Abs(matrix[i, j]);
            }
        }
        norm = sum.Max();
        return norm;
    }

    /// <summary>
    /// Нормирование вектора
    /// </summary>
    /// <param name="X">Вектор</param>
    /// <param name="size">Размерность</param>
    /// <returns></returns>
    private double Norm(double[] X, int size)
    {
        double norm = 0;
        for (int i = 0; i < size; i++)
        {
            X[i] = Math.Abs(X[i]);
        }
        norm = X.Max();
        return norm;
    }

    
    private double[] Subt(double[] a, double[] b)
    {
        double[] r = new double[b.GetLength(0)];
        for (int i = 0; i < a.GetLength(0); i++)
        {
            r[i] = a[i] - b[i];
        }
        return r;
    }
    private double[,] Subt(double[,] a, double[,] b)
    {
        double[,] r = new double[a.GetLength(0), a.GetLength(1)];
        for (int i = 0; i < a.GetLength(1); i++)
        {
            for (int j = 0; j < a.GetLength(0); j++)
            {
                r[i, j] = a[i, j] - b[i, j];
            }
        }
        return r;
    }

    private double[] Add(double[] a, double[] b)
    {
        double[] r = new double[a.GetLength(0)];
        for (int i = 0; i < a.GetLength(0); i++)
        {
            r[i] = a[i] + b[i];
        }
        return r;
    }

    private double[,] Reversed_m(double[,] m)
    {
        int n = m.GetLength(0);

        double[,] rev = new double[n, n];
        for (int i = 0; i < n; i++)
            rev[i, i] = 1;

        double[,] big_m = new double[n, 2 * n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
            {
                big_m[i, j] = m[i, j];
                big_m[i, j + n] = rev[i, j];
            }

        //Прямой ход (Зануление нижнего левого угла)
        for (int k = 0; k < n; k++) //k-номер строки
        {
            //меняем строки, если на гл.диагонали 0
            if (big_m[k, k] == 0)
            {
                for (int l = k + 1; l < n; l++)
                {
                    if (big_m[l, l] == 0)
                        continue;
                    else
                    {
                        for (int j = 0; j < n * 2; j++)
                        {
                            double temp;
                            temp = big_m[l, j];
                            big_m[l, j] = big_m[k, j];
                            big_m[k, j] = temp;
                        }
                        for (int j = 0; j < n; j++)
                        {
                            double temp;
                            temp = m[l, j];
                            m[l, j] = m[k, j];
                            m[k, j] = temp;
                        }
                        break;
                    }
                }
            }
            for (int i = 0; i < 2 * n; i++) //i-номер столбца
            {
                big_m[k, i] = big_m[k, i] / m[k, k]; //Деление k-строки на первый член !=0 для преобразования его в единицу
            }
            for (int i = k + 1; i < n; i++) //i-номер следующей строки после k
            {
                double K = big_m[i, k] / big_m[k, k]; //Коэффициент
                for (int j = 0; j < 2 * n; j++) //j-номер столбца следующей строки после k
                    big_m[i, j] = big_m[i, j] - big_m[k, j] * K; //Зануление элементов матрицы ниже первого члена, преобразованного в единицу
            }
            for (int i = 0; i < n; i++) //Обновление, внесение изменений в начальную матрицу
                for (int j = 0; j < n; j++)
                    m[i, j] = big_m[i, j];
        }

        //Обратный ход (Зануление верхнего правого угла)
        for (int k = n - 1; k > -1; k--) //k-номер строки
        {
            for (int i = 2 * n - 1; i > -1; i--) //i-номер столбца
                big_m[k, i] = big_m[k, i] / m[k, k];
            for (int i = k - 1; i > -1; i--) //i-номер следующей строки после k
            {
                double K = big_m[i, k] / big_m[k, k];
                for (int j = 2 * n - 1; j > -1; j--) //j-номер столбца следующей строки после k
                    big_m[i, j] = big_m[i, j] - big_m[k, j] * K;
            }
        }

        //Отделяем от общей матрицы
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                rev[i, j] = big_m[i, j + n];

        return rev;
    }

    /// <summary>
    /// Основной алгоритм Метода простых итераций
    /// </summary>
    /// <param name="matrix">Матрица</param>
    /// <param name="right">Правая часть</param>
    /// <param name="size">Размерность</param>
    /// <param name="e">Точность</param>
    public void Mpi(double[,] matrix, double[] right, int size, double e)
    {
        double e_n = 1;
        double[] X = new double[size];
        double[] X_prev = new double[size];

        for (int i = 0; i < size; i++)
        {
            X_prev[i] = right[i];
        }

        double[] r = new double[X_prev.GetLength(0)];
        int it = 0;
        while (e_n > e)
        {
            r = Mult(matrix, X_prev);
            for (int i = 0; i < size; i++)
            {
                X[i] = right[i] + r[i];
            }
            if (Norm(matrix, size) >= 1)
                e_n = Norm(Subt(X, X_prev), size);
            else
                e_n = Norm(matrix, size) / (1 - Norm(matrix, size)) * Norm(Subt(X, X_prev), size);
            for (int i = 0; i < size; i++)
            {
                X_prev[i] = X[i];
                X[i] = 0;
            }
            it++;
        }
        OutputFileMPI(X_prev ,it);
    }

    /// <summary>
    /// Основной алгоритм Метода Зейделя
    /// </summary>
    /// <param name="matrix">Матрица</param>
    /// <param name="right">Правая часть</param>
    /// <param name="size">Размерность</param>
    /// <param name="e">Точность</param>
    public void Zdl(double[,] matrix, double[] right, int size, double e)
    {
        //вспомогательные матрицы
        double[,] C = new double[size, size];
        double[,] D = new double[size, size];
        double[,] E = new double[size, size];

        //заполняем матрицы
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == j)
                    E[i, j] = 1;
                else
                    E[i, j] = 0;
                if (j >= i)
                {
                    D[i, j] = matrix[i, j];
                    C[i, j] = 0;
                }
                else
                {
                    C[i, j] = matrix[i, j];
                    D[i, j] = 0;
                }
            }
        }

        double e_n = 1;
        double[] X = new double[size];
        double[] X_prev = new double[size];

        for (int i = 0; i < size; i++)
        {
            X_prev[i] = right[i];
        }

        double[] r = new double[X_prev.GetLength(0)];

        int it = 0;

        while (e_n > e)
        {
            X = Add(Mult(Mult(Reversed_m(Subt(E, C)), D), X_prev), Mult(Reversed_m(Subt(E, C)), right));

            if (Norm(matrix, size) >= 1)
                e_n = Norm(Subt(X, X_prev), size);
            else
                e_n = Norm(D, size) / (1 - Norm(matrix, size)) * Norm(Subt(X, X_prev), size);

            for (int i = 0; i < size; i++)
            {
                X_prev[i] = X[i];
                X[i] = 0;
            }

            it++;
        }

        OutputFileZDL(X_prev, it);
    }

    /// <summary>
    /// Вывод Метода простых итераций
    /// </summary>
    /// <param name="X_prev">Вектор ответов</param>
    /// <param name="it">Количество итераций</param>
    private void OutputFileMPI(double[] X_prev, int it)
    {
        //Проверка на существование файла с ответами
        if (!File.Exists("Result_1_3_MPI.txt"))
        {
            File.Create("Result_1_3_MPI.txt").Close();
        }

        using (StreamWriter outputFile = new StreamWriter("Result_1_3_MPI.txt"))
        {
            outputFile.WriteLine("MPI:");
            foreach (double x in X_prev)
                outputFile.WriteLine(x);
            outputFile.WriteLine(it.ToString() + " iterations");
        }
    }

    /// <summary>
    /// Вывод метода Зейделя
    /// </summary>
    /// <param name="X_prev">Вектор ответов</param>
    /// <param name="it">Количество итераций</param>
    private void OutputFileZDL(double[] X_prev, int it)
    {
        //Проверка на существование файла с ответами
        if (!File.Exists("Result_1_3_ZDL.txt"))
        {
            File.Create("Result_1_3_ZDL.txt").Close();
        }

        using (StreamWriter outputFile = new StreamWriter("Result_1_3_ZDL.txt"))
        {
            outputFile.WriteLine("ZDL:");
            foreach (double x in X_prev)
                outputFile.WriteLine(x);
            outputFile.WriteLine(it.ToString() + " iterations");
        }
    }

}
