using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] Transform[] bulletOrigins;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] LayerMask targetLayerMask;

    [Header("Gun Properties")]
    [SerializeField] float fireRate = 1f;
    [SerializeField] float maxFireRate = 4f;
    [SerializeField] float gunRange = 75f;
    [SerializeField] float bulletForce = 1000f;

    [Header("Shotgun Properties")]
    [SerializeField] int bulletAmount;
    [SerializeField] float bulletSpread;

    enum FireMode { Normal, Volley, Shotgun }
    [SerializeField] FireMode fireMode = FireMode.Normal;

    private float timer = 0f;

    int volleyModeIndex = 0;

    LevelManager levelManager;

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    void Update()
    {
        if (levelManager.IsLevelRunning)
            GunLoop();
    }

    void GunLoop()
    {
        if (timer >= 1f / fireRate)
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
            Debug.Log("Shotgun bullet " + i);

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

            rb.AddForce(direction * bulletForce, ForceMode.Impulse);
            Destroy(bullet, 5f);
        }
    }

    public void IncreaseFireRate(float increase)
    {
        fireRate += increase;
        fireRate = Mathf.Clamp(fireRate, 0f, maxFireRate);
    }
}
