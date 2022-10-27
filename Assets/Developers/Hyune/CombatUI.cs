using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    public GameObject targetTransform;
    public Image reticle;
    public Image minigameIcon;

    private float miniGameValue;

    private Transform targetReference;

    private bool isTargeting = false;

    // Start is called before the first frame update
    void Start()
    {
        targetTransform.SetActive(false);

        ShootingMechanic.OnShootingMechanic += ParseShootingMechanicData;
    }

    private void OnDestroy()
    {
        ShootingMechanic.OnShootingMechanic -= ParseShootingMechanicData;
    }

    private void Update()
    {
        if (isTargeting)
        {
            targetTransform.GetComponent<RectTransform>().position = WorldToUISpace(GetComponent<Canvas>(), targetReference.position);
        }
    }

    private Vector3 WorldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

    void SetIconScale(float value)
    {
        // Current
        float target = Mathf.Cos(value);
       
        // Ratio = Current / MaxScale
        float calculatedRatio = Mathf.Abs(target / 1);

        minigameIcon.rectTransform.localScale = new Vector3(calculatedRatio, calculatedRatio, calculatedRatio);
    }

    void ParseShootingMechanicData(ShootingMechanicData data)
    {
        switch (data.package)
        {
            case ShootingMechanicCommands.StateChange:

                switch (data.currentState)
                {
                    case ShootStates.idle:
                        targetTransform.SetActive(false);

                        isTargeting = false;
                        break;
                    case ShootStates.targeting:
                        targetTransform.SetActive(true);
                        reticle.enabled = true;
                        minigameIcon.enabled = false;

                        isTargeting = true;
                        break;
                    case ShootStates.skillchecking:
                        targetTransform.SetActive(true);
                        reticle.enabled = true;
                        minigameIcon.enabled = true;

                        isTargeting = true;
                        break;
                }

                break;
            case ShootingMechanicCommands.UpdateMinigameValue:
                miniGameValue = data.currentMinigameValue;
                SetIconScale(miniGameValue);

                break;
            case ShootingMechanicCommands.Hit:
                break;
            case ShootingMechanicCommands.Miss:
                break;
            case ShootingMechanicCommands.UpdateTarget:
                targetReference = data.targetReference;
                break;
        }
    }
}
