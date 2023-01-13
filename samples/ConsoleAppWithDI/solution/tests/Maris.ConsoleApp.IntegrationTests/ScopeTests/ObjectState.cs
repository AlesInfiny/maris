namespace Maris.ConsoleApp.IntegrationTests.ScopeTests;

internal class ObjectState
{
    internal ObjectState(Guid objectId, Type objectType, Condition condition)
    {
        this.ObjectId = objectId;
        this.ObjectType = objectType;
        this.Condition = condition;
    }

    internal Guid ObjectId { get; private set; }

    internal Type ObjectType { get; private set; }

    internal Condition Condition { get; private set; }

    internal DateTime CreateDate { get; } = DateTime.Now;
}
