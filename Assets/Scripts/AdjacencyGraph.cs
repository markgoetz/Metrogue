using System.Collections.Generic;
using UnityEngine;

public class AdjacencyGraph {
	private List<int>[] graph;
	private int[] distances;

	public AdjacencyGraph(List<Room> rooms) {
		_Init(rooms);
		_CalculateAdjacency(rooms);
		_CalculateDistances(rooms);
	}

	private void _Init(List<Room> rooms) {
		graph = new List<int>[rooms.Count];
		distances = new int[rooms.Count];
	}

	private void _CalculateAdjacency(List<Room> rooms) {
		for (int firstIndex = 0; firstIndex < rooms.Count; firstIndex++) {
			graph[firstIndex] = new List<int>();
			for (int secondIndex = 0; secondIndex < rooms.Count; secondIndex++) {
				if (firstIndex == secondIndex)
					continue;

				IntRect firstRect = rooms[firstIndex].rectangle;
				IntRect secondRect = rooms[secondIndex].rectangle;

				if (_IsRectAdjacent(firstRect, secondRect)) {
					graph[firstIndex].Add(secondIndex);
				}
			}
		}
	}

	private bool _IsRectAdjacent(IntRect firstRect, IntRect secondRect) {
		if ((secondRect.xMin == firstRect.xMax + 2 || firstRect.xMin == secondRect.xMax + 2) &&
			!(secondRect.yMin > firstRect.yMax || secondRect.yMax < firstRect.yMin))
			return true;

		if ((secondRect.yMin == firstRect.yMax + 2 || firstRect.yMin == secondRect.yMax + 2) &&
			!(secondRect.xMin > firstRect.xMax || secondRect.xMax < firstRect.xMin))
			return true;

		return false;
	}

	private void _CalculateDistances(List<Room> rooms) {
		for (int i = 0; i < rooms.Count; i++) {
			distances[i] = (i == 0) ? 0 : 99999;
		}

		List<int> roomsToProcess = new List<int>();
		roomsToProcess.Add(0);
		List<int> roomsProcessed = new List<int>();

		while (roomsToProcess.Count > 0) {
			int roomIndex = roomsToProcess[0];
			roomsToProcess.RemoveAt(0);

			List<int> adjacentRooms = GetAdjacentRooms(roomIndex);
			adjacentRooms.ForEach(nextRoom => {
				if (!roomsProcessed.Contains(nextRoom)) {
					roomsToProcess.Add(nextRoom);
					if (distances[nextRoom] > distances[roomIndex] + 1)
						distances[nextRoom] = distances[roomIndex] + 1;
				}
			});
			roomsProcessed.Add(roomIndex);
		}
	}

	public List<int> GetAdjacentRooms(int roomIndex) {
		return graph[roomIndex];
	}

	public int[] GetDistances() {
		return distances;
	}
}