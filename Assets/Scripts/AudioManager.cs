using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Image soundButton;
    private bool toggle;
    // play sound by doing this
    //AudioManager.Instance.Method(param,param);
    [SerializeField] private AudioClip[] musicClip;
    [SerializeField] private Sprite muted;
    [SerializeField] private Sprite not_muted;

    #region Instance
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AudioManager>();

                if(instance == null)
                {
                   instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }

            return instance;
        }
        private set 
        {
            instance = value;
        }
    }
    #endregion

    #region Fields
    private AudioSource musicSource_0;
    private AudioSource musicSource_1;
    private AudioSource sfxSource;
    #endregion 

    private bool firstMusicIsON;
    private void Awake()
    {
        //dont destroy this gameobject
        DontDestroyOnLoad(this.gameObject);

        //Create audioSources and save them as references
        musicSource_0 = this.gameObject.AddComponent<AudioSource>();
        musicSource_1 = this.gameObject.AddComponent<AudioSource>();
        sfxSource     = this.gameObject.AddComponent<AudioSource>();

        musicSource_0.loop = true;
        musicSource_1.loop = true;

    }

    private void Start()
    {
        toggle = false;
        PlayMusic(musicClip[Random.Range(0,musicClip.Length)]);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource = (firstMusicIsON) ? musicSource_0 : musicSource_1;

        activeSource.clip        = musicClip;
        activeSource.volume      = 1;
        activeSource.Play();
    }
    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1f)
    {
        AudioSource activeSource = (firstMusicIsON) ? musicSource_0 : musicSource_1;

        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));

    }
    public void PlayMusicWithCrossFade(AudioClip newClip, float transitionTime = 1f)
    {
        AudioSource activeSource = (firstMusicIsON) ? musicSource_0 : musicSource_1;
        AudioSource newSource = (firstMusicIsON) ? musicSource_1 : musicSource_0;

        //SwapSource
        firstMusicIsON = !firstMusicIsON;

        //Set the new clips and start the crossfade
        newSource.clip = newClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));

    }
    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        //Make sure the source is active and Playing
        if(!activeSource.isPlaying)
        {
            activeSource.Play();
        }

        float t = 0.0f;

        //fade out
        for (t = 0; t < transitionTime; t+= Time.deltaTime)
        {
            activeSource.volume = (1 - (t / transitionTime));
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        //fade in
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (t / transitionTime);
            yield return null;
        }

    }
    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        float t = 0f;

        for(t = 0f;t <= transitionTime;t += Time.deltaTime)
        {
            original.volume = (1 - (t / transitionTime));
            newSource.volume = (t / transitionTime);
            yield return null;
        }

        original.Stop();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlaySFX(AudioClip clip, float volume) 
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource_0.volume = volume;
        musicSource_1.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void ToggleSound()
    {

        toggle = !toggle;

        if (toggle)
        {
            AudioListener.volume = 1f;
            soundButton.sprite = not_muted;
        }
        else
        {
            AudioListener.volume = 0f;
            soundButton.sprite = muted;
        }

    }





}
