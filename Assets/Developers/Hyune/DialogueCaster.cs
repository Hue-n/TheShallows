using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void OnDialogueCast(Dialogue dia, int command);

public class DialogueCaster : MonoBehaviour
{
    public static event OnDialogueCast OnDialogueCast;
    //private bool inDialogue = false;

    public Character currentCharacter;

    void Start()
    {
        DialogueSystem.OnDialogueFinish += OnDialogueFinish;
    }

    void OnDestroy()
    {
        DialogueSystem.OnDialogueFinish -= OnDialogueFinish;
    }

    void Cast()
    {
        Dialogue temp = (Dialogue)ScriptableObject.CreateInstance("Dialogue");
        Line templ = new Line();
        currentCharacter = null;

        templ.speaker = currentCharacter;
        templ.line = " ";
        temp.voiceLines.Add(templ);

        OnDialogueCast?.Invoke(temp, 0);
        //inDialogue = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*
       if (Input.GetKeyDown(KeyCode.P) && !inDialogue)
       {
        dialogueCast ?. Invoke(test);
        inDialogue = true;
       } 
       */
    }

    void OnDialogueFinish()
    {
        //inDialogue = false;
    }
}
