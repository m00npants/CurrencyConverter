using System;

public class Money
{
    public decimal Amount { get; private set; }
    public Currency CurrencyType { get; private set; }

    public Money(decimal amount, Currency currencyType)
    {
        Amount = amount;
        CurrencyType = currencyType;
    }

    public void ConvertCurrency(Currency newCurrency, CurrencyConverter converter)
    {
        if (CurrencyType == newCurrency)
            return;

        Amount = converter.Convert(Amount, CurrencyType, newCurrency);
        CurrencyType = newCurrency;
    }
}

public class CurrencyConverter
{
    public decimal DollarRate { get; }
    public decimal EuroRate { get; }
    public decimal SEKRate { get; }

    public CurrencyConverter(decimal dollarRate, decimal euroRate, decimal sekRate)
    {
        DollarRate = dollarRate;
        EuroRate = euroRate;
        SEKRate = sekRate;
    }

    public decimal Convert(decimal amount, Currency fromCurrency, Currency toCurrency)
    {
        if (fromCurrency == toCurrency)
            return amount;

        decimal baseAmount = amount;

        // Convert from currency to SEK as the base currency
        switch (fromCurrency)
        {
            case Currency.Dollar:
                baseAmount = amount / DollarRate;
                break;
            case Currency.Euro:
                baseAmount = amount / EuroRate;
                break;
            case Currency.SEK:
                baseAmount = amount;
                break;
        }

        // Convert from SEK to currency
        switch (toCurrency)
        {
            case Currency.Dollar:
                return baseAmount * DollarRate;
            case Currency.Euro:
                return baseAmount * EuroRate;
            case Currency.SEK:
                return baseAmount;
            default:
                throw new InvalidOperationException("Unsupported currency.");
        }
    }
}

public class Program
{
    public static void Main()
    {
        // Currency rates
        CurrencyConverter converter = new CurrencyConverter(0.095m, 0.085m, 1.0m);

        Console.WriteLine("Welcome to the m00npants money Converter!");

        bool continueConverting = true;
        while (continueConverting)
        {
            // User inputs the amount
            Console.Write("Enter the amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            // User selects what currency he has
            Console.WriteLine("Select your currency:");
            Currency fromCurrency = GetCurrencyFromUserInput();

            // User selects what currency he wants
            Console.WriteLine("Select what currency you want:");
            Currency toCurrency = GetCurrencyFromUserInput();

            // makes an money objekt and converts it
            Money money = new Money(amount, fromCurrency);
            money.ConvertCurrency(toCurrency, converter);

            //  converted result
            Console.WriteLine($"Converted amount: {money.Amount} {money.CurrencyType}");

            // Ask if the user wants to continue or not 
            Console.WriteLine("Do you want another conversion? (yes/no)");
            string userResponse = Console.ReadLine().Trim().ToLower();

            if (userResponse != "yes" && userResponse != "y")
            {
                continueConverting = false;
                Console.WriteLine("Thank you for using the m00npants money Converter. Goodbye!");
            }
        }
    }

    private static Currency GetCurrencyFromUserInput()
    {
        while (true)
        {
            Console.WriteLine("1. Dollar");
            Console.WriteLine("2. Euro");
            Console.WriteLine("3. SEK");
            Console.Write("Enter your choice (1, 2, or 3): ");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    return Currency.Dollar;
                case "2":
                    return Currency.Euro;
                case "3":
                    return Currency.SEK;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
