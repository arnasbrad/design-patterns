// ===================== SINGLETON =====================
public sealed class Singleton
{
    private static Singleton instance = null;
    private static readonly object padlock = new object();

    private Singleton() { }

    public static Singleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}

// ===================== FACTORY METHOD =====================
public abstract class Creator
{
    public abstract IProduct FactoryMethod();
    
    public string SomeOperation()
    {
        var product = FactoryMethod();
        return "Creator: " + product.Operation();
    }
}

public class ConcreteCreatorA : Creator
{
    public override IProduct FactoryMethod()
    {
        return new ConcreteProductA();
    }
}

public class ConcreteCreatorB : Creator
{
    public override IProduct FactoryMethod()
    {
        return new ConcreteProductB();
    }
}

public interface IProduct
{
    string Operation();
}

public class ConcreteProductA : IProduct
{
    public string Operation()
    {
        return "Result of ConcreteProductA";
    }
}

public class ConcreteProductB : IProduct
{
    public string Operation()
    {
        return "Result of ConcreteProductB";
    }
}

// ===================== ABSTRACT FACTORY =====================
public interface IAbstractFactory
{
    IAbstractProductA CreateProductA();
    IAbstractProductB CreateProductB();
}

public interface IAbstractProductA
{
    string UsefulFunctionA();
}

public interface IAbstractProductB
{
    string UsefulFunctionB();
    string AnotherUsefulFunctionB(IAbstractProductA collaborator);
}

public class ConcreteFactory1 : IAbstractFactory
{
    public IAbstractProductA CreateProductA()
    {
        return new ConcreteProductA1();
    }

    public IAbstractProductB CreateProductB()
    {
        return new ConcreteProductB1();
    }
}

public class ConcreteFactory2 : IAbstractFactory
{
    public IAbstractProductA CreateProductA()
    {
        return new ConcreteProductA2();
    }

    public IAbstractProductB CreateProductB()
    {
        return new ConcreteProductB2();
    }
}

public class ConcreteProductA1 : IAbstractProductA
{
    public string UsefulFunctionA()
    {
        return "Product A1";
    }
}

public class ConcreteProductA2 : IAbstractProductA
{
    public string UsefulFunctionA()
    {
        return "Product A2";
    }
}

// ===================== STRATEGY =====================
public interface IStrategy
{
    void Algorithm();
}

public class ConcreteStrategyA : IStrategy
{
    public void Algorithm()
    {
        Console.WriteLine("ConcreteStrategyA algorithm");
    }
}

public class ConcreteStrategyB : IStrategy
{
    public void Algorithm()
    {
        Console.WriteLine("ConcreteStrategyB algorithm");
    }
}

public class Context
{
    private IStrategy _strategy;

    public Context(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public void DoSomeBusinessLogic()
    {
        _strategy.Algorithm();
    }
}

// ===================== OBSERVER =====================
public interface IObserver
{
    void Update(ISubject subject);
}

public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

public class Subject : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    public int State { get; set; }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }
}

public class ConcreteObserverA : IObserver
{
    public void Update(ISubject subject)
    {
        if (subject is Subject concreteSubject)
        {
            Console.WriteLine($"ConcreteObserverA reacted to state {concreteSubject.State}");
        }
    }
}

// ===================== BUILDER =====================
public interface IBuilder
{
    void BuildPartA();
    void BuildPartB();
    void BuildPartC();
}

public class ConcreteBuilder : IBuilder
{
    private Product _product = new Product();

    public ConcreteBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _product = new Product();
    }

    public void BuildPartA()
    {
        _product.Add("PartA");
    }

    public void BuildPartB()
    {
        _product.Add("PartB");
    }

    public void BuildPartC()
    {
        _product.Add("PartC");
    }

    public Product GetProduct()
    {
        Product result = _product;
        Reset();
        return result;
    }
}

public class Product
{
    private List<string> _parts = new List<string>();

    public void Add(string part)
    {
        _parts.Add(part);
    }
}

public class Director
{
    private IBuilder _builder;

    public Director(IBuilder builder)
    {
        _builder = builder;
    }

    public void BuildMinimalProduct()
    {
        _builder.BuildPartA();
    }

    public void BuildFullProduct()
    {
        _builder.BuildPartA();
        _builder.BuildPartB();
        _builder.BuildPartC();
    }
}

