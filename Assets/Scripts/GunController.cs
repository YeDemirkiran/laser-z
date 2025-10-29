using UnityEngine;

public class GunController : MonoBehaviour
{
    public float fireRate = 1f;
    public float maxFireRate = 4f;
    public float bulletSpeed = 1000f;
    public float gunRange = 75f;
    public Transform crosshair;
    public GameObject bulletPrefab;
    public LayerMask bulletLayerMask;

    private float timer = 0f;

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
            bool isHit = Physics.Raycast(crosshair.position, crosshair.forward, gunRange, bulletLayerMask);

            if (isHit)
            {
                GameObject bullet = Instantiate(bulletPrefab);

                bullet.transform.position = crosshair.transform.position;
                bullet.transform.rotation = crosshair.transform.rotation;

                Rigidbody rb = bullet.GetComponent<Rigidbody>();

                rb.AddForce(crosshair.forward * bulletSpeed);
                Destroy(bullet, 5f);

                timer = 0f;
            }        
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
