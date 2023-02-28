namespace lab1_LU
{
    public class Program
    {
        static void Main()
        {
            Matrix matrix = new Matrix();
            int size = 1;

            Console.WriteLine("Проверьте, что создали и заполнили файл Matrix.txt");
            Console.WriteLine("Нажмите Enter для продолжения");

            var pressed = Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("Введите размерность матрицы");
            try
            {
                size = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Неверный формат размера");
                return;
            }

            if (!File.Exists("Matrix.txt"))
            {
                Console.WriteLine("Matrix.txt Not exist!");
                return;
            }
            string[] lines = File.ReadAllLines("Matrix.txt").Take(size).ToArray();

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
            matrix.LUP(mat, rightPart);
            Console.WriteLine("Программа завершена, проверьте файл Result");
        }
    }
}
