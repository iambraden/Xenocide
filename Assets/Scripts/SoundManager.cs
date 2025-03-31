using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType{
    MenuMusic, // In GameManager.cs
    GameMusic, // In GameManager.cs
    BossMusic, // In GameManager.cs
    PlayerShoot, //In playerController.cs
    PlayerHit, //IN PlayerHealth.cs
    EnemyShoot, //In EnemyController.cs and BossController.cs
    EnemyHit, //In EnemyController.cs and BossController.cs
    EnemyDeath, //In EnemyController.cs and BossController.cs
    Hover, // In ButtonSound.cs
    Click,// In ButtonSound.cs
    Dash, // In playerController.cs
    PowerUp, // In GameManager.cs
    ForceFieldCharge, // In PlayerController.cs
    Warning, // In GameManager.cs
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;
    private Coroutine fadeRoutine;
    
    // master volume property
    private static float _masterVolume = 1f;
    public static float MasterVolume {
        get { return _masterVolume; }
        set { 
            _masterVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
            if (instance != null && instance.audioSource != null) {
                instance.audioSource.volume = _masterVolume;
            }
        }
    }

    private void Awake() {
        instance = this;
        
        // load saved volume setting
        if (PlayerPrefs.HasKey("MasterVolume")) {
            _masterVolume = PlayerPrefs.GetFloat("MasterVolume");
        }
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = _masterVolume;
    }

    public static void PlaySound(SoundType sound, float volume = 1) {
        // apply master volume
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume * _masterVolume);
    }

    public static void StopMusic() {
        instance.audioSource.Stop();
    }
    
    public static void FadeOutMusic(float fadeDuration) {
        if (instance.fadeRoutine != null) {
            instance.StopCoroutine(instance.fadeRoutine);
        }
        instance.fadeRoutine = instance.StartCoroutine(instance.FadeOut(fadeDuration));
    }

    private IEnumerator FadeOut(float duration) {
        float startVolume = audioSource.volume;
        
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }
        
        audioSource.Stop();
        audioSource.volume = _masterVolume;
    }
}
