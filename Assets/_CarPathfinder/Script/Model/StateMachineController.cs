using System;
using UnityEngine;


namespace MoNo.Extensions
{
    public enum StateType
    {
        StartView,
        Ingame,
        Success,
        Fail,
        Result,
        None,
    }

    public enum TriggerType
    {
        ToStart,
        ToIngame,
        ToSuccess,
        ToFail,
        ToResult,
        ToNone,
    }

    public class StateMachineController : Singleton<StateMachineController>
    {


        private readonly StateMachine<StateType, TriggerType> _stateMachine;

        public StateMachineController() : base()
        {
            // Generate StateMachine
            _stateMachine = new StateMachine<StateType, TriggerType>(StateType.StartView);

            // Add State Transition
            _stateMachine.AddTransition(StateType.StartView, StateType.Ingame, TriggerType.ToIngame);
            _stateMachine.AddTransition(StateType.Ingame, StateType.Success, TriggerType.ToSuccess);
            _stateMachine.AddTransition(StateType.Ingame, StateType.Fail, TriggerType.ToFail);
            _stateMachine.AddTransition(StateType.Success, StateType.Result, TriggerType.ToResult);

            _stateMachine.SetupState(StateType.StartView, () => { Debug.Log("Enter StartView"); },
                () => { Debug.Log("Exit StartView"); });
            _stateMachine.SetupState(StateType.Ingame, () => { Debug.Log("Enter Ingame"); },
                () => { Debug.Log("Exit Ingame"); });
            _stateMachine.SetupState(StateType.Success, () => { Debug.Log("Enter Success"); },
                () => { Debug.Log("Exit Success"); });
            _stateMachine.SetupState(StateType.Fail, () => { Debug.Log("Enter Fail"); },
                () => { Debug.Log("Exit Fail"); });
            _stateMachine.SetupState(StateType.Result, () => { Debug.Log("Enter Result"); },
                () => { Debug.Log("Exit Result"); });

        }

        public void SetUpState(StateType state, Action enterState = null, Action exitState = null,
            Action<float> updateState = null)
        {
            _stateMachine.SetupState(state, enterState, exitState, updateState);
        }

        public void ExecuteTrigger(TriggerType trigger)
        {
            _stateMachine.ExecuteTrigger(trigger);
        }

        public void Update(float deltaTime)
        {
            _stateMachine.Update(deltaTime);
        }
    }
}