using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityManager : MonoBehaviour {

	private List<VisibilityItem>[] items;
	private bool[] isVisible;

	public void SetRoomCount(int roomCount) {
		items = new List<VisibilityItem>[roomCount];
		isVisible = new bool[roomCount];
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
		if (isVisible[roomIndex])
			return;

		items[roomIndex].ForEach(item => item.Show());
		isVisible[roomIndex] = true;
	}

	public static VisibilityManager GetInstance() {
		return GameObject.FindGameObjectWithTag("VisibilityManager").GetComponent<VisibilityManager>();
	}
}
