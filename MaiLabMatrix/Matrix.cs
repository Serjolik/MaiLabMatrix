public class Matrix
{

    private void OutputFile(int n, double[,] L, double[,] U, double[,] lu, double[] x)
    {
        //Проверка на существование файла с ответами
        if (!File.Exists("Result.txt"))
        {
            File.Create("Result.txt").Close();
        }

        using (StreamWriter outputFile = new StreamWriter("Result.txt"))
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

    private double[] Mult(double[,] matrixA, double[] matrixB)
    {
        if (matrixA.GetLength(1) != matrixB.GetLength(0))
            throw new Exception("Матрицы нельзя перемножить");

        double[] r = new double[matrixB.GetLength(0)];

        for (int i = 0; i < matrixA.GetLength(0); i++)
        {
            for (int j = 0; j < matrixB.GetLength(0); j++)
            {
                r[i] += matrixA[i, j] * matrixB[j];
            }
        }
        return r;
    }

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


    private double[,] Transposition(double[,] a)
    {
        double[,] r = new double[a.GetLength(0), a.GetLength(1)];
        for (int i = 0; i < a.GetLength(0); i++)
        {
            for (int j = 0; j < a.GetLength(1); j++)
            {
                r[i, j] = a[j, i];
            }
        }
        return r;
    }

    private double[,] SwapRows(double[,] matrix, int pivot, int i, int n)
    {
        for (int j = 0; j < n; j++)
        {
            double temp;
            temp = matrix[pivot, j];
            matrix[pivot, j] = matrix[i, j];
            matrix[i, j] = temp;
        }
        return matrix;
    }

    public void LUP(double[,] A, double[] rightPart)
    {
        int n = A.GetLength(0);
        double[,] C = new double[n, n];
        double[,] P = new double[n, n];
        C = A;

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

        //Находим P*b
        double[] Pb = Mult(P, rightPart);

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

        double[,] lu = new double[n, n];
        lu = Mult(Transposition(P), Mult(L, U));

        //вывод
        OutputFile(n, L, U, lu, x);
    }

}
