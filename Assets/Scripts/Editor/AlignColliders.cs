#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class AlignColliders : MonoBehaviour {
    [MenuItem("Tools/Align Colliders")]
    public static void AlignSelectedColliders() {
        GameObject[] selectedObjects = Selection.gameObjects;
        if (selectedObjects.Length != 2) {
            Debug.LogError("Select exactly two objects!");
            return;
        }

        Collider objectCollider = selectedObjects[0].GetComponent<Collider>();
        Collider targetCollider = selectedObjects[1].GetComponent<Collider>();

        if (objectCollider != null && targetCollider != null) {
            Vector3 targetTop = targetCollider.bounds.max;
            Vector3 objectBottom = objectCollider.bounds.min;

            Vector3 offset = selectedObjects[0].transform.position - objectBottom;
            selectedObjects[0].transform.position = targetTop + offset;
        }
    }
}
#endif