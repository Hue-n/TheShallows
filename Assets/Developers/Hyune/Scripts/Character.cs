using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class Character : ScriptableObject
{
    public Sprite sprite;
    public List<AudioClip> voiceLines = new List<AudioClip>(3);
}
