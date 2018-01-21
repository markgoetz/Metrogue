using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {
	public int hitPoints;

	private int _currentHitPoints;

	void Start() {
		_currentHitPoints = hitPoints;
	}

	public void TakeDamage(int damage) {
		_currentHitPoints -= damage;

		if (_currentHitPoints <= 0) {
			SendMessage("Damageable_OnDestroy");
			gameObject.SetActive(false);
		}
	}
}
