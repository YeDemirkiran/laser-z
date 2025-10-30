using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform enemies;
    public Transform collectables;
    public float speed = 10f;
    public float scoreIncreaseSpeed = 1f;
    public float zombieSpawnRate = 2f;
    public float collectableSpawnRate = 7f;

    public GameObject zombiePrefab;
    public GameObject collectablePrefab;

    public Transform[] seritler;

    float zombieSpawnTimer = 0f;
    float collectableSpawnTimer = 0f;

    int previousZombieDecision;

    public float Score { get; private set; } = 0f;

    void Update()
    {
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
                collectable.transform.position = seritler[i].position;
                collectable.transform.SetParent(collectables);
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
            zombie.transform.position = seritler[decision].position + (Vector3.right * Random.Range(-1f, 1f));
            zombie.transform.SetParent(enemies);
            zombieSpawnTimer = 0f;
        }
        else
        {
            zombieSpawnTimer += Time.deltaTime;
        }
    }

    void MoveChildren()
    {
        foreach (Transform enemy in enemies)
        {
            enemy.Translate(new Vector3(0f, 0f, speed * Time.deltaTime));
        }
        foreach (Transform collectable in collectables)
        {
            collectable.Translate(new Vector3(0f, 0f, speed * Time.deltaTime));
        }
    }
}
