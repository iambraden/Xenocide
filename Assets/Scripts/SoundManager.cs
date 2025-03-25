using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType{
    MenuMusic,
    GameMusic, // In GameManager.cs
    BossMusic,
    PlayerShoot, //In playerController.cs
    PlayerHit, 
    EnemyShoot, //In EnemyController.cs and BossController.cs
    EnemyHit, //In EnemyController.cs and BossController.cs
    EnemyDeath, //In EnemyController.cs and BossController.cs
    GameOver
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake(){
        instance = this;
    }

    private void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1){
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
}
