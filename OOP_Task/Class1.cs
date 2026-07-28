using System;

public class Product
{
	private struct productInformation
	{
		private string name = "elrkg";
		private string manufacturer = "elrkg";
		private double price = 1100;
		private string expirationDate = "elrkg";
		private string productionDate = "elrkg";
	}
	public Product()
	{
	}

	private override ToString()
	{
		Console.WriteLine($"{productInformation}");
	}
}
