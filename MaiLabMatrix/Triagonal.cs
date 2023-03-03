public class Triagonal : Singleton<Triagonal>
{   // Алгоритм лабораторной 1.2
    public void Program(Matrix matrix, int size)
    {
        // Записываем массив строк
        string[] lines = File.ReadAllLines("Matrix.txt").Take(size).ToArray();

        double[] a = new double[size];
        double[] b = new double[size];
        double[] c = new double[size];
        double[] d = new double[size];


        // разобрать в массивы
        for (int i = 0; i < size; i++)
        {
            double[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Double.Parse).ToArray();

            for (int j = 0; j < size - 1; j++)
            {
                switch (j)
                {
                    case 0:
                        a[i] = row[j];
                        break;
                    case 1:
                        b[i] = row[j];
                        break;
                    case 2:
                        c[i] = row[j];
                        break;
                    case 3:
                        d[i] = row[j];
                        break;
                }
            }
        }
        // Запускаем Алгоритм из библиотеки matrix
        matrix.Progonka(a, b, c, d, size);
        Console.WriteLine("Программа завершена, проверьте файл Result_1_2");
    }
}
    