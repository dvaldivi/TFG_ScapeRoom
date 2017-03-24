using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactuable : MonoBehaviour {
    public bool valido;
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

    //boton tablero 
    private float original_y;
    private float factor;
    public bool torre;
    public GameObject mitorre;
    //moverse
    public int t_hover;
    public bool encima;
    // Use this for initialization
    void Start () {
        valido = false;
        encima = false;
        if (this.GetComponent<Collider>()) {
            valido = true;
            if (mi_tipo.Equals(Tipo_interactuable.Boton)) {
                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<Collider>().isTrigger = true;
                pos_original = this.transform.position;
            }
        }
        factor = 1;
    }
	
	// Update is called once per frame
    /*
     * UPDATE
     */
	void Update () {
        if (valido) {
            if (pulsado && tiempo_pulsado_mant >= 0 ) {
                 tiempo_pulsado_mant -= Time.deltaTime;
                if (tiempo_pulsado_mant < 0) {
                    suelta();
                }

            }
            if (encima)
                t_hover++;
           
            if (t_hover > 100) {
                if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMueve>().moviendo)
                {
                 
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMueve>().mueveA(this.transform);
                    t_hover = 0;
                    Debug.Log(this.name);
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
            Mover_libremente_funcion(Funcion.unhover, null);
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_tablero))
        {
            Mover_tablero_funcion(Funcion.unhover, null);
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
            Mover_libremente_funcion(Funcion.hover, null);
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

   
    internal void coge(Transform transform)
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
            Mover_libremente_funcion(Funcion.coge, transform);
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
            Mover_libremente_funcion(Funcion.mueve, point);
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
    internal void suelta()
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
            Mover_libremente_funcion(Funcion.suelta, null);
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
            if(!pulsado)
            GetComponent<Renderer>().material = rojo;
        }
        else if (v.Equals(Funcion.unhover))
        {
            if (!pulsado)
                GetComponent<Renderer>().material = normal;
        }
        else if (v.Equals(Funcion.coge))
        {

            this.transform.position = pos_original + desplazamiento;
            tiempo_pulsado_mant = 5;
            pulsado = true;
        }
        else if (v.Equals(Funcion.mueve))
        {

        }
        else if (v.Equals(Funcion.suelta))
        {
            this.transform.position = pos_original ;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Renderer>().material = normal;
            pulsado = false;
        }


    }
    /*
     *MOVERSE
     */
    private void Moverse_funcion(Funcion v, object p)
    {
        if (v.Equals(Funcion.hover))
        {
            encima = true;
           
        }
        else if (v.Equals(Funcion.unhover))
        {
            encima = false;
            t_hover = 0;
        }
        else if (v.Equals(Funcion.coge))
        {

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
            GetComponent<Renderer>().material = normal;
            pulsado = false;
        }
    }

    private void Mover_tablero_funcion(Funcion v, Transform vector3)
    {
       // throw new NotImplementedException();
    }

    private void Mover_libremente_funcion(Funcion v, Transform vector3)
    {
        if (v.Equals(Funcion.hover))
        {
            GetComponent<Renderer>().material = rojo;
        }
        else if (v.Equals(Funcion.unhover))
        {

            GetComponent<Renderer>().material = normal;
        }
        else if (v.Equals(Funcion.coge))
        {

            factor = 1;
            original_y = transform.position.y;
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;

            Debug.Log("coge");
           
        }
        else if (v.Equals(Funcion.mueve))
        {

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && factor < 2f)
            {
                factor += 0.1f;

            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && factor > 0.5f)
            {
                factor -= 0.1f;

            }
            this.transform.position = vector3.position + vector3.forward * factor;
            if (this.transform.position.y < original_y)
            {

                this.transform.position = new Vector3(this.transform.position.x, original_y, this.transform.position.z);
            }
        }
        else if (v.Equals(Funcion.suelta))
        {

            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponent<Renderer>().material = normal;
            if (mitorre != null)
            {
                if (Vector3.Distance(this.transform.position, mitorre.transform.position) < 50)
                {
                    this.transform.position = new Vector3(mitorre.transform.position.x, this.transform.position.y, mitorre.transform.position.z);
                    torre = true;
                    mitorre.gameObject.GetComponent<torre_eq>().añade_cubo();
                    this.transform.parent = mitorre.transform.parent.transform;
                    this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                }
                else
                {
                    torre = false;
                    mitorre = null;
                    this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
            }
        }




    }
    private void Menu_boton_funcion(Funcion v, Transform vector3)
    {
       // throw new NotImplementedException();
    }

}