// ===================== PROTOTYPE =====================
public abstract class Prototype
{
    public int Id { get; set; }

    public Prototype(int id)
    {
        Id = id;
    }

    public abstract Prototype Clone();
}

public class ConcretePrototype1 : Prototype
{
    public ConcretePrototype1(int id) : base(id) { }

    public override Prototype Clone()
    {
        return new ConcretePrototype1(Id);
    }
}

public class ConcretePrototype2 : Prototype
{
    public ConcretePrototype2(int id) : base(id) { }

    public override Prototype Clone()
    {
        return new ConcretePrototype2(Id);
    }
}

// ===================== DECORATOR =====================
public abstract class Component
{
    public abstract string Operation();
}

public class ConcreteComponent : Component
{
    public override string Operation()
    {
        return "ConcreteComponent";
    }
}

public abstract class Decorator : Component
{
    protected Component _component;

    public Decorator(Component component)
    {
        _component = component;
    }

    public override string Operation()
    {
        return _component.Operation();
    }
}

public class ConcreteDecoratorA : Decorator
{
    public ConcreteDecoratorA(Component component) : base(component) { }

    public override string Operation()
    {
        return $"ConcreteDecoratorA({base.Operation()})";
    }
}

public class ConcreteDecoratorB : Decorator
{
    public ConcreteDecoratorB(Component component) : base(component) { }

    public override string Operation()
    {
        return $"ConcreteDecoratorB({base.Operation()})";
    }
}

// ===================== COMMAND =====================
public interface ICommand
{
    void Execute();
}

public class SimpleCommand : ICommand
{
    private string _payload;

    public SimpleCommand(string payload)
    {
        _payload = payload;
    }

    public void Execute()
    {
        Console.WriteLine($"SimpleCommand: {_payload}");
    }
}

public class ComplexCommand : ICommand
{
    private Receiver _receiver;
    private string _a;
    private string _b;

    public ComplexCommand(Receiver receiver, string a, string b)
    {
        _receiver = receiver;
        _a = a;
        _b = b;
    }

    public void Execute()
    {
        _receiver.DoSomething(_a);
        _receiver.DoSomethingElse(_b);
    }
}

public class Receiver
{
    public void DoSomething(string a)
    {
        Console.WriteLine($"Receiver: Working on ({a}.)");
    }

    public void DoSomethingElse(string b)
    {
        Console.WriteLine($"Receiver: Also working on ({b}.)");
    }
}

public class Invoker
{
    private ICommand _onStart;
    private ICommand _onFinish;

    public void SetOnStart(ICommand command)
    {
        _onStart = command;
    }

    public void SetOnFinish(ICommand command)
    {
        _onFinish = command;
    }

    public void DoSomethingImportant()
    {
        _onStart?.Execute();
        _onFinish?.Execute();
    }
}

// ===================== ADAPTER =====================
public interface ITarget
{
    string GetRequest();
}

public class Adaptee
{
    public string GetSpecificRequest()
    {
        return "Specific request.";
    }
}

public class Adapter : ITarget
{
    private readonly Adaptee _adaptee;

    public Adapter(Adaptee adaptee)
    {
        _adaptee = adaptee;
    }

    public string GetRequest()
    {
        return $"Adapter: {_adaptee.GetSpecificRequest()}";
    }
}

// ===================== FACADE =====================
public class Subsystem1
{
    public string Operation1()
    {
        return "Subsystem1: Ready!\n";
    }

    public string OperationN()
    {
        return "Subsystem1: Go!\n";
    }
}

public class Subsystem2
{
    public string Operation1()
    {
        return "Subsystem2: Get ready!\n";
    }

    public string OperationZ()
    {
        return "Subsystem2: Fire!\n";
    }
}

public class Facade
{
    protected Subsystem1 _subsystem1;
    protected Subsystem2 _subsystem2;

    public Facade(Subsystem1 subsystem1, Subsystem2 subsystem2)
    {
        _subsystem1 = subsystem1;
        _subsystem2 = subsystem2;
    }

    public string Operation()
    {
        string result = "Facade initializes subsystems:\n";
        result += _subsystem1.Operation1();
        result += _subsystem2.Operation1();
        result += "Facade orders subsystems to perform the action:\n";
        result += _subsystem1.OperationN();
        result += _subsystem2.OperationZ();
        return result;
    }
}

