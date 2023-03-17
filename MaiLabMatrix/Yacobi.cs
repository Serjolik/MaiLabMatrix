public class Yacobi : Singleton<Yacobi>
{
    public void Program(Matrix matrix, int size)
    {   // Used methods in algorithm 1.1 in Matrix.cs
        double e = 0.01;
        string[] lines = File.ReadAllLines("Matrix.txt").ToArray();
        double[,] a = new double[size, size];
        double max = 0;
        int n = 0, m = 0; //временные индексы

        // разобрать в массив
        for (int i = 0; i < size; i++)
        {
            double[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Double.Parse).ToArray();
            for (int j = 0; j < size; j++)
            {
                a[i, j] = row[j];
            }
        }

        double t_k = 1;
        int it = 0;
        List<double[,]> U = new List<double[,]>();
        while (t_k > e)
        {
            it++;
            t_k = 0;
            //max
            max = 0;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                        continue;
                    else
                    {
                        if ((i < j) && (Math.Abs(a[i, j]) >= max))
                        {
                            max = Math.Abs(a[i, j]);
                            n = i;
                            m = j;
                        }
                    }
                }
            }

            //fi
            double fi = 0;
            if (a[n, n] == a[m, m])
                fi = Math.PI / 4;
            else
                fi = 0.5 * Math.Atan(2 * a[n, m] / (a[n, n] - a[m, m]));

            //U
            double[,] U_k = new double[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    U_k[i, j] = 0;
                    if (i == j)
                        U_k[i, j] = 1;
                    if ((i == n && j == n) || (i == m && j == m))
                        U_k[i, j] = Math.Cos(fi);
                    if (i == n && j == m)
                        U_k[i, j] = -Math.Sin(fi);
                    if (i == m && j == n)
                        U_k[i, j] = Math.Sin(fi);
                }
            }
            U.Add(U_k);
            //A(k+1)
            double[,] a_k = new double[size, size];
            a_k = matrix.Mult(matrix.Mult(matrix.Transposition(U_k), a), U_k);

            //критерий окончания
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i < j)
                    {
                        t_k += a_k[i, j] * a_k[i, j];
                    }
                }
            }
            t_k = Math.Sqrt(t_k);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    a[i, j] = a_k[i, j];
                    a_k[i, j] = 0;
                }
            }

        }

        //nums
        using (StreamWriter outputFile = new StreamWriter("jacobi_ownnums.txt"))
        {
            outputFile.WriteLine("Own numbers:");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                        outputFile.WriteLine(a[i, j]);
                }
            }
            outputFile.WriteLine(it.ToString() + " iterations");
        }

        //vecs
        double[,] res = new double[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (i == j)
                    res[i, j] = 1;
                else
                    res[i, j] = 0;
            }
        }
        foreach (double[,] x in U)
        {
            res = matrix.Mult(res, x);
        }

        using (StreamWriter outputFile = new StreamWriter("jacobi_ownvecs.txt"))
        {
            outputFile.WriteLine("Own vectors:");
            for (int i = 0; i < size; i++)
            {
                outputFile.WriteLine("x" + (i + 1));
                for (int j = 0; j < size; j++)
                {
                    outputFile.WriteLine(res[j, i]);
                }
            }
            outputFile.WriteLine(it.ToString() + " iterations");
        }


    }
}