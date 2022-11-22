using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public Dialogue currentDial;

    public DefaultControls controls;

    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    private void OnDisable()
    {
        controls.Controller.Disable();
        
    }

    private void Awake()
    {
        controls = new DefaultControls();
        controls.Controller.Attack.performed += ctx => Interact();
    }

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;

        
    }

    public void AssignDialogue(Dialogue data)
    {
        currentDial = data;

        int count = 0;

        while(count < data.text.Length)
        {
            //Debug.Log(count + " / " + data.text.Length);
            lines[count] = data.text[count]; ;
            names[count] = data.characters[count];
            count++;
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
        if(nameComponent.text == string.Empty)
        {
            FindObjectOfType<FocalPoint>().SetFocalPoint(GameObject.FindGameObjectWithTag("Player"));
            gameObject.SetActive(false);
        }
            
    }

    public void StartDialogue()
    {
        nameComponent.gameObject.SetActive(true);
        textComponent.gameObject.SetActive(true);
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
            FindObjectOfType<FocalPoint>().SetFocalPoint(GameObject.FindGameObjectWithTag("Player"));
            textComponent.text = string.Empty;
            nameComponent.text = string.Empty;
            gameObject.SetActive(false);
            
        }
    }
}
