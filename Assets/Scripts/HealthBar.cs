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

    void Update()
    {
        slider.value = controller.Health / 100f;
    }
}
