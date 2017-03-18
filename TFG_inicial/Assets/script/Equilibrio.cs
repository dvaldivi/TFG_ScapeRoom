using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equilibrio : MonoBehaviour {
    public  enum Tipo { Planta, Fuego, Agua, Vacio };
    public GameObject Fuego_recoger;
    public GameObject Agua_recoger;
    public GameObject Planta_recoger;
    public Transform pos_Fuego;
    public Transform pos_Agua;
    public Transform pos_Planta;
    public GameObject mesa;
    public int n_Fuego;
    public int n_Agua;
    public int n_Planta;
    private float tiempo; 
    // Use this for initialization
    void Start () {
        tiempo = 0;
        this.GetComponent<BoxCollider>().enabled = false;
      

    }

    // Update is called once per frame
    private void cae(Tipo tipo) {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Rigidbody>()){
                child.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                
            }
        }
       
        this.GetComponent<BoxCollider>().enabled = true;
       
        mesa.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -30));
        if (tipo.Equals(Tipo.Agua))
        {
            mesa.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -30));
        }
        else if (tipo.Equals(Tipo.Planta))
        {
            mesa.transform.rotation = Quaternion.Euler(new Vector3(-30, 0, 0));
        }
        else {
            mesa.transform.rotation = Quaternion.Euler(new Vector3(30, 0, 0));

        }
        

    }
	void Update () {
        tiempo += Time.deltaTime;
        

        if (!comprueba())
        {
            if (n_Fuego - n_Agua > 2 || n_Fuego - n_Planta > 2)
            {
                cae(Tipo.Fuego);
            }
            else if (n_Planta - n_Agua > 2 || n_Planta - n_Fuego > 2)
            {
                cae(Tipo.Planta);
            }
            else if (n_Agua - n_Planta > 2 || n_Agua - n_Fuego > 2)
            {
                cae(Tipo.Agua);
            }
        }
    }
    public void aparece_cubo(Tipo tipo) {
        GameObject temp;
        if (tipo.Equals(Tipo.Fuego)) {
            temp = Instantiate(Fuego_recoger, pos_Fuego.position + new Vector3(0,20,0), Quaternion.Euler(new Vector3(0, 0, 0)));
            n_Fuego++;

        }
        else if (tipo.Equals(Tipo.Agua))
        {
            n_Agua++;
            temp = Instantiate(Agua_recoger, pos_Agua.position + new Vector3(0, 20, 0), Quaternion.Euler(new Vector3(0, 0, 0)));

        }
        else {
            n_Planta++;
            temp = Instantiate(Planta_recoger, pos_Planta.position + new Vector3(0, 20, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        temp.transform.parent = this.transform;
        temp.GetComponent<Rigidbody>().AddTorque(transform.right * 300 * 300);
    }
    public void añade_cubo_vacio(Tipo tipo)
    {
        if (tipo.Equals(Tipo.Fuego))
        {
            n_Fuego++;

        }
        else if (tipo.Equals(Tipo.Agua))
        {
            n_Agua++;

        }
        else
        {
            n_Planta++;
        }
        if (!comprueba()) GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().lose = true;
    }
    public void quita_cubo_vacio(Tipo tipo)
    {
        if (tipo.Equals(Tipo.Fuego))
        {
            n_Fuego--;

        }
        else if (tipo.Equals(Tipo.Agua))
        {
            n_Agua--;

        }
        else
        {
            n_Planta--;
        }
        if (!comprueba()) GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().lose = true;
    }
    private bool comprueba()
    {

     
        if (Math.Abs(n_Fuego - n_Agua)>  2 || Math.Abs(n_Fuego - n_Planta) > 2 || 
        Math.Abs(n_Planta - n_Agua) > 2 || Math.Abs(n_Planta - n_Fuego) > 2 || 
       
        (Math.Abs(n_Agua - n_Planta) > 2 || Math.Abs(n_Agua - n_Fuego) > 2 )){

            return false;
        }
        
           
        
     
        return true;
    }
}
