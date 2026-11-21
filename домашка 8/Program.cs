//9.3
using System;
using System.Collections.Generic;
using System.IO;

class BankTransaction
{
    public readonly DateTime Time;
    public readonly decimal Amount;
    public BankTransaction(decimal amount) { Time = DateTime.Now; Amount = amount; }
    public override string ToString() => $"{Time} {Amount:+0;-0}";
}

class BankAccount : IDisposable
{
    private decimal balance;
    private Queue<BankTransaction> transactions = new Queue<BankTransaction>();

    public decimal Balance => balance;

    public void Deposit(decimal amount) { balance += amount; transactions.Enqueue(new BankTransaction(amount)); }
    public void Withdraw(decimal amount) { balance -= amount; transactions.Enqueue(new BankTransaction(-amount)); }

    public void Dispose()
    {
        using (StreamWriter sw = new StreamWriter("transactions.txt"))
            foreach (var t in transactions)
                sw.WriteLine(t);
        GC.SuppressFinalize(this);
    }
}

class Program
{
    static void Main()
    {
        using (var acc = new BankAccount())
        {
            acc.Deposit(500m);
            acc.Withdraw(200m);
            Console.WriteLine($"Баланс: {acc.Balance}");
        }
        Console.WriteLine("Проводки записаны в transactions.txt");
    }
}