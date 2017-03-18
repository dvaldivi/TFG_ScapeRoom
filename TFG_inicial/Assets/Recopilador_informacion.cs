using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recopilador_informacion : MonoBehaviour {
    public GameObject Player;
    public List<GameObject> static_objects;
    public List<GameObject>  dinamic_objects;
    public int n_objetos;
    public bool visto; 
    // Use this for initialization
    void Start () {
        visto = false;

    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > 1 && !visto) {
            anade_objetos();
            visto = true;
        }
	}

    private void anade_objetos()
    {
        //
        static_objects = new List<GameObject>();
        dinamic_objects = new List<GameObject>();
        Player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        n_objetos =0;
        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<Collider>())
            {
                //objeto dinamico
                print(go.name + " is an active object");
                dinamic_objects.Add(go.gameObject);
            }
            else
            {
                //objeto estatico
                print(go.name.Substring(0, 3) + " is an static object");
                //para que no me recoja la mano como objeto estatico
                if (go.name.Substring(0, 3) != "Bip")
                    static_objects.Add(go.gameObject);
                else
                    n_objetos--;
            }
            n_objetos++;
        }
    }
}
