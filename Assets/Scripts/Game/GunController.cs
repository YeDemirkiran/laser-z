using UnityEngine;

public class GunController : MonoBehaviour
{
    [System.Serializable]
    class GunStats
    {
        public float fireRate; 
        public float bulletForce;
        public float damage;
    }

    enum FireMode { Normal, Volley, Shotgun }

    [Header("Configuration")]
    [SerializeField] FireMode fireMode = FireMode.Normal;
    [SerializeField] Transform[] bulletOrigins;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] LayerMask targetLayerMask;

    [Header("Gun Stats")]
    [SerializeField] GunStats[] gunLevels;
    GunStats currentGunStat;
    int currentLevelIndex = 0;
    [SerializeField] float gunRange = 75f;

    [Header("Extra: Shotgun Properties")]
    [SerializeField] int bulletAmount;
    [SerializeField] float bulletSpread;

    private float timer = 0f;

    int volleyModeIndex = 0;

    LevelManager levelManager;

    bool maxLevelReached = false;

    public System.Action OnMaxLevelReach { get; set; }

    private void Start()
    {
        levelManager = LevelManager.Instance;
        currentGunStat = gunLevels[0];
    }

    void Update()
    {
        if (levelManager.IsLevelRunning)
            GunLoop();
    }

    void GunLoop()
    {
        if (timer >= 1f / currentGunStat.fireRate)
        {
            if (fireMode == FireMode.Shotgun)
            {
                FireGunMultiple(bulletOrigins[0], bulletAmount, bulletSpread);
            }
            else if (fireMode == FireMode.Normal)
            {
                foreach (Transform origin in bulletOrigins)
                    FireGunSingle(origin);
            }
            else
            {
                FireGunSingle(bulletOrigins[volleyModeIndex]);
                volleyModeIndex = (volleyModeIndex + 1) % bulletOrigins.Length;
            }
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void FireGunSingle(Transform origin)
    {
        FireGun(origin.position, origin.forward);
    }

    void FireGunMultiple(Transform origin, int amount, float spread)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 position = origin.position;
            Vector3 direction = origin.forward;

            float spreadX = Random.Range(-spread, spread);

            direction = Quaternion.Euler(0f, spreadX, 0) * direction;
            position += new Vector3(Random.Range(-0.25f, 0.25f), 0f, 0f);
            FireGun(position, direction, true);
        }
    }

    void FireGun(Vector3 position, Vector3 direction, bool bypassRaycast = false)
    {
        bool isHit;

        if (!bypassRaycast)
            isHit = Physics.Raycast(position, direction, gunRange, targetLayerMask);
        else
            isHit = true;

        if (isHit)
        {
            GameObject bullet = Instantiate(bulletPrefab);

            bullet.transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.AddForce(direction * currentGunStat.bulletForce, ForceMode.Impulse);
            Destroy(bullet, 5f);
        }
    }

    /// <summary>
    /// Upgrade the current gun level to the next one.
    /// <br />
    /// <br />
    /// Returns true if maximum level is reached, and returns false otherwise.
    /// </summary>
    public bool UpgradeGun()
    {
        if (maxLevelReached)
            return false;

        currentLevelIndex = Mathf.Clamp(currentLevelIndex + 1, 0, gunLevels.Length - 1);
        currentGunStat = gunLevels[currentLevelIndex];

        maxLevelReached = currentLevelIndex == gunLevels.Length - 1;

        if (maxLevelReached)
            OnMaxLevelReach?.Invoke();
        return maxLevelReached;
    }
}
