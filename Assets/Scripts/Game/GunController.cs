using UnityEngine;

public class GunController : MonoBehaviour
{
    public float fireRate = 1f;
    public float maxFireRate = 4f;
    public float bulletSpeed = 1000f;
    public float gunRange = 75f;
    public Transform[] bulletOrigins;
    public GameObject bulletPrefab;
    public LayerMask bulletLayerMask;

    public enum FireMode { Normal, Volley }
    [SerializeField] FireMode fireMode = FireMode.Normal;

    private float timer = 0f;

    int volleyModeIndex = 0;

    private void Start()
    {
        timer = 1f / fireRate;
    }

    void Update()
    {
        if (fireRate > maxFireRate)
            fireRate = maxFireRate;

        if (timer >= 1f / fireRate)
        {
            if (fireMode == FireMode.Normal)
            {
                foreach (Transform origin in bulletOrigins)
                    FireGun(origin);
            }
            else
            {
                FireGun(bulletOrigins[volleyModeIndex]);
                volleyModeIndex = (volleyModeIndex + 1) % bulletOrigins.Length;
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    void FireGun(Transform origin)
    {
        bool isHit = Physics.Raycast(origin.position, origin.forward, gunRange, bulletLayerMask);

        if (isHit)
        {
            GameObject bullet = Instantiate(bulletPrefab);

            bullet.transform.SetPositionAndRotation(origin.transform.position, origin.transform.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.AddForce(origin.forward * bulletSpeed);
            Destroy(bullet, 5f);

            timer = 0f;
        }
    }
}
