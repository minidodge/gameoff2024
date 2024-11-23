using System.Collections;
using UnityEngine;

public class Shrinker : MonoBehaviour {
    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float AnimationTime = 1f;
    [SerializeField] private float animationScale = 1f;
    [SerializeField] private float WaitSecondsToStart = 0f;
    [SerializeField] private bool playOnAwake = false;

    // Local state
    private float startTimer;
    private float timer;


    // Start is called before the first frame update
    void Start() {
        startTimer = WaitSecondsToStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playOnAwake) return;
        
        // Wait for our timer 
        startTimer -= Time.deltaTime;
        if (startTimer > 0) {
            return;
        }

        StartCoroutine(ShrinkRoutine());
    }
    
    public void InvokeShrinkRoutine() {
        StartCoroutine(ShrinkRoutine());
    }


    IEnumerator ShrinkRoutine() {
        var startingScale = transform.localScale;
        var endingScale = Vector3.zero;
        float t;

        while (timer < AnimationTime) {
            t = animationCurve.Evaluate(timer / AnimationTime);
            transform.localScale = Vector3.Lerp(startingScale, endingScale, t);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}