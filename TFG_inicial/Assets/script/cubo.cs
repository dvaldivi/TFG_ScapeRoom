using UnityEngine;
using System.Collections;
using System;

public class cubo : MonoBehaviour
{
    private Vector3 original;
    public enum Tipo { Planta, Fuego, Agua, Vacio };
    public Tipo mitipo;
    public Vector2 vector_tablero;
    private GameObject gameController;
    GameObject anterior = null;
    public bool destruir;
    public bool visible;
    public float tiempo_visible;
    private Material mimaterial;
    public Material Agua;
    public Material Fuego;
    public Material Vacio;
    public Material Planta;
    // Use this for initialization
    void Start()
    {
        destruir = false;
        original = this.transform.position;
        gameController = GameObject.FindGameObjectWithTag("GameController");
        Material[] mats = GetComponent<Renderer>().materials;
        if (!visible)
        {
            mimaterial = mats[0];
            GetComponent<Renderer>().material = Vacio;

        }


    }

    // Update is called once per frame
    void Update()
    {
        if (destruir)
        {
            Destroy(this.gameObject, 0.01f);

        }
        else if (!visible)
        {
            if (tiempo_visible > 0)
            {
                GetComponent<Renderer>().material = mimaterial;
                tiempo_visible += -Time.deltaTime;
            }
            else
            {
                GetComponent<Renderer>().material = Vacio;
            }
        }
    }
    public void hover()
    {
        this.transform.position = new Vector3(original.x, original.y, original.z - 5);
    }
    public void visualiza()
    {
        this.tiempo_visible = 5;
    }
    public void unhover()
    {
        this.transform.position = new Vector3(original.x, original.y, original.z);

    }
    public void mueve(Vector3 vector)
    {

        this.transform.position = new Vector3(posicionRelativa((int)vector.x), posicionRelativa((int)vector.y), (int)original.z - 10);


    }
    public void coge()
    {
        this.GetComponent<Collider>().enabled = false;

    }
    public void cambia_original(Vector3 vector)
    {
        this.original = vector;


    }
    public void esVector(Vector2 vector)
    {
        this.vector_tablero = vector;

    }
    public Vector2 dameVector()
    {
        return this.vector_tablero;

    }
    public void suelta()
    {

        int x = posicionRelativa((int)this.transform.position.x);
        int y = posicionRelativa((int)this.transform.position.y);
        RaycastHit hit;
        GameObject otro = null;
        if (Physics.Raycast(new Vector3((int)x, (int)y, (int)original.z - 10), Vector3.forward * 3000, out hit))
        {
            if (hit.collider.gameObject.GetComponent<cubo>())
            {
                otro = hit.collider.gameObject;
                otro.transform.position = original;
                otro.GetComponent<cubo>().cambia_original(original);


            }

        }

        this.transform.position = new Vector3((int)x, (int)y, original.z);
        original = this.transform.position;
        this.GetComponent<Collider>().enabled = true;
        if (otro != null)
        {
            Vector2 vector_otro = otro.GetComponent<cubo>().dameVector();
            Vector2 vector_mio = dameVector();
            this.vector_tablero = vector_otro;
            otro.GetComponent<cubo>().esVector(vector_mio);
            if (gameController.GetComponent<GameController>().contrario(otro))
            {
                gameController.GetComponent<GameController>().numero_elementos_restante--;
                otro.gameObject.GetComponent<cubo>().destruir = true;

            }
            if (gameController.GetComponent<GameController>().contrario(this.gameObject))
            {
                gameController.GetComponent<GameController>().numero_elementos_restante--;
                destruir = true;

            }

        }


    }

    private int posicionRelativa(int x)
    {
        int factor = 8;
        int division = x / factor;
        int modulo = x % factor;

        if (modulo < factor / 2)

            return factor * division;
        else
            return factor * (division + 1);

    }

}
