namespace BuildingBlocks.Exceptions;

public class InternalServerExceptions : Exception
{
    public InternalServerExceptions(string message) : base(message) 
    {
    }

    public InternalServerExceptions(string message, string details) : base(message)
    {
        Details = details;
    }

    public string? Details { get; }
}