// ===================== BRIDGE =====================
public interface IImplementation
{
    string OperationImplementation();
}

public class ConcreteImplementationA : IImplementation
{
    public string OperationImplementation()
    {
        return "ConcreteImplementationA: Result";
    }
}

public class ConcreteImplementationB : IImplementation
{
    public string OperationImplementation()
    {
        return "ConcreteImplementationB: Result";
    }
}

public class Abstraction
{
    protected IImplementation _implementation;

    public Abstraction(IImplementation implementation)
    {
        _implementation = implementation;
    }

    public virtual string Operation()
    {
        return "Abstract: Base operation with:\n" + 
            _implementation.OperationImplementation();
    }
}

public class ExtendedAbstraction : Abstraction
{
    public ExtendedAbstraction(IImplementation implementation) : base(implementation) { }

    public override string Operation()
    {
        return "ExtendedAbstraction: Extended operation with:\n" +
            _implementation.OperationImplementation();
    }
}




////////////////////////////
////////////////////////////
////////////////////////////
////////////////////////////
////////////////////////////
////////////////////////////
////////////////////////////


// USAGES:



// ===================== SINGLETON =====================
// Usage
public class SingletonDemo
{
    public static void Demo()
    {
        Singleton s1 = Singleton.Instance;
        Singleton s2 = Singleton.Instance;
        
        Console.WriteLine($"Are instances the same? {s1 == s2}");
    }
}
// Output:
// Are instances the same? True

// ===================== FACTORY METHOD =====================
// Usage
public class FactoryMethodDemo
{
    public static void Demo()
    {
        Console.WriteLine("App: Launched with ConcreteCreatorA.");
        Creator creator = new ConcreteCreatorA();
        Console.WriteLine(creator.SomeOperation());

        Console.WriteLine("\nApp: Launched with ConcreteCreatorB.");
        creator = new ConcreteCreatorB();
        Console.WriteLine(creator.SomeOperation());
    }
}
// Output:
// App: Launched with ConcreteCreatorA.
// Creator: Result of ConcreteProductA
// 
// App: Launched with ConcreteCreatorB.
// Creator: Result of ConcreteProductB

// ===================== ABSTRACT FACTORY =====================
// Usage
public class AbstractFactoryDemo
{
    public static void Demo()
    {
        Console.WriteLine("Client: Testing client code with ConcreteFactory1");
        IAbstractFactory factory1 = new ConcreteFactory1();
        var productA1 = factory1.CreateProductA();
        var productB1 = factory1.CreateProductB();
        Console.WriteLine(productA1.UsefulFunctionA());
        
        Console.WriteLine("\nClient: Testing the same client code with ConcreteFactory2");
        IAbstractFactory factory2 = new ConcreteFactory2();
        var productA2 = factory2.CreateProductA();
        var productB2 = factory2.CreateProductB();
        Console.WriteLine(productA2.UsefulFunctionA());
    }
}
// Output:
// Client: Testing client code with ConcreteFactory1
// Product A1
//
// Client: Testing the same client code with ConcreteFactory2
// Product A2

// ===================== STRATEGY =====================
// Usage
public class StrategyDemo
{
    public static void Demo()
    {
        var context = new Context(new ConcreteStrategyA());
        Console.WriteLine("Client: Strategy is set to ConcreteStrategyA");
        context.DoSomeBusinessLogic();

        Console.WriteLine("\nClient: Strategy is set to ConcreteStrategyB");
        context.SetStrategy(new ConcreteStrategyB());
        context.DoSomeBusinessLogic();
    }
}
// Output:
// Client: Strategy is set to ConcreteStrategyA
// ConcreteStrategyA algorithm
//
// Client: Strategy is set to ConcreteStrategyB
// ConcreteStrategyB algorithm

// ===================== OBSERVER =====================
// Usage
public class ObserverDemo
{
    public static void Demo()
    {
        var subject = new Subject();
        var observerA = new ConcreteObserverA();
        subject.Attach(observerA);

        Console.WriteLine("Client: Changing subject state to 1");
        subject.State = 1;
        subject.Notify();

        Console.WriteLine("\nClient: Changing subject state to 2");
        subject.State = 2;
        subject.Notify();
    }
}
// Output:
// Client: Changing subject state to 1
// ConcreteObserverA reacted to state 1
//
// Client: Changing subject state to 2
// ConcreteObserverA reacted to state 2

