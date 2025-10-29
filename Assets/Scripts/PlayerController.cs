using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public float steerSpeed = 1f;
    public float health = 100f;

    void Update()
    {
        if (health <= 0f)
            Destroy(gameObject);
        Vector3 pos = transform.position;

        pos.z += speed * Time.deltaTime;

        pos.x += Input.GetAxisRaw("Horizontal") * steerSpeed * Time.deltaTime;

        transform.position = pos;
    }
}
