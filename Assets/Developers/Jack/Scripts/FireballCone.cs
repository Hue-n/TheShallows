using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class FireballCone : MonoBehaviour
{
    public bool active = false;
    [SerializeField] private ShootingMechanic shootMech;

    private DefaultControls controls;

    private void Awake()
    {
        controls.Controller.Attack.performed += ctx => ShootFireball();
        controls.Controller.ChooseTarget.performed += ctx => Aim(ctx.ReadValue<Vector2>());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ShootFireball()
    {

         StartCoroutine(Timer());

    }

    public void Aim(Vector2 input)
    {
       
    }

    public IEnumerator Timer()
    {
        GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(.2f);
        GetComponent<BoxCollider>().enabled = false;

        shootMech.ToggleFireball();

        gameObject.SetActive(false);

        yield break;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Damage(999);
        }
    }
}
