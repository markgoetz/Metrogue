using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
	public Tileset tileset;
	public IntPoint mapSize;
	public IntPoint minRoomSize;
	public IntPoint maxRoomSize;
	public float mapFullness;

	private TileType[][] tiles;
	private List<Room> rooms;
	private IntPoint origin;
	private AdjacencyGraph adjacency;
	private TileRenderer tileRenderer;
	private VisibilityManager visibilityManager;

	void Start () {
		Init();
	}

	public void GenerateMap(IntPoint origin) {
		this.origin = origin;
		GenerateRooms();
		CalculateAdjacency();
		CalculateRoomDifficulty();
		PlaceDoors();
		RenderTiles();
		SetVisibility();
	}

	void Init() {
		rooms = new List<Room>();
		tiles = new TileType[mapSize.y][];

		for (int i = 0; i < mapSize.y; i++) {
			tiles[i] = new TileType[mapSize.x];
		}

		tileRenderer = new TileRenderer(rooms);
		visibilityManager = VisibilityManager.GetInstance();
	}

	void GenerateRooms() {
		float fullness = 0f;
		while (fullness < mapFullness && IsAnySpaceAvailable()) {
			Room room = GenerateRoom();
			rooms.Add(room);
			PlaceRoomTiles(room);
			fullness = GetFullness();
		}
	}

	Room GenerateRoom() {
		bool canPlaceRoom = false;
		int roomX = 0, roomY = 0, width = 0, height = 0;
		IntRect roomRect = new IntRect(0, 0, 0, 0);

		while (!canPlaceRoom) {
			width = Random.Range(minRoomSize.x, maxRoomSize.x);
			height = Random.Range(minRoomSize.y, maxRoomSize.y);

			if (rooms.Count > 0) {
				Room seedRoom = rooms[Random.Range(0, rooms.Count)];
				IntRect seedRect = seedRoom.rectangle;

				int direction = Random.Range(0, 3);
				switch (direction) {
					case 0: // up
						roomX = Random.Range(seedRect.xMin - width + 1, seedRect.xMax);
						roomY = seedRect.yMin - (height + 1);
						break;
					case 1: // right
						roomX = seedRect.xMax + 2;
						roomY = Random.Range(seedRect.yMin - height + 1, seedRect.yMax);
						break;
					case 2: // down
						roomX = Random.Range(seedRect.xMin - width + 1, seedRect.xMax);
						roomY = seedRect.yMax + 2;
						break;
					case 3: // left
						roomX = seedRect.xMin - (width + 1);
						roomY = Random.Range(seedRect.yMin - height + 1, seedRect.yMax);
						break;
				}
			}

			else {
				roomX = origin.x - Random.Range(0, width);
				roomY = origin.y - Random.Range(0, height);
			}

			roomRect = new IntRect(roomX, roomX + width - 1, roomY, roomY + height - 1);
			canPlaceRoom = IsSpaceClear(roomRect);
		}

		Room newRoom = new Room(roomRect);
		return newRoom;
	}

	bool IsAnySpaceAvailable() {
		for (int y = 1; y < mapSize.y - (minRoomSize.y + 1); y++) {
			for (int x = 1; x < mapSize.x - (minRoomSize.x + 1); x++) {
				IntRect testRect = new IntRect(x, x + minRoomSize.x - 1, y, y + minRoomSize.y - 1);
				if (IsSpaceClear(testRect)) {
					return true;
				}
			}
		}
		return false;
	}

	bool IsSpaceClear(IntRect rectangle) {
		for (int y = rectangle.yMin - 1; y <= rectangle.yMax + 1; y++) {
			if (y < 0 || y >= tiles.Length) return false;
			for (int x = rectangle.xMin - 1; x <= rectangle.xMax + 1; x++) {
				if (x < 0 || x >= tiles[y].Length) return false;
				if (tiles[y][x] != TileType.EMPTY && tiles[y][x] != TileType.WALL) return false;
			}
		}
		
		return true;
	}

	void PlaceRoomTiles(Room room) {
		for (int x = room.rectangle.xMin - 1; x <= room.rectangle.xMax + 1; x++) {
			for (int y = room.rectangle.yMin - 1; y <= room.rectangle.yMax + 1; y++) {

				if (x >= room.rectangle.xMin && x <= room.rectangle.xMax &&
					y >= room.rectangle.yMin && y <= room.rectangle.yMax)
					tiles[y][x] = TileType.FLOOR;
				else
					tiles[y][x] = TileType.WALL;
			}
		}
	}

	void CalculateAdjacency() {
		this.adjacency = new AdjacencyGraph(rooms);
	}

	void CalculateRoomDifficulty() {
		if (this.adjacency == null) {
			throw new System.Exception("Adjacency Graph was not initialized.");
		}

		int[] distances = this.adjacency.GetDistances();
		int maxDistance = distances.Max();

		for (int i = 0; i < distances.Length; i++) {
			rooms[i].difficulty = distances[i] / maxDistance;
		}
	}

	void PlaceDoors() {
		if (this.adjacency == null) {
			throw new System.Exception("Adjacency Graph was not initialized.");
		}

		for (int i = 0; i < rooms.Count; i++) {
			List<int> adjacentRooms = this.adjacency.GetAdjacentRooms(i);

			adjacentRooms.ForEach(nextIndex => {
				if (nextIndex < i) {
					AddDoor(rooms[i].rectangle, rooms[nextIndex].rectangle);
				}
			});
		}
	}

	void AddDoor(IntRect firstRect, IntRect secondRect) {
		int doorX = -1, doorY = -1;

		if (firstRect.xMin == secondRect.xMax + 2 && !(secondRect.yMin > firstRect.yMax || secondRect.yMax < firstRect.yMin)) {
			doorX = secondRect.xMax + 1;
		} else if (secondRect.xMin == firstRect.xMax + 2 && !(secondRect.yMin > firstRect.yMax || secondRect.yMax < firstRect.yMin)) {
			doorX = firstRect.xMax + 1;
		} else if (firstRect.yMin == secondRect.yMax + 2 && !(secondRect.xMin > firstRect.xMax || secondRect.xMax < firstRect.xMin)) {
			doorY = secondRect.yMax + 1;
		} else if (secondRect.yMin == firstRect.yMax + 2 && !(secondRect.xMin > firstRect.xMax || secondRect.xMax < firstRect.xMin)) {
			doorY = firstRect.yMax + 1;
		}

		if (doorX == -1) {
			int minX = Mathf.Min(firstRect.xMax, secondRect.xMax);
			int maxX = Mathf.Max(firstRect.xMin, secondRect.xMin);
			doorX = Random.Range(minX, maxX + 1);
		} else if (doorY == -1) {
			int minY = Mathf.Min(firstRect.yMax, secondRect.yMax);
			int maxY = Mathf.Max(firstRect.yMin, secondRect.yMin);
			doorY = Random.Range(minY, maxY + 1);
		}

		tiles[doorY][doorX] = TileType.DOOR;
	}

	void RenderTiles() {
		tileRenderer.Render(tiles, tileset);
	}

	void SetVisibility() {
		visibilityManager.SetRoomCount(rooms.Count);
		List<GameObject> tileList = tileRenderer.TileList;

		foreach (GameObject tile in tileList) {
			if (!tile.GetComponent<VisibilityItem>())
				continue;

			for (int i = 0; i < rooms.Count; i++) {
				Room room = rooms[i];

				if (tile.transform.position.x >= room.rectangle.xMin - 1 &&
					tile.transform.position.x <= room.rectangle.xMax + 1 &&
					tile.transform.position.z >= room.rectangle.yMin - 1 &&
					tile.transform.position.z <= room.rectangle.yMax + 1) {
						visibilityManager.AddItemToRoom(tile.GetComponent<VisibilityItem>(), i);
				}
			}
		}

		visibilityManager.ShowItemsInRoom(0);
	}

	float GetFullness() {
		int tileCount = 0;
		for (int x = 0; x < mapSize.x; x++) {
			for (int y = 0; y < mapSize.y; y++) {
				if (tiles[y][x] != TileType.EMPTY) {
					tileCount++;
				}
			}
		}
		float fullness = (float)tileCount / (float)(mapSize.x * mapSize.y);
		return fullness;
	}
}
