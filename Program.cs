using System;
using System.Collections.Generic;

namespace ReadmeExamples
{
    // --- Payment contract and implementations ---
    public interface IPayment
    {
        void Pay(decimal amount);
    }

    public class Cash : IPayment
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Cash payment: {amount:C0}");
        }
    }

    public class Debit : IPayment
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Debit payment: {amount:C0}");
        }
    }

    public class MasterCard : IPayment
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"MasterCard payment: {amount:C0}");
        }
    }

    public class Visa : IPayment
    {
        public void Pay(decimal amount)
        {
            Console.WriteLine($"Visa payment: {amount:C0}");
        }
    }

    // Consumer depends on the abstraction (IPayment)
    public class Cashier
    {
        private readonly IPayment _payment;

        public Cashier(IPayment payment)
        {
            _payment = payment;
        }

        public void Checkout(decimal amount)
        {
            _payment.Pay(amount);
        }
    }

    // --- Small Vehicle example (abstract vs concrete, multiple interfaces) ---
    public abstract class Vehicle
    {
        protected string Brand;
        protected string Model;
        protected int Year;

        public Vehicle(string brand, string model, int year)
        {
            Brand = brand;
            Model = model;
            Year = year;
        }
    }

    public interface ILoader
    {
        void Load();
        void Unload();
    }

    public interface IDrivable
    {
        void Move();
        void Stop();
    }

    public class Honda : Vehicle
    {
        public Honda(string brand, string model, int year) : base(brand, model, year)
        {
        }
    }

    public class Caterpillar : Vehicle, ILoader, IDrivable
    {
        public Caterpillar(string brand, string model, int year) : base(brand, model, year)
        {
        }

        public void Load()
        {
            Console.WriteLine("Loading");
        }

        public void Move()
        {
            Console.WriteLine("Moving");
        }

        public void Stop()
        {
            Console.WriteLine("Stopping");
        }

        public void Unload()
        {
            Console.WriteLine("Unloading");
        }
    }
}

// --- Program entrypoint (uses the ReadmeExamples namespace) ---
class Program
{
    static void Main(string[] args)
    {
        // Demo: wire different IPayment implementations into Cashier
        var cashier = new ReadmeExamples.Cashier(new ReadmeExamples.Cash());
        cashier.Checkout(99.999m);

        var cashier2 = new ReadmeExamples.Cashier(new ReadmeExamples.MasterCard());
        cashier2.Checkout(2000.999m);

        // Small demo of Caterpillar
        var cat = new ReadmeExamples.Caterpillar("CAT", "D8", 2020);
        cat.Load();
        cat.Move();
        cat.Stop();
        cat.Unload();
    }
}


