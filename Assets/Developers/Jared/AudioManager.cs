using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum AudioManagerChannels
{ 
    MusicChannel = 0,
    SoundEffectChannel,
    VoiceChannel
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public static float musicChannelVol = 1f;
    public static float soundeffectChannelVol = 1f;
    public static float voiceChannelVol = 1f;

    public AudioSource musicChannel;
    public AudioSource soundeffectChannel;
    public AudioSource voiceChannel;

    public AudioClip BackgroundMusic;

    public AudioClip WaterAmbience;

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
        if (SceneManager.GetActiveScene().name == "WaveSurvival")
        {
            AudioManager.Instance.PlaySound(AudioManagerChannels.MusicChannel, BackgroundMusic, 1f);
            AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, WaterAmbience, 1f);
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
                soundeffectChannel.Stop();
                soundeffectChannel.clip = clip;
                soundeffectChannel.Play();
                break;
            case AudioManagerChannels.VoiceChannel:
                voiceChannel.Stop();
                voiceChannel.clip = clip;
                voiceChannel.Play();
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
                soundeffectChannel.Stop();
                soundeffectChannel.clip = clip;
                soundeffectChannel.pitch = pitch;
                soundeffectChannel.Play();
                break;
            case AudioManagerChannels.VoiceChannel:
                voiceChannel.Stop();
                voiceChannel.clip = clip;
                voiceChannel.pitch = pitch;
                voiceChannel.Play();
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
        }
    }
}
