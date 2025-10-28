using UnityEngine;

public class GunController : MonoBehaviour
{
    public float fireRate = 1f;
    public float bulletSpeed = 1000f;
    public Transform crosshair;
    public GameObject bulletPrefab;

    private float timer = 0f;

    void Update()
    {
        if (timer > 1f / fireRate)
        {
            GameObject bullet = Instantiate(bulletPrefab);

            bullet.transform.position = crosshair.transform.position;
            bullet.transform.rotation = crosshair.transform.rotation;

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.AddForce(new Vector3(0f, 0f, bulletSpeed));
            Destroy(bullet, 1.5f);

            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
