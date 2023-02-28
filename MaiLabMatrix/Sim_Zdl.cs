public class Sim_Zdl
{
    public void Program(Matrix matrix, int size)
    {
        double e = 0.01;

        string[] lines = File.ReadAllLines("Matrix.txt").Take(size).ToArray();

        double[,] a = new double[size, size];
        double[] b = new double[size];

        double[] x_n = new double[size];
        // разобрать в массивы
        for (int i = 0; i < size; i++)
        {

            double[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Double.Parse).ToArray();
            for (int j = 0, n = 0; n < size + 1; n++, j++)
            {

                if (j == i)
                {
                    x_n[i] = row[j];
                    a[i, j] = 0;
                }

                else if (j == size)
                {
                    b[i] = row[j];
                }

                else
                {
                    a[i, j] = row[j];
                }
            }
        }


        //приводим к эквивалентному виду
        for (int i = 0; i < size; i++)
        {
            for (int j = 0, n = 0; n < size; n++, j++)
            {
                a[i, j] = -a[i, j] / x_n[i];
            }
            b[i] /= x_n[i];
        }

        matrix.Mpi(a, b, size, e);
        matrix.Zdl(a, b, size, e);

        Console.WriteLine("Программа завершена, проверьте файлы Result_1_3");
    }
}
