// ===================== SINGLETON =====================
public sealed class Singleton
{
    private static readonly Singleton instance = new();
    private Singleton() { }
    public static Singleton Instance => instance;
    
    public void DoWork() => Console.WriteLine("Singleton working");
}

// Usage
Singleton.Instance.DoWork();

// ===================== FACTORY =====================
public interface IAnimal
{
    void MakeSound();
}

public class Dog : IAnimal
{
    public void MakeSound() => Console.WriteLine("Woof!");
}

public class Cat : IAnimal
{
    public void MakeSound() => Console.WriteLine("Meow!");
}

public class AnimalFactory
{
    public IAnimal CreateAnimal(string type) => type.ToLower() switch
    {
        "dog" => new Dog(),
        "cat" => new Cat(),
        _ => throw new ArgumentException("Invalid animal type")
    };
}

// Usage
var factory = new AnimalFactory();
var dog = factory.CreateAnimal("dog");
dog.MakeSound();

// ===================== ABSTRACT FACTORY =====================
public interface IButton { void Render(); }
public interface ITextBox { void Render(); }

public class WinButton : IButton
{
    public void Render() => Console.WriteLine("Rendering Windows button");
}

public class MacButton : IButton
{
    public void Render() => Console.WriteLine("Rendering Mac button");
}

public class WinTextBox : ITextBox
{
    public void Render() => Console.WriteLine("Rendering Windows textbox");
}

public class MacTextBox : ITextBox
{
    public void Render() => Console.WriteLine("Rendering Mac textbox");
}

public interface IGuiFactory
{
    IButton CreateButton();
    ITextBox CreateTextBox();
}

public class WinFactory : IGuiFactory
{
    public IButton CreateButton() => new WinButton();
    public ITextBox CreateTextBox() => new WinTextBox();
}

public class MacFactory : IGuiFactory
{
    public IButton CreateButton() => new MacButton();
    public ITextBox CreateTextBox() => new MacTextBox();
}

// Usage
IGuiFactory factory = new WinFactory();
var button = factory.CreateButton();
button.Render();

// ===================== STRATEGY =====================
public interface ISortStrategy
{
    void Sort(List<int> data);
}

public class QuickSort : ISortStrategy
{
    public void Sort(List<int> data)
    {
        Console.WriteLine("Quick sorting...");
        // Implementation here
    }
}

public class BubbleSort : ISortStrategy
{
    public void Sort(List<int> data)
    {
        Console.WriteLine("Bubble sorting...");
        // Implementation here
    }
}

public class Sorter
{
    private ISortStrategy _strategy;
    
    public void SetStrategy(ISortStrategy strategy)
    {
        _strategy = strategy;
    }
    
    public void Sort(List<int> data)
    {
        _strategy.Sort(data);
    }
}

// Usage
var sorter = new Sorter();
sorter.SetStrategy(new QuickSort());
sorter.Sort(new List<int> { 1, 5, 3, 2, 4 });

// ===================== OBSERVER =====================
public interface IObserver
{
    void Update(string message);
}

public class WeatherStation
{
    private readonly List<IObserver> _observers = new();
    private string _weatherUpdate;

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    
    public void SetWeather(string weather)
    {
        _weatherUpdate = weather;
        Notify();
    }
    
    private void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_weatherUpdate);
        }
    }
}

public class WeatherDisplay : IObserver
{
    public void Update(string message)
    {
        Console.WriteLine($"Weather Display: {message}");
    }
}

// Usage
var weatherStation = new WeatherStation();
var display = new WeatherDisplay();
weatherStation.Attach(display);
weatherStation.SetWeather("Sunny");

// ===================== BUILDER =====================
public class Computer
{
    public string CPU { get; set; }
    public string RAM { get; set; }
    public string Storage { get; set; }
}

public class ComputerBuilder
{
    private readonly Computer _computer = new();
    
    public ComputerBuilder AddCPU(string cpu)
    {
        _computer.CPU = cpu;
        return this;
    }
    
    public ComputerBuilder AddRAM(string ram)
    {
        _computer.RAM = ram;
        return this;
    }
    
    public ComputerBuilder AddStorage(string storage)
    {
        _computer.Storage = storage;
        return this;
    }
    
    public Computer Build() => _computer;
}

// Usage
var computer = new ComputerBuilder()
    .AddCPU("Intel i7")
    .AddRAM("16GB")
    .AddStorage("512GB SSD")
    .Build();

