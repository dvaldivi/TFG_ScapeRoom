using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comunicacion_servidor : MonoBehaviour
{
    public bool enviado = false;
    // Use this for initialization
    void Start()
    {
        // StartCoroutine(UploadPNG());
        /*
        StartCoroutine(envia_tiempo_objetos("hola", 2));
        StartCoroutine(envia_tiempo_objetos("prueba", 23));
        StartCoroutine(envia_tiempo_objetos("tatata", 23));*/



    }

    // Update is called once per frame
    void Update()
    {


    }
    public bool envia_servidor(string fecha, int time)
    {

        StartCoroutine(envia_tiempo_objetos(fecha, time));
        return true;
    }

    private string screenShotURL = "http://monitorizer.sytes.net:8000/polls/add_moves/";

    // Use this for initialization





        
    IEnumerator envia_tiempo_objetos(string nombre, int movimientos)
    {
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();

        WWWForm form = new WWWForm();
        form.AddField("status", "won");
        form.AddField("name", nombre);
        form.AddField("movimientos", movimientos.ToString());
        WWW link = new WWW("http://monitorizer.sytes.net:8000/polls/add_moves/", form);

        // this is what you need to add
        yield return link;


        if (!string.IsNullOrEmpty(link.error))
        {
            print(link.error);
        }
        else
        {
            print("Finished Uploading Screenshot");
        }
        print("Finished Uploading Screenshot23232");

    }



    internal void envia_info_servidor_Json(JSONObject j)
    {
        StartCoroutine(envia_tiempo_objetos(j));

    }

    private IEnumerator envia_tiempo_objetos(JSONObject j)
    {

        // We should only read the screen after all rendering is complete

        yield return new WaitForEndOfFrame();

        WWWForm form = new WWWForm();
        form.AddField("json", j.Print());

        WWW link = new WWW("http://monitorizer.sytes.net:8000/polls/add_session/", form);

        // this is what you need to add
        yield return link;


        if (!string.IsNullOrEmpty(link.error))
        {
            print(link.error);
        }
        else
        {
            print("Finished Uploading Screenshot");
        }
        Debug.Log("Enviado a servidor");

    }
    /*
    internal bool envia_info_servidor(string name, int movimientos)
    {

        StartCoroutine(envia_tiempo_objetos(name, movimientos));
        Debug.Log(name + "  " + movimientos.ToString());
        return true;
    }
    */

}
