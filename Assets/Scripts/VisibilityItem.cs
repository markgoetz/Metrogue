using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class VisibilityItem : MonoBehaviour {
    private MeshRenderer renderer;
    void Awake() {
        renderer = GetComponent<MeshRenderer>();
        Hide();
    }

    public void Hide() {
        renderer.material.color = Color.clear;
    }

    public void Show() {
        renderer.material.color = Color.white;
    }
}