using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileRenderer {
	private List<Room> rooms;
	private List<GameObject> tileList;

	public TileRenderer(List<Room> rooms) {
		this.rooms = rooms;
		tileList = new List<GameObject>();
	}

	public void Render(TileType[][] tiles, Tileset tileset) {
		GameObject[] roomObjects = new GameObject[rooms.Count];
		for (int i = 0; i < rooms.Count; i++) {
			roomObjects[i] = new GameObject();
			roomObjects[i].name = "Room " + i;
		}

		for (int y = 0; y < tiles.Length; y++) {
			for (int x = 0; x < tiles[y].Length; x++) {

				GameObject tile = null;
				GameObject room = null;

				for (int r = 0; r < rooms.Count; r++) {
					IntRect rectangle = rooms[r].rectangle;
					if (x >= rectangle.xMin && x <= rectangle.xMax && y >= rectangle.yMin && y <= rectangle.yMax) {
						room = roomObjects[r];
						break;
					}
				}

				switch (tiles[y][x]) {
					case TileType.FLOOR:
						tile = GameObject.Instantiate(tileset.floor, new Vector3(x, 0, y), Quaternion.identity);
						break;
					case TileType.WALL:
						tile = GameObject.Instantiate(tileset.walls[0], new Vector3(x, 1, y), Quaternion.identity);
						break;
					case TileType.DOOR:
						tile = GameObject.Instantiate(tileset.door, new Vector3(x, 1, y), Quaternion.identity);
						break;
				}

				if (tile != null && room != null) {
					tile.transform.parent = room.transform;
				}

				if (tile != null) {
					tileList.Add(tile);
				}
			}
		}
	}

	public List<GameObject> TileList {
		get {
			return tileList;
		}
	}
}