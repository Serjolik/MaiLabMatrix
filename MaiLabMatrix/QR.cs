using System;
using System.Drawing;
using System.Linq;
public class QR : Singleton<QR>
{
    public void Program(Matrix matrix, int size)
    {
        var slau = SLAU.GetSLAU(size);
        Console.WriteLine("Press enter");
        Console.WriteLine();
        slau.QRdecomposition();
        Console.ReadLine();
    }
}

public class SLAU
{
    public SLAU(int size)
    {   //конструктор принимает количество уравнений
        Matrix = new double[size, size];
        N = size;
    }

    //ввод данных с клавиатуры
    public static SLAU GetSLAU(int size)  
    {
        var slau = new SLAU(size);
        string[] lines = File.ReadAllLines("Matrix.txt").ToArray();
        double[,] a = new double[size, size];

        // разобрать в массив
        for (int i = 0; i < size; i++)
        {
            double[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Double.Parse).ToArray();
            for (int j = 0; j < size; j++)
            {
                a[i, j] = row[j];
            }
        }
        slau.Matrix = a;
        return slau;
    }

    //вывод решения СЛАУ
    public void PrintResult(double[] res)
    {
        for (int i = 0; i < res.Length; i++)
        {
            Console.WriteLine("{0}) {1:0.000}", i+1, res[i]);
        }
        Console.WriteLine();
        Console.WriteLine();
    }

    //QR-алгоритм
    public void QRdecomposition()
    {
        var A = new double[N, N];
        var newA = new double[N, N];
        var Q = new double[N, N];
        var R = new double[N, N];
        var eigenvectors = new double[N,N];
        var eigenvalues = new double[N];
        var dif = 0.0;
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                A[i, j] = Matrix[i, j];
                if (i == j)
                {
                    eigenvectors[i, j] = 1.0;
                }
                else
                {
                    eigenvectors[i, j] = 0.0;
                }
            }
        }

        do
        {

            GetQR(A, Q, R);

            var ev = MultiplyMatrix(eigenvectors, Q);
            eigenvectors = ev;

            newA = MultiplyMatrix(R, Q);


            dif = 0.0;
            for (int i = 0; i < N; i++)
            {
                if (dif < Math.Abs(eigenvalues[i] - newA[i, i]))
                {
                    dif = Math.Abs(eigenvalues[i] - newA[i, i]);
                }
                eigenvalues[i] = newA[i, i];
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    A[i, j] = newA[i, j];
                }
            }
        } while (dif > 0.000001);
        
        Console.WriteLine("Собственные числа:");
        PrintResult(eigenvalues);
        for (int n = 0; n < N; n++)
        {
            for (int k = 0; k < N; k++)
            {
                for (int j = 0; j < N; j++)
                {
                    eigenvectors[k, j] = Matrix[k, j];
                    if (k == j) eigenvectors[k, k] = Matrix[k, k] - eigenvalues[n];
                }
            }

            double tmp;
            for (int i = 0; i < N; i++)
            {
                tmp = eigenvectors[i, i];
                for (int j = N - 1; j >= i; j--)
                    eigenvectors[i, j] /= tmp;
                for (int j = i + 1; j < N; j++)
                {
                    tmp = eigenvectors[j, i];
                    for (int k = N - 1; k >= i; k--)
                        eigenvectors[j, k] -= tmp * eigenvectors[i, k];
                }
            }
            var eigenvector = new double[N];
            eigenvector[N - 1] = eigenvectors[N - 1, N - 1];
            for (int i = N - 2; i >= 0; i--)
            {
                eigenvector[i] = eigenvectors[i, N - 1];
                for (int j = i; j < N; j++)
                {
                    eigenvector[i] -= eigenvectors[i, j] * eigenvector[j];
                }
            }
            Console.WriteLine("Собственный вектор {0}:", n + 1);
            PrintResult(Rationing(eigenvector));
        }
    }


    //получение матриц Q и R
    private void GetQR(double[,] A, double[,] Q, double [,] R)
    {
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Q[i, j] = 0.0;
                R[i, j] = 0.0;
            }
        }
        var QT = new double[N, N];
        for (int i = 0; i < N; i++)
            QT[i, 0] = A[i, 0];

        R[0, 0] = 1;
        var sum = 0.0;
        for (int k = 0; k < N; k++)
        {                
            for (int i = 0; i < N; i++)
            {
                sum += A[i, k] * A[i, k];
            }
            R[k, k] = Math.Sqrt(sum);
            sum = 0;
            for (int i = 0; i < N; i++)
                QT[i, k] = A[i, k] / R[k, k];

            for (int j = k; j < N; j++)
            {
                for (int i = 0; i < N; i++)
                {
                    sum += QT[i, k] * A[i, j];
                }
                R[k, j] = sum;
                sum = 0;
                for (int i = 0; i < N; i++)
                    A[i, j] = A[i, j] - QT[i, k] * R[k, j];
            }
        }
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                Q[i, j] = QT[i, j];
            }
        }
    }

    //перемножение матриц
    private double[,] MultiplyMatrix(double[,] R, double[,] Q)
    {
        var A = new double[N, N];
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                A[i, j] = 0;
                for (int k = 0; k < N; k++)
                {
                    A[i, j] += R[i, k] * Q[k, j];
                }
            }
        }
        return A;
    }

    //нормирование
    private double[] Rationing(double[] eigenvector)
    {
        var sum = 0.0;
        for (int i = 0; i < N; i++)
        {
            sum += eigenvector[i] * eigenvector[i];
        }
        for (int i = 0; i < N; i++)
        {
            eigenvector[i] = eigenvector[i] / Math.Sqrt(sum);
        }
        return eigenvector;
    }


    //ПОЛЯ КЛАССА
    //коэффициенты 
    public double[,] Matrix { get; set; }

    //массив свободных членов
    public double[] B { get; set; }

    //размерность матрицы
    public int N { get; set; }
}