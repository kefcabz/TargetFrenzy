using UnityEngine;
using System;

public class PrisonerHealth : MonoBehaviour
{
    public int headshotPoints = 100;
    public int bodyshotPoints = 50;
    public Collider headCollider;
    public Collider bodyCollider;

    public event Action OnPrisonerDeath; 

    public void TakeHit(Collider hitCollider)
    {
        if (hitCollider == headCollider)
        {
            GameManager.Instance.AddScore(headshotPoints);
        }
        else if (hitCollider == bodyCollider)
        {
            GameManager.Instance.AddScore(bodyshotPoints);
        }

        OnPrisonerDeath?.Invoke();
        OnPrisonerDeath = null; 

        Destroy(gameObject);
    }
}
