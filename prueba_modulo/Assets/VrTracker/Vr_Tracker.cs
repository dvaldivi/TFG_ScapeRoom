using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vr_Tracker : MonoBehaviour
{
    /*RECOGER INFO OBJETOS
     * */
    struct myStruct
    {
        int a;
        int b;
    }
    private GameObject GAMECONTROLLER;
    public List<String> noqueridos_nombre_exacto;
    public List<String> noqueridos_nombre_parecido;
    public List<String> noqueridos_tags;
    public float time_life_minimo;
    public List<GameObject> static_objects;
    public List<GameObject> dinamic_objects;
    public List<KeyValuePair<string, int>> dinamic_objects_delete;
    

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
        dinamic_objects_delete =  new List<KeyValuePair<string, int>>();

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
       
        
        dinamic_objects_delete.Add(new KeyValuePair<string,int>(other.GetComponent<Vr_Tracker_Object_info>().name, other.GetComponent<Vr_Tracker_Object_info>().movimientos));
        dinamic_objects.Remove(other);


    }
    public void send_info(string status,float time_new)
    {
        if (!visto2)
        {
            int time = (int)time_new;
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
            List<GameObject> all_objects =  new List<GameObject>();
            int sum_mov = 0;

            
            
            for (int i = 0; i < dinamic_objects.Count; i++)

            {

                
                
                if (dinamic_objects[i].GetComponent<Vr_Tracker_Object_info>().movimientos != 0)
                {
                    
                    sum_mov += dinamic_objects[i].GetComponent<Vr_Tracker_Object_info>().movimientos;
                    arr.AddField(dinamic_objects[i].GetComponent<Vr_Tracker_Object_info>().name, dinamic_objects[i].GetComponent<Vr_Tracker_Object_info>().movimientos);
                }
                

            }
            
        
            for (int i = 0; i < dinamic_objects_delete.Count; i++)
            {
                if (dinamic_objects_delete[i].Value != 0)
                {
                   
                    sum_mov += dinamic_objects_delete[i].Value;
                    arr.AddField(dinamic_objects_delete[i].Key, dinamic_objects_delete[i].Value);
                }
                
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
            enviado = true;
        }
        else
        {
            enviado = true;
            print("Finished Uploading Info");
        }
       

    }
}
