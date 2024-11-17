using SUPERCharacter;
using UnityEngine;
using UnityEngine.Events;

public class InteractEvents : MonoBehaviour, IInteractable
{
    [Tooltip("Set to false to disable all interaction and events.")]
    public bool isInteractable = true;

    [Tooltip("If true, the door will only open once and will not close.")]
    public bool isOneTimeUse = false;

    public UnityEvent onInteract;
    
    // Allow interaction via UnityEvents
    public void InvokeInteract()
    {
        Interact();
    }
    
    public bool Interact()
    {
        if (!isInteractable) return false;

        // Run the other OnInteract events.
        if (onInteract != null)
        {
            onInteract.Invoke();
        }

        if(isOneTimeUse) isInteractable = false;

        return true;
    }

}
