using System;
class Program
{
    static void Main()
    {
        Matrix clMatrix;

        List<List<float>> matrix = new List<List<float>>()
        {
        new List<float> { 1, 0.5f, 0.5f },
        new List<float> { 0.5f, 1, 0.5f },
        new List<float> { 0.5f, 0.5f, 1 },
        };

        clMatrix = new Matrix(matrix);


        clMatrix.PrintResult();

        Console.WriteLine("--------------------");
        Console.WriteLine(clMatrix.MultiplierFinder(1));
        Console.WriteLine(clMatrix.MultiplierFinder(2));

        clMatrix.LineSubtraction(
            clMatrix.LineMultiplication(
                clMatrix.Line(0), 2), 1);

        clMatrix.SwapLines(1, 2);

        Console.WriteLine("--------------------");
        clMatrix.PrintResult();

        Console.ReadKey();
    }
}
