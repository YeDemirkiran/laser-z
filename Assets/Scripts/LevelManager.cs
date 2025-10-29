using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Transform enemies;
    public Transform collectables;
    public float speed = 10f;
    public float zombieSpawnRate = 2f;

    public GameObject zombiePrefab;

    public Transform[] seritler;

    float zombieSpawnTimer = 0f;

    int previousDecision = -5;

    void Update()
    {
        SpawnZombie();
        MoveChildren();
    }

    void SpawnZombie()
    {
        if (zombieSpawnTimer > zombieSpawnRate)
        {
            int decision = previousDecision;
            while (previousDecision == decision)
                decision = Random.Range(0, 3);
            previousDecision = decision;

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
