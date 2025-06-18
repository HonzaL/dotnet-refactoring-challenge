namespace RefactoringChallenge.Services.Exceptions;

public class UnknownCustomerException : Exception
{
    public int CustomerId { get; }
    
    public UnknownCustomerException(int customerId) : this($"Zákazník s ID {customerId} nebyl nalezen.", customerId)
    {
    }

    private UnknownCustomerException(string? message, int customerId) : this(message, null, customerId)
    {
    }

    private UnknownCustomerException(string? message, Exception? innerException, int customerId) : base(message, innerException)
    {
        CustomerId = customerId;
    }
}
