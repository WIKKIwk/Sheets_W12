namespace W12CSheets.Client.Patterns;

/// <summary>
/// Facade pattern for simplified interface
/// </summary>
public class SubsystemA
{
    public string OperationA1() => "SubsystemA: Operation A1";
    public string OperationA2() => "SubsystemA: Operation A2";
}

public class SubsystemB
{
    public string OperationB1() => "SubsystemB: Operation B1";
    public string OperationB2() => "SubsystemB: Operation B2";
}

public class SubsystemC
{
    public string OperationC1() => "SubsystemC: Operation C1";
    public string OperationC2() => "SubsystemC: Operation C2";
}

public class Facade
{
    private readonly SubsystemA _subsystemA;
    private readonly SubsystemB _subsystemB;
    private readonly SubsystemC _subsystemC;

    public Facade()
    {
        _subsystemA = new SubsystemA();
        _subsystemB = new SubsystemB();
        _subsystemC = new SubsystemC();
    }

    public string Operation()
    {
        var result = "Facade initializes subsystems:\n";
        result += _subsystemA.OperationA1() + "\n";
        result += _subsystemB.OperationB1() + "\n";
        result += "Facade orders subsystems to perform actions:\n";
        result += _subsystemA.OperationA2() + "\n";
        result += _subsystemC.OperationC1();
        return result;
    }
}
