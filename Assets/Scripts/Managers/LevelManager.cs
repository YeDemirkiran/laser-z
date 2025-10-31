using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] Transform enemiesParent;
    [SerializeField] Transform collectablesParent;
    [SerializeField] Transform[] lines;
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] GameObject collectablePrefab;

    [Header("Level Building")]
    [SerializeField] float zombieSpawnRate = 2f;
    [SerializeField] float collectableSpawnRate = 7f;
    [SerializeField] float levelSpeed = 10f;
    [SerializeField] float scoreIncreaseSpeed = 1f;

    float zombieSpawnTimer;
    float collectableSpawnTimer;
    int previousZombieDecision;
    bool levelRunning;

    public float Score { get; private set; } = 0f;

    void Update()
    {
        if (!levelRunning)
            return;
        SpawnCollectable();
        SpawnZombie();
        MoveChildren();
        UpdateScore();
    }

    void UpdateScore()
    {
        Score += scoreIncreaseSpeed * Time.deltaTime;
    }

    void SpawnCollectable()
    {
        if (collectableSpawnTimer > collectableSpawnRate)
        {
            int decision = Random.Range(0, 3);

            for (int i = 0; i <= decision; i++)
            {
                GameObject collectable = Instantiate(collectablePrefab);
                Destroy(collectable, 10f);
                collectable.transform.position = lines[i].position;
                collectable.transform.SetParent(collectablesParent);
                collectableSpawnTimer = 0f;
            }       
        }
        else
        {
            collectableSpawnTimer += Time.deltaTime;
        }
    }

    void SpawnZombie()
    {
        if (zombieSpawnTimer > zombieSpawnRate)
        {
            int decision = previousZombieDecision;
            while (previousZombieDecision == decision)
                decision = Random.Range(0, 3);
            previousZombieDecision = decision;

            GameObject zombie = Instantiate(zombiePrefab);
            Destroy(zombie, 10f);
            zombie.transform.position = lines[decision].position + (Vector3.right * Random.Range(-1f, 1f));
            zombie.transform.SetParent(enemiesParent);
            zombieSpawnTimer = 0f;
        }
        else
        {
            zombieSpawnTimer += Time.deltaTime;
        }
    }

    void MoveChildren()
    {
        foreach (Transform enemy in enemiesParent)
        {
            enemy.Translate(new Vector3(0f, 0f, levelSpeed * Time.deltaTime));
        }
        foreach (Transform collectable in collectablesParent)
        {
            collectable.Translate(new Vector3(0f, 0f, levelSpeed * Time.deltaTime));
        }
    }

    public void StartLevel()
    {
        levelRunning = true;
    }

    public void StopLevel()
    {
        levelRunning = false;
    }
}
