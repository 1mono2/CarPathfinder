using System.Collections.Generic;
using UniRx;
using UnityEngine;

public enum StateType
{
    
    StartView,
    Ingame,
    Success,
    Fail,
    Result,
    Nothing,
}

public enum TriggerType
{
    ToStartView,
    ToIngame,
    ToSuccess,
    ToFail,
    ToResult,
    ToNothing,
}

[System.Serializable]
public class LevelProgressStateReactiveProperty : ReactiveProperty<StateType>
{
    private Dictionary<StateType, List<Transition<StateType, TriggerType>>> _transitionLists = new();

    public LevelProgressStateReactiveProperty() : base(default)
    {
        ChangeState(default);
    }

    public LevelProgressStateReactiveProperty(StateType initialValue) : base(initialValue)
    {
        ChangeState(initialValue);
    }
    
    public void ExecuteTrigger(TriggerType triggerType)
    {
        var transitions = _transitionLists[this.Value];
        foreach (var transition in transitions)
        {
            if(transition.Trigger == triggerType)
            {
                ChangeState(transition.To);
                break;
            }
        }
    }
    
    public void AddTransition(StateType from, StateType to, TriggerType trigger)
    {
        if (!_transitionLists.ContainsKey(from))
            _transitionLists.Add(from, new List<Transition<StateType, TriggerType>>());

        var transition = new Transition<StateType, TriggerType>
        {
            To = to,
            Trigger = trigger,
        };
        _transitionLists[from].Add(transition);
    }

    private void ChangeState(StateType to)
    {
        this.Value = to;
        Debug.Log($"ChangeState: {this.Value}");
    }
    
}

public class Transition<TState, TTrigger>
{
    public TState To { get; set; }
    public TTrigger Trigger { get; set; }
}
