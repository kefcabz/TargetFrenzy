using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class PrisonerSpawner : MonoBehaviour
{
    public GameObject prisonerPrefab;
    public Transform courtyardCenter;
    public Transform[] roadTargets;
    public float spawnRadius = 100f;
    public int startingPrisoners = 5;
    public float waveDelay = 4f; // next wave delay
    public float speedIncreasePerWave = 1.5f;
    public TextMeshProUGUI waveCountdownText; 
    private int currentWave = 1;
    private int prisonersPerWave;
    private int prisonersRemaining; 
    private bool waveInProgress = false;

    void Start()
    {
        prisonersPerWave = startingPrisoners;
        StartNextWave();
    }

    void StartNextWave()
    {
        if (waveInProgress)
            return;

        waveInProgress = true;
        prisonersRemaining = prisonersPerWave;

        for (int i = 0; i < prisonersPerWave; i++)
        {
            SpawnPrisoner();
        }
    }

    void SpawnPrisoner()
    {
        Vector3 offset = Random.insideUnitSphere * spawnRadius;
        offset.y = 0f;

        Vector3 spawnPos = courtyardCenter.position + offset;
        GameObject prisoner = Instantiate(prisonerPrefab, spawnPos, Quaternion.identity);

        NavMeshAgent agent = prisoner.GetComponent<NavMeshAgent>();
        if (agent != null)
            agent.Warp(spawnPos);

        PrisonerMovement pm = prisoner.GetComponent<PrisonerMovement>();
        Transform nearestRoad = GetNearestRoad(spawnPos);
        pm.targetPosition = nearestRoad.position;

        pm.speed += (currentWave - 1) * speedIncreasePerWave;

        PrisonerHealth ph = prisoner.GetComponent<PrisonerHealth>();
        if (ph != null)
        {
            ph.OnPrisonerDeath += () =>
            {
                prisonersRemaining--;

                // Only when all prisoners in the wave are dead
                if (prisonersRemaining <= 0 && waveInProgress)
                {
                    waveInProgress = false;
                    prisonersPerWave += 2;
                    currentWave++;
                    StartCoroutine(WaveCountdownCoroutine());
                }
            };
        }
    }

    IEnumerator WaveCountdownCoroutine()
    {
        if (waveCountdownText != null)
        {
            waveCountdownText.gameObject.SetActive(true);

            for (int i = Mathf.RoundToInt(waveDelay); i > 0; i--)
            {
                waveCountdownText.text = "Next wave in " + i;
                yield return new WaitForSeconds(1f);
            }

            waveCountdownText.gameObject.SetActive(false);
        }

        StartNextWave();
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
