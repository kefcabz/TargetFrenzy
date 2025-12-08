using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class PrisonerSpawner : MonoBehaviour
{
    public GameObject prisonerPrefab;
    public Transform courtyardCenter;
    public Transform[] roadTargets;
    public Transform watchTower;
    public float spawnRadius = 350f;
    public int startingPrisoners = 5;
    public float waveDelay = 4f; 
    public float speedIncreasePerWave = 2.5f;
    public TextMeshProUGUI waveCountdownText;
    public TextMeshProUGUI waveNumberText; 
    public int currentWave = 1;
    private int prisonersPerWave;
    private int prisonersRemaining;
    private bool waveInProgress = false;
    private bool countdownRunning = false;
    public TextMeshProUGUI prisonersRemainingText;

    void Start()
    {
        prisonersPerWave = startingPrisoners;
        if (prisonersRemainingText != null)
        {
            prisonersRemainingText.gameObject.SetActive(false);
        }

        UpdatePrisonersRemainingUI();
    }

    public void StartWaves()
    {
        StartCoroutine(StartWaveWithDelay());
    }

    private IEnumerator StartWaveWithDelay()
    {
        yield return null;

        if (prisonersRemainingText != null)
            prisonersRemainingText.gameObject.SetActive(true);

        StartNextWave();
    }


    void StartNextWave()
    {
        if (waveInProgress)
            return;

        waveInProgress = true;
        prisonersRemaining = prisonersPerWave;

        UpdatePrisonersRemainingUI() ;

        if (waveNumberText != null)
            waveNumberText.text = "Wave: " + currentWave;

        for (int i = 0; i < prisonersPerWave; i++)
        {
            SpawnPrisoner();
        }
    }


    void SpawnPrisoner()
    {
        Vector3 spawnPos = Vector3.zero;
        bool validSpawn = false;
        int attempts = 0;
        float minDistanceFromTower = 5f; 

        while (!validSpawn && attempts < 20)
        {
            Vector3 offset = Random.insideUnitSphere * spawnRadius;
            offset.y = 0f;
            Vector3 randomPos = courtyardCenter.position + offset;

            if (watchTower != null && Vector3.Distance(randomPos, watchTower.position) < minDistanceFromTower)
            {
                attempts++;
                continue; 
            }

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
            {
                spawnPos = hit.position;
                spawnPos.y += 0.5f; 
                validSpawn = true;
            }
            else
            {
                attempts++;
            }
        }

        GameObject prisoner = Instantiate(prisonerPrefab, spawnPos, Quaternion.identity);

        NavMeshAgent agent = prisoner.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(spawnPos);
            agent.ResetPath();
        }

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
                UpdatePrisonersRemainingUI();

                if (prisonersRemaining <= 0 && waveInProgress && !countdownRunning)
                {
                    waveInProgress = false;
                    countdownRunning = true;
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
            prisonersRemainingText.gameObject.SetActive(false);
            for (int i = Mathf.RoundToInt(waveDelay); i > 0; i--)
            {
                waveCountdownText.text = "Next wave in " + i;
                yield return new WaitForSeconds(1f);
            }
            waveCountdownText.gameObject.SetActive(false);
            prisonersRemainingText.gameObject.SetActive(true); 
        }
        countdownRunning = false;

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

    private void UpdatePrisonersRemainingUI()
    {
        if (prisonersRemainingText != null)
        {
            prisonersRemainingText.text = "Prisoners Remaining: " + prisonersRemaining;
        }
    }

}
