namespace Maris.ConsoleApp.IntegrationTests.ScopeTests;

internal class TestObject1 : TestObjectBase
{
    private readonly TestObject2 obj2;

    public TestObject1(TestObject2 obj2) => this.obj2 = obj2;

    internal void DoSomething()
    {
        this.LogHistory();
        this.obj2.DoSomething();
    }
}
