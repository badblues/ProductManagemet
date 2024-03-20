namespace Persistence.Exceptions;

public class EntityAlreadyExists : Exception
{
    public EntityAlreadyExists(string message) : base(message) { }
}
