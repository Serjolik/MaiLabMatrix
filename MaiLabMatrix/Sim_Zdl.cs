public class Sim_Zdl : Singleton<Sim_Zdl>
{   // Алгоритм лабораторной 1.3
    public void Program(Matrix matrix, int size)
    {
        // Точность вычислений
        double e = 0.01;

        // Записываем массив строк
        string[] lines = File.ReadAllLines("Matrix.txt").Take(size).ToArray();

        // Выделяем память под матрицу и под правую часть
        double[,] mat = new double[size, size];
        double[] rightPart = new double[size];

        // Под главную диагональ (как понимаю, судя по алгоритму)
        double[] x_n = new double[size];

        // разобрать в массивы
        for (int i = 0; i < size; i++)
        {

            double[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Double.Parse).ToArray();
            for (int j = 0, n = 0; n < size + 1; n++, j++)
            {
                if (j == i)
                {   // Если главная диагональ
                    x_n[i] = row[j];
                    mat[i, j] = 0;
                }

                else if (j == size)
                {   // Если последняя строка
                    rightPart[i] = row[j];
                }

                else
                {
                    mat[i, j] = row[j];
                }
            }
        }


        //приводим к эквивалентному виду
        for (int i = 0; i < size; i++)
        {
            for (int j = 0, n = 0; n < size; n++, j++)
            {
                mat[i, j] = -mat[i, j] / x_n[i];
            }
            rightPart[i] /= x_n[i];
        }

        // Запускаем Алгоритм MPI и Алгоритм ZDL из библиотеки
        matrix.Mpi(mat, rightPart, size, e);
        matrix.Zdl(mat, rightPart, size, e);

        Console.WriteLine("Программа завершена, проверьте файлы Result_1_3");
    }
}
