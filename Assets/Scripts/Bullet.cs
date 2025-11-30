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
    if (collision.gameObject.CompareTag("Prisoner"))
    {
        PrisonerHealth prisonerHealth = collision.gameObject.GetComponent<PrisonerHealth>();
        if (prisonerHealth != null)
        {
            prisonerHealth.TakeHit(collision.contacts[0].thisCollider);
        }

        Destroy(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}

}
