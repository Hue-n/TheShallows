using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// have a way to break out of the cameraMovement.
// use yield break;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    [SerializeField] public List<Line> voiceLines = new List<Line>();
}

[System.Serializable]
public class Line
{
    public Character speaker;
    public string line = "";
}
