using UnityEngine;
using System.Collections;
using Leap;

public class apuntar : MonoBehaviour
{

    Leap.Controller m_leapController;
    
    public Vector3 cosa = new Vector3(0, 0, 0);
    public Vector3 vector_relativo_normalizado = new Vector3(0, 0, 0);
    const float PINCH_DISTANCE = 200.0f;
    private bool pinching_;

    public int layer_id;
    public int layer_mask_;
    private Collider[] close_colliders;
    public GameObject[] gameObjects;
    public GameObject seguimiento;
    public float t_cog = 2f;
    public float t_ult_cog = 0f;
    public bool puede_coger = true;
    public GameObject bola;
    public GameObject recogido;
    // Use this for initialization
    void Start()
    {

        m_leapController = new Controller();

        pinching_ = false;

        layer_mask_ = 1;
        layer_id = LayerMask.NameToLayer("Pelota");
        layer_mask_ = 1 << layer_id;
        gameObjects = new GameObject[1];
        recogido = null;
    }

    Hand GetLeftMostHand(Frame f)
    {
        float smallestVal = float.MaxValue;
        Hand h = null;
        for (int i = 0; i < f.Hands.Count; ++i)
        {
            if (f.Hands[i].PalmPosition.x < smallestVal)
            {
                //Debug.Log(smallestVal);
                smallestVal = f.Hands[i].PalmPosition.x;
                h = f.Hands[i];
            }
        }
        return h;
    }

    Hand GetRightMostHand(Frame f)
    {
        float largestVal = -float.MaxValue;
        Hand h = null;
        for (int i = 0; i < f.Hands.Count; ++i)
        {


            if (f.Hands[i].PalmPosition.x > largestVal)
            {
                largestVal = f.Hands[i].PalmPosition.x;
                h = f.Hands[i];

            }
        }
        return h;
    }

    void Update()
    {
        if (!puede_coger)
        {
            t_ult_cog += Time.deltaTime;
            if (t_ult_cog > t_cog)
            {
                puede_coger = true;
            }
        }

    }
    void FixedUpdate()
    {

        Frame frame = m_leapController.Frame();

        if (frame.Hands.Count >= 0)
        {

            Hand rightHand = GetRightMostHand(frame);
            if (rightHand != null)
            {
                InteractionBox box = frame.InteractionBox;



                Vector otra_Cosa = box.NormalizePoint(rightHand.PalmPosition, true);

                vector_relativo_normalizado = new Vector3(otra_Cosa.x, otra_Cosa.y, 1 - otra_Cosa.z);
                Vector3 pos_0_mano = new Vector3(0.35f, -0.3f, -0.15f);
                float factor_adaptacion = 0.3f;
                Vector3 adaptacion = new Vector3(factor_adaptacion, factor_adaptacion, factor_adaptacion);
                Vector3 pos_final = new Vector3(pos_0_mano.x + vector_relativo_normalizado.x * adaptacion.x,
                    pos_0_mano.y + vector_relativo_normalizado.y * adaptacion.y,
                    pos_0_mano.z + vector_relativo_normalizado.z * adaptacion.z);


                //seguimiento.transform.position = new Vector3(pos_final.x, pos_final.y,pos_final.z);
                
                cosa = new Vector3(pos_final.x, pos_final.y, pos_final.z);
                if (recogido != null)
                {
                    recogido.transform.position = new Vector3(pos_final.x, pos_final.y, pos_final.z);
                }
                if (rightHand.PinchStrength > 0.5)
                {

                    if (puede_coger)
                    {


                        close_colliders = Physics.OverlapSphere(pos_final, 0.13f);
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
                        {
                            
                                recogido = gameObjects[0];
                            if (recogido != null)
                            {
                                recogido.gameObject.GetComponent<Rigidbody>().useGravity = false;
                            }
                        }

                    }
                   

                }
                else
                {
                    if (recogido != null)
                    {
                        recogido.gameObject.GetComponent<Rigidbody>().useGravity = true;
                        recogido = null;
                    }

                    
                }

            }

        }
    }
}
