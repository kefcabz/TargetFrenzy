using UnityEngine;
using System.Collections;

public class FenceTrigger : MonoBehaviour
{
    public GameObject explosionVFX;
    public PrisonerSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prisoner"))
        {
            StartCoroutine(HandleFenceHit(other.gameObject));
        }
    }

    private IEnumerator HandleFenceHit(GameObject prisoner)
    {
        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, prisoner.transform.position, Quaternion.identity);
        }

        Destroy(prisoner);

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);

        if (spawner != null)
            GameManager.Instance.GameOver(spawner.currentWave);
        else
            GameManager.Instance.GameOver(1);
    }
}
