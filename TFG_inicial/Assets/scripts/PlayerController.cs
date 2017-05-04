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

    public GameObject interactuable;
    public bool seleccionado;
    private float tiempo;
    public HandModel Mano_izq;
    public Hand leap_hand_izq;
    public Hand leap_hand_drcha;
    public HandModel Mano_derecha;
    private bool isHit;
    public LeapProvider provider = null;
    private Transform camara_ob; 
  
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
        camara_ob = this.transform.FindChild("Main Camera").transform;
        wantedMode = CursorLockMode.Locked;
        SetCursorState();
        rotationX = camara_ob.transform.rotation.x;
        rotationY = camara_ob.transform.rotation.y;
        rotationZ = camara_ob.transform.rotation.z;
        seleccionado = false;
        tiempo = 0;
        

        leap_hand_izq = Mano_izq.GetLeapHand();
        leap_hand_drcha = Mano_derecha.GetLeapHand();
        /*
        if (modo_raton) {
            provider.enabled = true;
        }     else
            provider.enabled = false;
            */

    }

    // Update is called once per frame
    void Update()
    {
        if (modo_raton)
        {
            mano_falsa_izq.SetActive(true);
            mano_falsa_dcha.SetActive(true);
            camara();
            juego_raton();
        }
        else
        {

            mano_falsa_izq.SetActive(false);
            mano_falsa_dcha.SetActive(false);
            
            juego_oculus();

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
     
        interactuable = null;

    }


    /*
     * 
     * 
     * recoger objeto mas cercano a una posicion
     *    close_colliders = Physics.OverlapSphere(pos_final, 0.13f);
                        int j = 0;

                        for (int i = 0; i < close_colliders.Length && j < 1; i++)
                        {
                            Debug.Log(close_colliders[i].gameObject.layer);
                            if (close_colliders[i].gameObject.tag == "Grab")
                            {
                                GameObject temp = close_colliders[i].gameObject;
                                gameObjects[j] = temp;
                                j++;
                            }

                        }


     * */
    
    public void juego_oculus()
    {
        Debug.Log("juego oculus");
        RaycastHit hit;
        tiempo += Time.deltaTime;
        //mano derecha
        Vector3 posicion_relativa = Vector3.zero;

        Debug.DrawRay(camara_ob.transform.position, camara_ob.transform.forward, Color.green);
        //  Debug.DrawRay(transform.position, transform.forward * 3000, Color.red);

        posicion_relativa = posRelativa_raton(mano_falsa_izq.transform.position);

       
            if (interactuable != null && seleccionado && interactuable.GetComponent<Interactuable>().mi_tipo.Equals(Interactuable.Tipo_interactuable.Mover_libremente))
        {
            interactuable.GetComponent<Interactuable>().mueve(this.transform);

            if (Input.GetButtonUp("Fire1") && tiempo > 1f)
            {
                if (interactuable.GetComponent<Interactuable>())
                    interactuable.GetComponent<Interactuable>().suelta();

                interactuable = null;
                seleccionado = false;
                tiempo = 0;

            }
            else if (Input.GetButtonUp("Fire2") && tiempo > 1f)
            {
                interactuable.GetComponent<Rigidbody>().AddForce(this.transform.forward * 1000);
                if (interactuable.GetComponent<Interactuable>())
                    interactuable.GetComponent<Interactuable>().suelta();

                interactuable = null;
                seleccionado = false;
                tiempo = 0;

            }

        }

        if (Mano_derecha.IsTracked)
        {
          
            
            Collider[] close_colliders = Physics.OverlapSphere(Mano_derecha.GetPalmPosition(), 0.2f);
            int j = 0;
            GameObject obj_cercano = null;
            for (int i = 0; i < close_colliders.Length && j < 1; i++)
            {
                
                if (close_colliders[i].gameObject.GetComponent<Interactuable>() && !close_colliders[i].gameObject.GetComponent<Interactuable>().mi_tipo.Equals(Interactuable.Tipo_interactuable.Moverse))
                {
                    Debug.Log(close_colliders[i].gameObject.name);
                    obj_cercano = close_colliders[i].gameObject;
                    
                    j++;
                }


            }
            if (obj_cercano != null && !seleccionado) {

                if (interactuable == null)
                {

                    if (obj_cercano.GetComponent<Interactuable>())
                    {

                        Interactuable.Tipo_interactuable temp = obj_cercano.GetComponent<Interactuable>().mi_tipo;
                       
                        obj_cercano.GetComponent<Interactuable>().hover();
                        interactuable = obj_cercano;
                        

                    }

                }
                else if (interactuable != null && obj_cercano.name != interactuable.gameObject.name)
                {

                    if (interactuable.GetComponent<Interactuable>())
                    {
                        interactuable.GetComponent<Interactuable>().unhover();


                    }
                    if (obj_cercano.GetComponent<Interactuable>())
                    {
                        Interactuable.Tipo_interactuable temp = obj_cercano.GetComponent<Interactuable>().mi_tipo;

                        obj_cercano.GetComponent<Interactuable>().hover();
                        interactuable = obj_cercano;
                        

                    }


                }
                else
                {


                    if (interactuable.GetComponent<Interactuable>())
                    {
                        interactuable.GetComponent<Interactuable>().hover();


                    }
                    if (leap_hand_drcha.PinchStrength > 0.5 && tiempo > 1f)
                    {
                        tiempo = 0;

                        seleccionado = true;
                        if (interactuable.GetComponent<Interactuable>())
                        {
                            if (!interactuable.GetComponent<Interactuable>().mi_tipo.Equals(Interactuable.Tipo_interactuable.Boton))
                            {

                                interactuable.GetComponent<Interactuable>().coge(camara_ob.transform);
                            }
                            else if (!interactuable.GetComponent<Interactuable>().pulsado)
                            {
                                interactuable.GetComponent<Interactuable>().coge(camara_ob.transform);//es mas un pulsa

                                interactuable = null;
                                seleccionado = false;
                                tiempo = 0;

                            }


                        }



                    }
                }





            } 
        }
       
    }

    public void juego_raton()
    {
        RaycastHit hit;
        tiempo += Time.deltaTime;
        //mano derecha
        Vector3 posicion_relativa = Vector3.zero;
        
        Debug.DrawRay(camara_ob.transform.position, camara_ob.transform.forward, Color.green);
        //  Debug.DrawRay(transform.position, transform.forward * 3000, Color.red);

        posicion_relativa = posRelativa_raton(mano_falsa_izq.transform.position);


        if (interactuable != null && seleccionado && interactuable.GetComponent<Interactuable>().mi_tipo.Equals(Interactuable.Tipo_interactuable.Mover_libremente))
        {
            interactuable.GetComponent<Interactuable>().mueve(this.transform);

            if (Input.GetButtonUp("Fire1") && tiempo > 1f)
            {
                if (interactuable.GetComponent<Interactuable>())
                    interactuable.GetComponent<Interactuable>().suelta();

                interactuable = null;
                seleccionado = false;
                tiempo = 0;

            }
            else if (Input.GetButtonUp("Fire2") && tiempo > 1f)
            {
                interactuable.GetComponent<Rigidbody>().AddForce(this.transform.forward * 1000);
                if (interactuable.GetComponent<Interactuable>())
                    interactuable.GetComponent<Interactuable>().suelta();

                interactuable = null;
                seleccionado = false;
                tiempo = 0;

            }

        }
        else if ( Physics.Raycast(camara_ob.transform.position, camara_ob.transform.forward *2, out hit) && !seleccionado)
        {

            cruceta.transform.rotation = camara_ob.transform.rotation;
            cruceta.transform.position = hit.transform.position;
            /**/

            if (interactuable == null)
            {
                Debug.Log("poraqui" + hit.collider.name);
                if (hit.collider.gameObject.GetComponent<Interactuable>())
                {
                    Debug.Log(" choca");
                    Interactuable.Tipo_interactuable temp = hit.collider.gameObject.GetComponent<Interactuable>().mi_tipo;
                    if (temp.Equals(Interactuable.Tipo_interactuable.Moverse))
                    {
                        Debug.Log("cuate " + Vector3.Distance(this.transform.position, hit.collider.gameObject.transform.position));
                        float temp2 = Vector3.Distance(this.transform.position, hit.collider.gameObject.transform.position);
                        if (temp2 < 3 && temp2 > 0.6)
                        {
                            hit.collider.gameObject.GetComponent<Interactuable>().hover();
                            interactuable = hit.collider.gameObject;

                        }
                    }
                    else
                    {
                        hit.collider.gameObject.GetComponent<Interactuable>().hover();
                        interactuable = hit.collider.gameObject;
                    }

                }
                else {
                    Debug.Log("no interactuables");
                }
            }
            else if (interactuable != null && hit.collider.gameObject.name != interactuable.gameObject.name)
            {
               
                if (interactuable.GetComponent<Interactuable>())
                {
                    interactuable.GetComponent<Interactuable>().unhover();


                }
                if (hit.collider.gameObject.GetComponent<Interactuable>())
                {
                    Interactuable.Tipo_interactuable temp = hit.collider.gameObject.GetComponent<Interactuable>().mi_tipo;
                    if (temp.Equals(Interactuable.Tipo_interactuable.Moverse))
                    {
                        Debug.Log(Vector3.Distance(camara_ob.transform.position, hit.collider.gameObject.transform.position));
                        float temp2 = Vector3.Distance(camara_ob.transform.position, hit.collider.gameObject.transform.position);
                        if (temp2 < 3 && temp2 > 0.6)
                        {
                            hit.collider.gameObject.GetComponent<Interactuable>().hover();
                            interactuable = hit.collider.gameObject;

                        }
                    }
                    else
                    {
                        hit.collider.gameObject.GetComponent<Interactuable>().hover();
                        interactuable = hit.collider.gameObject;
                    }

                }


            }
            /**/




            else
            {
               

                if (interactuable.GetComponent<Interactuable>())
                {
                    interactuable.GetComponent<Interactuable>().hover();


                }
                if (Input.GetButton("Fire1") && tiempo > 1f && !interactuable.GetComponent<Interactuable>().mi_tipo.Equals(Interactuable.Tipo_interactuable.Moverse))
                {
                    tiempo = 0;

                    seleccionado = true;
                    if (interactuable.GetComponent<Interactuable>())
                    {
                        if (!interactuable.GetComponent<Interactuable>().mi_tipo.Equals(Interactuable.Tipo_interactuable.Boton))
                        {

                            interactuable.GetComponent<Interactuable>().coge(camara_ob.transform);
                        }
                        else if (!interactuable.GetComponent<Interactuable>().pulsado)
                        {
                            interactuable.GetComponent<Interactuable>().coge(camara_ob.transform);//es mas un pulsa

                            interactuable = null;
                            seleccionado = false;
                            tiempo = 0;

                        }


                    }



                }
            }
            /**/
            

        }
        else
        {
            if (interactuable != null)
            {
                interactuable.GetComponent<Interactuable>().unhover();
          

            }

        }
    }

    private void camara()
    {
        rotationY += Input.GetAxis("Mouse X") * sensitivityX;
        rotationX -= Input.GetAxis("Mouse Y") * sensitivityY;
        rotationZ -= 0;
        camara_ob.transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, rotationZ));
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
