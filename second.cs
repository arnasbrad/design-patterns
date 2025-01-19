// ===================== TEMPLATE METHOD =====================
public abstract class AbstractClass
{
    public void TemplateMethod()
    {
        BaseOperation1();
        RequiredOperation1();
        BaseOperation2();
        Hook1();
        RequiredOperation2();
        BaseOperation3();
        Hook2();
    }

    protected void BaseOperation1()
    {
        Console.WriteLine("AbstractClass: BaseOperation1");
    }

    protected void BaseOperation2()
    {
        Console.WriteLine("AbstractClass: BaseOperation2");
    }

    protected void BaseOperation3()
    {
        Console.WriteLine("AbstractClass: BaseOperation3");
    }

    protected abstract void RequiredOperation1();
    protected abstract void RequiredOperation2();

    protected virtual void Hook1() { }
    protected virtual void Hook2() { }
}

public class ConcreteClass : AbstractClass
{
    protected override void RequiredOperation1()
    {
        Console.WriteLine("ConcreteClass: RequiredOperation1");
    }

    protected override void RequiredOperation2()
    {
        Console.WriteLine("ConcreteClass: RequiredOperation2");
    }

    protected override void Hook1()
    {
        Console.WriteLine("ConcreteClass: Hook1 override");
    }
}

// ===================== ITERATOR =====================
public interface IIterator<T>
{
    bool HasNext();
    T Next();
    T Current();
}

public interface ICollection<T>
{
    IIterator<T> CreateIterator();
}

public class ConcreteCollection<T> : ICollection<T>
{
    private List<T> _items = new List<T>();

    public void AddItem(T item)
    {
        _items.Add(item);
    }

    public IIterator<T> CreateIterator()
    {
        return new ConcreteIterator<T>(this);
    }

    public int Count
    {
        get { return _items.Count; }
    }

    public T this[int index]
    {
        get { return _items[index]; }
    }
}

public class ConcreteIterator<T> : IIterator<T>
{
    private ConcreteCollection<T> _collection;
    private int _current = 0;

    public ConcreteIterator(ConcreteCollection<T> collection)
    {
        _collection = collection;
    }

    public bool HasNext()
    {
        return _current < _collection.Count;
    }

    public T Next()
    {
        T item = _collection[_current];
        _current++;
        return item;
    }

    public T Current()
    {
        return _collection[_current];
    }
}

// ===================== COMPOSITE =====================
public abstract class Component
{
    public string Name { get; set; }

    public Component(string name)
    {
        Name = name;
    }

    public abstract void Add(Component component);
    public abstract void Remove(Component component);
    public abstract void Display(int depth);
}

public class Composite : Component
{
    private List<Component> _children = new List<Component>();

    public Composite(string name) : base(name) { }

    public override void Add(Component component)
    {
        _children.Add(component);
    }

    public override void Remove(Component component)
    {
        _children.Remove(component);
    }

    public override void Display(int depth)
    {
        Console.WriteLine(new string('-', depth) + Name);
        foreach (Component component in _children)
        {
            component.Display(depth + 2);
        }
    }
}

public class Leaf : Component
{
    public Leaf(string name) : base(name) { }

    public override void Add(Component component)
    {
        Console.WriteLine("Cannot add to a leaf");
    }

    public override void Remove(Component component)
    {
        Console.WriteLine("Cannot remove from a leaf");
    }

    public override void Display(int depth)
    {
        Console.WriteLine(new string('-', depth) + Name);
    }
}

// ===================== FLYWEIGHT =====================
public class Flyweight
{
    private string _sharedState;

    public Flyweight(string sharedState)
    {
        _sharedState = sharedState;
    }

    public void Operation(string uniqueState)
    {
        Console.WriteLine($"Flyweight: Shared state: {_sharedState}, Unique state: {uniqueState}");
    }
}

public class FlyweightFactory
{
    private Dictionary<string, Flyweight> _flyweights = new Dictionary<string, Flyweight>();

    public FlyweightFactory(params string[] args)
    {
        foreach (var state in args)
        {
            _flyweights.Add(state, new Flyweight(state));
        }
    }

