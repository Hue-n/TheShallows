using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnim : MonoBehaviour
{
    private float materialSlots = 1;

    public float lifeTime = 1;

    private Material inst;

    private MeshRenderer _renderer;

    [SerializeField] private float currentVal = 1;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        materialSlots = _renderer.materials.Length;

        currentVal = _renderer.material.GetFloat("_Step");

        // fill the material slots with instances of each material in the mesh renderer.
        for (int i = 0; i < materialSlots; i++)
        {
            inst = new Material(_renderer.materials[i]);
            _renderer.sharedMaterials[i] = inst;
        }

        if (transform.parent != null)
        {
            Destroy(gameObject.transform.parent.gameObject, lifeTime);
        }
        else
        { 
            Destroy(gameObject, lifeTime);
        }
    }

    private void Update()
    {
        currentVal += Time.deltaTime;

        // update all the materials.
        for (int i = 0; i < materialSlots; i++)
        {
            inst = _renderer.sharedMaterials[i];
            inst.SetFloat("_Preview", 0);
            inst.SetFloat("_Step", currentVal);
        }
    }
}
