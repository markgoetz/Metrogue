using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {
	public int hitPoints;

	public void TakeDamage(int damage) {
		hitPoints -= damage;

		if (hitPoints <= 0) {
			gameObject.SetActive(false);
		}
	}
}
