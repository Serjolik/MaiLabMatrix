using System;
using System.Diagnostics;

public class Matrix
{
	private List<List<float>> matrix = new List<List<float>>();

	public Matrix(List<List<float>> matrix)
	{
		this.matrix = matrix;
	}


    /// <summary>
    /// Function returning a line in matrix by index
    /// </summary>
    /// <param name="index">line number</param>
    /// <returns>line in matrix</returns>
    public List<float> Line(int index)
	{	// returning needed lane
		return matrix[index];
	}


    /// <summary>
    /// Returned new line with multiplier
    /// </summary>
    /// <param name="line">multiplied line</param>
    /// <param name="multiplier">Multiplier</param>
    /// <returns>line * multiplier</returns>
    public List<float> LineMultiplication(List<float> line, float multiplier)
	{
		var newLine = new List<float>();

		foreach(var number in line)
		{
			newLine.Add(number * multiplier);
		}

		return (newLine);
    }


	/// <summary>
	/// Swap lines in matrix by index
	/// </summary>
	/// <param name="firstIndex">index of first line</param>
	/// <param name="secondIndex">index of second line</param>
	public void SwapLines(int firstIndex, int secondIndex)
	{	// Swap lines in our matrix
		var bufferLine = matrix[firstIndex];
		matrix[firstIndex] = matrix[secondIndex];
		matrix[secondIndex] = bufferLine;
	}


    /// <summary>
    /// Subtracts a line from selected line
    /// </summary>
    /// <param name="line">The line to be subtracted</param>
    /// <param name="lineNumber">The number of line from matrix from which we subtract</param>
    public void LineSubtraction(List<float> line, int lineNumber)
    {
		int index = 0;
		foreach (float element in matrix[lineNumber])
		{
			line[index] -= element;
			line[index] *= -1;
			index++;
		}
		matrix[lineNumber] = new List<float>(line);
    }


    /// <summary>
    /// Matrix output
    /// </summary>
    public void PrintResult()
	{
		foreach(List<float> line in matrix)
		{
			foreach(float element in line)
			{
				Console.WriteLine(element);
			}
			Console.WriteLine();
		}
	}


    /// <summary>
    /// Subtracts from a line another multiplied line
    /// </summary>
    /// <param name="lineNumber">Line number from which to subtract</param>
    /// <param name="baseLineNumber">Line number to subtract</param>
    /// <param name="multiplier">Subtracted line multiplier</param>
    public void SubstractFromMultiplied(int lineNumber, int baseLineNumber, float multiplier)
	{
		LineSubtraction(
			LineMultiplication(
				Line(baseLineNumber), multiplier), lineNumber);

    }


    /// <summary>
    /// Finding a multiplier to turn elements to the left of the main diagonal into 0
    /// </summary>
    /// <param name="lineNumber"></param>
    /// <returns></returns>
    public float MultiplierFinder(int lineNumber)
	{
		if (lineNumber <= 0)
		{
			Console.WriteLine("Error, line number < 1");
			Console.WriteLine("MultiplierFinder return 0");
			return 0;
		}

		return matrix[lineNumber][lineNumber - 1] / matrix[lineNumber - 1][lineNumber - 1];
	}

}