    public Flyweight GetFlyweight(string key)
    {
        if (!_flyweights.ContainsKey(key))
        {
            Console.WriteLine("FlyweightFactory: Creating new flyweight.");
            _flyweights.Add(key, new Flyweight(key));
        }
        else
        {
            Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
        }
        return _flyweights[key];
    }
}

// ===================== STATE =====================
public interface IState
{
    void Handle(Context context);
}

public class Context
{
    private IState _state;

    public Context(IState state)
    {
        TransitionTo(state);
    }

    public void TransitionTo(IState state)
    {
        Console.WriteLine($"Context: Transition to {state.GetType().Name}");
        _state = state;
    }

    public void Request()
    {
        _state.Handle(this);
    }
}

public class ConcreteStateA : IState
{
    public void Handle(Context context)
    {
        Console.WriteLine("ConcreteStateA handles request.");
        context.TransitionTo(new ConcreteStateB());
    }
}

public class ConcreteStateB : IState
{
    public void Handle(Context context)
    {
        Console.WriteLine("ConcreteStateB handles request.");
        context.TransitionTo(new ConcreteStateA());
    }
}

// ===================== PROXY =====================
public interface ISubject
{
    void Request();
}

public class RealSubject : ISubject
{
    public void Request()
    {
        Console.WriteLine("RealSubject: Handling request.");
    }
}

public class Proxy : ISubject
{
    private RealSubject _realSubject;

    public void Request()
    {
        if (_realSubject == null)
        {
            Console.WriteLine("Proxy: Creating RealSubject.");
            _realSubject = new RealSubject();
        }
        Console.WriteLine("Proxy: Checking access prior to firing a real request.");
        _realSubject.Request();
    }
}

// ===================== CHAIN OF RESPONSIBILITY =====================
public interface IHandler
{
    IHandler SetNext(IHandler handler);
    object Handle(object request);
}

public abstract class AbstractHandler : IHandler
{
    private IHandler _nextHandler;

    public IHandler SetNext(IHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }

    public virtual object Handle(object request)
    {
        if (_nextHandler != null)
        {
            return _nextHandler.Handle(request);
        }
        return null;
    }
}

public class MonkeyHandler : AbstractHandler
{
    public override object Handle(object request)
    {
        if ((string)request == "Banana")
        {
            return $"Monkey: I'll eat the {request}";
        }
        return base.Handle(request);
    }
}

public class SquirrelHandler : AbstractHandler
{
    public override object Handle(object request)
    {
        if ((string)request == "Nut")
        {
            return $"Squirrel: I'll eat the {request}";
        }
        return base.Handle(request);
    }
}

// ===================== INTERPRETER =====================
public interface IExpression
{
    int Interpret(Dictionary<string, int> context);
}

public class NumberExpression : IExpression
{
    private int _number;

    public NumberExpression(int number)
    {
        _number = number;
    }

    public int Interpret(Dictionary<string, int> context)
    {
        return _number;
    }
}

public class AddExpression : IExpression
{
    private IExpression _left;
    private IExpression _right;

    public AddExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public int Interpret(Dictionary<string, int> context)
    {
        return _left.Interpret(context) + _right.Interpret(context);
    }
}

// ===================== MEDIATOR =====================
public interface IMediator
{
    void Notify(object sender, string ev);
}

public class ConcreteMediator : IMediator
{
    private Component1 _component1;
    private Component2 _component2;

    public ConcreteMediator(Component1 component1, Component2 component2)
    {
        _component1 = component1;
        _component1.SetMediator(this);
        _component2 = component2;
        _component2.SetMediator(this);
    }

    public void Notify(object sender, string ev)
    {
        if (ev == "A")
        {
            Console.WriteLine("Mediator reacts on A and triggers following operations:");
            _component2.DoC();
        }
        if (ev == "D")
        {
            Console.WriteLine("Mediator reacts on D and triggers following operations:");
            _component1.DoB();
            _component2.DoC();
        }
    }
}

public class BaseComponent
{
    protected IMediator _mediator;

