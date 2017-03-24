using UnityEngine;
using System.Collections;
using System;
using Leap;
using Leap.Unity;

public class PlayerController : MonoBehaviour
{
    public GameObject cruceta;
    /*Modo de juego con raton */
    public bool modo_raton;
    public GameObject mano_falsa_izq;
    public GameObject mano_falsa_dcha;
    /*variables normales*/
    private float rotationX;
    private float rotationY;
    public float sensitivityX;
    public float sensitivityY;
    private float rotationZ;

    private GameObject interactuable;
    private bool seleccionado;
    private float tiempo;
    public HandModel Mano_izq;
    public Hand leap_hand_izq;
    public Hand leap_hand_drcha;
    public HandModel Mano_derecha;
    private bool isHit;
    public LeapProvider provider = null;
    public bool viendo_solucion;
    public GameObject efectoFuego;
    public GameObject miefectoFuego;
    public Transform pos_camara;
    CursorLockMode wantedMode;
    // Use this for initialization
    // Apply requested cursor state
    void SetCursorState()
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }
    void Start()
    {
        wantedMode = CursorLockMode.Locked;
        SetCursorState();
        rotationX = this.transform.rotation.x;
        rotationY = this.transform.rotation.y;
        rotationZ = this.transform.rotation.z;
        seleccionado = false;
        tiempo = 0;
        viendo_solucion = false;

        leap_hand_izq = Mano_izq.GetLeapHand();
        leap_hand_drcha = Mano_derecha.GetLeapHand();
        miefectoFuego = (GameObject)Instantiate(efectoFuego, new Vector3(0, 0, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
        miefectoFuego.transform.localScale = new Vector3(4, 4, 4);
        miefectoFuego.gameObject.SetActive(false);
        miefectoFuego.gameObject.GetComponent<Efecto>().equilibrio_bool = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (modo_raton) {
            mano_falsa_izq.SetActive(true);
            mano_falsa_dcha.SetActive(true);
            camara();
            juego_raton();
        }
        else {

            mano_falsa_izq.SetActive(false);
            mano_falsa_dcha.SetActive(false);
            leap_hand_drcha = Mano_derecha.GetLeapHand();
            leap_hand_izq = Mano_izq.GetLeapHand();
            juego();
        }
       





    }

    private Vector3 posRelativa(Vector v)
    {

        float x_temp = v.x - transform.position.x;
        float y_temp = v.y - transform.position.y;

        float leapRangex = 0.6f;
        //48
        float appRangex = 15;
        x_temp = (x_temp - (-0.3f)) * (appRangex / leapRangex) + 0;
        y_temp = (y_temp - (-0.3f)) * (appRangex / leapRangex) + 0;
        return new Vector3(x_temp, y_temp, 10);
    }

    private Vector3 posRelativa_raton(Vector3 v)
    {

        float x_temp = v.x - transform.position.x;
        float y_temp = v.y - transform.position.y;

        float leapRangex = 0.6f;
        //48
        float appRangex = 15;
        x_temp = (x_temp - (-0.3f)) * (appRangex / leapRangex) + 0;
        y_temp = (y_temp - (-0.3f)) * (appRangex / leapRangex) + 0;
        return new Vector3(x_temp, y_temp, 10);
    }
    public void reset()
    {
        viendo_solucion = false;
        interactuable = null;

    }
    public void juego()
    {
        RaycastHit hit;
        tiempo += Time.deltaTime;
        //mano derecha
        Vector3 posicion_relativa = Vector3.zero;


        

        /*si esta la mano derecha */

        if (Mano_derecha.IsTracked)
        {
            /*prueba de pausa
             se puede utilizar unir pulgar y indice para realizar pausa,
             otro gesto podria ser levantar pulgar y cerrar los demas dedos, autostop             
             */
            

            posicion_relativa = posRelativa(leap_hand_drcha.PalmPosition);
            /*si no esta viendo la solucion*/
            if (!viendo_solucion && Physics.Raycast(pos_camara.transform.position + posicion_relativa, transform.forward * 100000, out hit))
            {
                /*si no tengo ningun cubo recogido*/
                if (!seleccionado)
                {
                    /*si lo que estoy tocando es un cubo y es distinto al ultimo cubo que toque, 
                     esto sirve para que resalte el cubo que puede coger */
                    if (hit.collider.gameObject.tag == "cubo" && hit.collider.gameObject != interactuable)
                    {
                        if (interactuable != null)
                        {
                            if (interactuable.GetComponent<cubo>())
                                interactuable.GetComponent<cubo>().unhover();
                            else if (interactuable.GetComponent<cubo_recoger>())
                                interactuable.GetComponent<cubo_recoger>().unhover();


                        }
                        if (hit.collider.gameObject.GetComponent<cubo>())
                            hit.collider.gameObject.GetComponent<cubo>().hover();
                        else if (hit.collider.gameObject.GetComponent<cubo_recoger>())
                            hit.collider.gameObject.GetComponent<cubo_recoger>().hover();
                        else
                        {
                            if (hit.collider.gameObject.GetComponent<darvuelta>())
                                hit.collider.gameObject.GetComponent<darvuelta>().hover();
                            if (hit.collider.gameObject.GetComponent<Reset>())
                                hit.collider.gameObject.GetComponent<Reset>().hover();
                            viendo_solucion = true;

                        }
                        interactuable = hit.collider.gameObject;
                    }
                    else if (hit.collider.gameObject.tag == "cubo" && hit.collider.gameObject == interactuable)
                    {
                        /*seleccionar un cubo para moverlo en el tablero*/
                        if (leap_hand_drcha.PinchStrength > 0.5 && tiempo > 1f)
                        {
                            tiempo = 0;

                            seleccionado = true;
                            if (interactuable.GetComponent<cubo>())
                                interactuable.GetComponent<cubo>().coge();
                            else if (interactuable.GetComponent<cubo_recoger>())
                                interactuable.GetComponent<cubo_recoger>().coge(this.transform);



                        }
                    }
                    /*si pasa por un sitio que no sea cubo, quitar resalto*/
                    else if (interactuable != null)
                    {
                        if (interactuable.GetComponent<cubo>())
                            interactuable.GetComponent<cubo>().unhover();
                        else if (interactuable.GetComponent<cubo_recoger>())
                            interactuable.GetComponent<cubo_recoger>().unhover();
                        else
                            interactuable.GetComponent<darvuelta>().unhover();
                        interactuable = null;
                    }

                }
                /*si tengo uno seleccionado/ recogido*/
                else
                {
                    if (interactuable.GetComponent<cubo>())
                        interactuable.GetComponent<cubo>().mueve(hit.point);
                    else if (interactuable.GetComponent<cubo_recoger>())
                        interactuable.GetComponent<cubo_recoger>().mueve(this.transform);
                    
                    if (leap_hand_drcha.PinchStrength < 0.5 && tiempo > 1f)
                    {
                        if (interactuable.GetComponent<cubo>())
                        interactuable.GetComponent<cubo>().suelta();
                        else if (interactuable.GetComponent<cubo_recoger>())
                            interactuable.GetComponent<cubo_recoger>().suelta();
                        interactuable = null;
                        seleccionado = false;
                        tiempo = 0;

                    }
                }
            }


        }
        //mano izq 
        posicion_relativa = Vector3.zero;
     
        if (Mano_izq.IsTracked)
        {

            posicion_relativa = posRelativa(leap_hand_izq.PalmPosition);

            Debug.DrawRay(pos_camara.transform.position + posicion_relativa, transform.forward * 3000, Color.green);

            if (!viendo_solucion && Physics.Raycast(pos_camara.transform.position + posicion_relativa, transform.forward * 3000, out hit))
            {
                if (hit.collider.gameObject.tag == "cubo")
                {
                    if (hit.collider.gameObject.GetComponent<cubo>())
                     hit.collider.gameObject.GetComponent<cubo>().visualiza();


                }
                miefectoFuego.gameObject.SetActive(true);
                miefectoFuego.transform.position = hit.point-new Vector3(0,0,10);

            }





        }
        else {
            miefectoFuego.gameObject.SetActive(false);
        }
        
           



    }


    public void juego_raton()
    {
        RaycastHit hit;
        tiempo += Time.deltaTime;
        //mano derecha
        Vector3 posicion_relativa = Vector3.zero;


      //  Debug.DrawRay(transform.position, transform.forward * 3000, Color.red);

        posicion_relativa = posRelativa_raton(mano_falsa_izq.transform.position);


        if (interactuable != null && seleccionado && interactuable.GetComponent<Interactuable>().mi_tipo.Equals(Interactuable.Tipo_interactuable.Mover_libremente))
            interactuable.GetComponent<Interactuable>().mueve(this.transform);


        // if (!viendo_solucion && Physics.Raycast(pos_camara.transform.position + posicion_relativa, transform.forward * 100000, out hit))
        if (!viendo_solucion && Physics.Raycast(transform.position, transform.forward * 3000, out hit))
        {

            cruceta.transform.position = hit.point;
            cruceta.transform.rotation = this.transform.rotation;
            if (!seleccionado)
                {
                    if (hit.collider.gameObject.GetComponent<Interactuable>() && hit.collider.gameObject != interactuable && !miefectoFuego.gameObject.activeSelf)
                    {
                        if (interactuable != null)
                        {
                        if (interactuable.GetComponent<Interactuable>()) {
                            interactuable.GetComponent<Interactuable>().unhover();
                        }
                            

                        }

                    if (hit.collider.gameObject.GetComponent<Interactuable>())
                        hit.collider.gameObject.GetComponent<Interactuable>().hover();
                   
                        interactuable = hit.collider.gameObject;
                    }
                    else if (hit.collider.gameObject.GetComponent<Interactuable>() && hit.collider.gameObject == interactuable)
                    {

                    if (Input.GetButton("Fire1") && tiempo > 1f)
                    {
                        tiempo = 0;

                        seleccionado = true;
                        if (interactuable.GetComponent<Interactuable>()){
                            if (!interactuable.GetComponent<Interactuable>().mi_tipo.Equals(Interactuable.Tipo_interactuable.Boton))
                            {

                                interactuable.GetComponent<Interactuable>().coge(this.transform);
                            }
                            else if(!interactuable.GetComponent<Interactuable>().pulsado)
                            {
                                interactuable.GetComponent<Interactuable>().coge(this.transform);//es mas un pulsa
                                
                                interactuable = null;
                                seleccionado = false;
                                tiempo = 0;

                            }


                        } 
                       


                    }
                    }
                    else if (interactuable != null)
                    {
                        if (interactuable.GetComponent<Interactuable>())
                            interactuable.GetComponent<Interactuable>().unhover();
                        
                        interactuable = null;
                    }

                }
                else
                {
               
                if (Input.GetButtonUp("Fire1") && tiempo > 1f)
                    {
                    if(interactuable.GetComponent<Interactuable>())
                        interactuable.GetComponent<Interactuable>().suelta();
                  
                    interactuable = null;
                    seleccionado = false;
                    tiempo = 0;

                    }
                }
            }


      
        




    }

    private void camara()
    {
        rotationY += Input.GetAxis("Mouse X") * sensitivityX;
        rotationX -= Input.GetAxis("Mouse Y") * sensitivityY;
        rotationZ -= 0 ;
        this.transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, rotationZ));
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        // Release cursor on escape keypress
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = wantedMode = CursorLockMode.None;

        switch (Cursor.lockState)
        {
            case CursorLockMode.None:
                GUILayout.Label("Cursor is normal");
                if (GUILayout.Button("Lock cursor"))
                    wantedMode = CursorLockMode.Locked;
                if (GUILayout.Button("Confine cursor"))
                    wantedMode = CursorLockMode.Confined;
                break;
            case CursorLockMode.Confined:
                GUILayout.Label("Cursor is confined");
                if (GUILayout.Button("Lock cursor"))
                    wantedMode = CursorLockMode.Locked;
                if (GUILayout.Button("Release cursor"))
                    wantedMode = CursorLockMode.None;
                break;
            case CursorLockMode.Locked:
                GUILayout.Label("Raton bloqueado, pulsa Esc desbloquear");
                if (GUILayout.Button("Unlock cursor"))
                    wantedMode = CursorLockMode.None;
                if (GUILayout.Button("Confine cursor"))
                    wantedMode = CursorLockMode.Confined;
                break;
        }

        GUILayout.EndVertical();

        SetCursorState();
    }
}
