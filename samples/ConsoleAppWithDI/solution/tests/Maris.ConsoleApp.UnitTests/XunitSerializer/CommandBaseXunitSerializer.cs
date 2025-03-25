using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.UnitTests.XunitSerializer;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(CommandBaseXunitSerializer), typeof(CommandBase))]

namespace Maris.ConsoleApp.UnitTests.XunitSerializer;

public class CommandBaseXunitSerializer : JsonXunitSerializer<CommandBase>
{
}
