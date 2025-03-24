using Maris.ConsoleApp.Core;
using Maris.ConsoleApp.UnitTests.XunitSerializer;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(ConsoleAppContextXunitSerializer), typeof(ConsoleAppContext))]

namespace Maris.ConsoleApp.UnitTests.XunitSerializer;

public class ConsoleAppContextXunitSerializer : JsonXunitSerializer<ConsoleAppContext>
{
}
