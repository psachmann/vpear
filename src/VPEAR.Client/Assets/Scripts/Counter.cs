using Fluxor;

public class CounterState
{
    public CounterState(int counter)
    {
        this.Counter = counter;
    }

    public int Counter { get; }
}

public class CounterFeature : Feature<CounterState>
{
    public override string GetName()
    {
        return nameof(CounterState);
    }

    protected override CounterState GetInitialState()
    {
        return new CounterState(10);
    }
}

public class IncrementCounterAction
{
}

public class DecrementCounterAction
{
}

public static partial class Reducers
{
    [ReducerMethod]
    public static CounterState ReduceIncrementCounterAction(CounterState state, IncrementCounterAction action)
    {
        return new CounterState(state.Counter + 1);
    }

    [ReducerMethod]
    public static CounterState ReduceDecrementCounterAction(CounterState state, DecrementCounterAction action)
    {
        return new CounterState(state.Counter - 1);
    }
}
