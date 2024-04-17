using MassTransit;

namespace MasstransitHasConventionRepro.Entities;

public class MySagaState : SagaStateMachineInstance, IGenericSaga<MySagaStateMachineData>
{
    public Guid CorrelationId { get; set; }
    public required MySagaStateMachineData StateData { get; set; }
    public int CurrentState { get; set; }
}

public interface IGenericSaga<TState> : ISaga
{
    public TState StateData { get; set; }
    public int CurrentState { get; set; }
}

public class MySagaStateMachineData
{
    public int Count { get; set; }
}