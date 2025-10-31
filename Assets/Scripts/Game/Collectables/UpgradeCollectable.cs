using TMPro;
using UnityEngine;

public class UpgradeCollectable : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] int m_UpgradeUnlock = 25;

    int UpgradeUnlock
    {
        get
        {
            return m_UpgradeUnlock;
        }
        set
        {
            m_UpgradeUnlock = value < 0 ? 0 : value;
            if (m_UpgradeUnlock == 0)
                unlocked = true;
            text.text = m_UpgradeUnlock.ToString();
        }
    }

    bool disabled;
    bool unlocked;

    private void Start()
    {
        UpgradeUnlock = m_UpgradeUnlock;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (disabled)
            return;

        if (unlocked && other.gameObject.CompareTag("Player"))
        {
            if (other.attachedRigidbody == null)
                return;
            if (!other.attachedRigidbody.TryGetComponent<PlayerController>(out PlayerController player))
                return;
            GunController gun = player.GetComponentInChildren<GunController>();
            if (gun == null)
                return;

            gun.fireRate += 1;
            disabled = true;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            UpgradeUnlock -= 1;
            Destroy(other.gameObject);
        }
    }
}
