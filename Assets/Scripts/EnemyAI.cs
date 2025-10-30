using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float health = 100f;
    public float speed = 5f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            float sign = Mathf.Sign(other.transform.position.x - transform.position.x);
            Vector3 pos = transform.position;
            pos.x += speed * sign * Time.deltaTime;
            transform.position = pos;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            health -= 25f;
            if (health <= 0f)
                Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.AddHealth(-25f);
        }
    }
}
