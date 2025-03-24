namespace Maris.ConsoleApp.IntegrationTests.ScopeTests;

internal enum Condition : inta
{
    Unknown = 0,
    Creating = 1,
    Alive = 2,
    AlreadyDisposed = 3,
    ObjectDisposing = 4,
    ObjectDisposed = 5,
}
