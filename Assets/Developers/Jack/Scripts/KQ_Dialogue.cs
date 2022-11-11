using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KQ_Dialogue : MonoBehaviour
{
    public TextMeshProUGUI nameComponent;
    public TextMeshProUGUI textComponent;

    public string[] names;
    public string[] lines;
    public float textSpeed;

    private int index;

    public DefaultControls controls;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;

        controls = new DefaultControls();
        controls.Controller.Attack.performed += ctx => Interact();
    }

    public void AssignDialogue(Dialogue data)
    {
        int count = 0;

        foreach(string character in data.characters)
        {
            names[count] = character;
            lines[count] = data.text[count];
        }
    }

    public void Interact()
    {

            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue()
    {
        nameComponent.text = names[0];
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            nameComponent.text = names[index];
            StartCoroutine(TypeLine());
        }
        else
        {

            gameObject.SetActive(false);
        }
    }
}
