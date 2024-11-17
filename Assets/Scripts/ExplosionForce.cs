using UnityEngine;

public class ExplosionForce : MonoBehaviour
{
    public float explosionForce = 1000f;
    public float explosionRadius = 5f;
    public float upwardsModifier = 1f;
    public ForceMode forceMode = ForceMode.Impulse;

    public void ApplyExplosionForce(Vector3 explosionPosition)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, forceMode);
        }
    }
    
    public void InvokeApplyExplosionForceAtPosition()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, forceMode);
        }
    }
}