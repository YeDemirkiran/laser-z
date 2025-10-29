using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float healthGive = 25f;
    public int upgradeUnlock = 25;
    private bool flag = false;
    public TextMeshProUGUI text;
    bool disabled = false;
    public enum TYPE { Healt, Upgrade, Gun}
    public TYPE type;

    private void Start()
    {
        if (type == TYPE.Healt)
            text.text = $"{healthGive.ToString()}";
        else if (type == TYPE.Upgrade)
            text.text = $"{upgradeUnlock.ToString()}";
    }

    private void OnTriggerEnter(Collider other)
    {
        
        Debug.LogWarning("Collectable hit: " + other.name);
        if (!disabled && other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.attachedRigidbody.GetComponent<PlayerController>();
            if (type == TYPE.Healt)
                player.health += healthGive;
            else if (type == TYPE.Upgrade && flag == true)
                player.GetComponentInChildren<GunController>().fireRate += 1;
            disabled = true;
        }
        else if (!disabled && other.gameObject.CompareTag("Bullet"))
        {
            if (type == TYPE.Healt)
            {
                healthGive += 1f;
                text.text = $"{healthGive.ToString()}";
            }
            else if (type == TYPE.Upgrade && upgradeUnlock > 0)
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
