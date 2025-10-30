using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject gameHUD;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject deathMenu;

    [SerializeField] UnityEvent onGameHUDActivate;
    [SerializeField] UnityEvent onPauseMenuActivate;
    [SerializeField] UnityEvent onDeathMenuActivate;

    public void EnableGameHUD()
    {
        gameHUD.SetActive(true);
        onGameHUDActivate?.Invoke();
    }

    public void EnablePauseMenu()
    {
        pauseMenu.SetActive(true);
        onPauseMenuActivate?.Invoke();
    }

    public void EnableDeathMenu()
    {
        deathMenu.SetActive(true);
        onDeathMenuActivate?.Invoke();
    }

    public void RestartLevel()
    {
        GameManager.Instance.ReloadCurrentScene();
    }
}
