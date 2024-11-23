using System;
using System.Collections;
using System.Timers;
// using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Punch : MonoBehaviour {
    // Configuration
    [Header("Punch with linear motion")]
    [SerializeField] private float punchAmount = 0.3f;
    [SerializeField] private float punchSpeed = 10f;

    [Header("Punch with animation curve")]
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private float AnimationTime = 1f;
    [Tooltip("How many times the original size to grow to")]
    [SerializeField] private float animationScale = 2f;
    
    [Header("Events")]
    [SerializeField] private UnityEvent PunchComplete;

    // State
    private bool isPunching = false;
    
    private float timer = 0f;

    private void OnEnable() {
        isPunching = false;
    }

    private void OnDisable() {
        isPunching = false;
    }

    // [Button]
    // Start a punch!
    public void DoPunch() {
        if (!isPunching) {
            isPunching = true;
            StartCoroutine(PunchRoutine(punchAmount, punchSpeed));
        }
    }
    
    // [Button]
    // Start a punch!
    public void DoPunchCurved() {
        if (!isPunching) {
            isPunching = true;
            StartCoroutine(PunchRoutineNew(punchSpeed));
        }
    }

    IEnumerator PunchRoutine(float amount, float speed) {
        var startingScale = gameObject.transform.localScale;
        var startingX = startingScale.x;
        var endingX = startingX + amount;

        // Grow
        while (gameObject.transform.localScale.x < endingX) {
            gameObject.transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * speed;
            yield return null;
        }

        // Shrink
        while (gameObject.transform.localScale.x > startingX) {
            gameObject.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime * speed;
            timer += Time.deltaTime;
            yield return null;
        }

        // Restore original scale in case of any step inconsistencies
        gameObject.transform.localScale = startingScale;

        isPunching = false;
        
        // Invoke event
        PunchComplete.Invoke();
    }
    
    
    IEnumerator PunchRoutineNew(float speed) {
        var startingScale = transform.localScale;
        var endingScale = startingScale * animationScale;
        float t;
        
        while (timer < AnimationTime) {
            t = animationCurve.Evaluate(timer / AnimationTime);
            transform.localScale = Vector3.Lerp(startingScale, endingScale, t);
            timer += Time.deltaTime;
            yield return null;
        }
        
        // Restore original scale in case of any step inconsistencies
        transform.localScale = startingScale;
        
        // Reset timer and state vars
        isPunching = false;
        timer = 0f;
        
        // Invoke event
        PunchComplete.Invoke();
    }
}