    public BaseComponent(IMediator mediator = null)
    {
        _mediator = mediator;
    }

    public void SetMediator(IMediator mediator)
    {
        _mediator = mediator;
    }
}

public class Component1 : BaseComponent
{
    public void DoA()
    {
        Console.WriteLine("Component 1 does A.");
        _mediator.Notify(this, "A");
    }

    public void DoB()
    {
        Console.WriteLine("Component 1 does B.");
        _mediator.Notify(this, "B");
    }
}

public class Component2 : BaseComponent
{
    public void DoC()
    {
        Console.WriteLine("Component 2 does C.");
        _mediator.Notify(this, "C");
    }

    public void DoD()
    {
        Console.WriteLine("Component 2 does D.");
        _mediator.Notify(this, "D");
    }
}

// ===================== MEMENTO =====================
public class Memento
{
    private string _state;

    public Memento(string state)
    {
        _state = state;
    }

    public string GetState()
    {
        return _state;
    }
}

public class Originator
{
    private string _state;

    public void SetState(string state)
    {
        Console.WriteLine($"Originator: Setting state to {state}");
        _state = state;
    }

    public Memento SaveState()
    {
        Console.WriteLine("Originator: Saving to Memento.");
        return new Memento(_state);
    }

    public void RestoreState(Memento memento)
    {
        _state = memento.GetState();
        Console.WriteLine($"Originator: State after restoring: {_state}");
    }
}

public class Caretaker
{
    private List<Memento> _mementos = new List<Memento>();
    private Originator _originator;

    public Caretaker(Originator originator)
    {
        _originator = originator;
    }

    public void Backup()
    {
        Console.WriteLine("Caretaker: Saving Originator's state...");
        _mementos.Add(_originator.SaveState());
    }

    public void Undo()
    {
        if (_mementos.Count == 0)
        {
            return;
        }

        var memento = _mementos.Last();
        _mementos.Remove(memento);

        Console.WriteLine("Caretaker: Restoring state...");
        _originator.RestoreState(memento);
    }
}

// ===================== VISITOR =====================
public interface IComponent
{
    void Accept(IVisitor visitor);
}

public class ConcreteComponentA : IComponent
{
    public void Accept(IVisitor visitor)
    {
        visitor.VisitConcreteComponentA(this);
    }

    public string ExclusiveMethodOfConcreteComponentA()
    {
        return "A";
    }
}

public class ConcreteComponentB : IComponent
{
    public void Accept(IVisitor visitor)
    {
        visitor.VisitConcreteComponentB(this);
    }

    public string SpecialMethodOfConcreteComponentB()
    {
        return "B";
    }
}

public interface IVisitor
{
    void VisitConcreteComponentA(ConcreteComponentA element);
    void VisitConcreteComponentB(ConcreteComponentB element);
}

public class ConcreteVisitor1 : IVisitor
{
    public void VisitConcreteComponentA(ConcreteComponentA element)
    {
        Console.WriteLine($"Visitor1: {element.ExclusiveMethodOfConcreteComponentA()}");
    }

    public void VisitConcreteComponentB(ConcreteComponentB element)
    {
        Console.WriteLine($"Visitor1: {element.SpecialMethodOfConcreteComponentB()}");
    }
}

public class ConcreteVisitor2 : IVisitor
{
    public void VisitConcreteComponentA(ConcreteComponentA element)
    {
        Console.WriteLine($"Visitor2: {element.ExclusiveMethodOfConcreteComponentA()}");
    }

    public void VisitConcreteComponentB(ConcreteComponentB element)
    {
        Console.WriteLine($"Visitor2: {element.SpecialMethodOfConcreteComponentB()}");
    }
}

// USAGES:

// ===================== TEMPLATE METHOD =====================
public class TemplateMethodDemo
{
    public static void Demo()
    {
        Console.WriteLine("Template Method Pattern Demo:");
        AbstractClass concreteClass = new ConcreteClass();
        concreteClass.TemplateMethod();
    }
}
// Output:
// Template Method Pattern Demo:
// AbstractClass: BaseOperation1
// ConcreteClass: RequiredOperation1
// AbstractClass: BaseOperation2
// ConcreteClass: Hook1 override
// ConcreteClass: RequiredOperation2
// AbstractClass: BaseOperation3

