using UnityEngine;

public class SoundToggleUI : MonoBehaviour
{
    [SerializeField] GameObject onButton;
    [SerializeField] GameObject offButton;
    [SerializeField] string playerPrefFieldName;

    private void OnEnable()
    {
        bool on = PlayerPrefs.GetInt(playerPrefFieldName, 1) != 0;
        onButton.SetActive(on);
        offButton.SetActive(!on);
    }
}
