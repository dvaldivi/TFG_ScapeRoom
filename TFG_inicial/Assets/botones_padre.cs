using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botones_padre : MonoBehaviour {
    public string clave;
    public string cadena_actual;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    }
}
