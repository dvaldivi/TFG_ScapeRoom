using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;

public class Authentication : MonoBehaviour {
    
    public  string Login_url = "http://monitorizer.sytes.net:8000/api-token-auth/"; //"http://localhost:8000/api-token-auth/";
    public  string Register_url = "http://monitorizer.sytes.net:8000/polls/users/register/"; //"http://localhost:8000/polls/users/register/";
    public  string Check_AppCode_url = "http://monitorizer.sytes.net:8000/polls/class/checkAppCode";
    public string nextLevelName;
    private const float timerInterval = 3f;

    public GameObject login_reference;
    public GameObject register_reference;
    public GameObject appCode_reference;
    public GameObject notification_reference;
    
    private bool dismissNotification;
    private bool nextNotification;
    private bool timerOn;
    private float timerTime;
    private List<string> notificationStack;
    private int numNotificationStack;


    
    public void setParameters(string Login_url_new, string Register_url_new, string Check_AppCode_url_new, string      nextLevelName_new) {
        Login_url = Login_url_new;
        Register_url = Register_url_new;
        Check_AppCode_url = Check_AppCode_url_new;
        nextLevelName = nextLevelName_new;

    }
    // Use this for initialization
    void Start () {
        VRSettings.LoadDeviceByName("");
        VRSettings.enabled = false;
        dismissNotification = false;
        nextNotification = false;
        timerOn = false;
        timerTime = 0f;
        numNotificationStack = 0;
        notificationStack = new List<string>();
    }
	
	// Update is called once per frame
	void Update () {

        if (timerOn)
        {
            timerTime -= Time.deltaTime;
            if (timerTime < 0.0f)
            {
                if (numNotificationStack == 1)
                {
                    numNotificationStack--;
                    dismissNotification = true;

                }
                else if (numNotificationStack > 1)
                {
                    numNotificationStack--;
                    nextNotification = true;
                }
            }

        }

        if (dismissNotification)
        {
            
            var component = notification_reference.GetComponent<CanvasGroup>();
            var actualAlpha = component.alpha;
            if (actualAlpha > 0f)
            {
                component.alpha = actualAlpha - 0.05f;

            }
            else
            {
                dismissNotification = false;
            }
        }
        else if (nextNotification)
        {
            
            Text textField = GameObject.Find("Notification_Text").GetComponent<Text>();
            textField.text = notificationStack[0];
            notificationStack.RemoveAt(0);
            nextNotification = false;
            timerTime = timerInterval;
        }

    }


    void login(string username, string password)
    {
        WWWForm login_parameter = new WWWForm();
        login_parameter.AddField("username", username);
        login_parameter.AddField("password", password);
        WWW login_request = new WWW(Login_url, login_parameter);

        StartCoroutine(WaitForLoginRequest(login_request, username));
    }

    void register(string username, string password, string email)
    {
        WWWForm register_parameter = new WWWForm();
        register_parameter.AddField("user.username", username);
        register_parameter.AddField("user.password", password);
        register_parameter.AddField("user.email", email);
        WWW register_request = new WWW(Register_url, register_parameter);

        StartCoroutine(WaitForRegisterRequest(register_request));
    }

    public void check_AppCode(string appCode)
    {
        WWWForm check_appCode_parameter = new WWWForm();
        check_appCode_parameter.AddField("appCode", appCode);
        WWW check_appCode_request = new WWW(Check_AppCode_url, check_appCode_parameter);

        StartCoroutine(WaitForCheckCodeRequest(check_appCode_request, appCode));
    }

     IEnumerator WaitForLoginRequest(WWW request, string username)
    {
        yield return request;

        if(request.error == null)
        {
            
            Token token = Token.CreateFromJSON(request.text);
            if (token.token != null)
            {
                Debug.Log("WWW Ok!: " + request.text);
                //Token token = Token.CreateFromJSON(request.text);
                PlayerPrefs.SetString("UserName", username);
                PlayerPrefs.SetString("UserToken", token.token);

                go_to_AppCode();
            }
            else
            {
                show_notification("no logueado correctamente");
            }
        }
        else
        {
            show_notification("Error: " + request.error);
            Debug.Log("WWW Error: " + request.error);
        }
    }

