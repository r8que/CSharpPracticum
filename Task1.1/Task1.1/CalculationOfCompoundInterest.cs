using System;

public class CalculationOfCompoundInterest
{
	public CalculationOfCompoundInterest()
	{

	}
    // Переменные имеют стартовое значение -1, чтобы в случае ошибки ввода на них остались эти значения
    double initial_deposit = -1;
    int years = -1;
    double interest_years = -1;

    bool initializationData()
    {
        Console.WriteLine("Введите начальный депозит, количество лет и процентную ставку, нажимая enter после каждого числа");
        initial_deposit = Convert.ToDouble(Console.ReadLine());
        years = Convert.ToInt32(Console.ReadLine());
        interest_years = Convert.ToDouble(Console.ReadLine());

        // Проверка на правильность введенных данных
        if ((initial_deposit <= 0) || (interest_years <= 0) || (years <= 0))
        {
            Console.WriteLine("Init Error");
            return false;
        }
        else
        {
            Console.WriteLine("Init Complete");
            return true;
        }
    }
    void calculationOfCompoundInterest()
    {
        for (int i = 0; i < years; i++)
        {
            Console.WriteLine($"Год {1 + i} : {initial_deposit += initial_deposit * interest_years / 100}");
        }
    }
    public void start()
    {
        // Если данные введены верно, то метод вернет значение true
        if (initializationData())
        {
            calculationOfCompoundInterest();
            return;
        }
        else
            return;
    }
}
