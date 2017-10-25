using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Toggl.Multivac;

namespace Toggl.Foundation.Sync
{
    internal sealed class StateMachine : IStateMachine
    {
        private readonly TimeSpan stateTimeout = TimeSpan.FromMinutes(1);

        private readonly Subject<StateMachineEvent> stateTransitions = new Subject<StateMachineEvent>();
        public IObservable<StateMachineEvent> StateTransitions { get; }

        private readonly ITransitionHandlerProvider transitionHandlerProvider;
        private readonly IScheduler scheduler;
        private readonly ISubject<Unit> delayCancellation;

        private bool isRunning;
        private bool isFrozen;

        public StateMachine(ITransitionHandlerProvider transitionHandlerProvider, IScheduler scheduler, ISubject<Unit> delayCancellation)
        {
            Ensure.Argument.IsNotNull(transitionHandlerProvider, nameof(transitionHandlerProvider));
            Ensure.Argument.IsNotNull(scheduler, nameof(scheduler));
            Ensure.Argument.IsNotNull(delayCancellation, nameof(delayCancellation));

            this.transitionHandlerProvider = transitionHandlerProvider;
            this.scheduler = scheduler;
            this.delayCancellation = delayCancellation;

            StateTransitions = stateTransitions.AsObservable();
            isFrozen = false;
        }

        public void Start(ITransition transition)
        {
            Ensure.Argument.IsNotNull(transition, nameof(transition));
            
            if (isRunning)
                throw new InvalidOperationException("Cannot start state machine if it is already running.");

            if (isFrozen)
                throw new InvalidOperationException("Cannot start state machine again if it was frozen.");

            isRunning = true;
            onTransition(transition);
        }

        public void Freeze()
        {
            delayCancellation.OnNext(Unit.Default);
            delayCancellation.OnCompleted();
            isFrozen = true;
        }

        private void performTransition(ITransition transition, TransitionHandler transitionHandler)
        {
            stateTransitions.OnNext(new StateMachineTransition(transition));

            transitionHandler(transition)
                .SingleAsync()
                .Timeout(scheduler.Now + stateTimeout, scheduler)
                .Subscribe(onTransition, onError);
        }

        private void onTransition(ITransition transition)
        {
            var transitionHandler = transitionHandlerProvider.GetTransitionHandler(transition.Result);

            if (transitionHandler == null || isFrozen == true)
            {
                isRunning = false;
                reachDeadEnd(transition);
                return;
            }

            performTransition(transition, transitionHandler);
        }

        private void onError(Exception exception)
        {
            isRunning = false;
            reportError(exception);
        }

        private void reachDeadEnd(ITransition transition)
        {
            stateTransitions.OnNext(new StateMachineDeadEnd(transition));
        }

        private void reportError(Exception exception)
        {
            stateTransitions.OnNext(new StateMachineError(exception));
        }
    }
}
