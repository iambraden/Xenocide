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
    Dash // In playerController.cs
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;
    private Coroutine fadeRoutine;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1) {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
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
        audioSource.volume = startVolume;
    }
}
