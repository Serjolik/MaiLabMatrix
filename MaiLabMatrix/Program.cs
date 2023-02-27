using System;
class Program
{
    static void Main()
    {
        Matrix clMatrix;

        List<List<float>> matrix = new List<List<float>>()
        {
        new List<float> { 1, 2, 3, 4 },
        new List<float> { 5, 1, 3, 1 },
        new List<float> { 3, 7, 2, 6 },
        };

        clMatrix = new Matrix(matrix);


        clMatrix.PrintResult();
        /*
        Console.WriteLine("--------------------");
        Console.WriteLine(clMatrix.MultiplierFinder(1));
        Console.WriteLine(clMatrix.MultiplierFinder(2));

        clMatrix.LineSubtraction(
            clMatrix.LineMultiplication(
                clMatrix.Line(0), 2), 1);

        clMatrix.SwapLines(1, 2);

        Console.WriteLine("--------------------");
        clMatrix.PrintResult();

        clMatrix = new Matrix(matrix);

        clMatrix.PrintResult();
        */
        Console.WriteLine("--------------------");
        clMatrix.Gauss(3);
        clMatrix.PrintResult();

        Console.ReadKey();
    }
}
