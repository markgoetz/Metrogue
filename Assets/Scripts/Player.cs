using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Player : MonoBehaviour {
	private BoxCollider playerCollider;
	void Start () {
		this.playerCollider = GetComponent<BoxCollider>();
	}

	public void SpawnAt(IntPoint origin) {
		this.transform.position = new Vector3(origin.x, 1, origin.y);
	}
	
	void Update () {
		Vector3 movementAmount = new Vector3();

		if (Input.GetButtonDown("Left")) {
			movementAmount = new Vector3(-1, 0, 0);
		} else if (Input.GetButtonDown("Right")) {
			movementAmount = new Vector3(1, 0, 0);
		} else if (Input.GetButtonDown("Up")) {
			movementAmount = new Vector3(0, 0, 1);
		} else if (Input.GetButtonDown("Down")) {
			movementAmount = new Vector3(0, 0, -1);
		}

		if (movementAmount != Vector3.zero && _CanMove(movementAmount)) {
			transform.Translate(movementAmount);
		}
	}

	private bool _CanMove(Vector3 movementAmount) {
		int layerMask = 1 << LayerMask.NameToLayer("Block");
		Vector3 origin = transform.position;
		return !Physics.Raycast(origin, movementAmount, movementAmount.magnitude, layerMask);
	}
}
