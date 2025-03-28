using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverScreen : MonoBehaviour
{
    public TMPro.TextMeshProUGUI pointsText;
    public Animator fadeAnimator;
    public Image fadeImage;
    
    public void Setup(int score){
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        pointsText.text = "Score: " + score.ToString();
    }
    
    public void RestartGame()
    {
        StartCoroutine(RestartGameScene());
    }
    
    private IEnumerator RestartGameScene()
    {
        // Reset time scale to unpause game
        Time.timeScale = 1f;
        
        // Fade animation
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("Fade");
        
        yield return new WaitForSecondsRealtime(1f);
        
        // Reload the game
        SceneManager.LoadScene("Game");
    }
    
    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