// ===================== TEMPLATE METHOD =====================
public abstract class AbstractClass
{
    public void TemplateMethod()
    {
        BaseOperation1();
        RequiredOperation1();
        BaseOperation2();
        Hook1();
        RequiredOperation2();
        BaseOperation3();
        Hook2();
    }

    protected void BaseOperation1()
    {
        Console.WriteLine("AbstractClass: BaseOperation1");
    }

    protected void BaseOperation2()
    {
        Console.WriteLine("AbstractClass: BaseOperation2");
    }

    protected void BaseOperation3()
    {
        Console.WriteLine("AbstractClass: BaseOperation3");
    }

    protected abstract void RequiredOperation1();
    protected abstract void RequiredOperation2();

    protected virtual void Hook1() { }
    protected virtual void Hook2() { }
}

public class ConcreteClass : AbstractClass
{
    protected override void RequiredOperation1()
    {
        Console.WriteLine("ConcreteClass: RequiredOperation1");
    }

    protected override void RequiredOperation2()
    {
        Console.WriteLine("ConcreteClass: RequiredOperation2");
    }

    protected override void Hook1()
    {
        Console.WriteLine("ConcreteClass: Hook1 override");
    }
}

// ===================== ITERATOR =====================
public interface IIterator<T>
{
    bool HasNext();
    T Next();
    T Current();
}

public interface ICollection<T>
{
    IIterator<T> CreateIterator();
}

public class ConcreteCollection<T> : ICollection<T>
{
    private List<T> _items = new List<T>();

    public void AddItem(T item)
    {
        _items.Add(item);
    }

    public IIterator<T> CreateIterator()
    {
        return new ConcreteIterator<T>(this);
    }

    public int Count
    {
        get { return _items.Count; }
    }

    public T this[int index]
    {
        get { return _items[index]; }
    }
}

public class ConcreteIterator<T> : IIterator<T>
{
    private ConcreteCollection<T> _collection;
    private int _current = 0;

    public ConcreteIterator(ConcreteCollection<T> collection)
    {
        _collection = collection;
    }

    public bool HasNext()
    {
        return _current < _collection.Count;
    }

    public T Next()
    {
        T item = _collection[_current];
        _current++;
        return item;
    }

    public T Current()
    {
        return _collection[_current];
    }
}

// ===================== COMPOSITE =====================
public abstract class Component
{
    public string Name { get; set; }

    public Component(string name)
    {
        Name = name;
    }

    public abstract void Add(Component component);
    public abstract void Remove(Component component);
    public abstract void Display(int depth);
}

public class Composite : Component
{
    private List<Component> _children = new List<Component>();

    public Composite(string name) : base(name) { }

    public override void Add(Component component)
    {
        _children.Add(component);
    }

    public override void Remove(Component component)
    {
        _children.Remove(component);
    }

    public override void Display(int depth)
    {
        Console.WriteLine(new string('-', depth) + Name);
        foreach (Component component in _children)
        {
            component.Display(depth + 2);
        }
    }
}

public class Leaf : Component
{
    public Leaf(string name) : base(name) { }

    public override void Add(Component component)
    {
        Console.WriteLine("Cannot add to a leaf");
    }

    public override void Remove(Component component)
    {
        Console.WriteLine("Cannot remove from a leaf");
    }

    public override void Display(int depth)
    {
        Console.WriteLine(new string('-', depth) + Name);
    }
}

// ===================== FLYWEIGHT =====================
public class Flyweight
{
    private string _sharedState;

    public Flyweight(string sharedState)
    {
        _sharedState = sharedState;
    }

    public void Operation(string uniqueState)
    {
        Console.WriteLine($"Flyweight: Shared state: {_sharedState}, Unique state: {uniqueState}");
    }
}

public class FlyweightFactory
{
    private Dictionary<string, Flyweight> _flyweights = new Dictionary<string, Flyweight>();

