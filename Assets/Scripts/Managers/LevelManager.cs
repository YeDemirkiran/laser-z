using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] Transform enemiesParent;
    [SerializeField] Transform collectablesParent;
    [SerializeField] Transform[] lines;
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] GameObject[] collectablePrefab;
    [SerializeField] Renderer groundRenderer;
    Material groundMaterial;

    [Header("Level Building")]
    [SerializeField] float zombieSpawnRate = 2f;
    [SerializeField] float maxZombieSpawnRate = 0.5f;
    [SerializeField] float zombieSpawnRateChanger = 0.05f;
    [SerializeField] float collectableSpawnRate = 7f;

    [Header("Level Speed")]
    float levelSpeed = 10f;
    [SerializeField] float startingSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float scoreIncreaseSpeed = 1f;

    [Header("Other")]
    [SerializeField] string bestScorePlayerPrefsName;

    float zombieSpawnTimer;
    float collectableSpawnTimer;
    int previousZombieDecision;

    bool levelRunning;
    public bool IsLevelRunning => levelRunning;

    public float Score { get; private set; } = 0f;

    private void Start()
    {
        zombieSpawnTimer = zombieSpawnRate;
        collectableSpawnTimer = collectableSpawnRate / 2f;

        groundMaterial = groundRenderer.material;
    }

    void Update()
    {
        if (!levelRunning)
            return;

        SpawnCollectable();
        SpawnZombie();
        SetSpeed();
        MoveObjects();
        UpdateScore();
    }

    void UpdateScore()
    {
        Score += scoreIncreaseSpeed * Time.deltaTime;
        scoreIncreaseSpeed += acceleration * Time.deltaTime;
    }

    void SpawnCollectable()
    {
        if (collectableSpawnTimer > collectableSpawnRate)
        {
            int decision = Random.Range(0, 3);
            float lerp = (levelSpeed - startingSpeed) / (maxSpeed - startingSpeed);

            for (int i = 0; i <= decision; i++)
            {
                int prefabDecision = Random.Range(0, collectablePrefab.Length);
                GameObject collectable = Instantiate(collectablePrefab[prefabDecision]);
                if (collectable.TryGetComponent(out HealthCollectable health))
                    health.HealthGain = Mathf.Lerp(0f, -25f, lerp);
                else if (collectable.TryGetComponent(out UpgradeCollectable upgrade))
                {
                    upgrade.UpgradeUnlock = (int)Mathf.Lerp(2f, 10f, lerp);
                    upgrade.fireRateIncrease = Mathf.Lerp(0.5f, 2f, lerp);
                }
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

    void SetSpeed()
    {
        levelSpeed += acceleration * Time.deltaTime;
        if (levelSpeed > maxSpeed)
            levelSpeed = maxSpeed;

        zombieSpawnRate -= zombieSpawnRateChanger * Time.deltaTime;
        if (zombieSpawnRate < maxZombieSpawnRate)
            zombieSpawnRate = maxZombieSpawnRate;
    }

    void MoveObjects()
    {
        float delta = levelSpeed * Time.deltaTime;
        float groundDelta = levelSpeed * (groundMaterial.mainTextureScale.y / 1000f) * Time.deltaTime;

        foreach (Transform enemy in enemiesParent)
        {
            enemy.Translate(new Vector3(0f, 0f, delta));
        }
        foreach (Transform collectable in collectablesParent)
        {
            collectable.Translate(new Vector3(0f, 0f, delta));
        }
        Vector2 texturePos = groundMaterial.mainTextureOffset;
        texturePos.y -= groundDelta;
        groundMaterial.mainTextureOffset = texturePos;
    }

    public void StartLevel()
    {
        levelSpeed = startingSpeed;
        levelRunning = true;
    }

    public void StopLevel()
    {
        levelRunning = false;

        //Debug.Log("Stopped level");

        float bestScore = PlayerPrefs.GetFloat(bestScorePlayerPrefsName, 0f);

        //Debug.Log("Best: " + bestScore);
        //Debug.Log("Current: " + Score);

        if (Score > bestScore)
        {
            //Debug.Log("Current: " + Score);
            PlayerPrefs.SetFloat(bestScorePlayerPrefsName, Score);
            PlayerPrefs.Save();
        }
    }
}
