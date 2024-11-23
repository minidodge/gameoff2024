using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour {
    [SerializeField] private Material outlineMaterial;
    private List<Material[]> originalMaterials = new List<Material[]>();
    private List<Renderer> renderers = new List<Renderer>();

    void Start() {
        GetRenderers(transform);
    }

    private void GetRenderers(Transform parent) {
        Renderer parentRenderer = parent.GetComponent<Renderer>();
        if (parentRenderer != null) {
            renderers.Add(parentRenderer);
            originalMaterials.Add(parentRenderer.materials);
        }

        foreach (Transform child in parent) {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null) {
                renderers.Add(childRenderer);
                originalMaterials.Add(childRenderer.materials);
            }
            GetRenderers(child);
        }
    }

    public void ApplyOutline() {
        foreach (Renderer renderer in renderers) {
            List<Material> materials = new List<Material>(renderer.materials);
            materials.Add(outlineMaterial);
            renderer.materials = materials.ToArray();
        }
    }

    public void RemoveOutline() {
        for (int i = 0; i < renderers.Count; i++) {
            renderers[i].materials = originalMaterials[i];
        }
    }
}