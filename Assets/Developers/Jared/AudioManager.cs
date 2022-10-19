using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume = 0.7f;
    
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;
    private AudioSource source;
    public void SetSource (AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }
    public void Play ()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }
    
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource testSource;

    [SerializeField]
    Sound[] sounds;

    public List<AudioClip> soundTestList = new List<AudioClip>();

    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogError("More than one AudioManager in the scene.");
        }else
        {
            instance = this;
        }
    }

    void Start ()
    {
        testSource = GetComponent<AudioSource>();

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent (this.transform);
            sounds[i].SetSource (_go.AddComponent<AudioSource>());
        }
    }

    public void PlaySound (string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
        // no sound with _name
        Debug.LogWarning ("AudioManager: Sound not found in list, " + _name);
    }

    public void PlaySoundTest(int index)
    {
        testSource.Stop();
        testSource.clip = soundTestList[index];
        testSource.Play();
    }

    //When we implement different sound effects, follow the rest of the turtorial to incorperate this system into other scripts. https://youtu.be/HhFKtiRd0qI?t=1276
}
