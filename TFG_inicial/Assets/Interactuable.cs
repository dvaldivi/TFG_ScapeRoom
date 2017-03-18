using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactuable : MonoBehaviour {
    public bool valido;
    public enum Tipo_interactuable { Mover_tablero, Mover_libremente,Boton,Menu_boton};
    public enum Funcion { unhover, hover, coge, mueve,suelta };
    public Tipo_interactuable mi_tipo;

    public Material normal;
    public Material rojo;
    private Material mimaterial;
    private float original_y;
    private float factor;
    public bool torre;
    public GameObject mitorre;


    // Use this for initialization
    void Start () {
        valido = false;
        if (this.GetComponent<Collider>()) {
            valido = true;
        }
        factor = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (valido) {
            


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
            throw new NotImplementedException();
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
        else
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
    internal void suelta()
    {
        if (mi_tipo.Equals(Tipo_interactuable.Boton))
        {
            Boton_funcion(Funcion.suelta, new Vector3(0, 0, 0));
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Menu_boton))
        {
            Menu_boton_funcion(Funcion.suelta, new Vector3(0, 0, 0));
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_libremente))
        {
            Mover_libremente_funcion(Funcion.suelta, new Vector3(0, 0, 0));
        }
        else if (mi_tipo.Equals(Tipo_interactuable.Mover_tablero))
        {
            Mover_tablero_funcion(Funcion.suelta, new Vector3(0, 0, 0));
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private void Boton_funcion(Funcion v, Transform point)
    {
        throw new NotImplementedException();
    }
    private void Mover_tablero_funcion(Funcion v, Transform vector3)
    {
        throw new NotImplementedException();
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
            if (mitorre != null)
            {
                mitorre.gameObject.GetComponent<torre_eq>().quita_cubo();
                this.transform.parent = null;
            }
            mitorre = null;
            torre = false;
        }
        else if (v.Equals(Funcion.mueve))
        {

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && factor < 1.3f)
            {
                factor += 0.1f;

            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && factor > 0.3f)
            {
                factor -= 0.1f;

            }
            this.transform.position = vector3.position + vector3.forward * 60 * factor;
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
        throw new NotImplementedException();
    }

}
