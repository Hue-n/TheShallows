using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new dialogue", menuName = "Dialogue")]

public class Dialogue : ScriptableObject
{
    public string[] characters;
    [TextArea(minLines:2, maxLines:6)]
    public string[] text;

}
