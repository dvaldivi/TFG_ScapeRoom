using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botones_padre : MonoBehaviour {
    public string clave;
    public string cadena_actual;
    public Vector3 desplazamiento;
    public Vector3 original;
    public bool activada;
    public GameObject trampilla;
    public GameObject zona_mover;
    // Use this for initialization
    void Start () {
        activada = false;
        original = trampilla.transform.position;
        desplazamiento = trampilla.transform.position + desplazamiento;
    }
	
	// Update is called once per frame
	void Update () {
        if (activada)
        {
            Vector3 temp = desplazamiento - original;
            if (Vector3.Distance(desplazamiento, original) > 0.01f)
            {
                trampilla.transform.position = trampilla.transform.position + temp / 50;
                original = trampilla.transform.position;
            }
            zona_mover.SetActive(true);
        }
        else {
            zona_mover.SetActive(false);
        }
	}
    public void caracter(char car) {
        if (cadena_actual.Length < clave.Length)
        {
            cadena_actual = cadena_actual + car;
        }
        else {
            cadena_actual = cadena_actual + car;
            cadena_actual = cadena_actual.Substring(1, clave.Length);
        }
        if (cadena_actual.Equals(clave)) {
            activada = true;
        }
    }
}
