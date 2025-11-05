using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage { get; set; } = 25f;

    public enum BulletMode { DestroyOnHit, PassThrough }
    [SerializeField] BulletMode bulletMode = BulletMode.DestroyOnHit;

    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHit(other.gameObject);
    }

    void OnHit(GameObject hitObject)
    {
        Debug.Log("Bullet hit something!");


        if (hitObject.CompareTag("Enemy"))
        {
            if (!hitObject.TryGetComponent<EnemyAI>(out var enemy))
                return;
            Debug.Log("Bullet hit enemy!");
            enemy.GiveDamage(Damage);
            if (bulletMode == BulletMode.DestroyOnHit)
                Destroy(gameObject);
        }
        else if (hitObject.CompareTag("End Bounds"))
            Destroy(gameObject);
    }
}
