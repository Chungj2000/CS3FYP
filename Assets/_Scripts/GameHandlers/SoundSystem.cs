using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 * Script for storing and playing various audio/music cues.
 */
public class SoundSystem : MonoBehaviour {

    public static SoundSystem INSTANCE {get; private set;}

    [SerializeField] private AudioClip musicLoop;
    [SerializeField] private AudioClip gameOverSFX;
    [SerializeField] private AudioMixer audioMixer;
    
    private const string masterVolume = "MasterVolume";
    private AudioSource audioSource;

    private void Awake() {

        audioSource = GetComponent<AudioSource>();

        if(INSTANCE == null) {
            INSTANCE = this;
            //Debug.Log("GameMaster instance created.");
        } else {
            Debug.LogError("More than one GameMaster instance created.");
            Destroy(this);
            return;
        }

    }

    //Convert float from slider to a logarithemic scale for the master volume of the AudioMixer.
    public void SetVolume(float sliderValue) {
        audioMixer.SetFloat(masterVolume, Mathf.Log10(sliderValue) * 20f);
    }

    //Play a audio clip from source.
    public void PlaySFX(AudioClip audio) {
        audioSource.clip = audio;
        audioSource.loop = false;
        audioSource.Play();
    }

    //Loop a audio clip as BGM from source.
    public void PlayMusic(AudioClip audio) {
        audioSource.clip = audio;
        audioSource.loop = true;
        audioSource.Play();
    }

    //Getters.
    public AudioClip GetMusicLoop() {
        return musicLoop;
    }

    public AudioClip GetGameOverSFX() {
        return gameOverSFX;
    }

}
