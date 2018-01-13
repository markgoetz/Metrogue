using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Player : ActorBehavior {
	public GameObject blasterShot;
	protected bool _readyForInput;
	protected string _bufferedInput;

	private string[] INPUTS_TO_BUFFER = {
		"Up",
		"Left",
		"Right",
		"Down",
		"Fire1"
	};

	void Awake () {
		this._readyForInput = false;
		this._bufferedInput = "";
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
			_CheckForInput();
		} else {
			_BufferInput();
		}
	}

	protected void _CheckForInput() {
		Vector3 movementAmount = new Vector3();

		if (_IsInputActive("Left")) {
			movementAmount = new Vector3(-1, 0, 0);
		} else if (_IsInputActive("Right")) {
			movementAmount = new Vector3(1, 0, 0);
		} else if (_IsInputActive("Up")) {
			movementAmount = new Vector3(0, 0, 1);
		} else if (_IsInputActive("Down")) {
			movementAmount = new Vector3(0, 0, -1);
		} else if (_IsInputActive("Fire1")) {
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

	private void _BufferInput() {
		foreach (string inputToBuffer in INPUTS_TO_BUFFER) {
			if (Input.GetButtonDown(inputToBuffer)) {
				_bufferedInput = inputToBuffer;
			}
		}
	}

	private bool _IsInputActive(string inputName) {
		if (_bufferedInput == inputName) {
			_bufferedInput = "";
			return true;
		}

		return Input.GetButtonDown(inputName);
	}

	public bool IsAlive {
		get {
			return true;
		}
	}
}
