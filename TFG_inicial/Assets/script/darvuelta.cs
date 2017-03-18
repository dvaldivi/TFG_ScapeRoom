using UnityEngine;
using System.Collections;

public class darvuelta : MonoBehaviour {
    private Vector3 original;
    public GameObject tablero;
    public int angulo;
    public int angulo_deseado;
    public bool voltea;
    private float time;
    private bool caraA;

    void Start()
    {
        original = this.transform.position;
        voltea = false;
        time = 0;
        caraA = true;
    }



    public void hover()
    {
        daVuelta();
       
       
    }
    public void unhover()
    {
      //  tablero.transform.rotation = Quaternion.Euler(0, 180, 0);


    }
    private void daVuelta()
    {

        if (!voltea && time <= 0)
        {
            angulo = (int)tablero.transform.rotation.eulerAngles.y;
            angulo_deseado = (int)System.Math.Round(tablero.transform.rotation.eulerAngles.y + 180) % 360;
            voltea = true;
        }

    }
    public void nodaVuelta()
    {
        
        
    }
    void Update() {
        
        if (voltea) {
            
            if (System.Math.Round(tablero.transform.rotation.eulerAngles.y) != angulo_deseado )
            {

                tablero.transform.Rotate(0, -5, 0);
                
            }
            else {
                if (!caraA)
                {
                    caraA = true;
                    time = 0.2f;
                }
                else
                {
                    caraA = false;
                    time = 1f;
                }
                
                
                voltea = false;
               
            }
           
        }
        else if (time >= 0 ) {
            time -= Time.deltaTime;
            if (time <= 0) {
                if (caraA)
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().viendo_solucion = false;
                else
                    daVuelta();
            }

        }

    }
}