    public FlyweightFactory(params string[] args)
    {
        foreach (var state in args)
        {
            _flyweights.Add(state, new Flyweight(state));
        }
    }

    public Flyweight GetFlyweight(string key)
    {
        if (!_flyweights.ContainsKey(key))
        {
            Console.WriteLine("FlyweightFactory: Creating new flyweight.");
            _flyweights.Add(key, new Flyweight(key));
        }
        else
        {
            Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
        }
        return _flyweights[key];
    }
}

// ===================== STATE =====================
public interface IState
{
    void Handle(Context context);
}

public class Context
{
    private IState _state;

    public Context(IState state)
    {
        TransitionTo(state);
    }

    public void TransitionTo(IState state)
    {
        Console.WriteLine($"Context: Transition to {state.GetType().Name}");
        _state = state;
    }

    public void Request()
    {
        _state.Handle(this);
    }
}

public class ConcreteStateA : IState
{
    public void Handle(Context context)
    {
        Console.WriteLine("ConcreteStateA handles request.");
        context.TransitionTo(new ConcreteStateB());
    }
}

public class ConcreteStateB : IState
{
    public void Handle(Context context)
    {
        Console.WriteLine("ConcreteStateB handles request.");
        context.TransitionTo(new ConcreteStateA());
    }
}

// ===================== PROXY =====================
public interface ISubject
{
    void Request();
}

public class RealSubject : ISubject
{
    public void Request()
    {
        Console.WriteLine("RealSubject: Handling request.");
    }
}

public class Proxy : ISubject
{
    private RealSubject _realSubject;

    public void Request()
    {
        if (_realSubject == null)
        {
            Console.WriteLine("Proxy: Creating RealSubject.");
            _realSubject = new RealSubject();
        }
        Console.WriteLine("Proxy: Checking access prior to firing a real request.");
        _realSubject.Request();
    }
}

// ===================== CHAIN OF RESPONSIBILITY =====================
public interface IHandler
{
    IHandler SetNext(IHandler handler);
    object Handle(object request);
}

public abstract class AbstractHandler : IHandler
{
    private IHandler _nextHandler;

    public IHandler SetNext(IHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }

    public virtual object Handle(object request)
    {
        if (_nextHandler != null)
        {
            return _nextHandler.Handle(request);
        }
        return null;
    }
}

public class MonkeyHandler : AbstractHandler
{
    public override object Handle(object request)
    {
        if ((string)request == "Banana")
        {
            return $"Monkey: I'll eat the {request}";
        }
        return base.Handle(request);
    }
}

public class SquirrelHandler : AbstractHandler
{
    public override object Handle(object request)
    {
        if ((string)request == "Nut")
        {
            return $"Squirrel: I'll eat the {request}";
        }
        return base.Handle(request);
    }
}

// ===================== INTERPRETER =====================
public interface IExpression
{
    int Interpret(Dictionary<string, int> context);
}

public class NumberExpression : IExpression
{
    private int _number;

    public NumberExpression(int number)
    {
        _number = number;
    }

    public int Interpret(Dictionary<string, int> context)
    {
        return _number;
    }
}

public class AddExpression : IExpression
{
    private IExpression _left;
    private IExpression _right;

    public AddExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public int Interpret(Dictionary<string, int> context)
    {
        return _left.Interpret(context) + _right.Interpret(context);
    }
}

// ===================== MEDIATOR =====================
public interface IMediator
{
    void Notify(object sender, string ev);
}

public class ConcreteMediator : IMediator
{
    private Component1 _component1;
    private Component2 _component2;

    public ConcreteMediator(Component1 component1, Component2 component2)
    {
        _component1 = component1;
        _component1.SetMediator(this);
        _component2 = component2;
        _component2.SetMediator(this);
    }

    public void Notify(object sender, string ev)
    {
        if (ev == "A")
        {
            Console.WriteLine("Mediator reacts on A and triggers following operations:");
            _component2.DoC();
        }
        if (ev == "D")
        {
            Console.WriteLine("Mediator reacts on D and triggers following operations:");
            _component1.DoB();
            _component2.DoC();
        }
    }
}

