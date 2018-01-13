using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour {
	public Player player;
	public MapGenerator mapGenerator;

	private IntPoint origin;

	void Awake () {
		origin = new IntPoint(
			Random.Range(1, mapGenerator.mapSize.x - 1),
			Random.Range(1, mapGenerator.mapSize.y - 1)
		);
	}

	void Start() {
		mapGenerator.GenerateMap(origin);
		player.SpawnAt(origin);

		StartCoroutine("RunGameLoop");
	}

	IEnumerator RunGameLoop() {
		Actor[] actors;
		while (player.IsAlive) {
			actors = FindObjectsOfType(typeof(Actor)) as Actor[];
			foreach (Actor actor in actors) {
				actor.GainEnergy();
				while (actor.HasAction) {
					yield return actor.TakeAction();
					yield return new WaitForSeconds(.1f);
				}
			}

			for (int i = 0; i < actors.Length; i++) {
				if (actors[i].ShouldBeDestroyed) {
					Destroy(actors[i].gameObject);
				}
			}

			yield return null;
		}

		yield break;
	}

	public static GameLoop GetInstance() {
		return GameObject.FindGameObjectWithTag("GameController").GetComponent<GameLoop>();
	}
}
