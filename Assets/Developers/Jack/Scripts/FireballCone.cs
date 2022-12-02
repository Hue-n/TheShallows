using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireballCone : MonoBehaviour
{
    public bool active = false;
    [SerializeField] private ShootingMechanic shootMech;
    [SerializeField] private PlayerCon_KrakenQuest playerCon;
    private DefaultControls controls;

    private bool canShoot = true;

    private void Awake()
    {
        controls = new DefaultControls();

        controls.Controller.Attack.performed += ctx => ShootFireball();
        controls.Controller.ChooseTarget.performed += ctx => Aim(ctx.ReadValue<Vector2>());

    }

    private void OnEnable()
    {
        controls.Controller.Enable();
    }

    private void OnDisable()
    {
        controls.Controller.Disable();
    }

    public void ShootFireball()
    {
         if (playerCon.fbAmmo > 0 && canShoot)
        {
            playerCon.fbAmmo -= 1;
            playerCon.SetFBAmmoText();
            StartCoroutine(Timer());
            canShoot = false;
        }
         

    }

    public void Aim(Vector2 input)
    {
        Vector3.RotateTowards(transform.rotation.eulerAngles, input, 1, 0);
    }

    public IEnumerator Timer()
    {
        GetComponent<BoxCollider>().enabled = true;
        yield return new WaitForSeconds(.1f);
        GetComponent<BoxCollider>().enabled = false;
        canShoot = true;


        GetComponent<BoxCollider>().enabled = false;
        shootMech.ToggleFireball();

        yield break;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Damage(999);
        }
        if (other.CompareTag("Debris"))
        {
            Destroy(other.gameObject);
        }
    }
}
