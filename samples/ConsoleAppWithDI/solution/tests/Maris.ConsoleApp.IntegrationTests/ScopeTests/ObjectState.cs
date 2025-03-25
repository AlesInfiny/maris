namespace Maris.ConsoleApp.IntegrationTests.ScopeTests;

internal class ObjectState
{
    private readonly TimeProvider timeProvider;
    private readonly DateTime createDate;

    internal ObjectState(Guid objectId, Type objectType, Condition condition, TimeProvider timeProvider)
    {
        this.ObjectId = objectId;
        this.ObjectType = objectType;
        this.Condition = condition;
        this.timeProvider = timeProvider;
        this.createDate = this.timeProvider.GetLocalNow().UtcDateTime;
    }

    internal Guid ObjectId { get; private set; }

    internal Type ObjectType { get; private set; }

    internal Condition Condition { get; private set; }

    internal DateTime CreateDate { get => this.createDate; }
}
