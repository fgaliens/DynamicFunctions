namespace DynamicFunctions;

public abstract class DynamicFunctionsException : Exception
{
    protected DynamicFunctionsException() : base()
    {
    }

    protected DynamicFunctionsException(string message) : base(message)
    {
    }
}