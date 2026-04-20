namespace DynamicFunctions.Compilation.Exceptions;

public class UnsupportedFunctionTypeException(string typeName) 
    : DynamicFunctionsException($"Unsupported function type: '{typeName}'");
