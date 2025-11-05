using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage { get; set; } = 25f;

    public enum BulletMode { DestroyOnHit, PassThrough }
    [SerializeField] BulletMode bulletMode = BulletMode.DestroyOnHit;

    private void OnCollisionEnter(Collision collision)
    {
        OnHit(collision.rigidbody.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHit(other.attachedRigidbody.gameObject);
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
        }
        else if (hitObject.CompareTag("Collectable"))
        {
            if (hitObject.TryGetComponent<HealthCollectable>(out var healthColl))
                healthColl.IncreaseHealthGain(1f);
            else if (hitObject.TryGetComponent<UpgradeCollectable>(out var upgradeColl))
                upgradeColl.DecreaseUpgradeUnlock(1);
            else if (hitObject.TryGetComponent<NewGunCollectable>(out var newGunColl))
                newGunColl.DecreaseUpgradeUnlock(1);
        }
        
        if (bulletMode == BulletMode.DestroyOnHit || hitObject.CompareTag("End Bounds"))
            Destroy(gameObject);
    }
}
