using MassTransit;
using MasstransitHasConventionRepro.Entities;
using MasstransitHasConventionRepro.Events;

namespace MasstransitHasConventionRepro.Saga
{
    public sealed class MySagaStateMachine : MassTransitStateMachine<MySagaState>
    {
        public MySagaStateMachine()
        {
            InstanceState(state => state.CurrentState, Started);

            Event(() => OnMyEvent, e => e.CorrelateById(ctx => ctx.Message.Id));

            Initially(
                When(OnMyEvent)
                    .Then(ctx =>
                    {
                        ctx.Saga.StateData = new MySagaStateMachineData { Count = 1 };
                    })
                    .TransitionTo(Started));

            During(Started, 
                When(OnMyEvent).Then(ctx => ctx.Saga.StateData.Count++));
        }

        public Event<MyEvent>? OnMyEvent { get; set; }

        public State? Started { get; set; }
    }
}
