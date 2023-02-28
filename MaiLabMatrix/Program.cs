namespace lab1_LU
{
    public class Program
    {
        static void Main()
        {
            Matrix matrix = new Matrix();
            LU lu = new LU();
            Triagonal triagonal = new Triagonal();
            Sim_Zdl sim_zdl = new Sim_Zdl();

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

            Console.WriteLine("Выберите необходимый алгоритм:");
            Console.WriteLine("1.1");
            Console.WriteLine("1.2");
            Console.WriteLine("1.3");
            Console.WriteLine();
            pressed = Console.ReadLine();
            Console.WriteLine();
            
            switch (pressed)
            {
                case ("1.1"):
                    lu.Program(matrix, size);
                    break;
                case ("1.2"):
                    triagonal.Program(matrix, size);
                    break;
                case ("1.3"):
                    sim_zdl.Program(matrix, size);
                    break;
                default:
                    Console.WriteLine("Неверный выбор алгоритма");
                    return;
            }
        }
    }
}
