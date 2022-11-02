using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AudioManagerChannels
{ 
    MusicChannel = 0,
    SoundEffectChannel,
    VoiceChannel,
    AmbienceChannel
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public static float musicChannelVol = 1f;
    public static float soundeffectChannelVol = 1f;
    public static float voiceChannelVol = 1f;
    public static float ambienceChannelVol = 1f;

    public AudioSource musicChannel;
    public AudioSource soundeffectChannel;
    public AudioSource voiceChannel;
    public AudioSource ambienceChannel;

    public AudioClip TitleMusic;
    public AudioClip MainGameMusic;

    public AudioClip WaterAmbience;

    public AudioClip cannonHit;
    public AudioClip cannonMiss;
    public AudioClip cannonFire;
    public AudioClip waveStart;

    void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start ()
    {
        musicChannel = GetComponents<AudioSource>()[0];
        soundeffectChannel = GetComponents<AudioSource>()[1];
        voiceChannel = GetComponents<AudioSource>()[2];
        ambienceChannel = GetComponents<AudioSource>()[3];

        SceneManager.sceneLoaded += OnSceneLoaded;

        Instance.PlaySound(AudioManagerChannels.MusicChannel, TitleMusic, 1f);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.buildIndex)
        {
            case 0:
                Instance.PlaySound(AudioManagerChannels.MusicChannel, TitleMusic, 1f);
                Instance.StopSound(AudioManagerChannels.AmbienceChannel);
                break;
            case 1:
                Instance.PlaySound(AudioManagerChannels.MusicChannel, MainGameMusic, 1f);
                Instance.PlaySound(AudioManagerChannels.AmbienceChannel, WaterAmbience, 1f);
                break;
            case 2:
                Instance.StopSound(AudioManagerChannels.MusicChannel);
                Instance.StopSound(AudioManagerChannels.AmbienceChannel);
                break;
        }
    }

    public static void SetChannelVolume(int target, float value)
    {
        switch (target)
        {
            case 0:
                musicChannelVol = value;
                Instance.musicChannel.volume = musicChannelVol;
                break;
            case 1:
                soundeffectChannelVol = value;
                Instance.soundeffectChannel.volume = soundeffectChannelVol;
                break;
            case 2:
                voiceChannelVol = value;
                Instance.voiceChannel.volume = voiceChannelVol;
                break;
            case 3:
                ambienceChannelVol = value;
                Instance.ambienceChannel.volume = ambienceChannelVol;
                break;
            default:
                break;
        }
    }

    public void SetMusicLoop()
    {
        musicChannel.loop = !musicChannel.loop;
    }

    public void PlaySound(AudioManagerChannels target, AudioClip clip)
    {
        switch (target)
        {
            case AudioManagerChannels.MusicChannel:
                musicChannel.Stop();
                musicChannel.clip = clip;
                musicChannel.Play();
                break;
            case AudioManagerChannels.SoundEffectChannel:
                soundeffectChannel.PlayOneShot(clip);
                break;
            case AudioManagerChannels.VoiceChannel:
                voiceChannel.Stop();
                voiceChannel.clip = clip;
                voiceChannel.Play();
                break;
            case AudioManagerChannels.AmbienceChannel:
                ambienceChannel.Stop();
                ambienceChannel.clip = clip;
                ambienceChannel.Play();
                ambienceChannel.loop = true;
                break;
        }
    }

    public void PlaySound(AudioManagerChannels target, AudioClip clip, float pitch)
    {
        switch (target)
        {
            case AudioManagerChannels.MusicChannel:
                musicChannel.Stop();
                musicChannel.clip = clip;
                musicChannel.pitch = pitch;
                musicChannel.Play();
                break;
            case AudioManagerChannels.SoundEffectChannel:
                //soundeffectChannel.Stop();
                //soundeffectChannel.clip = clip;
                soundeffectChannel.pitch = pitch;
                soundeffectChannel.PlayOneShot(clip);
                break;
            case AudioManagerChannels.VoiceChannel:
                voiceChannel.Stop();
                voiceChannel.clip = clip;
                voiceChannel.pitch = pitch;
                voiceChannel.Play();
                break;
            case AudioManagerChannels.AmbienceChannel:
                ambienceChannel.Stop();
                ambienceChannel.clip = clip;
                ambienceChannel.Play();
                ambienceChannel.loop = true;
                break;
        }
    }

    public void StopSound(AudioManagerChannels target)
    {
        switch (target)
        {
            case AudioManagerChannels.MusicChannel:
                musicChannel.Stop();
                break;
            case AudioManagerChannels.SoundEffectChannel:
                soundeffectChannel.Stop();
                break;
            case AudioManagerChannels.VoiceChannel:
                voiceChannel.Stop();
                break;
            case AudioManagerChannels.AmbienceChannel:
                ambienceChannel.Stop();
                ambienceChannel.loop = false;
                break;
        }
    }
}
