using System.Collections;
using System;
using System.Collections.Generic;
using UniRx;

namespace MoNo.Extensions
{
    public class StateMapping
    {
        public Subject<Unit> OnEnter { get; set; } = new();
        public Action OnExit { get; set; }
        public Action<float> OnUpdate { get; set; }
    }

    public class Transition<TState, TTrigger>
    {
        public TState To { get; set; }
        public TTrigger Trigger { get; set; }
    }


// TState & TTrigger is generic type
    public class StateMachine<TState, TTrigger>
        where TState : struct, IConvertible, IComparable
        where TTrigger : struct, IConvertible, IComparable
    {
        // Current state
        private TState _stateType;

        // Current state mapping
        private StateMapping _stateMapping;

        // A dictionary contains a state mapping. It connect state to state mapping
        private Dictionary<TState, StateMapping> _stateMappings = new();

        // A dictionary contains a list of transitions
        private Dictionary<TState, List<Transition<TState, TTrigger>>> _transitionLists = new();

        public StateMachine(TState initState)
        {
            // get all enum values of TState
            var enumValues = Enum.GetValues(typeof(TState));
            for (int i = 0; i < enumValues.Length; i++)
            {
                var mapping = new StateMapping();
                _stateMappings.Add((TState)enumValues.GetValue(i), mapping);
            }

            ChangeState(initState);
        }


        public void ExecuteTrigger(TTrigger trigger)
        {
            var transitions = _transitionLists[_stateType];
            foreach (var transition in transitions)
            {
                if (transition.Trigger.Equals(trigger))
                {
                    ChangeState(transition.To);
                    break;
                }
            }
        }

        /// <summary>
        /// Resister transition from state to state by trigger
        /// If the same is registered, later registered transitions will be called.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="trigger"></param>
        public void AddTransition(TState from, TState to, TTrigger trigger)
        {
            if (!_transitionLists.ContainsKey(from))
                _transitionLists.Add(from, new List<Transition<TState, TTrigger>>());

            var transition = new Transition<TState, TTrigger>
            {
                To = to,
                Trigger = trigger
            };
            _transitionLists[from].Add(transition);
        }

        public void SetupState(TState state, Action onEnter = null, Action onExit = null, Action<float> onUpdate = null)
        {
            var mapping = _stateMappings[state];
            mapping.OnEnter.Subscribe(_ => onEnter?.Invoke());
            mapping.OnExit = onExit;
            mapping.OnUpdate = onUpdate;
        }

        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            if (_stateMapping is { OnUpdate: { } })
                _stateMapping.OnUpdate.Invoke(deltaTime);
        }

        private void ChangeState(TState to)
        {
            if (_stateMapping is { OnExit: { } })
                _stateMapping.OnExit.Invoke();

            _stateType = to;
            _stateMapping = _stateMappings[to];
            if (_stateMapping.OnEnter != null)
                _stateMapping.OnEnter.OnNext(Unit.Default);
        }
    }
}