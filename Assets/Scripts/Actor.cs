using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ActorBehavior))]
public class Actor : MonoBehaviour {
    public float speed;

    protected ActorBehavior behavior;
    protected float _energy;
    protected bool _shouldBeDestroyed;

    void Start() {
        _Init();
    }
    
    void _Init() {
        _energy = 0;
    }

    public void GainEnergy() {
        _energy += speed;
    }

    public IEnumerator TakeAction() {
        if (_shouldBeDestroyed)
            yield return null;

        this.behavior = GetComponent<ActorBehavior>();
        _energy -= 1;
        behavior.TakeAction();
        while (!behavior.IsDone) {
            yield return null;
        }
    }

    public void SetToDestroy() {
        _shouldBeDestroyed = true;
        _energy = 0;
        gameObject.SetActive(false);
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

    public bool ShouldBeDestroyed {
        get {
            return _shouldBeDestroyed;
        }
    }
}