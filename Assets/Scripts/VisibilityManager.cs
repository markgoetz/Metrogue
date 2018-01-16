using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : MonoBehaviour {

	private List<VisibilityItem>[] items;

	public void SetRoomCount(int roomCount) {
		items = new List<VisibilityItem>[roomCount];
		for (int i = 0; i < roomCount; i++) {
			items[i] = new List<VisibilityItem>();
		}
	}

	public void AddItemToRoom(VisibilityItem item, int roomIndex) {
		if (roomIndex < 0 || roomIndex >= items.Length)
			throw new System.Exception("Room index is out of bounds");

		items[roomIndex].Add(item);
	}

	public void ShowItemsInRoom(int roomIndex) {
		items[roomIndex].ForEach(item => item.Show());
	}

	public static VisibilityManager GetInstance() {
		return GameObject.FindGameObjectWithTag("VisibilityManager").GetComponent<VisibilityManager>();
	}
}
