using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : ActorBehavior {
	protected override IEnumerator _ExecuteAction() {
		transform.Translate(new Vector3(0, 0, 1));

		this._Finish();
		yield break;
	}
}
