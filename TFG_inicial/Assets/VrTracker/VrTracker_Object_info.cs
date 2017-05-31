using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vr_Tracker_Object_info : MonoBehaviour
{
    public string name;
    public int movimientos;
    private float time_update;
    public float life_time;
    private Vector3 anterior;
    private Vector3 actual;
    private bool isShuttingDown;
    // Use this for initialization
    void Start()
    {
        name = this.gameObject.name;
        anterior = this.transform.position;
        actual = this.transform.position;
        isShuttingDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        life_time += Time.deltaTime;
        if (life_time > 1)
        {
            if (time_update <= 0)
            {
                time_update = 1;
                if (cambiaPosicion())
                {
                    movimientos++;
                }
            }

            time_update -= Time.deltaTime;
        }


    }

    private bool cambiaPosicion()
    {
        actual = this.transform.position;

        if (anterior != actual)
        {
            anterior = actual;
            return true;
        }
        else
            return false;
    }
    void OnDestroy()
    {
        if (!isShuttingDown)
        {
            if (GameObject.FindGameObjectWithTag("GameController"))
            {

                GameObject temp = GameObject.FindGameObjectWithTag("GameController");
                temp.GetComponent<Vr_Tracker>().borraObjeto(this.gameObject);
            }
        }


    }

    internal void delete(string name, int movimientos)
    {
        this.name = name;
        this.movimientos = movimientos;
    }

    void  OnApplicationQuit()
    {
        isShuttingDown = true;
    }
}
