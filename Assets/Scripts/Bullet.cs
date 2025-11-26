using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 20f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
 
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Prisoner"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
