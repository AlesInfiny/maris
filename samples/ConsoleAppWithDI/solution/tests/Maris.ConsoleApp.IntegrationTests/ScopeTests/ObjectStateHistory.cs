namespace Maris.ConsoleApp.IntegrationTests.ScopeTests;

internal static class ObjectStateHistory
{
    private static readonly List<ObjectState> HistoryStore = [];

    internal static IReadOnlyCollection<ObjectState> Histories => HistoryStore.AsReadOnly();

    internal static void Add(ObjectState objectState) => HistoryStore.Add(objectState);

    internal static void Clear() => HistoryStore.Clear();
}
