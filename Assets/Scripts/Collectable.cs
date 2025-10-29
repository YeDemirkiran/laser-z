using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float healthGive = 25f;
    public TextMeshProUGUI text;
    bool disabled = false;

    private void Start()
    {
        text.text = $"{healthGive.ToString()}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!disabled && other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.attachedRigidbody.GetComponent<PlayerController>();
            player.health += healthGive;
            disabled = true;
        }
        else if (!disabled && other.gameObject.CompareTag("Bullet"))
        {
            healthGive += 1f;
            text.text = $"{healthGive.ToString()}";
        }
    }
}
