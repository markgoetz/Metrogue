using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : ActorBehavior {
	public int damage;

	protected override IEnumerator _ExecuteAction() {
		GameObject target = _GetTarget(transform.rotation * new Vector3(0, 0, 1));
		if (target != null) {
			if (target.GetComponent<Damageable>() != null) {
				target.GetComponent<Damageable>().TakeDamage(damage);
			}

			GetComponent<Actor>().SetToDestroy();
			_Finish();
			yield break;
		}

		transform.Translate(new Vector3(0, 0, 1));
		_Finish();
		yield break;
	}

	private GameObject _GetTarget(Vector3 movementAmount) {
		int layerMask = 1 << LayerMask.NameToLayer("Block");
		Vector3 origin = transform.position;
		RaycastHit hit;
		Physics.Raycast(origin, movementAmount, out hit, movementAmount.magnitude, layerMask);
		if (hit.collider == null)
			return null;

		return hit.collider.gameObject;
	}
}
