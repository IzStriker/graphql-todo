namespace Backend.GraphQL.Exceptions;

public class EmailInUseException : Exception
{
    public EmailInUseException() : base("An user with this email already exists")
    {

    }
}