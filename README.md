# 🗄️ C# Interface Deep Dive: Decoupling with IPayment

This repository explores the concept and practical application of **Interfaces** in C#, focusing on achieving **loose coupling** through dependency inversion. The core example simulates a robust, extensible payment processing system.

ملاحظة: المحتوى مكتوب بالإنجليزية. أقدر أترجمه لك للعربية إن رغبت — فقط قل لي.

## 📝 Table of Contents

1.  [Introduction: What is an Interface?](#1-introduction-what-is-an-interface)
2.  [Abstract Vs. Concrete Type](#2-abstract-vs-concrete-type)
3.  [Implementation Techniques](#3-implementation-techniques)
    * [Implicit Interface Implementation](#implicit-interface-implementation)
    * [Explicit Interface Implementation](#explicit-interface-implementation)
4.  [Core Concept: Loose Coupling](#4-core-concept-loose-coupling)
    * [Tight Coupling Vs. Loose Coupling](#tight-coupling-vs-loose-coupling)
5.  [Real-World Example: Payment Processor](#5-real-world-example-payment-processor)
    * [Code Example](#code-example)
6.  [Advanced Topic: Multiple Interface Inheritance](#6-advanced-topic-multiple-interface-inheritance)

---

## 1. Introduction: What is an Interface?

An **interface** in C# defines a **contract**—a set of declarations (methods, properties, events, indexers) that implementing classes must fulfill.

* Interfaces **cannot** be instantiated directly (`new IPayment()` is invalid).
* They contain **declarations only**, providing no implementation (no method bodies).

### Interface Definition (`IPayment.cs`)

```csharp
// IPayment.cs
public interface IPayment
{
    // A contract: any class implementing this must have a Pay method
    void Pay(decimal amount);
}
```

-----

## 2. Abstract Vs. Concrete Type

Understanding these terms is crucial for working with interfaces and achieving polymorphism.

  * **Abstract Type (The Interface):** This is the interface itself (e.g., `IPayment`). It represents a **concept** or **role**. The `Cashier` class depends on this abstract type.
  * **Concrete Type (The Class):** This is the class that provides the actual logic (e.g., `Cash`, `MasterCard`). These are the classes that can be instantiated.

### Example in `Cashier` Class

```csharp
public class Cashier
{
    // Declaring a field using the ABSTRACT TYPE (IPayment)
    private readonly IPayment _payment;

    // Dependency Injection: the concrete type is passed in
    public Cashier(IPayment payment)
    {
        _payment = payment;
    }

    // The Cashier calls the contract method, regardless of the concrete type
    public void Checkout(decimal amount)
    {
        _payment.Pay(amount);
    }
}
```

-----

## 3. Implementation Techniques

### Implicit Interface Implementation

This is the standard and most common method. The interface member is implemented as a **public** method in the class.

  * **Characteristics:** The method is visible and callable directly on the concrete class instance.

<!-- end list -->

```csharp
public class Cash : IPayment
{
    // Implicit: Public method matching the interface signature
    public void Pay(decimal amount)
    {
        // Print as local currency rounded to whole units (example behaviour in README)
        Console.WriteLine($"Cash payment: {amount:C0}");
    }
}
```

### Explicit Interface Implementation

Used when you need to **hide** the interface member from the concrete class's public API, or to implement two interfaces that have methods with the same name (name collision).

  * **Characteristics:** The method must be prefixed with the interface name and **does not** use an access modifier (like `public`). It can *only* be called when the object is cast to the interface type.

<!-- end list -->

```csharp
public interface ILogger { void Log(); }
public interface ITransfer { void Log(); }

public class Service : ILogger, ITransfer
{
    // Explicitly implementing ILogger's Log()
    void ILogger.Log()
    {
        Console.WriteLine("Logging service activity...");
    }
    // Explicitly implementing ITransfer's Log()
    void ITransfer.Log()
    {
        Console.WriteLine("Logging transfer details...");
    }
}

// Usage:
// Service service = new Service();
// service.Log(); // ERROR: Explicitly implemented methods are hidden

// To call a specific interface implementation:
// ILogger logger = service;
// logger.Log(); // Calls the ILogger implementation
```

-----

## 4. Core Concept: Loose Coupling

Interfaces are the key mechanism for achieving **loose coupling**, a cornerstone of robust software architecture.

### Tight Coupling Vs. Loose Coupling

| Characteristic | Tight Coupling | Loose Coupling |
| :--- | :--- | :--- |
| **Dependency** | Class A depends on **Concrete Class B**. | Class A depends on **Abstract Interface IB**. |
| **Flexibility** | Rigid. Changes to B often break A. | Flexible. New implementations of IB can be swapped in without changing A. |
| **Testability** | Hard to test A in isolation (B must also be used). | Easy to test A by creating a fake ("Mock") implementation of IB. |
| **Example** | `Cashier` depends directly on `MasterCard`. | `Cashier` depends on `IPayment`. |

-----

## 5. Real-World Example: Payment Processor

The provided code demonstrates the **Strategy Pattern**, where the `Cashier` (the Context) delegates its action (`Checkout`) to a specific `IPayment` (the Strategy).

### The Interface and Implementations

```csharp
// The Contract (already shown earlier - implementations below are illustrative)
// Concrete Strategies
public class Cash : IPayment { /* ... implementation ... */ }
public class MasterCard : IPayment { /* ... implementation ... */ }
public class Visa : IPayment { /* ... implementation ... */ }
// ... (adding a new payment type is simple)
```

### Code Example

```csharp
// 1. Cashier uses the Cash strategy
var cashier = new Cashier(new Cash());
cashier.Checkout(99.999m);
// Example output (depending on current culture): Cash payment: $100

// 2. Cashier is easily reconfigured to use the MasterCard strategy
var cashier2 = new Cashier(new MasterCard());
cashier2.Checkout(2000.999m);
// Example output: MasterCard payment: $2,001
```

The `Cashier` class is completely **unaware** of whether it's processing Cash or MasterCard; it just follows the `IPayment` contract.

-----

## 6. Advanced Topic: Multiple Interface Inheritance

A core feature of C# is that a class can implement **multiple interfaces**. This is the mechanism C# uses to allow objects to have multiple roles or capabilities, as **class inheritance** is restricted to a single base class.

```csharp
interface ILoggable 
{
    void LogActivity();
}

interface ISearchable 
{
    List<string> Search(string query);
}

// A single DocumentProcessor object can fulfill both contracts
class DocumentProcessor : ILoggable, ISearchable 
{
    public void LogActivity() 
    { 
        /* ... logic to record the action ... */ 
    }
    
    public List<string> Search(string query) 
    { 
        /* ... logic to find content ... */ 
        return new List<string>();
    }
}
```

**Author:** Mahmoud Hany  
**Language:** C#  
**Topic:** Interface
