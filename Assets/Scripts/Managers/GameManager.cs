using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Running = 0, Paused = 1 }

    [Header("Game State")]

    [SerializeField] GameState m_GameState = GameState.Running;
    public GameState CurrentGameState 
    {
        get => m_GameState;
        private set
        {
            m_GameState = value;
            if (m_GameState == GameState.Running)
                OnResume();
            else if (m_GameState == GameState.Paused)
                OnPause();
        }
    }
    public bool IsGamePaused => CurrentGameState == GameState.Paused;

    public UnityEvent OnPauseEvent;
    public UnityEvent OnResumeEvent;

    bool dryRun = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        dryRun = true;
        CurrentGameState = m_GameState;
    }

    public void ReloadCurrentScene(float delay = 0f)
    {
        if (delay > 0f)
            Invoke(nameof(ReloadCurrentScene), delay);
        else
            ReloadCurrentScene();
    }

    void ReloadCurrentScene()
    {
        ResumeGameDry();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        CurrentGameState = GameState.Paused;
    }

    public void PauseGameDry()
    {
        dryRun = true;
        PauseGame();
    }

    public void ResumeGame()
    {
        CurrentGameState = GameState.Running;
    }

    public void ResumeGameDry()
    {
        dryRun = true;
        ResumeGame();
    }

    void OnPause()
    {
        Time.timeScale = 0f;
        if (dryRun)
            dryRun = false;
        else
            OnPauseEvent?.Invoke();
    }
    void OnResume()
    {
        Time.timeScale = 1f;
        if (dryRun)
            dryRun = false;
        else
            OnResumeEvent?.Invoke();
    }
}
