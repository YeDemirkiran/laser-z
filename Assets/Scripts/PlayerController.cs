using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float steerSpeed = 1f;
    public float health = 100f;

    void Update()
    {
        if (health <= 0f)
            Destroy(gameObject);

        Vector3 pos = transform.position;
        pos.x += Input.GetAxisRaw("Horizontal") * steerSpeed * Time.deltaTime;
        transform.position = pos;
    }
}
