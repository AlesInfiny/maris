namespace Maris.ConsoleApp.IntegrationTests.ScopeTests;

internal class TestObjectBase : IDisposable
{
    private readonly Guid objectId = Guid.NewGuid();
    private bool disposed;

    public TestObjectBase() =>
        ObjectStateHistory.Add(new(
            this.objectId,
            this.GetType(),
            Condition.Creating));

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected void LogHistory() =>
        ObjectStateHistory.Add(new(
            this.objectId,
            this.GetType(),
            this.disposed ? Condition.AlreadyDisposed : Condition.Alive));

    protected virtual void Dispose(bool disposing)
    {
        ObjectStateHistory.Add(new(this.objectId, this.GetType(), Condition.ObjectDisposing));

        if (!this.disposed)
        {
            this.disposed = true;
            ObjectStateHistory.Add(new(this.objectId, this.GetType(), Condition.ObjectDisposed));
        }
    }
}
