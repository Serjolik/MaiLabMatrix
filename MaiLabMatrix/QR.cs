using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

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
        } while (dif > 0.001);

        Console.WriteLine("Матрица");
        foreach (var num in A)
        {
            Console.WriteLine(num);
        }
        Console.WriteLine("--------");

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
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = i + 1; j < 3; j++)
            {
                if (Math.Abs(A[j, i]) > 0.0001)
                {
                    complex(A, i);
                }
            }
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

    private void complex(double[,] MatrA, int i)
    {
        Complex Lamd_i = new Complex();
        double a = 1;
        double b = -1 * (MatrA[i + 1, i + 1] + MatrA[i,i]);
        double c = MatrA[i,i] * MatrA[i + 1,i + 1] - MatrA[i,i + 1] * MatrA[i + 1,i];
        double desc = b * b - 4 * a * c;
        Lamd_i.setRe(-1 * b / (2 * a));
        Lamd_i.setIm(Math.Sqrt(Math.Abs(desc)) / (2 * a));
        Console.WriteLine("--------");
        Console.WriteLine("Собственные значения матрицы");
        for (int j = 1; j < 3; j++)
        {
            if (j != i && j != i + 1)
            {
                Console.WriteLine("lambda" + j + " = " + MatrA[i,i]);
            }
        }
        Console.WriteLine("lambda" + i + " = " + Lamd_i.getRe() + " + " + Lamd_i.getIm() + "i");
        Console.WriteLine("lambda" + (i + 1) + " = " + Lamd_i.getRe() + " - " + Lamd_i.getIm() + "i");
    }

    public class Complex
    {
        public void setRe(double re)
        {
            this.re = re;
        }

        public void setIm(double im)
        {
            this.im = im;
        }

        public double getRe()
        {
            return re;
        }

        public double getIm()
        {
            return im;
        }

        private double re;
        private double im;




    }


    //ПОЛЯ КЛАССА
    //коэффициенты 
    public double[,] Matrix { get; set; }

    //массив свободных членов
    public double[] B { get; set; }

    //размерность матрицы
    public int N { get; set; }
}