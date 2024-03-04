using Microsoft.Extensions.Time.Testing;

namespace Maris.ConsoleApp.IntegrationTests.ScopeTests;

internal class TestObjectBase : IDisposable
{
    private readonly Guid objectId = Guid.NewGuid();
    private bool disposed;
    private TimeProvider fakeTimeProvider = new FakeTimeProvider();

    public TestObjectBase()
    {
        ObjectStateHistory.Add(new(
            this.objectId,
            this.GetType(),
            Condition.Creating,
            this.fakeTimeProvider));
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected void LogHistory() =>
        ObjectStateHistory.Add(new(
            this.objectId,
            this.GetType(),
            this.disposed ? Condition.AlreadyDisposed : Condition.Alive,
            this.fakeTimeProvider));

    protected virtual void Dispose(bool disposing)
    {
        ObjectStateHistory.Add(new(this.objectId, this.GetType(), Condition.ObjectDisposing, this.fakeTimeProvider));

        if (!this.disposed)
        {
            this.disposed = true;
            ObjectStateHistory.Add(new(this.objectId, this.GetType(), Condition.ObjectDisposed, this.fakeTimeProvider));
        }
    }
}
