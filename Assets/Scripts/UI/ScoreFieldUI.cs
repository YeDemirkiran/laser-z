using TMPro;
using UnityEngine;

public class ScoreFieldUI : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;
    [SerializeField] TextMeshProUGUI scoreText;

    void Update()
    {
        scoreText.text = levelManager.Score.ToString("0");
    }
}