// ===================== BUILDER =====================
// Usage
public class BuilderDemo
{
    public static void Demo()
    {
        var builder = new ConcreteBuilder();
        var director = new Director(builder);

        Console.WriteLine("Client: Building minimal product");
        director.BuildMinimalProduct();
        var product1 = builder.GetProduct();

        Console.WriteLine("\nClient: Building full product");
        director.BuildFullProduct();
        var product2 = builder.GetProduct();
    }
}
// Output:
// Client: Building minimal product
// Client: Building full product

// ===================== PROTOTYPE =====================
// Usage
public class PrototypeDemo
{
    public static void Demo()
    {
        var p1 = new ConcretePrototype1(1);
        var c1 = p1.Clone();
        
        Console.WriteLine($"Original object id: {p1.Id}");
        Console.WriteLine($"Cloned object id: {c1.Id}");
    }
}
// Output:
// Original object id: 1
// Cloned object id: 1

// ===================== DECORATOR =====================
// Usage
public class DecoratorDemo
{
    public static void Demo()
    {
        var simple = new ConcreteComponent();
        var decorator1 = new ConcreteDecoratorA(simple);
        var decorator2 = new ConcreteDecoratorB(decorator1);

        Console.WriteLine("Client: Simple component:");
        Console.WriteLine(simple.Operation());

        Console.WriteLine("\nClient: Decorated component:");
        Console.WriteLine(decorator2.Operation());
    }
}
// Output:
// Client: Simple component:
// ConcreteComponent
//
// Client: Decorated component:
// ConcreteDecoratorB(ConcreteDecoratorA(ConcreteComponent))

// ===================== COMMAND =====================
// Usage
public class CommandDemo
{
    public static void Demo()
    {
        var invoker = new Invoker();
        invoker.SetOnStart(new SimpleCommand("Say Hi!"));
        
        var receiver = new Receiver();
        invoker.SetOnFinish(new ComplexCommand(receiver, "Send email", "Save report"));

        Console.WriteLine("Client: Running command pattern");
        invoker.DoSomethingImportant();
    }
}
// Output:
// Client: Running command pattern
// SimpleCommand: Say Hi!
// Receiver: Working on (Send email)
// Receiver: Also working on (Save report)

// ===================== ADAPTER =====================
// Usage
public class AdapterDemo
{
    public static void Demo()
    {
        var adaptee = new Adaptee();
        ITarget target = new Adapter(adaptee);

        Console.WriteLine("Adaptee interface is incompatible with the client.");
        Console.WriteLine("But with adapter client can call its method:");
        Console.WriteLine(target.GetRequest());
    }
}
// Output:
// Adaptee interface is incompatible with the client.
// But with adapter client can call its method:
// Adapter: Specific request.

// ===================== FACADE =====================
// Usage
public class FacadeDemo
{
    public static void Demo()
    {
        var subsystem1 = new Subsystem1();
        var subsystem2 = new Subsystem2();
        var facade = new Facade(subsystem1, subsystem2);
        
        Console.WriteLine("Client: Using facade");
        Console.WriteLine(facade.Operation());
    }
}
// Output:
// Client: Using facade
// Facade initializes subsystems:
// Subsystem1: Ready!
// Subsystem2: Get ready!
// Facade orders subsystems to perform the action:
// Subsystem1: Go!
// Subsystem2: Fire!

// ===================== BRIDGE =====================
// Usage
public class BridgeDemo
{
    public static void Demo()
    {
        var implementation = new ConcreteImplementationA();
        var abstraction = new Abstraction(implementation);
        Console.WriteLine(abstraction.Operation());

        implementation = new ConcreteImplementationB();
        abstraction = new ExtendedAbstraction(implementation);
        Console.WriteLine("\n" + abstraction.Operation());
    }
}
// Output:
// Abstract: Base operation with:
// ConcreteImplementationA: Result
//
// ExtendedAbstraction: Extended operation with:
// ConcreteImplementationB: Result

// Main Program to run all demos
public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Singleton Pattern ===");
        SingletonDemo.Demo();
        Console.WriteLine("\n=== Factory Method Pattern ===");
        FactoryMethodDemo.Demo();
        Console.WriteLine("\n=== Abstract Factory Pattern ===");
        AbstractFactoryDemo.Demo();
        // ... continue with other patterns
    }
}
