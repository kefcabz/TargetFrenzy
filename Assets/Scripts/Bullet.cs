using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 20f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider hitCollider = collision.collider;

        PrisonerHealth prisonerHealth = hitCollider.GetComponentInParent<PrisonerHealth>();

        if (prisonerHealth != null)
        {
            prisonerHealth.TakeHit(hitCollider);
        }

        Destroy(gameObject);
    }
}
