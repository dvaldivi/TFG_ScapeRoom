using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nota_visible_agua : MonoBehaviour {
    public float cantidad;
    public Material normal;
    public Material rojo;
    private Material mimaterial;
    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material = normal;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void llena()
    {
        if (cantidad > 0)
        {
            GetComponent<Renderer>().material = rojo;
            Debug.Log("Solucion");

        }
        else {
            cantidad += 10;
        }

    }
}
