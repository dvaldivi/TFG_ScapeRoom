using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Menu_seleccionar : MonoBehaviour {
    private Vector3 original;

    public Menu_principal.Tipo_menu mitipo;
    
    void Start()
    {
      
        original = this.transform.position;
       

    }

    
    public void hover()
    {
        this.transform.position = new Vector3(original.x, original.y, original.z - 10);
    }
    
    public void unhover()
    {
        this.transform.position = new Vector3(original.x, original.y, original.z);

    }
    public void mueve(Vector3 vector)
    {

     


    }
    public void coge()
    {
        this.GetComponent<Collider>().enabled = false;

    }
    public void cambia_original(Vector3 vector)
    {
        this.original = vector;


    }
    
    public void suelta()
    {

        
       


    }
    public void selecciona() {
        Debug.Log("seleccion " + mitipo);
        
        transform.parent.gameObject.transform.parent.gameObject.GetComponent<Menu_principal>().manage_option(mitipo);

        unhover();
    }

   
}
