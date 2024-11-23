using System.Collections;
using UnityEngine;

public class BalloonHighlight : MonoBehaviour {
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float animationTime = .4f;
    [SerializeField] private float scaleMultiplier = 1.25f;

    private Vector3 originalScale;
    private Coroutine currentCoroutine;

    void Start() {
        originalScale = transform.localScale;
    }

    public void ScaleUp() {
        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ScaleRoutine(originalScale * scaleMultiplier));
    }

    public void ScaleDown() {
        if (currentCoroutine != null) {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(ScaleRoutine(originalScale));
    }

    private IEnumerator ScaleRoutine(Vector3 targetScale) {
        Vector3 startingScale = transform.localScale;
        float timer = 0f;

        while (timer < animationTime) {
            float t = animationCurve.Evaluate(timer / animationTime);
            transform.localScale = Vector3.Lerp(startingScale, targetScale, t);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}