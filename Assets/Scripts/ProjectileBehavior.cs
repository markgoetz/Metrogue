using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : ActorBehavior {
	protected override IEnumerator _ExecuteAction() {
		if (!_CanMove(transform.rotation * new Vector3(0, 0, 1))) {
			GetComponent<Actor>().SetToDestroy();
			_Finish();
			yield break;
		}

		transform.Translate(new Vector3(0, 0, 1));
		_Finish();
		yield break;
	}

	private bool _CanMove(Vector3 movementAmount) {
		int layerMask = 1 << LayerMask.NameToLayer("Block");
		Vector3 origin = transform.position;
		return !Physics.Raycast(origin, movementAmount, movementAmount.magnitude, layerMask);
	}
}
