using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    public Image health;
    public Image deathGague;
    public bool waveSurvival = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waveSurvival)
        {
            health.fillAmount = GameManager.Instance.playerInstance.GetComponent<PlayerController>().currentHP / 100;
            deathGague.fillAmount = GameManager.Instance.playerInstance.GetComponent<ShootingMechanic>().currentDeathGague / 100;
        }
        else
        {
            health.fillAmount = KQGameManager.Instance.playerInstance.GetComponent<PlayerCon_KrakenQuest>().currentHP / 100;
            deathGague.fillAmount = KQGameManager.Instance.playerInstance.GetComponent<ShootingMechanic>().currentDeathGague / 100;
        }
        
    }
}
