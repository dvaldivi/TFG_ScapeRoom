using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool tutorial;
    public bool visibles;
    public bool torres;


    /*para nivel equilibrio */


    public GameObject Vacio_recoger;
    public int n_fuego;
    public int n_agua;
    public int n_planta;
    public int n_vacios;

    public bool modo_raton;
    public GameObject Fuego;
    public GameObject Agua;
    public GameObject Planta;
    public GameObject efectoFuego;
    public GameObject efectoAgua;
    public GameObject efectoPlanta;
    public GameObject Vacio;
    public GameObject Text;
    public int dimension;
    public cubo.Tipo[,] tablero_solucion;
    public GameObject[,] tablero_solucion_gameobject;
    
    int numero_fuego;
    int numero_agua;
    int numero_planta;
    private bool reset_player;
    private Text_iterator textScript;
    /*prueba 2 para git */
    /*modo raton; empiezo */
    public bool win;
    public bool lose;
    public int numero_elementos_restante;
    public float time;
    private bool enviado_servidor;
   
    void Start()
    {
     
        Cursor.visible = false;
        time = 0;
        win = false;
        lose = false;
        reset_player = false;
        if (tutorial)
        {
            textScript = Text.GetComponent<Text_iterator>();
            
            if (SceneManager.GetActiveScene().buildIndex == 1)
                creaTableroTutorial1();
            else if (SceneManager.GetActiveScene().buildIndex == 2)
                creaTableroTutorial2();
            else {
                creaTableroTutorial3();
            }
        }
        else
        {
            textScript = Text.GetComponent<Text_iterator>();
            Text.active = false;
            creaTablero();
        }

    }

    void Update()
    {
        if (lose)
        {
            Debug.Log("has perdido");
            reset();
            //Application.LoadLevel(Application.loadedLevel);
            lose = false;
        }
        else if (!win)
        {
            time += Time.deltaTime;
        }
        else if (!enviado_servidor)
        {
            enviar_servidor(System.DateTime.Today);
            Debug.Log("has ganado");
            nextLevel();
        }
        if (modo_raton)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().modo_raton = true;
        }
        if (numero_elementos_restante == 0 && !win)
        {
            if (tutorial)
            {
                textScript.set_text_with_callback("tfg", "Bien hecho superando el tutorial!");
            }
            win = true;
        }
        if (reset_player)
        {
            if (time < 2f)
                time += Time.deltaTime;
            else
            {
                reset_player = false;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().reset();
            }

        }
    }
    // Use this for initialization
    private void creaTablero()
    {
        tablero_solucion = new cubo.Tipo[dimension, dimension];

        GameObject solucion = new GameObject();
        solucion.name = "solucion";
        GameObject juego = new GameObject();
        juego.name = "juego";
        solucion.transform.parent = this.transform;
        juego.transform.parent = this.transform;
        tablero_solucion_gameobject = new GameObject[dimension, dimension];
       
        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                int pieza_rand = UnityEngine.Random.Range(0, 10);
                if (pieza_rand > 4)
                {
                    pieza_rand = 3;
                }
                else
                {
                    pieza_rand = UnityEngine.Random.Range(0, 4);
                }
                GameObject pieza_actual = null;
                if (i == dimension / 2 && j == dimension / 2)
                    pieza_rand = 3;
                if (pieza_rand == 0)
                {
                    pieza_actual = (GameObject)Instantiate(Fuego, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                    n_fuego++;
                }
                else if (pieza_rand == 1)
                {
                    n_planta++;
                    pieza_actual = (GameObject)Instantiate(Planta, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                else if (pieza_rand == 2)
                {
                    n_agua++;
                    pieza_actual = (GameObject)Instantiate(Agua, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                else
                {
                    pieza_actual = (GameObject)Instantiate(Vacio, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                }


                pieza_actual.GetComponent<Collider>().enabled = false;
                pieza_actual.GetComponent<cubo>().visible = true;
                tablero_solucion[i, j] = pieza_actual.GetComponent<cubo>().mitipo;
                if (!(pieza_actual.GetComponent<cubo>().mitipo == global::cubo.Tipo.Vacio))
                    numero_elementos_restante++;

                pieza_actual.GetComponent<cubo>().esVector(new Vector2(i, j));
                tablero_solucion_gameobject[i, j] = pieza_actual;
                pieza_actual.transform.parent = solucion.transform;
                cubo.Tipo tipo_contrario = contrario(pieza_actual.GetComponent<cubo>().mitipo);
                if (tipo_contrario == global::cubo.Tipo.Fuego)
                {
                    pieza_actual = (GameObject)Instantiate(Fuego, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else if (tipo_contrario == global::cubo.Tipo.Agua)
                {
                    pieza_actual = (GameObject)Instantiate(Agua, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else if (tipo_contrario == global::cubo.Tipo.Planta)
                {
                    pieza_actual = (GameObject)Instantiate(Planta, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else
                {
                    pieza_actual = (GameObject)Instantiate(Vacio, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                pieza_rand = UnityEngine.Random.Range(0, 2);
                if(!visibles)
                      pieza_actual.GetComponent<cubo>().visible = (pieza_rand == 0);
                else
                    pieza_actual.GetComponent<cubo>().visible = true;
                pieza_actual.transform.parent = juego.transform;
                pieza_actual.GetComponent<cubo>().esVector(new Vector2(dimension - i - 1, dimension - j - 1));









            }
        }

        if (torres)
        {
            if (n_fuego > n_agua && n_fuego > n_planta)
            {
                n_vacios = n_fuego - n_agua + n_fuego - n_planta;

            }
            else if (n_planta > n_agua && n_planta > n_agua)
            {
                n_vacios = n_planta - n_agua + n_planta - n_fuego ;

            }
            else {
                n_vacios = n_agua - n_planta + n_agua - n_fuego;
            }

            for (int i = 0; i < n_vacios; i++) {
              GameObject temp =  Instantiate(Vacio_recoger, new Vector3(-60 + UnityEngine.Random.Range(0, 5), 20 + i*10 ,-60f + UnityEngine.Random.Range(0, 5)), Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0,180), UnityEngine.Random.Range(0, 180), UnityEngine.Random.Range(0, 180))));
                //temp.transform.parent = GameObject.FindGameObjectWithTag("Equilibrio").transform;
            }

        }
    }

    public void nextLevel()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        level++;

        if (level < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(level);
        else
            Debug.Log("ultimo nivel");
    }

    private void creaTableroTutorial1()
    {
        time = 0;
        win = false;
        tablero_solucion = new cubo.Tipo[dimension, dimension];

        Vector3 posicion = new Vector3(0, 0, 0);
        GameObject solucion = new GameObject();
        solucion.name = "solucion";
        GameObject juego = new GameObject();
        juego.name = "juego";
        solucion.transform.parent = this.transform;
        juego.transform.parent = this.transform;
        tablero_solucion_gameobject = new GameObject[dimension, dimension];

        numero_elementos_restante = 2;

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                GameObject pieza_actual = null;
                if (i == 3 && j == 4)
                {
                    pieza_actual = (GameObject)Instantiate(Fuego, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                else if( i == 6 && j == 2)
                {
                    pieza_actual = (GameObject)Instantiate(Agua, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                else
                {
                    pieza_actual = (GameObject)Instantiate(Vacio, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                pieza_actual.GetComponent<Collider>().enabled = false;
                pieza_actual.GetComponent<cubo>().visible = true;
                tablero_solucion[i, j] = pieza_actual.GetComponent<cubo>().mitipo;
                pieza_actual.GetComponent<cubo>().esVector(new Vector2(i, j));

                tablero_solucion_gameobject[i, j] = pieza_actual;
                pieza_actual.transform.parent = solucion.transform;
                cubo.Tipo tipo_contrario = contrario(pieza_actual.GetComponent<cubo>().mitipo);


                if (tipo_contrario == global::cubo.Tipo.Fuego)
                {
                    pieza_actual = (GameObject)Instantiate(Fuego, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else if (tipo_contrario == global::cubo.Tipo.Agua)
                {
                    pieza_actual = (GameObject)Instantiate(Agua, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else if (tipo_contrario == global::cubo.Tipo.Planta)
                {
                    pieza_actual = (GameObject)Instantiate(Planta, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else
                {
                    pieza_actual = (GameObject)Instantiate(Vacio, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }

                pieza_actual.transform.parent = juego.transform;
                pieza_actual.GetComponent<cubo>().visible = true;
                pieza_actual.GetComponent<cubo>().esVector(new Vector2(dimension - i - 1, dimension - j - 1));
            }
        }
    }

    private void creaTableroTutorial2()
    {
        time = 0;
        win = false;
        tablero_solucion = new cubo.Tipo[dimension, dimension];

        Vector3 posicion = new Vector3(0, 0, 0);
        GameObject solucion = new GameObject();
        solucion.name = "solucion";
        GameObject juego = new GameObject();
        juego.name = "juego";
        solucion.transform.parent = this.transform;
        juego.transform.parent = this.transform;
        tablero_solucion_gameobject = new GameObject[dimension, dimension];

        numero_elementos_restante = 2;

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                GameObject pieza_actual = null;
                if (i == 2   && j == 2)
                {
                    pieza_actual = (GameObject)Instantiate(Fuego, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                else if (i == 2 && j == 1)
                {
                    pieza_actual = (GameObject)Instantiate(Agua, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                else
                {
                    pieza_actual = (GameObject)Instantiate(Vacio, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                pieza_actual.GetComponent<Collider>().enabled = false;
                pieza_actual.GetComponent<cubo>().visible = true;
                tablero_solucion[i, j] = pieza_actual.GetComponent<cubo>().mitipo;
                pieza_actual.GetComponent<cubo>().esVector(new Vector2(i, j));

                tablero_solucion_gameobject[i, j] = pieza_actual;
                pieza_actual.transform.parent = solucion.transform;
                cubo.Tipo tipo_contrario = contrario(pieza_actual.GetComponent<cubo>().mitipo);


                if (tipo_contrario == global::cubo.Tipo.Fuego)
                {
                    pieza_actual = (GameObject)Instantiate(Fuego, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else if (tipo_contrario == global::cubo.Tipo.Agua)
                {
                    pieza_actual = (GameObject)Instantiate(Agua, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else if (tipo_contrario == global::cubo.Tipo.Planta)
                {
                    pieza_actual = (GameObject)Instantiate(Planta, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else
                {
                    pieza_actual = (GameObject)Instantiate(Vacio, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }

                pieza_actual.transform.parent = juego.transform;
                pieza_actual.GetComponent<cubo>().visible = false;
                pieza_actual.GetComponent<cubo>().esVector(new Vector2(dimension - i - 1, dimension - j - 1));
            }
        }
    }

    private void creaTableroTutorial3()
    {

        time = 0;
        win = false;
        tablero_solucion = new cubo.Tipo[dimension, dimension];

        Vector3 posicion = new Vector3(0, 0, 0);
        GameObject solucion = new GameObject();
        solucion.name = "solucion";
        GameObject juego = new GameObject();
        juego.name = "juego";
        solucion.transform.parent = this.transform;
        juego.transform.parent = this.transform;
        tablero_solucion_gameobject = new GameObject[dimension, dimension];

        numero_elementos_restante = 4;

        for (int i = 0; i < dimension; i++)
        {
            for (int j = 0; j < dimension; j++)
            {
                GameObject pieza_actual = null;
                if (i == 2 )
                {
                    pieza_actual = (GameObject)Instantiate(Fuego, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                    n_fuego++;
                }
                else if (i == 0 && j == 0)
                {
                    pieza_actual = (GameObject)Instantiate(Agua, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                    n_agua++;
                }
                else
                {
                    pieza_actual = (GameObject)Instantiate(Vacio, new Vector3(i * 8, j * 8, 50) + new Vector3(0, 0, 2), Quaternion.Euler(new Vector3(0, 0, 0)));
                }
                pieza_actual.GetComponent<Collider>().enabled = false;
                pieza_actual.GetComponent<cubo>().visible = true;
                tablero_solucion[i, j] = pieza_actual.GetComponent<cubo>().mitipo;
                pieza_actual.GetComponent<cubo>().esVector(new Vector2(i, j));

                tablero_solucion_gameobject[i, j] = pieza_actual;
                pieza_actual.transform.parent = solucion.transform;
                cubo.Tipo tipo_contrario = contrario(pieza_actual.GetComponent<cubo>().mitipo);


                if (tipo_contrario == global::cubo.Tipo.Fuego)
                {
                    pieza_actual = (GameObject)Instantiate(Fuego, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else if (tipo_contrario == global::cubo.Tipo.Agua)
                {
                    pieza_actual = (GameObject)Instantiate(Agua, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else if (tipo_contrario == global::cubo.Tipo.Planta)
                {
                    pieza_actual = (GameObject)Instantiate(Planta, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }
                else
                {
                    pieza_actual = (GameObject)Instantiate(Vacio, new Vector3((dimension - i - 1) * 8, (dimension - j - 1) * 8, 46), Quaternion.Euler(new Vector3(0, 180, 0)));
                }

                pieza_actual.transform.parent = juego.transform;
                pieza_actual.GetComponent<cubo>().visible = true;
                pieza_actual.GetComponent<cubo>().esVector(new Vector2(dimension - i - 1, dimension - j - 1));
            }
        }
        if (n_fuego > n_agua && n_fuego > n_planta)
        {
            n_vacios = n_fuego - n_agua + n_fuego - n_planta;

        }
        else if (n_planta > n_agua && n_planta > n_agua)
        {
            n_vacios = n_planta - n_agua + n_planta - n_fuego;

        }
        else
        {
            n_vacios = n_agua - n_planta + n_agua - n_fuego;
        }

        for (int i = 0; i < n_vacios; i++)
        {
            GameObject temp = Instantiate(Vacio_recoger, new Vector3(-60 + UnityEngine.Random.Range(0, 5), 20 + i * 10, -60f + UnityEngine.Random.Range(0, 5)), Quaternion.Euler(new Vector3(UnityEngine.Random.Range(0, 180), UnityEngine.Random.Range(0, 180), UnityEngine.Random.Range(0, 180))));
            
            //temp.transform.parent = GameObject.FindGameObjectWithTag("Equilibrio").transform;
        }
    }
    private void borraTablero()
    {
        n_fuego = 0;
        n_planta = 0;
        n_agua = 0;
        
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

    }
    

    private void enviar_servidor(DateTime today)
    {
        GetComponent<Comunicacion_servidor>().envia_servidor(DateTime.Now.ToString(), (int)time);
        
        enviado_servidor = true;
    }

    public void reset()
    {
        borraTablero();
        creaTablero();
        reset_player = true;



    }
    public bool contrario(GameObject cubo)
    {
        Vector2 posicion = cubo.GetComponent<cubo>().dameVector();
        cubo.Tipo tipo = cubo.GetComponent<cubo>().mitipo;

        cubo.Tipo tiposolucion = tablero_solucion[(int)posicion.x, (int)posicion.y];

        if (tiposContrarios(tipo, tiposolucion))
        {
            Destroy(tablero_solucion_gameobject[(int)posicion.x, (int)posicion.y], 0.5f);
            GameObject efecto = null;
            if (tiposolucion == global::cubo.Tipo.Fuego)
            {
                efecto = (GameObject)Instantiate(efectoFuego, cubo.transform.position - new Vector3(0, 0, -2), Quaternion.Euler(new Vector3(0, 180, 0)));

            }
            else if (tiposolucion == global::cubo.Tipo.Planta)
            {
                efecto = (GameObject)Instantiate(efectoPlanta, cubo.transform.position - new Vector3(0, 0, -2), Quaternion.Euler(new Vector3(0, 180, 0)));

            }
            else if (tiposolucion == global::cubo.Tipo.Agua)
            {
                efecto = (GameObject)Instantiate(efectoAgua, cubo.transform.position - new Vector3(0, 0, -2), Quaternion.Euler(new Vector3(0, 180, 0)));

            }
            Destroy(efecto, 1.5f);
            return true;
            Debug.Log("sol");
        }


        else return false;

    }
    private bool tiposContrarios(cubo.Tipo cubo, cubo.Tipo solucion)
    {

        if (solucion == global::cubo.Tipo.Planta && cubo == global::cubo.Tipo.Fuego ||
            solucion == global::cubo.Tipo.Agua && cubo == global::cubo.Tipo.Planta ||
            solucion == global::cubo.Tipo.Fuego && cubo == global::cubo.Tipo.Agua)
            return true;
        else
            return false;
    }
    private cubo.Tipo contrario(cubo.Tipo cubo)
    {
        if (cubo == global::cubo.Tipo.Fuego)
        {
            return global::cubo.Tipo.Agua;
        }
        else if (cubo == global::cubo.Tipo.Agua)
        {
            return global::cubo.Tipo.Planta;
        }
        else if (cubo == global::cubo.Tipo.Planta)
        {
            return global::cubo.Tipo.Fuego;
        }
        else
        {
            return global::cubo.Tipo.Vacio;
        }

    }

    void OnGUI()
    {
        int temp = (int)time;
        GUI.Label(new Rect(10, 10, 150, 100), temp.ToString());
        if(win)
            GUI.Label(new Rect(10, 30, 150, 100), "has ganado");
        if(lose)
            GUI.Label(new Rect(10, 60, 150, 100), "has perdido");



    }
}