public class BaseComponent
{
    protected IMediator _mediator;

    public BaseComponent(IMediator mediator = null)
    {
        _mediator = mediator;
    }

    public void SetMediator(IMediator mediator)
    {
        _mediator = mediator;
    }
}

public class Component1 : BaseComponent
{
    public void DoA()
    {
        Console.WriteLine("Component 1 does A.");
        _mediator.Notify(this, "A");
    }

    public void DoB()
    {
        Console.WriteLine("Component 1 does B.");
        _mediator.Notify(this, "B");
    }
}

public class Component2 : BaseComponent
{
    public void DoC()
    {
        Console.WriteLine("Component 2 does C.");
        _mediator.Notify(this, "C");
    }

    public void DoD()
    {
        Console.WriteLine("Component 2 does D.");
        _mediator.Notify(this, "D");
    }
}

// ===================== MEMENTO =====================
public class Memento
{
    private string _state;

    public Memento(string state)
    {
        _state = state;
    }

    public string GetState()
    {
        return _state;
    }
}

public class Originator
{
    private string _state;

    public void SetState(string state)
    {
        Console.WriteLine($"Originator: Setting state to {state}");
        _state = state;
    }

    public Memento SaveState()
    {
        Console.WriteLine("Originator: Saving to Memento.");
        return new Memento(_state);
    }

    public void RestoreState(Memento memento)
    {
        _state = memento.GetState();
        Console.WriteLine($"Originator: State after restoring: {_state}");
    }
}

public class Caretaker
{
    private List<Memento> _mementos = new List<Memento>();
    private Originator _originator;

    public Caretaker(Originator originator)
    {
        _originator = originator;
    }

    public void Backup()
    {
        Console.WriteLine("Caretaker: Saving Originator's state...");
        _mementos.Add(_originator.SaveState());
    }

    public void Undo()
    {
        if (_mementos.Count == 0)
        {
            return;
        }

        var memento = _mementos.Last();
        _mementos.Remove(memento);

        Console.WriteLine("Caretaker: Restoring state...");
        _originator.RestoreState(memento);
    }
}

// ===================== VISITOR =====================
public interface IComponent
{
    void Accept(IVisitor visitor);
}

public class ConcreteComponentA : IComponent
{
    public void Accept(IVisitor visitor)
    {
        visitor.VisitConcreteComponentA(this);
    }

    public string ExclusiveMethodOfConcreteComponentA()
    {
        return "A";
    }
}

public class ConcreteComponentB : IComponent
{
    public void Accept(IVisitor visitor)
    {
        visitor.VisitConcreteComponentB(this);
    }

    public string SpecialMethodOfConcreteComponentB()
    {
        return "B";
    }
}

public interface IVisitor
{
    void VisitConcreteComponentA(ConcreteComponentA element);
    void VisitConcreteComponentB(ConcreteComponentB element);
}

public class ConcreteVisitor1 : IVisitor
{
    public void VisitConcreteComponentA(ConcreteComponentA element)
    {
        Console.WriteLine($"Visitor1: {element.ExclusiveMethodOfConcreteComponentA()}");
    }

    public void VisitConcreteComponentB(ConcreteComponentB element)
    {
        Console.WriteLine($"Visitor1: {element.SpecialMethodOfConcreteComponentB()}");
    }
}

public class ConcreteVisitor2 : IVisitor
{
    public void VisitConcreteComponentA(ConcreteComponentA element)
    {
        Console.WriteLine($"Visitor2: {element.ExclusiveMethodOfConcreteComponentA()}");
    }

    public void VisitConcreteComponentB(ConcreteComponentB element)
    {
        Console.WriteLine($"Visitor2: {element.SpecialMethodOfConcreteComponentB()}");
    }
}

// USAGES:

