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
        // unpause game
        Time.timeScale = 1f;
        
        // fade animation
        if (fadeAnimator != null)
            fadeAnimator.SetTrigger("Fade");
        
        yield return new WaitForSecondsRealtime(1f);
        
        SceneManager.LoadScene("Game");
    }
    
    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
