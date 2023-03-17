namespace lab1_LU
{
    
    public class Program
    {   // Основная программа
        static void Main()
        {
            // Загрузка основной библиотеки с функциями
            Matrix matrix = new Matrix();

            // Загрузка файлов отвечающих за разные методы
            LU lu = new LU();
            Triagonal triagonal = new Triagonal();
            Sim_Zdl sim_zdl = new Sim_Zdl();
            Yacobi yacobi = new Yacobi();

            int size; // Размерность матрицы

            Console.WriteLine("Проверьте, что создали и заполнили файл Matrix.txt");
            Console.WriteLine("Нажмите Enter для продолжения");

            var pressed = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Введите размерность матрицы");

            try
            {   // Попытка конвертировать строку в int
                size = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Неверный формат размера");
                return;
            }

            if (size <= 1)
            {   // Проверка на размерность
                Console.WriteLine("Размерность слишком маленькая");
                return;
            }

            if (!File.Exists("Matrix.txt"))
            {   // Проверка на существовавние файла
                Console.WriteLine("Matrix.txt Not exist!");
                return;
            }

            // Графическая часть
            Console.WriteLine("Выберите необходимый алгоритм:");
            Console.WriteLine("1.1");
            Console.WriteLine("1.2");
            Console.WriteLine("1.3");
            Console.WriteLine("1.4");
            Console.WriteLine();
            pressed = Console.ReadLine();
            Console.WriteLine();
            
            switch (pressed)
            {   // Выбираем тип программы, передаём в неё размерность матрицы и ссылку на библиотеку Matrix
                case ("1.1"):
                    lu.Program(matrix, size);
                    break;
                case ("1.2"):
                    triagonal.Program(matrix, size);
                    break;
                case ("1.3"):
                    sim_zdl.Program(matrix, size);
                    break;
                case ("1.4"):
                    yacobi.Program(matrix, size);
                    break;
                default:
                    Console.WriteLine("Неверный выбор алгоритма");
                    return;
            }
        }
    }
}