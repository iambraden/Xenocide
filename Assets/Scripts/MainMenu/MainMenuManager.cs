using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
    public Image Black;
    public Animator anim;

    public void Start() {
        SoundManager.PlaySound(SoundType.MenuMusic, 0.7f);
    }

    public void StartGame() {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene() {
        SoundManager.FadeOutMusic(1f);
        anim.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene("Game");
    }

    public void OpenOptions() {
       //TO DO 
    }

    public void QuitGame() {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}