using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    public Animator fadeAnimator;
    public Image fadeImage; // image reference
    
    [Tooltip("Name of the main menu scene in build settings")]
    public string mainMenuSceneName = "MainMenu"; // scene name
    
    [Tooltip("Name of the game scene in build settings")]
    public string gameSceneName = "Game"; // scene name
    
    void Start()
    {
        pauseMenu.SetActive(false);
        
        // debug for fade
        if (fadeAnimator == null)
            Debug.LogWarning("Fade Animator not assigned in " + gameObject.name);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void RestartGame()
    {
        StartCoroutine(RestartGameScene());
    }
    
    private IEnumerator RestartGameScene()
    {
        ResumeGame();
        
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("Fade");
        else
            Debug.LogWarning("Fade animation skipped - animator not assigned");
        
        yield return new WaitForSecondsRealtime(1f);
        
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void GoToMainMenu()
    {
        StartCoroutine(LoadMainMenuScene());
    }
    
    private IEnumerator LoadMainMenuScene()
    {
        // resume the game (unfreeze time)
        ResumeGame();
        
        // trigger fade animation
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("Fade");
        else
            Debug.LogWarning("Fade animation skipped - animator not assigned");
        
        // wait for animation to play
        yield return new WaitForSecondsRealtime(1f);
        
        // load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    public void QuitGame()
    {
        Time.timeScale = 1f;
        
        // trigger fade before quitting
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("Fade");
        
        // small delay before quitting
        StartCoroutine(DelayedQuit());
    }
    
    private IEnumerator DelayedQuit()
    {
        yield return new WaitForSecondsRealtime(1f);
        
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
