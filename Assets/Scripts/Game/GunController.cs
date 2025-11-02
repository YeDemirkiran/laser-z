using System;
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
    enum FireMode { Normal, Volley }
    [SerializeField] FireMode fireMode = FireMode.Normal;

    private float timer = 0f;

    int volleyModeIndex = 0;

    private void Start()
    {
        timer = 1f / fireRate;
    }

    void Update()
    {
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
        bool isHit = Physics.Raycast(origin.position, origin.forward, gunRange, targetLayerMask);

        if (isHit)
        {
            GameObject bullet = Instantiate(bulletPrefab);

            bullet.transform.SetPositionAndRotation(origin.transform.position, origin.transform.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.AddForce(origin.forward * bulletForce, ForceMode.Impulse);
            Destroy(bullet, 5f);

            timer = 0f;
        }
    }

    public void IncreaseFireRate(float increase)
    {
        fireRate += increase;
        fireRate = Mathf.Clamp(fireRate, 0f, maxFireRate);
    }
}
