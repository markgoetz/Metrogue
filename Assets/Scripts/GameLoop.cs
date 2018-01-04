using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour {
	public Player player;
	public MapGenerator mapGenerator;

	private IntPoint origin;

	// Use this for initialization
	void Start () {
		origin = new IntPoint(
			Random.Range(1, mapGenerator.mapSize.x - 1),
			Random.Range(1, mapGenerator.mapSize.y - 1)
		);

		mapGenerator.GenerateMap(origin);
		player.SpawnAt(origin);
	}
}
