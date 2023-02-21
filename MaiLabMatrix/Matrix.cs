using System;

public class Matrix
{
	private List<List<float>> matrix = new List<List<float>>();

	public Matrix(List<List<float>> matrix)
	{
		this.matrix = matrix;
	}

	public List<float> Line(int index)
	{	// returning needed lane
		return matrix[index];
	}

	public List<float> LineMultiplication(List<float> line, float multiplier)
	{	// Returned new line with multiplier
		var newLine = new List<float>();

		foreach(var number in line)
		{
			newLine.Add(number * multiplier);
		}

		return (newLine);
    }

	public void SwapLines(int firstIndex, int secondIndex)
	{	// Swap lines in our matrix
		var bufferLine = matrix[firstIndex];
		matrix[firstIndex] = matrix[secondIndex];
		matrix[secondIndex] = bufferLine;
	}

	public void LineSubtraction(List<float> line, int lineNumber)
    {   // Subtracts a line from the selected line
		int index = 0;
		foreach (float element in matrix[lineNumber])
		{
			line[index] -= element;
			line[index] *= -1;
			index++;
		}
		matrix[lineNumber] = new List<float>(line);
    }

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

}
