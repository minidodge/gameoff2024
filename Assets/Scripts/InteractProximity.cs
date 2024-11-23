using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class InteractProximity : MonoBehaviour
{
    public LayerMask layerMask;
    public List<string> tags;
    public UnityEvent onTriggerEnterEvent;
    public UnityEvent onTriggerExitEvent;

    void OnTriggerEnter(Collider other)
    {
        if (Utilities.IsInLayerMask(other.gameObject.layer, layerMask) || tags.Contains(other.gameObject.tag))
        {
            onTriggerEnterEvent.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (Utilities.IsInLayerMask(other.gameObject.layer, layerMask) || tags.Contains(other.gameObject.tag))
        {
            onTriggerExitEvent.Invoke();
        }
    }
}