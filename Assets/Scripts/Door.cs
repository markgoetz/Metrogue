using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Door : MonoBehaviour {
	private int _room1, _room2;
	private VisibilityManager _visibilityManager;

	void Start() {
		_visibilityManager = VisibilityManager.GetInstance();
	}

	public void SetAdjacentRooms(int room1, int room2) {
		_room1 = room1;
		_room2 = room2;
	}

	public void Open() {
		_visibilityManager.ShowItemsInRoom(_room1);
		_visibilityManager.ShowItemsInRoom(_room2);
	}

	public void Damageable_OnDestroy() {
		Open();
	}
}
