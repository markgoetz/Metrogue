using UnityEngine;
using System;
using System.Collections;

enum BehaviorState {
    NOT_READY,
    WAITING,
    DONE
};

abstract public class ActorBehavior : MonoBehaviour {
    private BehaviorState state;

    public void TakeAction() {
        state = BehaviorState.WAITING;
        _ExecuteAction();
    }
    
    abstract protected void _ExecuteAction();

    protected void _Finish() {
        state = BehaviorState.DONE;
    }

    public bool IsDone {
        get {
            return state == BehaviorState.DONE;
        }
    }
}