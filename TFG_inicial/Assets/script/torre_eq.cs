using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torre_eq : MonoBehaviour {
    public Equilibrio.Tipo tipo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void añade_cubo()
    {
        this.transform.parent.gameObject.GetComponent<Equilibrio>().añade_cubo_vacio(tipo);
    }
    public void quita_cubo()
    {
        this.transform.parent.gameObject.GetComponent<Equilibrio>().quita_cubo_vacio(tipo);

    }

}
