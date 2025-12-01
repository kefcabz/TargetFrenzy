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
        int pointsToAdd = 0;

        if (hitCollider == headCollider)
            pointsToAdd = headshotPoints;
        else if (hitCollider == bodyCollider)
            pointsToAdd = bodyshotPoints;

        GameManager.Instance.AddScore(pointsToAdd);

        OnPrisonerDeath?.Invoke();
        OnPrisonerDeath = null;

        Destroy(gameObject);
    }

}
