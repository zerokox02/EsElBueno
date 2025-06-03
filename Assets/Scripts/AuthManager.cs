using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class AuthManager : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI messageText;

    private string apiKey = "AIzaSyDY4BFoQpPgfY7UYP8wBxC4vdQkwLSyei0";

    public void OnRegisterButton()
    {
        StartCoroutine(RegisterUser(emailInput.text, passwordInput.text));
    }

    public void OnLoginButton()
    {
        StartCoroutine(LoginUser(emailInput.text, passwordInput.text));
    }

    IEnumerator RegisterUser(string email, string password)
    {
        string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={apiKey}";
        yield return SendAuthRequest(email, password, url, "Registro");
    }

    IEnumerator LoginUser(string email, string password)
    {
        string url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}";
        yield return SendAuthRequest(email, password, url, "Login");
    }

    IEnumerator SendAuthRequest(string email, string password, string url, string tipo)
    {
        string jsonData = JsonUtility.ToJson(new FirebaseAuth(email, password));
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            messageText.text = $"{tipo} exitoso.";
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            messageText.text = $"Error en {tipo}: {request.error}";
            Debug.LogError(request.downloadHandler.text);
        }
    }

[System.Serializable]
public class FirebaseErrorResponse
{
    public Error error;

    [System.Serializable]
    public class Error
    {
        public int code;
        public string message;
    }
}

    [System.Serializable]
    public class FirebaseAuth
    {
        public string email;
        public string password;
        public bool returnSecureToken = true;

        public FirebaseAuth(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}
