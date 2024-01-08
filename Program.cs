using System;
using System.Globalization;

class Program
{
    static void Main()
    {
        StartProgram();
        CurrencyEur wallet1 = new CurrencyEur(150);
        CurrencyUSD wallet2 = (CurrencyUSD)wallet1;
        CurrencyRub wallet3 = (CurrencyRub)wallet1;
        Console.WriteLine($"Wallet 1 (Eur): {wallet1.Value}");
        Console.WriteLine($"Wallet 2 (USD): {wallet2.Value}");
        Console.WriteLine($"Wallet 3 (Rub): {wallet3.Value}");
    }

    static void StartProgram()
    {
        Console.WriteLine("Введите курсы обмена (рубль, доллар, евро): ");
        string input = Console.ReadLine();
        string[] curs = input.Split(" ");

        if (curs.Length == 3 &&
            double.TryParse(curs[0], out double rubToEurRate) &&
            double.TryParse(curs[1], out double rubToUsdRate) &&
            double.TryParse(curs[2], out double eurToUsdRate))
        {
            Currency.EurToUsdRate = eurToUsdRate;
            Currency.RubToUsdRate = rubToUsdRate;
            Currency.RubToEurRate = rubToEurRate;
        }
        else
        {
            Console.WriteLine("Ошибка ввода. Пожалуйста, введите корректные значения для рубля, доллара и евро.");
            StartProgram(); // Повторный запрос, если ввод некорректен
        }
    }
}

class Currency
{
    protected double money;

    public Currency() { }

    public Currency(double money)
    {
        this.money = money;
    }

    public double Value
    {
        get { return money; }
        set { money = value; }
    }

    public static double EurToUsdRate { get; set; }
    public static double RubToUsdRate { get; set; }
    public static double RubToEurRate { get; set; }
}

class CurrencyUSD : Currency
{
    public CurrencyUSD(double money) : base(money) { }

    public static implicit operator CurrencyUSD(CurrencyEur eur)
    {
        return new CurrencyUSD(eur.Value * Currency.EurToUsdRate);
    }

    public static implicit operator CurrencyUSD(CurrencyRub rub)
    {
        return new CurrencyUSD(rub.Value * Currency.RubToUsdRate);
    }
}

class CurrencyEur : Currency
{
    public CurrencyEur(double money) : base(money) { }

    public static implicit operator CurrencyEur(CurrencyUSD usd)
    {
        return new CurrencyEur(usd.Value / Currency.EurToUsdRate);
    }

    public static implicit operator CurrencyEur(CurrencyRub rub)
    {
        return new CurrencyEur(rub.Value / Currency.RubToEurRate);
    }
}

class CurrencyRub : Currency
{
    public CurrencyRub(double money) : base(money) { }

    public static implicit operator CurrencyRub(CurrencyUSD usd)
    {
        return new CurrencyRub(usd.Value / Currency.RubToUsdRate);
    }

    public static implicit operator CurrencyRub(CurrencyEur eur)
    {
        return new CurrencyRub(eur.Value * Currency.RubToEurRate);
    }
}