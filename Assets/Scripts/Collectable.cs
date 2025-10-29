using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float healthGive = 25f;
    public TextMeshProUGUI text;
    bool disabled = false;

    private void Start()
    {
        text.text = $"{(healthGive < 0 ? '-' : '+')}{healthGive.ToString()}";
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Hit player");
    //        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
    //        player.health += healthGive;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (!disabled && other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.attachedRigidbody.GetComponent<PlayerController>();
            player.health += healthGive;
            disabled = true;
        }
    }
}
