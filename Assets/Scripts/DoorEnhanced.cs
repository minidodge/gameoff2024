using System.Collections;
using UnityEngine;
using SUPERCharacter;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DoorEnhanced : MonoBehaviour, IInteractable
{
    public Vector3 rotationAngle = new(0, -105, 0);
    public bool reverse, doorOpen = false;
    public float doorMoveDuration = 0.4f; // Time in seconds that the door takes to open / close
    private bool _isOpenInProgress = false;

    [Tooltip("If true, run the default door open routine. If false, only run the OnInteract events.")]
    public bool shouldOpenDoorOnInteract = true;

    [Tooltip("Set to false to disable all interaction and events.")]
    public bool isInteractable = true;

    [Tooltip("If true, the door will only open once and will not close.")]
    public bool isOneTimeUse = false;

    public UnityEvent onInteract;
    public UnityEvent onOpenStart;
    public UnityEvent onOpenComplete;
    public UnityEvent onCloseStart;
    public UnityEvent onCloseComplete;

    // Allow interaction via UnityEvents
    public void InvokeInteract()
    {
        Interact();
    }

    public bool Interact()
    {
        if (!isInteractable) return false;
        if (_isOpenInProgress) return false;

        // Run the default door opener, if configured to do so.
        if (shouldOpenDoorOnInteract)
        {
            _isOpenInProgress = true;
            StartCoroutine(OpenDoor());
        }

        // Run the other OnInteract events.
        if (onInteract != null)
        {
            onInteract.Invoke();
        }

        if(isOneTimeUse) isInteractable = false;

        return true;
    }

    IEnumerator OpenDoor()
    {
        float elapsedTime = 0;
        Vector3 rotationToApply = rotationAngle;
        if (reverse) rotationToApply *= -1;

        // Calculate the target rotation at the end of the animation
        var initialRotation = transform.localRotation;
        var targetRotation = Quaternion.Euler(transform.localEulerAngles + rotationToApply);

        // Trigger the appropriate start event
        if (doorOpen)
        {
            onCloseStart?.Invoke();
        }
        else
        {
            onOpenStart?.Invoke();
        }

        while (elapsedTime < doorMoveDuration)
        {
            float alpha = elapsedTime / doorMoveDuration;
            transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is set
        transform.localRotation = targetRotation;

        // Trigger the appropriate complete event
        if (doorOpen)
        {
            onCloseComplete?.Invoke();
        }
        else
        {
            onOpenComplete?.Invoke();
        }

        reverse = !reverse;
        doorOpen = !doorOpen;
        _isOpenInProgress = false;
    }
}
