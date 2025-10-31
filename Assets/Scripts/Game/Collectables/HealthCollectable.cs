using TMPro;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] float m_HealthGain = -25f;

    public float HealthGain
    {
        get
        {
            return m_HealthGain;
        }
        set
        {
            m_HealthGain = value;
            text.text = m_HealthGain.ToString("0");
        }
    }

    bool disabled;

    private void Start()
    {
        HealthGain = m_HealthGain;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (disabled)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            if (!other.attachedRigidbody.TryGetComponent(out PlayerController player))
                return;
            player.AddHealth(HealthGain);
            disabled = true;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            HealthGain += 1f;
            Destroy(other.gameObject);
        }
    }
}
