using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Configuration")]
    [SerializeField] Transform enemiesParent;
    [SerializeField] Transform collectablesParent;
    [SerializeField] Transform[] lines;
    [SerializeField] GameObject zombiePrefab;
    [SerializeField] GameObject healthCollectable, upgradeCollectable, gunCollectable;
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
    [SerializeField] float groundMoveSpeed = 0.25f;

    float zombieSpawnTimer;
    float collectableSpawnTimer;
    int previousZombieDecision;

    bool levelRunning;
    public bool IsLevelRunning => levelRunning;

    public float Score { get; private set; } = 0f;

    bool currentGunMaxLevel = false;

    int currentGunIndex = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

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
                GameObject collectable = null;

                if (i == 0 || i == 2)
                {
                    collectable = SpawnHealthCollectable(lerp);
                }
                else if (i == 1)
                {
                    if (!currentGunMaxLevel)
                        collectable = SpawnUpgradeCollectable(lerp);
                    else
                        collectable = SpawnGunCollectable(lerp);
                }
                Destroy(collectable, 15f);
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

    GameObject SpawnHealthCollectable(float lerp)
    {
        GameObject collectable = Instantiate(healthCollectable);
        var healthColl = collectable.GetComponent<HealthCollectable>();
        healthColl.HealthGain = Mathf.Lerp(0f, -25f, lerp);

        return collectable;
    }

    GameObject SpawnUpgradeCollectable(float lerp)
    {
        GameObject collectable = Instantiate(upgradeCollectable);
        var upgradeColl = collectable.GetComponent<UpgradeCollectable>();
        upgradeColl.UpgradeUnlock = (int)Mathf.Lerp(2f, 10f, lerp);

        return collectable;
    }

    GameObject SpawnGunCollectable(float lerp)
    {
        GameObject collectable = Instantiate(gunCollectable);
        var gunColl = collectable.GetComponent<NewGunCollectable>();
        gunColl.UpgradeUnlock = (int)Mathf.Lerp(2f, 10f, lerp);
        gunColl.gunID = currentGunIndex + 1;

        return collectable;
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
        float groundDelta = (levelSpeed * groundMoveSpeed) * Time.deltaTime;

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

        float bestScore = PlayerPrefs.GetFloat(bestScorePlayerPrefsName, 0f);

        if (Score > bestScore)
        {
            PlayerPrefs.SetFloat(bestScorePlayerPrefsName, Score);
            PlayerPrefs.Save();
        }
    }

    public void IncreaseScore(float score)
    {
        Score += score;
    }

    internal void NotifyCurrentGunMaxLevelReach()
    {
        Debug.Log("The player's current gun has reached max level.");
        currentGunMaxLevel = true;
    }

    internal void NotifyGunChange(int newGunIndex)
    {
        Debug.Log("The player's changed guns.");
        currentGunMaxLevel = false;
        currentGunIndex = newGunIndex;
    }
}
