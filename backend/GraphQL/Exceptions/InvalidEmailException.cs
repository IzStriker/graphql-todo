namespace Backend.GraphQL.Exceptions;

public class InvalidEmailException : Exception
{
    public InvalidEmailException() : base("Invalid Email address")
    {

    }
}