// ===================== PROTOTYPE =====================
public abstract class ShapePrototype
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Color { get; set; }
    
    public abstract ShapePrototype Clone();
}

public class Rectangle : ShapePrototype
{
    public int Width { get; set; }
    public int Height { get; set; }
    
    public override ShapePrototype Clone()
    {
        return new Rectangle
        {
            X = X,
            Y = Y,
            Color = Color,
            Width = Width,
            Height = Height
        };
    }
}

// Usage
var rect = new Rectangle { X = 10, Y = 20, Width = 100, Height = 200, Color = "Red" };
var clonedRect = rect.Clone() as Rectangle;

// ===================== DECORATOR =====================
public interface ICoffee
{
    string GetDescription();
    double GetCost();
}

public class SimpleCoffee : ICoffee
{
    public string GetDescription() => "Simple Coffee";
    public double GetCost() => 1.0;
}

public abstract class CoffeeDecorator : ICoffee
{
    protected readonly ICoffee _coffee;
    protected CoffeeDecorator(ICoffee coffee) => _coffee = coffee;
    public virtual string GetDescription() => _coffee.GetDescription();
    public virtual double GetCost() => _coffee.GetCost();
}

public class MilkDecorator : CoffeeDecorator
{
    public MilkDecorator(ICoffee coffee) : base(coffee) { }
    public override string GetDescription() => $"{_coffee.GetDescription()}, with Milk";
    public override double GetCost() => _coffee.GetCost() + 0.5;
}

// Usage
ICoffee coffee = new SimpleCoffee();
coffee = new MilkDecorator(coffee);
Console.WriteLine($"{coffee.GetDescription()} costs ${coffee.GetCost()}");

// ===================== COMMAND =====================
public interface ICommand
{
    void Execute();
}

public class Light
{
    public void TurnOn() => Console.WriteLine("Light is on");
    public void TurnOff() => Console.WriteLine("Light is off");
}

public class LightOnCommand : ICommand
{
    private readonly Light _light;
    public LightOnCommand(Light light) => _light = light;
    public void Execute() => _light.TurnOn();
}

public class RemoteControl
{
    private ICommand _command;
    public void SetCommand(ICommand command) => _command = command;
    public void PressButton() => _command.Execute();
}

// Usage
var light = new Light();
var lightOn = new LightOnCommand(light);
var remote = new RemoteControl();
remote.SetCommand(lightOn);
remote.PressButton();

// ===================== ADAPTER =====================
public interface ITarget
{
    void Request();
}

public class Adaptee
{
    public void SpecificRequest()
    {
        Console.WriteLine("Specific request");
    }
}

public class Adapter : ITarget
{
    private readonly Adaptee _adaptee;
    public Adapter(Adaptee adaptee) => _adaptee = adaptee;
    public void Request() => _adaptee.SpecificRequest();
}

// Usage
ITarget target = new Adapter(new Adaptee());
target.Request();

// ===================== FACADE =====================
public class SubSystemOne
{
    public void MethodOne() => Console.WriteLine("SubSystemOne Method");
}

public class SubSystemTwo
{
    public void MethodTwo() => Console.WriteLine("SubSystemTwo Method");
}

public class Facade
{
    private readonly SubSystemOne _one;
    private readonly SubSystemTwo _two;
    
    public Facade()
    {
        _one = new SubSystemOne();
        _two = new SubSystemTwo();
    }
    
    public void OperationWrapper()
    {
        _one.MethodOne();
        _two.MethodTwo();
    }
}

// Usage
var facade = new Facade();
facade.OperationWrapper();

// ===================== BRIDGE =====================
public interface IDrawAPI
{
    void DrawCircle(int radius, int x, int y);
}

public class RedCircle : IDrawAPI
{
    public void DrawCircle(int radius, int x, int y)
    {
        Console.WriteLine($"Drawing red circle of radius {radius}");
    }
}

public abstract class Shape
{
    protected IDrawAPI _drawAPI;
    protected Shape(IDrawAPI drawAPI) => _drawAPI = drawAPI;
    public abstract void Draw();
}

public class Circle : Shape
{
    private int _radius;
    private int _x;
    private int _y;
    
    public Circle(int radius, int x, int y, IDrawAPI drawAPI) : base(drawAPI)
    {
        _radius = radius;
        _x = x;
        _y = y;
    }
    
    public override void Draw()
    {
        _drawAPI.DrawCircle(_radius, _x, _y);
    }
}

// Usage
var redCircle = new Circle(100, 100, 100, new RedCircle());
redCircle.Draw();
