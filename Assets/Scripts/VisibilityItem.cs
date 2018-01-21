using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class VisibilityItem : MonoBehaviour {
    private float fadeInTime = 0.75f;
    private MeshRenderer renderer;
    private bool isVisible;

    void Awake() {
        renderer = GetComponent<MeshRenderer>();
        isVisible = false;
        Hide();
    }

    public void Hide() {
        renderer.material.color = Color.clear;
        isVisible = false;
    }

    public void Show() {
        if (isVisible) return;
        StartCoroutine(FadeInCoroutine());
    }
    
    private IEnumerator FadeInCoroutine() {
        isVisible = true;
        float elapsedTime = 0;

        while (elapsedTime < fadeInTime) {
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / fadeInTime;
            Color fadeInColor = new Color(ratio, ratio, ratio, ratio);
            renderer.material.color = fadeInColor;

            yield return null;
        }

        yield break;
    }
}