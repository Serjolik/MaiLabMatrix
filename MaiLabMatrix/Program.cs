using System;
class Program
{
    static void Main()
    {
        Matrix clMatrix;

        List<List<float>> matrix = new List<List<float>>()
        {
        new List<float> { 1, 1, 1 },
        new List<float> { 2, 2, 2 },
        new List<float> { 3, 3, 3 },
        };

        clMatrix = new Matrix(matrix);

        clMatrix.SwapLines(1, 2);

        clMatrix.LineSubtraction(clMatrix.LineMultiplication(clMatrix.Line(0), 5), 1);

        clMatrix.PrintResult();

        Console.ReadKey();
    }
}
