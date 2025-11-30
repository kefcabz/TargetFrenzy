using UnityEngine;
using UnityEngine.AI;

public class PrisonerSpawner : MonoBehaviour
{
    public GameObject prisonerPrefab;
    public int numberToSpawn = 5;
    public Transform courtyardCenter;
    public Transform[] roadTargets;
    public float spawnRadius = 100f;

    void Start()
    {
        SpawnPrisoners();
    }

    void SpawnPrisoners()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 offset = Random.insideUnitSphere * spawnRadius;
            offset.y = 0f;

            Vector3 spawnPos = courtyardCenter.position + offset;

            GameObject prisoner = Instantiate(prisonerPrefab, spawnPos, Quaternion.identity);
            NavMeshAgent agent = prisoner.GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.Warp(spawnPos);  

            Transform nearestRoad = GetNearestRoad(spawnPos);
            prisoner.GetComponent<PrisonerMovement>().targetPosition = nearestRoad.position;
        }
    }

    Transform GetNearestRoad(Vector3 pos)
    {
        Transform best = roadTargets[0];
        float bestDist = Vector3.Distance(pos, best.position);

        foreach (Transform road in roadTargets)
        {
            float d = Vector3.Distance(pos, road.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = road;
            }
        }

        return best;
    }
}
