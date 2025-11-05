using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public EnemyAI enemyAI;
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    void Update()
    {
        slider.value = enemyAI.Health / 100f;
    }
}
