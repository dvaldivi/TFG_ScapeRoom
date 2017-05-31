using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vr_Tracker : MonoBehaviour
{
    /*RECOGER INFO OBJETOS
     * */
    private GameObject GAMECONTROLLER;
    public List<String> noqueridos_nombre_exacto;
    public List<String> noqueridos_nombre_parecido;
    public List<String> noqueridos_tags;
    public float time_life_minimo;
    public List<GameObject> static_objects;
    public List<GameObject> dinamic_objects;
    private List<GameObject> dinamic_objects_delete;
    public int n_objetos;
    private bool visto;
    private bool visto2;
    /*COMUNICACION SERVIDOR
     * */
    public bool enviado = false;
    public string Url_envio_datos = "http://monitorizer.sytes.net:8000/polls/add_session/";
    // Use this for initialization
    void Start()
    {
        visto = false;
        visto2 = false;
        GAMECONTROLLER = GameObject.FindGameObjectWithTag("GameController");
        Scene scene = SceneManager.GetActiveScene();

        Debug.Log("Active scene is '" + scene.name + "'.");


    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 1 && !visto)
        {
            anade_objetos();
            visto = true;
        }
        
      
    }

    private void anade_objetos()
    {

        static_objects = new List<GameObject>();
        dinamic_objects = new List<GameObject>();
        dinamic_objects_delete = new List<GameObject>();

        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        n_objetos = 0;

        Dictionary<String, int> diccionario = new Dictionary<String, int>();

     

        foreach (GameObject go in allObjects)
        {
            bool valido = true;
            
            if (go.GetComponent<Collider>())
            {
                //objeto dinamico
                GameObject temp = go.gameObject;
                String nombre_obj = temp.name.Replace("(Clone)", "");
                for (int i = 0; i < noqueridos_nombre_parecido.Count && valido ; i++)
                {
                    if (nombre_obj.Contains(noqueridos_nombre_parecido[i]))
                        valido = false;
                    

                }
               
             
                if (!valido || (valido && noqueridos_nombre_exacto.Contains(nombre_obj)))
                {
                    valido = false;
                }
                else
                {

                    if (diccionario.ContainsKey(nombre_obj))
                    {
                        int value = diccionario[nombre_obj];
                        String nuevo_name = nombre_obj + "_" + diccionario[nombre_obj];
                        value++;
                        diccionario[nombre_obj] = value;
                        nombre_obj = nuevo_name;
                    }
                    else
                    {
                        diccionario[nombre_obj] = 1;
                    }
                    go.name = nombre_obj;

                    temp.AddComponent<Vr_Tracker_Object_info>();

                    dinamic_objects.Add(go.gameObject);
                }
            }
            else
            {
                
                for (int i = 0; i < noqueridos_nombre_parecido.Count && valido; i++)
                {
                    if (go.name.Contains(noqueridos_nombre_parecido[i]))
                        valido = false;


                }


                if (!valido || (valido && noqueridos_nombre_exacto.Contains(go.name)))
                {
                    valido = false;
                }
                else
                {
                    if (go.name.Substring(0, 3) != "Bip")
                        static_objects.Add(go.gameObject);
                    else
                        n_objetos--;
                }
            }
            n_objetos++;
        }
    }

    internal void borraObjeto(GameObject other)
    {
       var temp = new GameObject();
        temp.AddComponent<Vr_Tracker_Object_info>();
        temp.GetComponent<Vr_Tracker_Object_info>().delete(other.GetComponent<Vr_Tracker_Object_info>().name, other.GetComponent<Vr_Tracker_Object_info>().movimientos);
        temp.gameObject.name = other.GetComponent<Vr_Tracker_Object_info>().name;
        if (other.GetComponent<Vr_Tracker_Object_info>().life_time > time_life_minimo)
        {
            dinamic_objects.Remove(other);
            dinamic_objects_delete.Add(temp);
        }
        

    }
    public void send_info(string status)
    {
        if (!visto2)
        {
            int time = (int)GAMECONTROLLER.GetComponent<GameController2>().time;
            String player = "prueba";
            JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
            Scene scene = SceneManager.GetActiveScene();
          
            /*j.AddField("devClass", "14139636727181677031");*/
            j.AddField("state", status);
            j.AddField("nivel", scene.name);
            j.AddField("usuario", PlayerPrefs.GetString("UserName"));
            j.AddField("devClass", PlayerPrefs.GetString("AppCode"));


            j.AddField("time_resolve", time);
            JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
            j.AddField("objetos", arr);
            List<GameObject> all_objects = static_objects = new List<GameObject>();
            for (int i = 0; i < dinamic_objects.Count; i++)
            {
                all_objects.Add(dinamic_objects[i]);
                

            }
            for (int i = 0; i < dinamic_objects_delete.Count; i++)
            {
                all_objects.Add(static_objects[i]);
                
            }
           
            int sum_mov = 0;

            for (int i = 0; i < all_objects.Count; i++)
            {
                if(all_objects[i].GetComponent<Vr_Tracker_Object_info>().movimientos != 0)
                    arr.AddField(all_objects[i].GetComponent<Vr_Tracker_Object_info>().name, all_objects[i].GetComponent<Vr_Tracker_Object_info>().movimientos);
                sum_mov += all_objects[i].GetComponent<Vr_Tracker_Object_info>().movimientos;
                
            }
            j.AddField("movimientos_tot", sum_mov);
            Debug.Log(j.Print());
            this.envia_info_servidor_Json(j);
            visto2 = true;
        }
    }

    internal void envia_info_servidor_Json(JSONObject j)
    {
        StartCoroutine(envia_tiempo_objetos(j));

    }

    private IEnumerator envia_tiempo_objetos(JSONObject j)
    {



        yield return new WaitForEndOfFrame();

        WWWForm form = new WWWForm();
        form.AddField("json", j.Print());

        WWW link = new WWW("http://monitorizer.sytes.net:8000/polls/add_session/", form);


        yield return link;


        if (!string.IsNullOrEmpty(link.error))
        {
            print(link.error);
        }
        else
        {
            print("Finished Uploading Info");
        }
       

    }
}
