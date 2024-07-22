namespace Backend.GraphQL.Exceptions;

public class FailedLoginException : Exception
{
    public FailedLoginException() : base("Incorrect Email or Password")
    {

    }
}