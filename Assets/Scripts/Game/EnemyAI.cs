using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float health = 100f;
    public float Health 
    {
        get => health;
        private set
        {
            health = Mathf.Clamp(value, 0f, 100f);
            if (health <= 0f)
                Die();
        }
    }

    public float speed = 5f;
    public float damage = 25f;
    public float scoreIncrease = 10f;

    bool disabled;

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
        if (!disabled && collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.AddHealth(damage * (damage > 0f ? -1f : 1f));
            disabled = true;
        }
    }

    void Die()
    {
        LevelManager.Instance.IncreaseScore(scoreIncrease);
        Destroy(gameObject);
    }

    public void GiveDamage(float amount)
    {
        if (amount < 0f)
            amount = -amount;
        Health -= amount;
    }
}
