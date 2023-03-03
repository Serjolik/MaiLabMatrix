public class LU : Singleton<LU>
{   // Алгоритм лабораторной 1.1
    public void Program(Matrix matrix, int size)
    {   // Записываем массив строк
        string[] lines = File.ReadAllLines("Matrix.txt").Take(size).ToArray();

        // Записываем двумерную матрицу и вектор правой части
        double[,] mat = new double[size, size];
        double[] rightPart = new double[size];

        // разобрать в массивы
        for (int i = 0; i < size; i++)
        {
            double[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Double.Parse).ToArray();
            for (int j = 0, n = 0; n < size + 1; n++, j++)
            {

                if (j == size)
                {
                    rightPart[i] = row[j];
                }
                else
                {
                    mat[i, j] = row[j];
                }
            }
        }

        // Запускаем Алгоритм из библиотеки matrix
        matrix.LUP(mat, rightPart);

        Console.WriteLine("Программа завершена, проверьте файл Result_1_1");
    }
}