// ===================== TEMPLATE METHOD =====================
public class TemplateMethodDemo
{
    public static void Demo()
    {
        Console.WriteLine("Template Method Pattern Demo:");
        AbstractClass concreteClass = new ConcreteClass();
        concreteClass.TemplateMethod();
    }
}
// Output:
// Template Method Pattern Demo:
// AbstractClass: BaseOperation1
// ConcreteClass: RequiredOperation1
// AbstractClass: BaseOperation2
// ConcreteClass: Hook1 override
// ConcreteClass: RequiredOperation2
// AbstractClass: BaseOperation3

// ===================== ITERATOR =====================
public class IteratorDemo
{
    public static void Demo()
    {
        Console.WriteLine("Iterator Pattern Demo:");
        var collection = new ConcreteCollection<string>();
        collection.AddItem("Item A");
        collection.AddItem("Item B");
        collection.AddItem("Item C");

        var iterator = collection.CreateIterator();
        
        while (iterator.HasNext())
        {
            Console.WriteLine($"Iterating: {iterator.Next()}");
        }
    }
}
// Output:
// Iterator Pattern Demo:
// Iterating: Item A
// Iterating: Item B
// Iterating: Item C

// ===================== COMPOSITE =====================
public class CompositeDemo
{
    public static void Demo()
    {
        Console.WriteLine("Composite Pattern Demo:");
        
        // Create root and branches
        Composite root = new Composite("Root");
        Composite branch1 = new Composite("Branch 1");
        Composite branch2 = new Composite("Branch 2");
        
        // Create leaves
        Leaf leaf1 = new Leaf("Leaf 1");
        Leaf leaf2 = new Leaf("Leaf 2");
        Leaf leaf3 = new Leaf("Leaf 3");
        
        // Build the tree structure
        root.Add(branch1);
        root.Add(branch2);
        branch1.Add(leaf1);
        branch1.Add(leaf2);
        branch2.Add(leaf3);
        
        // Display the tree
        root.Display(1);
    }
}
// Output:
// Composite Pattern Demo:
// -Root
// ---Branch 1
// -----Leaf 1
// -----Leaf 2
// ---Branch 2
// -----Leaf 3

// ===================== FLYWEIGHT =====================
public class FlyweightDemo
{
    public static void Demo()
    {
        Console.WriteLine("Flyweight Pattern Demo:");
        
        var factory = new FlyweightFactory("Shared State A", "Shared State B", "Shared State C");
        
        var flyweight1 = factory.GetFlyweight("Shared State A");
        flyweight1.Operation("Unique State 1");
        
        var flyweight2 = factory.GetFlyweight("Shared State B");
        flyweight2.Operation("Unique State 2");
        
        var flyweight3 = factory.GetFlyweight("Shared State A");  // Reusing existing
        flyweight3.Operation("Unique State 3");
    }
}
// Output:
// Flyweight Pattern Demo:
// FlyweightFactory: Reusing existing flyweight.
// Flyweight: Shared state: Shared State A, Unique state: Unique State 1
// FlyweightFactory: Reusing existing flyweight.
// Flyweight: Shared state: Shared State B, Unique state: Unique State 2
// FlyweightFactory: Reusing existing flyweight.
// Flyweight: Shared state: Shared State A, Unique state: Unique State 3

// ===================== STATE =====================
public class StateDemo
{
    public static void Demo()
    {
        Console.WriteLine("State Pattern Demo:");
        var context = new Context(new ConcreteStateA());
        
        context.Request();
        context.Request();
        context.Request();
    }
}
// Output:
// State Pattern Demo:
// Context: Transition to ConcreteStateA
// ConcreteStateA handles request.
// Context: Transition to ConcreteStateB
// ConcreteStateB handles request.
// Context: Transition to ConcreteStateA

// ===================== PROXY =====================
public class ProxyDemo
{
    public static void Demo()
    {
        Console.WriteLine("Proxy Pattern Demo:");
        
        Proxy proxy = new Proxy();
        proxy.Request();
        proxy.Request();  // Second request will reuse existing RealSubject
    }
}
// Output:
// Proxy Pattern Demo:
// Proxy: Creating RealSubject.
// Proxy: Checking access prior to firing a real request.
// RealSubject: Handling request.
// Proxy: Checking access prior to firing a real request.
// RealSubject: Handling request.

