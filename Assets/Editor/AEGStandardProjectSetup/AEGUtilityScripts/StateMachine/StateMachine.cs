using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected BaseState<EState> CurrentState;
    protected BaseState<EState> QueuedState;
    protected BaseState<EState> LastState;

    private Dictionary<EState, BaseState<EState>> _stateLookup = new Dictionary<EState, BaseState<EState>>();

    [SerializeField]
    private string _currentState, _queuedState, _lastState;

    public static Action<EState> OnStateChange;

    private bool _queuedStateThisFrame;

    private void Start()
    {
        CurrentState.EnterState(lastState: CurrentState.StateKey);
    }

    private void Update()
    {
        if (CurrentState.Equals(QueuedState))
        {
            CurrentState.UpdateState();
        }
        else
        {           
            LastState = CurrentState;
            CurrentState.ExitState();
            CurrentState = QueuedState;
            CurrentState.EnterState(lastState: QueuedState.StateKey);
            OnStateChange?.Invoke(CurrentState.StateKey);
        }

        _currentState = CurrentState.StateKey.ToString();
        _queuedState = QueuedState.StateKey.ToString();
        
        if (LastState != null)
        {
            _lastState = LastState.StateKey.ToString();
        }
    }

    /// <summary>
    /// _queuedStateThisFrame is used to make sure state changes from the same clicks (PlayerInput and EventSystem) aren't counted twice.
    /// It's crude, but it works.
    /// </summary>
    private void LateUpdate()
    {
        _queuedStateThisFrame = false;
    }

    public void QueueNextState(EState stateToQueue)
    {
        if (_queuedStateThisFrame == true) return;
        
        Debug.Log($"Queued State [{this.gameObject.name}]: " + stateToQueue);
        QueuedState = _stateLookup[stateToQueue];
        _queuedStateThisFrame = true;
    }

    public void AddStateToLookup(EState stateIdentifier, BaseState<EState> stateInstance)
    {
        _stateLookup.Add(stateIdentifier, stateInstance);
    }

    public BaseState<EState> LookupState(EState stateIdentifier)
    {
        return _stateLookup[stateIdentifier];
    }

    public void RevertToPreviousState()
    {
        QueuedState = LastState;
    }
}