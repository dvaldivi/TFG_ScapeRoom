using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Efecto : MonoBehaviour {
    
    public Equilibrio.Tipo tipo;
    private  GameObject Equilibrio;
    private bool enviado;
    public  bool equilibrio_bool;
    private bool torres_true;
    // Use this for initialization
    void Start () {
        torres_true = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().torres;
        
        if (torres_true)
        {

            enviado = false;
            Equilibrio = GameObject.FindGameObjectWithTag("Equilibrio");
        }
        else enviado = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (!enviado && !equilibrio_bool && torres_true)
        {
           Equilibrio.GetComponent<Equilibrio>().aparece_cubo(tipo);
            enviado = true;
        }
	}
}
