using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_principal : MonoBehaviour {
    public enum Tipo_menu { Continuar, Random, Estadisticas, Opciones, Tutorial , atras };
    public GameObject nivel1;
    public GameObject nivel2;
    public GameObject nivel3;
    public GameObject nivel4;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void manage_option(Tipo_menu mitipo) {
        int nivel_player = PlayerPrefs.GetInt("nivel_player");
        if (mitipo.Equals(Tipo_menu.Continuar))
        {
            if (nivel_player == 0)
            {
                SceneManager.LoadScene(1);
            }
            else
                SceneManager.LoadScene(nivel_player);

        }
        else if (mitipo.Equals(Tipo_menu.Tutorial))
        {
            //carga tu
            SceneManager.LoadScene(1);

        }
        else if (mitipo.Equals(Tipo_menu.Estadisticas))
        {
            //quita nivel 1 de menu, carga estadisticas y representa 
            // o redirreciona a nuestra pag
            nivel1.active = false;
            nivel2.active = false;
            nivel3.active = false;
            nivel4.active = true;

        }
        else if (mitipo.Equals(Tipo_menu.Opciones))
        {
            //quita nivel 1, abre nivel 2 
            nivel1.active = false;
            nivel2.active = true;
            nivel3.active = false;
            nivel4.active = false;


        }
        else if (mitipo.Equals(Tipo_menu.Random))
        {
            SceneManager.LoadScene(4);
            //quita nivel 1, abre nivel 3 
            nivel1.active = false;
            nivel2.active = true;
            nivel3.active = false;
            nivel4.active = false;

        }
        else if (mitipo.Equals(Tipo_menu.atras))
        {
            //quita nivel 1, abre nivel 3 
            nivel1.active = true;
            nivel2.active = false;
            nivel3.active = false;
            nivel4.active = false;

        }

    }
}
