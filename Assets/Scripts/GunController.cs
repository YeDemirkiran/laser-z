using UnityEngine;

public class GunController : MonoBehaviour
{
    public float bulletSpeed = 1000f;
    public Transform crosshair;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        GameObject bullet = Instantiate(bulletPrefab);

        bullet.transform.position = crosshair.transform.position;
        bullet.transform.rotation = crosshair.transform.rotation;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.AddForce(new Vector3(0f, 0f, bulletSpeed));
        Destroy(bullet, 1.5f);
    }
}
