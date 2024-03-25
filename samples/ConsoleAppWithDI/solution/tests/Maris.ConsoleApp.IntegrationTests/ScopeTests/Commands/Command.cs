using Maris.ConsoleApp.Core;

namespace Maris.ConsoleApp.IntegrationTests.ScopeTests.Commands;

internal class Command(TestObject1 obj1, TestObject2 obj2) : SyncCommand<Parameter>
{
    internal const string CommandName = "scope-test";
    private readonly TestObject1 obj1 = obj1;
    private readonly TestObject2 obj2 = obj2;

    protected override ICommandResult Execute(Parameter parameter)
    {
        this.obj1.DoSomething();
        this.obj2.DoSomething();
        return new SuccessResult();
    }
}
