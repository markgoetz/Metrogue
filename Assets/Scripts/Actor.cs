using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ActorBehavior))]
public class Actor : MonoBehaviour {
    public float speed;

    protected ActorBehavior behavior;
    public float _energy;

    void Start() {
        _Init();
    }
    
    void _Init() {
        _energy = 0;
    }

    public void TakeTurn() {
        _energy += speed;
        StartCoroutine("TakeActions");
    }

    public IEnumerator TakeActions() {
        this.behavior = GetComponent<ActorBehavior>();
        while (HasAction) {
            _energy -= 1;
            behavior.TakeAction();
            while (!behavior.IsDone) {
                yield return null;
            }
        }
    }

    public bool HasAction {
        get {
            return (_energy >= 1);
        }
    }

    public bool IsDone {
        get {
            return (!HasAction && behavior.IsDone);
        }
    }
}