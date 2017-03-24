using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMueve : MonoBehaviour {
    public Transform objetivo;
    public  GameObject guia;
    public bool moviendo;
    public float velocidad;
    // Use this for initialization
    void Start () {
        moviendo = false;
        velocidad = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (moviendo)
        {
            guia.transform.LookAt(objetivo);
            
            if (Vector3.Distance(new Vector3 (this.transform.position.x,0, this.transform.position.z), new Vector3(objetivo.transform.position.x, 0, objetivo.transform.position.z)) > 0.5f)
            {
                

                //vector desplazamiento 
                Vector3 desplazamiento = objetivo.transform.position - this.transform.position ;
                
                //this.transform.localRotation = guia.transform.localRotation;
                this.transform.localPosition = this.transform.localPosition + new Vector3(desplazamiento.x/100,0, desplazamiento.z/100);
            }
            else {
                moviendo = false;
            }
        }
    }
    public void mueveA(Transform position) {
        
        objetivo = position;
        moviendo = true;
        velocidad = 1 / Vector3.Distance(new Vector3(this.transform.position.x, 0, this.transform.position.z), new Vector3(objetivo.transform.position.x, 0, objetivo.transform.position.z));
    }
}
