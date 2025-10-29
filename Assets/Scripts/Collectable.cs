using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum Type { Health, Upgrade, Gun }

    public Type type;
    public TextMeshProUGUI text;
    public float healthGive = 25f;
    public int upgradeUnlock = 25;

    bool flag = false;
    bool disabled = false;


    private void Start()
    {
        if (type == Type.Health)
            text.text = $"{healthGive.ToString()}";
        else if (type == Type.Upgrade)
            text.text = $"{upgradeUnlock.ToString()}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (disabled)
            return;

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.attachedRigidbody.GetComponent<PlayerController>();
            if (type == Type.Health)
                player.health += healthGive;
            else if (type == Type.Upgrade && flag == true)
                player.GetComponentInChildren<GunController>().fireRate += 1;
            disabled = true;
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            if (type == Type.Health)
            {
                healthGive += 1f;
                text.text = $"{healthGive.ToString()}";
            }
            else if (type == Type.Upgrade && upgradeUnlock > 0)
            {
                upgradeUnlock -= 1;
                text.text = $"{upgradeUnlock.ToString()}";
                if (upgradeUnlock == 0)
                    flag = true;
            }
            Destroy(other.gameObject);
        }
    }
}
