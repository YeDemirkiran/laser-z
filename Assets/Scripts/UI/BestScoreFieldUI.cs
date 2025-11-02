using TMPro;
using UnityEngine;

public class BestScoreFieldUI : MonoBehaviour
{
    [SerializeField] string playerPrefsFieldName;
    [SerializeField] TextMeshProUGUI scoreText;

    void Update()
    {
        float score = PlayerPrefs.GetFloat(playerPrefsFieldName, 0f);
        scoreText.text = score.ToString("0");
    }
}