    IEnumerator WaitForRegisterRequest(WWW request)
    {
        yield return request;

        if (request.error == null)
        {
            ErrorResponse error = ErrorResponse.CreateFromJSON(request.text);
            if (error.status != "400")
            {
                Debug.Log("WWW Ok!: " + request.text);
                RegisterUser user = RegisterUser.CreateFromJSON(request.text);
                PlayerPrefs.SetString("UserName", user.username);
                PlayerPrefs.SetString("UserToken", user.token);

                go_to_AppCode();
            }
            else
            {
                show_notification("Error: " + error.error);
            }
        }
        else
        {
            show_notification("Error: " + request.error);
            Debug.Log("WWW Error: " + request.error);
        }
    }

    IEnumerator WaitForCheckCodeRequest(WWW request, string appCode)
    {
        yield return request;

        if (request.error == null)
        {
            Debug.Log("WWW Ok!: " + request.text);
            if (request.text.Length > 0)
            {
                var error = ErrorResponse.CreateFromJSON(request.text);
                if (error.status !=  "400")
                {
                    AppCode token = AppCode.CreateFromJSON(request.text);
                    PlayerPrefs.SetString("AppCode", appCode);

                    Application.LoadLevel(Application.loadedLevel + 1);
                }
                else
                {
                    show_notification("Error: " + error.error);
                }
            }
        }
        else
        {
            show_notification("Error: " + request.error);
            Debug.Log("WWW Error: " + request.error);
        }
    }
    public void continue_button()
    {
        /*GameObject.Find("Main_Menu").SetActive(false);**/
        login_reference.SetActive(true);
        register_reference.SetActive(false);
        appCode_reference.SetActive(false);
    }
    public void login_on_click()
    {
        InputField user_input = GameObject.Find("Login_Username_Input").GetComponent<InputField>();
        string user_text = user_input.text;

        InputField pass_input = GameObject.Find("Login_Password_Input").GetComponent<InputField>();
        string pass_text = pass_input.text;

        if(user_text.Length > 0 && pass_text.Length > 0)
            login(user_text, pass_text);
    }

    public void register_on_click()
    {
        InputField user_input = GameObject.Find("Register_Username_Input").GetComponent<InputField>();
        string user_text = user_input.text;

        InputField pass_input = GameObject.Find("Register_Password_Input").GetComponent<InputField>();
        string pass_text = pass_input.text;

        InputField repepat_pass_input = GameObject.Find("Register_Repeat_Password_Input").GetComponent<InputField>();
        string repeat_pass_text = repepat_pass_input.text;

        InputField email_input = GameObject.Find("Register_Email_Input").GetComponent<InputField>();
        string email_text = email_input.text;

        if(pass_text.Equals(repeat_pass_text) && email_text.Length > 0 && email_text.IndexOf("@") > 0 && email_text.IndexOf("@") < email_text.IndexOf(".") && user_text.Length > 0)
            register(user_text, pass_text, email_text);
    }

    public void appCode_onClick()
    {
        InputField appCode_input = GameObject.Find("AppCode_Input").GetComponent<InputField>();
        string appCode_text = appCode_input.text;

        check_AppCode(appCode_text);
    }

    public void no_appCode_on_click()
    {
        PlayerPrefs.SetString("AppCode", null);

        if (nextLevelName.Length > 0)
            Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void go_to_register()
    {
        login_reference.SetActive(false);
        register_reference.SetActive(true);
    }

    public void go_to_login()
    {
        login_reference.SetActive(true);
        register_reference.SetActive(false);
    }

    public void go_to_AppCode()
    {
        login_reference.SetActive(false);
        register_reference.SetActive(false);
        appCode_reference.SetActive(true);
    }

    public void show_notification(string text)
    {
        numNotificationStack++;
        notification_reference.SetActive(true);
        notification_reference.GetComponent<CanvasGroup>().alpha  = 1f;
        dismissNotification = false;
        nextNotification = false;


        if (numNotificationStack == 1)
        {
            Text textField = GameObject.Find("Notification_Text").GetComponent<Text>();
            textField.text = text;

            timerOn = true;
            timerTime = timerInterval;
        }
        else if (numNotificationStack > 1)
        {
            notificationStack.Add(text);
        }
    }



    private class Token
    {
        public string token;

        public static Token CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Token>(jsonString);
        }
    }

    private class RegisterUser
    {
        public string username;
        public string token;
        public string email;

        public static RegisterUser CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<RegisterUser>(jsonString);
        }
    }

    private class AppCode{
        public string classId;
        public string status;

        public static AppCode CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<AppCode>(jsonString);
        }
    }

    private class ErrorResponse
    {
        public string error;
        public string status;

        public static ErrorResponse CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ErrorResponse>(jsonString);
        }
    }
}