// ===================== CHAIN OF RESPONSIBILITY =====================
public class ChainOfResponsibilityDemo
{
    public static void Demo()
    {
        Console.WriteLine("Chain of Responsibility Pattern Demo:");
        
        var monkey = new MonkeyHandler();
        var squirrel = new SquirrelHandler();
        
        monkey.SetNext(squirrel);
        
        Console.WriteLine("Chain: Monkey > Squirrel");
        Console.WriteLine(monkey.Handle("Banana"));
        Console.WriteLine(monkey.Handle("Nut"));
    }
}
// Output:
// Chain of Responsibility Pattern Demo:
// Chain: Monkey > Squirrel
// Monkey: I'll eat the Banana
// Squirrel: I'll eat the Nut

// ===================== INTERPRETER =====================
public class InterpreterDemo
{
    public static void Demo()
    {
        Console.WriteLine("Interpreter Pattern Demo:");
        
        // Represents the expression: 5 + (10 + 20)
        IExpression expression = new AddExpression(
            new NumberExpression(5),
            new AddExpression(
                new NumberExpression(10),
                new NumberExpression(20)
            )
        );
        
        var context = new Dictionary<string, int>();
        Console.WriteLine($"5 + (10 + 20) = {expression.Interpret(context)}");
    }
}
// Output:
// Interpreter Pattern Demo:
// 5 + (10 + 20) = 35

// ===================== MEDIATOR =====================
public class MediatorDemo
{
    public static void Demo()
    {
        Console.WriteLine("Mediator Pattern Demo:");
        
        Component1 component1 = new Component1();
        Component2 component2 = new Component2();
        new ConcreteMediator(component1, component2);

        Console.WriteLine("Client triggers operation A.");
        component1.DoA();
        
        Console.WriteLine("\nClient triggers operation D.");
        component2.DoD();
    }
}
// Output:
// Mediator Pattern Demo:
// Client triggers operation A.
// Component 1 does A.
// Mediator reacts on A and triggers following operations:
// Component 2 does C.
// 
// Client triggers operation D.
// Component 2 does D.
// Mediator reacts on D and triggers following operations:
// Component 1 does B.
// Component 2 does C.

// ===================== MEMENTO =====================
public class MementoDemo
{
    public static void Demo()
    {
        Console.WriteLine("Memento Pattern Demo:");
        
        Originator originator = new Originator();
        Caretaker caretaker = new Caretaker(originator);
        
        originator.SetState("State #1");
        caretaker.Backup();
        
        originator.SetState("State #2");
        caretaker.Backup();
        
        originator.SetState("State #3");
        Console.WriteLine("\nNow restoring to previous state...");
        caretaker.Undo();
    }
}
// Output:
// Memento Pattern Demo:
// Originator: Setting state to State #1
// Caretaker: Saving Originator's state...
// Originator: Saving to Memento.
// Originator: Setting state to State #2
// Caretaker: Saving Originator's state...
// Originator: Saving to Memento.
// Originator: Setting state to State #3
// 
// Now restoring to previous state...
// Caretaker: Restoring state...
// Originator: State after restoring: State #2

// ===================== VISITOR =====================
public class VisitorDemo
{
    public static void Demo()
    {
        Console.WriteLine("Visitor Pattern Demo:");
        
        List<IComponent> components = new List<IComponent>
        {
            new ConcreteComponentA(),
            new ConcreteComponentB()
        };
        
        Console.WriteLine("The client code works with all visitors via the base Visitor interface:");
        var visitor1 = new ConcreteVisitor1();
        foreach (var component in components)
        {
            component.Accept(visitor1);
        }
        
        Console.WriteLine("\nIt allows the same client code to work with different types of visitors:");
        var visitor2 = new ConcreteVisitor2();
        foreach (var component in components)
        {
            component.Accept(visitor2);
        }
    }
}
// Output:
// Visitor Pattern Demo:
// The client code works with all visitors via the base Visitor interface:
// Visitor1: A
// Visitor1: B
// 
// It allows the same client code to work with different types of visitors:
// Visitor2: A
// Visitor2: B
