//1
using System;
using System.Collections.Generic;

class BankTransaction
{
    public readonly DateTime Time;
    public readonly decimal Amount;

    public BankTransaction(decimal amount)
    {
        Time = DateTime.Now;
        Amount = amount;
    }

    public override string ToString() => $"{Time:HH:mm:ss} {Amount:+0;-0}";
}

class BankAccount
{
    private string accountNumber;
    private decimal balance;
    private string accountType;
    private Queue<BankTransaction> transactions = new Queue<BankTransaction>();

    public string AccountNumber => accountNumber;
    public decimal Balance => balance;
    public string AccountType => accountType;
    public int TransactionCount => transactions.Count;

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

    public void Deposit(decimal amount)
    {
        balance += amount;
        transactions.Enqueue(new BankTransaction(amount));
    }

    public void Withdraw(decimal amount)
    {
        balance -= amount;
        transactions.Enqueue(new BankTransaction(-amount));
    }

    public void PrintTransactions()
    {
        foreach (var t in transactions)
            Console.WriteLine(t);
    }
}

class Program
{
    static void Main()
    {
        var acc = new BankAccount(1000m, "сберегательный");
        acc.Deposit(500m);
        acc.Withdraw(200m);
        acc.Deposit(50m);

        Console.WriteLine($"Счет: {acc.AccountNumber}");
        Console.WriteLine($"Тип: {acc.AccountType}");
        Console.WriteLine($"Баланс: {acc.Balance}");
        Console.WriteLine($"Операций: {acc.TransactionCount}");
        Console.WriteLine("Проводки:");
        acc.PrintTransactions();
    }
}