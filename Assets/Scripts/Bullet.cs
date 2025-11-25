using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 10f;
    public float airTime = 0;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        Vector3 velocity = new Vector3 (speed * Time.deltaTime,0, 0);
        airTime += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Prisoner"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            airTime = 0;
        }
        else
        {
            Destroy(gameObject);
            airTime = 0;
        }
    }
}
