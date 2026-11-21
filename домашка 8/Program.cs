//1
using System;

class BankAccount
{
    private string accountNumber;
    private decimal balance;
    private string accountType;

    public string AccountNumber => accountNumber;
    public decimal Balance => balance;
    public string AccountType => accountType;

    public BankAccount() : this(0, "обычный") { }
    public BankAccount(decimal balance) : this(balance, "обычный") { }
    public BankAccount(string accountType) : this(0, accountType) { }

    public BankAccount(decimal balance, string accountType)
    {
        GenerateNumber();
        this.balance = balance;
        this.accountType = accountType;
    }

    private void GenerateNumber() => accountNumber = Guid.NewGuid().ToString();
}

class Program
{
    static void Main()
    {
        var a1 = new BankAccount();
        var a2 = new BankAccount(1000m);
        var a3 = new BankAccount("сберегательный");
        var a4 = new BankAccount(500m, "кредитный");

        Console.WriteLine($"{a1.AccountNumber} {a1.Balance} {a1.AccountType}");
        Console.WriteLine($"{a2.AccountNumber} {a2.Balance} {a2.AccountType}");
        Console.WriteLine($"{a3.AccountNumber} {a3.Balance} {a3.AccountType}");
        Console.WriteLine($"{a4.AccountNumber} {a4.Balance} {a4.AccountType}");
    }
}