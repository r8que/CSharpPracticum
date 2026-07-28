using System;

public class Product
{
	public string name;
	private string manufacturer;
	private double price;
	private string expirationDate;
	private string productionDate;

	// Базовый конструктор, при котором информация отсутствует
    public Product()
    {
		getInformation();
    }


	// Конструктор для известного товара
	public Product(string name, string manufacturer, double price, string expirationDate, string productionDate)
    {
        getInformation(name, manufacturer, price, expirationDate, productionDate);
    }

    private void getInformation()
	{
        this.name = "Информация отсутствует";
		this.manufacturer = "Информация отсутствует";
		this.price = -1;
		this.expirationDate = "Информация отсутствует";
		this.productionDate = "Информация отсутствует";
	}

	private void getInformation(string nm, string mf, double pr, string eD, string pD)
	{
        this.name = nm;
        this.manufacturer = mf;
        this.price = pr;
        this.expirationDate = eD;
        this.productionDate = pD;
    }


	public override string ToString()
	{
		return $"Наименование: {this.name}\n" +
			   $"Производитель: {this.manufacturer}\n" +
			   $"Цена: {this.price}\n" +
			   $"Срок годности: {this.expirationDate} суток\n" +
			   $"Дата изготовления: {this.productionDate}\n";
	}
}
