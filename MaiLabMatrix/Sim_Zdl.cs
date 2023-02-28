public class Sim_Zdl
{   // �������� ������������ 1.3
    public void Program(Matrix matrix, int size)
    {
        // �������� ���������� ��� �������
        double e = 0.01;

        // ���������� ������ �����
        string[] lines = File.ReadAllLines("Matrix.txt").Take(size).ToArray();

        // �������� ������ ��� ������� � ��� ������ �����
        double[,] mat = new double[size, size];
        double[] rightPart = new double[size];

        // ��� ������� ��������� (��� �������, ���� �� ���������)
        double[] x_n = new double[size];

        // ��������� � �������
        for (int i = 0; i < size; i++)
        {

            double[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Double.Parse).ToArray();
            for (int j = 0, n = 0; n < size + 1; n++, j++)
            {
                if (j == i)
                {   // ���� ������� ���������
                    x_n[i] = row[j];
                    mat[i, j] = 0;
                }

                else if (j == size)
                {   // ���� ��������� ������
                    rightPart[i] = row[j];
                }

                else
                {
                    mat[i, j] = row[j];
                }
            }
        }


        //�������� � �������������� ����
        for (int i = 0; i < size; i++)
        {
            for (int j = 0, n = 0; n < size; n++, j++)
            {
                mat[i, j] = -mat[i, j] / x_n[i];
            }
            rightPart[i] /= x_n[i];
        }

        // ��������� �������� MPI � �������� ZDL �� ����������
        matrix.Mpi(mat, rightPart, size, e);
        matrix.Zdl(mat, rightPart, size, e);

        Console.WriteLine("��������� ���������, ��������� ����� Result_1_3");
    }
}
