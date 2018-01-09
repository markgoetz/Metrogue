using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour {
	public Player player;
	public MapGenerator mapGenerator;

	private List<Actor> actors;
	private IntPoint origin;

	// Use this for initialization
	void Awake () {
		origin = new IntPoint(
			Random.Range(1, mapGenerator.mapSize.x - 1),
			Random.Range(1, mapGenerator.mapSize.y - 1)
		);

		actors = new List<Actor>();
	}

	void Start() {
		mapGenerator.GenerateMap(origin);
		player.SpawnAt(origin);

		StartCoroutine("RunGameLoop");
	}

	IEnumerator RunGameLoop() {
		while (player.IsAlive) {
			foreach (Actor actor in actors) {
				actor.TakeTurn();
				while (!actor.IsDone) {
					yield return null;
				}
			}

			yield return null;
		}

		yield break;
	}

	public void RegisterActor(Actor actor) {
		this.actors.Add(actor);
	}

	public static GameLoop GetInstance() {
		return GameObject.FindGameObjectWithTag("GameController").GetComponent<GameLoop>();
	}
}
