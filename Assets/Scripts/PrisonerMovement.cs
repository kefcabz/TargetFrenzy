using UnityEngine;

public class PrisonerMovement : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed = 10f;

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        Vector3 dir = targetPosition - transform.position;
        if (dir.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
