using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float health = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            health -= 25f;
            if (health <= 0f)
                Destroy(gameObject);
        }
    }
}
