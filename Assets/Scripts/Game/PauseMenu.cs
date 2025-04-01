using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    public bool isTransitioning;
    public Animator fadeAnimator;
    public Image fadeImage; // image reference
    
    [Tooltip("Name of the main menu scene in build settings")]
    public string mainMenuSceneName = "MainMenu"; // scene name
    
    [Tooltip("Name of the game scene in build settings")]
    public string gameSceneName = "Game"; // scene name

    void start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isTransitioning) // Prevent pausing during transitions
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if(!gameManager.ifDead){
                if(isPaused){
                ResumeGame();
                }else if(!gameManager.isUpgradeActive){
                PauseGame();
                }
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
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        pauseMenu.SetActive(false);
        if (gameManager != null && !gameManager.isUpgradeActive){
            Time.timeScale = 1f;
        }
        isPaused = false;
    }
    
    public void RestartGame()
    {
        StartCoroutine(RestartGameScene());
    }
    
    private IEnumerator RestartGameScene()
    {
        isTransitioning = true;
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;  // Force time scale to 1 (avoids scene change errors)
        
        fadeAnimator.SetTrigger("Fade");
        
        yield return new WaitForSecondsRealtime(1f);
        
        SceneManager.LoadScene(gameSceneName);
        isTransitioning = false;
    }
    
    public void GoToMainMenu()
    {
        StartCoroutine(LoadMainMenuScene());
    }
    
    private IEnumerator LoadMainMenuScene()
    {
        isTransitioning = true;
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;  // Force time scale to 1 (avoids scene change errors)
        
        // trigger fade animation
        fadeAnimator.SetTrigger("Fade");
       
        // wait for animation to play
        yield return new WaitForSecondsRealtime(1f);
        
        // load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
        isTransitioning = false;
    }
    
    public void QuitGame()
    {
        isTransitioning = true;
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
        isTransitioning = false;
    }
}
