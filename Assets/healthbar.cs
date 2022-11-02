using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    public Image health;
    public Image deathGague;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health.fillAmount = GameManager.Instance.playerInstance.GetComponent<PlayerController>().currentHP / 100;
        deathGague.fillAmount = GameManager.Instance.playerInstance.GetComponent<ShootingMechanic>().currentDeathGague / 100;
    }
}
