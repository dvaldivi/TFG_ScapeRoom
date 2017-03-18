using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comunicacion_servidor : MonoBehaviour {

	// Use this for initialization
	void Start () {
       // StartCoroutine(UploadPNG());
    }

    // Update is called once per frame
    void Update()
    {


    }
        public bool envia_servidor(string fecha,int time) {

        StartCoroutine(envia_tiempo_usuario(fecha, time));
        return true;
    }

    public string screenShotURL = "http://cucuruchete.comeze.com/register.php";

    // Use this for initialization


        

    IEnumerator UploadPNG()
    {
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();

        WWWForm form = new WWWForm();
        form.AddField("user", "unity_user2");
        form.AddField("pass", "asdasdsa");
        WWW link = new WWW("http://cucuruchete.comeze.com/register.php", form);

        // this is what you need to add
        yield return link;


        if (!string.IsNullOrEmpty(link.error))
        {
            print(link.error);
        }
        else
        {
            print("Finished Uploading Screenshot");
        }

        /*
        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddField("user", "unity_user");
        form.AddField("pass", "unity_pass");
        form.AddField("direccion", "unity_dir");
        form.AddField("telefono", "unity_dir");
        // Upload to a cgi script
        WWW w = new WWW(screenShotURL, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
        }
        else
        {
            print("Finished Uploading Screenshot");
        }
        */
    }

    IEnumerator envia_tiempo_usuario(string fecha,int time)
    {
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();

        WWWForm form = new WWWForm();
        form.AddField("user", fecha);
        form.AddField("pass", time.ToString());
        WWW link = new WWW("http://cucuruchete.comeze.com/register.php", form);

        // this is what you need to add
        yield return link;


        if (!string.IsNullOrEmpty(link.error))
        {
            print(link.error);
        }
        else
        {
            print("Finished Uploading Screenshot");
        }

        /*
        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddField("user", "unity_user");
        form.AddField("pass", "unity_pass");
        form.AddField("direccion", "unity_dir");
        form.AddField("telefono", "unity_dir");
        // Upload to a cgi script
        WWW w = new WWW(screenShotURL, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
        }
        else
        {
            print("Finished Uploading Screenshot");
        }
        */
    }
}
