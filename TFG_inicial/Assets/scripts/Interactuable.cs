using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactuable : MonoBehaviour {
    
    public enum Tipo_interactuable { Mover_tablero, Mover_libremente,Boton,Menu_boton,Moverse};
    public enum Funcion { unhover, hover, coge, mueve,suelta };
    public Tipo_interactuable mi_tipo;
    //cosas de boton
    public Vector3 desplazamiento;
    public Vector3 pos_original;
    public bool pulsado;
    public float tiempo_pulsado_mant;
    
    //boton reacciona al pulsar 
    public Material normal;
    public Material rojo;
    private Material mimaterial;
    public bool clave;
    
    public char letra = 'a';
    //boton tablero 
    private float original_y;
    private float factor;
    public bool torre;
    public GameObject mitorre;
    //moverse
    public int t_hover;
    public int t_moverse;
    public bool encima;
    // Use this for initialization
    void Start () {
        if (letra == ' ') {
            letra = 'a';
        }
        encima = false;
        if (!this.GetComponent<Collider>()) {
            this.gameObject.AddComponent<BoxCollider>();
        }
        if (!this.GetComponent<Rigidbody>())
        {
            this.gameObject.AddComponent<Rigidbody>();
        }
        
        if (mi_tipo.Equals(Tipo_interactuable.Boton)) {
            if (this.GetComponent<Rigidbody>())
            {
                this.GetComponent<Rigidbody>().useGravity = false;
            }
            else
            {
                Debug.Log("Rigidbody not attached");
            }
               
            this.GetComponent<Collider>().isTrigger = true;
            pos_original = this.transform.position;
        }
        
        factor = 1;
    }
	
	// Update is called once per frame
    /*
     * UPDATE
     */
	void Update () {
        
        if (pulsado && tiempo_pulsado_mant >= 0 ) {
                tiempo_pulsado_mant -= Time.deltaTime;
            if (tiempo_pulsado_mant < 0) {
                suelta(null,null);
            }

        }
        if (mi_tipo.Equals(Tipo_interactuable.Moverse))
        {
            //Debug.Log(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.transform.position));
            if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.transform.position) < 0.6f)
            {
                
            
                this.GetComponent<Collider>().enabled = false;

            }
            else {

                this.GetComponent<Collider>().enabled = true;       

            }
            
             if (t_hover > 0)
            {
                t_hover -= 1;
            }
            if (t_hover > t_moverse)
            {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMueve>().moviendo)
                {
                    t_hover = t_moverse/10;
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMueve>().mueveA(this.transform);
                    t_hover = 0;

                    encima = false;
                }
            }
        }


        
	}

    internal void unhover()

    {

      

        
        if (mi_tipo.Equals(Tipo_interactuable.Boton))
        {
            Boton_funcion(Funcion.unhover, null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Menu_boton))
        {
            Menu_boton_funcion(Funcion.unhover, null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_libremente))
        {
            Mover_libremente_funcion(Funcion.unhover, null,null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_tablero))
        {
            Mover_tablero_funcion(Funcion.unhover, null);
        }

        else if (mi_tipo.Equals(Tipo_interactuable.Moverse))
        {
            Moverse_funcion(Funcion.unhover, null);
        }
        else {
           // throw new NotImplementedException();
        }
    }
    internal void hover()
    {
        if (mi_tipo.Equals(Tipo_interactuable.Boton))
        {
            Boton_funcion(Funcion.hover, null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Menu_boton))
        {
            Menu_boton_funcion(Funcion.hover, null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_libremente))
        {
            Mover_libremente_funcion(Funcion.hover, null,null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_tablero))
        {
            Mover_tablero_funcion(Funcion.hover, null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Moverse))
        {
            Moverse_funcion(Funcion.hover, null);
        }
        else
        {
            //throw new NotImplementedException();
        }
    }

   
    internal void coge(Transform transform,Boolean oculus)
    {
        if (mi_tipo.Equals(Tipo_interactuable.Boton))
        {
            Boton_funcion(Funcion.coge,transform);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Menu_boton))
        {
            Menu_boton_funcion(Funcion.coge, transform);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_libremente))
        {
            Mover_libremente_funcion(Funcion.coge, transform,null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_tablero))
        {
            Mover_tablero_funcion(Funcion.coge, transform);
        }
        else
        {
          //  throw new NotImplementedException();
        }
    }
    internal void mueve(Transform point)
    {

        if (mi_tipo.Equals(Tipo_interactuable.Boton))
        {
            Boton_funcion(Funcion.mueve, point);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Menu_boton))
        {
            Menu_boton_funcion(Funcion.mueve, point);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_libremente))
        {
            Mover_libremente_funcion(Funcion.mueve, point,null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_tablero))
        {
            Mover_tablero_funcion(Funcion.mueve, point);
        }
        else
        {
            //throw new NotImplementedException();
        }
    }
    internal void suelta(Transform posicion, Transform aceleracion)
    {
        if (mi_tipo.Equals(Tipo_interactuable.Boton))
        {
            Boton_funcion(Funcion.suelta, null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Menu_boton))
        {
            Menu_boton_funcion(Funcion.suelta, null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_libremente))
        {
            Mover_libremente_funcion(Funcion.suelta, posicion,aceleracion);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_tablero))
        {
            Mover_tablero_funcion(Funcion.suelta, null);
        }
        else
        {
            //throw new NotImplementedException();
        }
    }
    /*
     * BOTON SIMPLE
     */
    private void Boton_funcion(Funcion v, Transform point)
    {
        if (v.Equals(Funcion.hover))
        {
            if(!pulsado && rojo != null)
            GetComponent<Renderer>().material = rojo;
        }
        else if (v.Equals(Funcion.unhover))
        {
            if (!pulsado && normal != null)
                GetComponent<Renderer>().material = normal;
        }
        else if (v.Equals(Funcion.coge))
        {

            this.transform.position = pos_original + desplazamiento;
            tiempo_pulsado_mant = 1;
            if (this.gameObject.transform.parent != null && this.gameObject.transform.parent.gameObject.GetComponent<botones_padre>()) {
                if (this.gameObject.transform.parent.gameObject.GetComponent<botones_padre>().caracter(letra))
                {
                    pulsado = false;
                }
                else {
                    pulsado = true;
                }
                }
          
        }
        else if (v.Equals(Funcion.mueve))
        {

        }
        else if (v.Equals(Funcion.suelta))
        {
            this.transform.position = pos_original ;
            if (this.GetComponent<Rigidbody>())
            {
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            else {
                Debug.Log("Rigidbody not attached");
            }
            if(normal != null)
            GetComponent<Renderer>().material = normal;
            pulsado = false;
        }


    }
    /*
     *MOVERSE
     */
    private void Moverse_funcion(Funcion v, object p)
    {
        if (v.Equals(Funcion.hover) )
        {
            t_hover += 10;
            encima = true;
           
        }
        else if (v.Equals(Funcion.unhover))
        {/*
            encima = false;
            t_hover = 0;
            */
        }
        else if (v.Equals(Funcion.coge))
        {
            Debug.Log("entra en mov ");
            this.transform.position = pos_original + desplazamiento;
            tiempo_pulsado_mant = 5;
            pulsado = true;
        }
        else if (v.Equals(Funcion.mueve))
        {

        }
        else if (v.Equals(Funcion.suelta))
        {
            this.transform.position = pos_original;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            if(normal != null)
            GetComponent<Renderer>().material = normal;
            pulsado = false;
        }
    }

    private void Mover_tablero_funcion(Funcion v, Transform vector3)
    {
       // throw new NotImplementedException();
    }

    private void Mover_libremente_funcion(Funcion v, Transform posicion, Transform aceleracion)
    {
        if (v.Equals(Funcion.hover))

        {
            Debug.Log("libremente");
            if(rojo != null)
            GetComponent<Renderer>().material = rojo;
        }
        else if (v.Equals(Funcion.unhover))
        {
            if(normal != null)
            GetComponent<Renderer>().material = normal;
        }
        else if (v.Equals(Funcion.coge))
        {

            factor = 1;
            original_y = transform.position.y;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;


           
        }
        else if (v.Equals(Funcion.mueve))
        {
            factor = 0;

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && factor < 2f)
            {
                factor += 0.1f;

            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && factor > 0.5f)
            {
                factor -= 0.1f;

            }
            this.transform.position = posicion.position+ new Vector3(0,0.15f,0);
            this.transform.rotation = posicion.rotation ;
            /*  if (this.transform.position.y < original_y)
              {

                  this.transform.position = new Vector3(this.transform.position.x, original_y, this.transform.position.z);
              }*/
        }
        else if (v.Equals(Funcion.suelta))
        {

            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            this.transform.position = posicion.position + new Vector3(0,0.15f,0) ;
            Debug.Log("Envia con fuerza " + aceleracion.transform.position);
            this.GetComponent<Rigidbody>().AddForce(new Vector3(aceleracion.transform.position.x * 500f, aceleracion.transform.position.y * 1000f, aceleracion.transform.position.z * 2600f));
            /*if (normal != null)
                GetComponent<Renderer>().material = normal;*/
            /*if (mitorre != null)
            {
                if (Vector3.Distance(this.transform.position, mitorre.transform.position) < 50)
                {
                    this.transform.position = new Vector3(mitorre.transform.position.x, this.transform.position.y, mitorre.transform.position.z);
                    torre = true;
                    
                    this.transform.parent = mitorre.transform.parent.transform;
                    this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                }
                else
                {
                    torre = false;
                    mitorre = null;
                    this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
            }*/
        }




    }
    private void Menu_boton_funcion(Funcion v, Transform vector3)
    {
       // throw new NotImplementedException();
    }

}
