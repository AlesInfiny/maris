namespace Maris.ConsoleApp.IntegrationTests.ScopeTests;

internal class TestObject1(TestObject2 obj2) : TestObjectBase
{
    private readonly TestObject2 obj2 = obj2;

    internal void DoSomething()
    {
        this.LogHistory();
        this.obj2.DoSomething();
    }
}
