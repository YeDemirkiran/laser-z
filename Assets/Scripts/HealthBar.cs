using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerController controller;
    private Slider slider;
    void Start()
    {
       slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = controller.health / 100f;
    }
}
