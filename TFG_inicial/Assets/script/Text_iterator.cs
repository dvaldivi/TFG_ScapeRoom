using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Text_iterator : MonoBehaviour {
    public string[] dialog_array; // Array con los textos posteriores al primero
    public float dialog_speed; // Velocidad a la que se muestra el texto;
    public bool automatic; // Si mostrar el siguiente texto automaticamente
    public float automatic_time; // Tiempo desde que se ha mostrado todas las letras del texto actual hasta mostrar el siguiente texto
    public string manual_button; // Nombre del boton para que cambie/agilice el texto
    private string dialog_text; // El texto del GetComponent<Text>().text;
    private Text componente_text; // El componente texto, se usa para modificar el componente
    private int dialog_num; // Indica en que texto estamos (el 0 es el primero)
    private int dialog_size;// Indica el numero de textos que hay en total
    private float original_dialog_speed; // Velocidad original del texto
    private int num_char; // Indica en que caracter del texto actual estamos
    private bool done; // Indica si ya no hay mas texto
    private float time; // Tiempo desde que se ha mostrado todas las letras del texto actual
    private string callback; // Funcion de callback, actualmente usado para cambiar de escena
    // Use this for initialization
    void Start () {
        componente_text = GetComponent<Text>();
        dialog_text = componente_text.text;
        dialog_num = -1;
        dialog_size = dialog_array.Length - 1;
        num_char = 0;
        done = false;
        time = 0;
        callback = "noCallBack";
        if(dialog_speed < 1)
        {
            dialog_speed = 1;
            original_dialog_speed = 1;
        }
        else
        {
            original_dialog_speed = dialog_speed;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!done)
        {
            if (num_char < dialog_text.Length) // actual limite 480 caracteres
            {

                for (var i = 0; i < dialog_speed; i++) { // Velocidad a la que saca el texto
                    if (num_char < dialog_text.Length)
                    {
                        componente_text.text = dialog_text.Remove(num_char);
                        num_char++;
                    }
                }
                if ( !automatic && Input.GetKeyDown(manual_button)) // Aumenta la velocidad hasta sacar todo el texto;
                {
                    dialog_speed = 5;
                }
            }
            else
            {
                if (callback != "noCallBack") // En caso de que haya "funcion" de callback
                {
                    if (time < 3f)
                    {
                        time += Time.deltaTime;

                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().nextLevel();
                        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().lose = false;
                    }
                }
                else  if (dialog_num < dialog_size)
                {
                    if (automatic) // Cambia de texto cada "automatic_time" 
                    {

                        if(time < automatic_time)
                        {
                            time += Time.deltaTime;
                        }
                        else
                        {
                            iterate_text();
                            time = 0;
                        }
                    }
                    else // Cambia de texto cada vez que se pulsa el botón correspondiente.
                    {
                        if (Input.GetKeyDown(manual_button))
                        {
                            iterate_text();
                        }
                    }
                }
                else
                {
                    done = true;
                }
            }
        }
	}

    void iterate_text(int text = -1)
    {
        if (text < 0)
        {
            dialog_num++;
        }
        else
        {
            dialog_num = text;
        }
        dialog_text = dialog_array[dialog_num];
        num_char = 0;
        dialog_speed = original_dialog_speed;
    }

    public void set_text_with_callback(string callbackFunc, string text) // Funcion para poner un texto en concreto y cambiar de escena (callbackFunc = "noCallBack" si no se quiere cambiar escena)
    {
        callback = callbackFunc;
        dialog_num = dialog_size;
        dialog_text = text;
        num_char = 0;
        time = 0;
        dialog_speed = original_dialog_speed;
        done = false;
    }
}
