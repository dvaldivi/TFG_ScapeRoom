using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Recopilador_informacion : MonoBehaviour
{
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
    // Use this for initialization
    void Start()
    {
        visto = false;
        visto2 = false;
        GAMECONTROLLER = GameObject.FindGameObjectWithTag("GameController");
       
       

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

     
        Debug.Log("añade objeto");
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

                    temp.AddComponent<Object_info>();

                    dinamic_objects.Add(go.gameObject);
                }
            }
            else
            {
                //objeto estatico
                //print(go.name.Substring(0, 3) + " is an static object");
                //para que no me recoja la mano como objeto estatico
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
        temp.AddComponent<Object_info>();
        temp.GetComponent<Object_info>().delete(other.GetComponent<Object_info>().name, other.GetComponent<Object_info>().movimientos);
        temp.gameObject.name = other.GetComponent<Object_info>().name;
        if (other.GetComponent<Object_info>().life_time > time_life_minimo)
        {
            dinamic_objects.Remove(other);
            dinamic_objects_delete.Add(temp);
        }
        

    }
    public void envia_toda_info()
    {
        if (!visto2)
        {
            int time = (int)GAMECONTROLLER.GetComponent<GameController2>().time;
            String player = "prueba";
            JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
            Scene scene = SceneManager.GetActiveScene();
          
            /*j.AddField("devClass", "14139636727181677031");*/
            j.AddField("state", "won");
            j.AddField("nivel", scene.name);
            j.AddField("usuario", "admin");
        
        
            j.AddField("time_resolve", time);
            JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
            j.AddField("objetos", arr);
            List<GameObject> all_objects = static_objects = new List<GameObject>();
            for (int i = 0; i < dinamic_objects.Count; i++)
            {
                all_objects.Add(dinamic_objects[i]);
                //this.GetComponent<Comunicacion_servidor>().envia_info_servidor(dinamic_objects[i].GetComponent<Object_info>().name, dinamic_objects[i].GetComponent<Object_info>().movimientos);

            }
            for (int i = 0; i < dinamic_objects_delete.Count; i++)
            {
                all_objects.Add(static_objects[i]);
                //this.GetComponent<Comunicacion_servidor>().envia_info_servidor(dinamic_objects_delete[i].GetComponent<Object_info>().name, dinamic_objects_delete[i].GetComponent<Object_info>().movimientos);
            }
            //this.GetComponent<Comunicacion_servidor>().envia_info_servidorJson(all_objects);
            int sum_mov = 0;

            for (int i = 0; i < all_objects.Count; i++)
            {
                if(all_objects[i].GetComponent<Object_info>().movimientos != 0)
                    arr.AddField(all_objects[i].GetComponent<Object_info>().name, all_objects[i].GetComponent<Object_info>().movimientos);
                sum_mov += all_objects[i].GetComponent<Object_info>().movimientos;
                //this.GetComponent<Comunicacion_servidor>().envia_info_servidor(dinamic_objects_delete[i].GetComponent<Object_info>().name, dinamic_objects_delete[i].GetComponent<Object_info>().movimientos);
            }
            j.AddField("movimientos_tot", sum_mov);
            Debug.Log(j.Print());
            this.GetComponent<Comunicacion_servidor>().envia_info_servidor_Json(j);
            visto2 = true;
        }
    }
}
