using System.Drawing;

public class LU
{   // �������� ������������ 1.1
    public void Program(Matrix matrix, int size)
    {
        // ���������� ������ �����
        string[] lines = File.ReadAllLines("Matrix.txt").Take(size).ToArray();

        // ���������� ��������� ������� � ������ ������ �����
        double[,] mat = new double[size, size];
        double[] rightPart = new double[size];

        // ��������� � �������
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

        // ��������� �������� �� ���������� matrix
        matrix.LUP(mat, rightPart);

        Console.WriteLine("��������� ���������, ��������� ���� Result_1_1");
    }
}