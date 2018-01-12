using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Player : ActorBehavior {
	public GameObject blasterShot;
	protected bool _readyForInput;

	void Awake () {
		this._readyForInput = false;
	}

	public void SpawnAt(IntPoint origin) {
		this.transform.position = new Vector3(origin.x, 1, origin.y);
	}

	protected override IEnumerator _ExecuteAction() {
		this._readyForInput = true;
		yield break;
	}
	
	void LateUpdate () {
		if (this._readyForInput) {
			_checkForInput();
		}
	}

	protected void _checkForInput() {
		Vector3 movementAmount = new Vector3();

		if (Input.GetButtonDown("Left")) {
			movementAmount = new Vector3(-1, 0, 0);
		} else if (Input.GetButtonDown("Right")) {
			movementAmount = new Vector3(1, 0, 0);
		} else if (Input.GetButtonDown("Up")) {
			movementAmount = new Vector3(0, 0, 1);
		} else if (Input.GetButtonDown("Down")) {
			movementAmount = new Vector3(0, 0, -1);
		} else if (Input.GetButtonDown("Fire1")) {
			Instantiate(blasterShot, transform.position, transform.rotation);
			this._Finish();
		}

		if (movementAmount != Vector3.zero && _CanMove(movementAmount)) {
			transform.LookAt(transform.position + movementAmount);
			transform.position = transform.position + movementAmount;

			this._readyForInput = false;
			this._Finish();
		}
	}

	private bool _CanMove(Vector3 movementAmount) {
		int layerMask = 1 << LayerMask.NameToLayer("Block");
		Vector3 origin = transform.position;
		return !Physics.Raycast(origin, movementAmount, movementAmount.magnitude, layerMask);
	}

	public bool IsAlive {
		get {
			return true;
		}
	}
}
