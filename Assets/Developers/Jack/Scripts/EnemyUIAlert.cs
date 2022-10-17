using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIAlert : MonoBehaviour
{
    public Canvas canvas;
    private GameObject alertIMG;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        alertIMG = transform.GetChild(0).gameObject;
    }

    public void Alerter(float duration)
    {
        alertIMG.SetActive(true);
        StartCoroutine(Duration(duration));

    }

    private IEnumerator Duration(float dur)
    {
        yield return new WaitForSeconds(dur);


        alertIMG.SetActive(false);
        yield break;
    }
}
