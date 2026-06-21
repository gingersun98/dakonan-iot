using EasyTransition;
using Firebase.Auth;
using Google;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    #region UI References

    [Header("Account UI")]
    public GameObject accountDetail;
    public TextMeshProUGUI accountName;
    public TextMeshProUGUI[] everyCurrencyShowcase;

    [Header("Authentication Panels")]
    public Animator registerTab;
    public Animator loginTab;
    public Animator signInPanel;

    [Header("Authentication Inputs")]
    public TMP_InputField registerUsernameField;
    public TMP_InputField registerPasswordField;
    public TMP_InputField loginUsernameField;
    public TMP_InputField loginPasswordField;

    [Header("Password Visibility")]
    public Image registerPasswordShow;
    public Image loginPasswordShow;
    public Sprite showPassword;
    public Sprite hidePassword;

    [Header("Notifications & Popups")]
    public Animator notificationQRAppear;
    public Animator cameraQRMenu;
    public ScalePanel modernScalePanel;

    [Header("Loading & Transitions")]
    public GameObject loadingBlocker;
    public TransitionSettings transitionSettings;

    #endregion

    #region Runtime Data

    [Header("Runtime Data")]
    [HideInInspector] public int storedBalance;

    #endregion

    #region API Configuration

    [Header("Base URL")]
    public string baseLink = "https://scaleweight-to-unity-production.up.railway.app";

    [Header("Authentication Endpoints")]
    public string registerEndpoint = "/register";
    public string loginEndpoint = "/login";
    public string firebaseEndpoint = "/firebase-login";

    [Header("User Endpoints")]
    public string profileEndpoint = "/profile";

    [Header("Scale Endpoints")]
    public string getScaleEndpoint = "/scale/";

    [Header("Payment Endpoints")]
    public string depositEndpoint = "/deposit";
    public string paymentEndpoint = "/payment";

    #endregion

    private void Awake()
    {
        Instance = this;
    }


    private IEnumerator Start()
    {
        SoundManager.instance.PlayMusic("music");
        while (
            FirebaseManager.Instance == null ||
            !FirebaseManager.Instance.IsReady)
        {
            yield return null;
        }

        string token =
            PlayerPrefs.GetString("token", "");

        if (string.IsNullOrEmpty(token))
        {
            signInPanel.gameObject.SetActive(true);
        }
        else
        {
            GetProfile();
        }
    }

    public void PlayGame(int gameMode)
    {
        string token = PlayerPrefs.GetString("token", "");
        loadingBlocker.SetActive(true);
        var request = new RequestHelper
        {
            Uri = baseLink + "/payment",
            Method = "POST",
            Headers = new Dictionary<string, string>
        {
            { "Authorization", "Bearer " + token }
        }
        };

        RestClient.Request<PaymentResponse>(request)
            .Then(response =>
            {
                Debug.Log("Success: " + response.success);
                Debug.Log("Remaining: " + response.remaining);
                UpdateUI(null, response.remaining);
                loadingBlocker.SetActive(false);
                switch (gameMode)
                {
                    case 0: TravelScene(1); break;
                    default: print("Not implemented yet, stay tuned!"); break;
                }
            })
            .Catch(error =>
            {
                loadingBlocker.SetActive(false);
                Debug.LogError(error);
            });
    }

    public void TravelScene(int index)
    {
        TransitionManager.Instance().Transition(index, transitionSettings, 0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Register()
    {
        string username = registerUsernameField.text.Trim();
        string password = registerPasswordField.text.Trim();
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            loadingBlocker.SetActive(true);
            RestClient.Post<LoginResponse>(baseLink + registerEndpoint, new LoginRequest
            {
                username = username,
                password = password
            }).Then(response =>
            {
                loadingBlocker.SetActive(false);
                if (registerTab.gameObject.activeSelf)
                registerTab.Play("PanelDisappear", 0, 0f);
                if (loginTab.gameObject.activeSelf)
                loginTab.Play("PanelDisappear", 0, 0f);
                if (signInPanel.gameObject.activeSelf)
                signInPanel.Play("PanelDisappear", 0, 0f);

                PlayerPrefs.SetString("token", response.token);
                PlayerPrefs.SetInt("HasMadeAccount", 1);
                PlayerPrefs.Save();
                print("Succesful registered an account : " + username);
                accountDetail.SetActive(true);
                accountName.text = username;
                UpdateUI(username, 0);
            }).Catch(error =>
            {
                loadingBlocker.SetActive(false);
                Debug.LogError("Failure in registering a new account : " + error);
            });
        }
    }

    public void Login()
    {
        string username = loginUsernameField.text.Trim();

        string password = loginPasswordField.text.Trim();

        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            loadingBlocker.SetActive(true);

            RestClient.Post<LoginResponse>(baseLink + loginEndpoint,

                new LoginRequest
                {
                    username = username,
                    password = password
                }

            ).Then(response =>
            {
                loadingBlocker.SetActive(false);
                if (registerTab.gameObject.activeSelf)
                    registerTab.Play("PanelDisappear", 0, 0f);
                if (loginTab.gameObject.activeSelf)
                    loginTab.Play("PanelDisappear", 0, 0f);
                if (signInPanel.gameObject.activeSelf)
                    signInPanel.Play("PanelDisappear", 0, 0f);

                Debug.Log("Successfully logged in with Token: " + response.token);
                PlayerPrefs.SetInt("HasMadeAccount", 1);
                PlayerPrefs.SetString("token", response.token);
                PlayerPrefs.Save();
                accountDetail.SetActive(true);
                accountName.text = username;
                GetProfile();

            }).Catch(error =>
            {
                loadingBlocker.SetActive(false);

                Debug.LogError("Failure to log in : " + error);
            });
        }
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteKey("token");
        PlayerPrefs.Save();
        ClearAllInputs();
        signInPanel.gameObject.SetActive(true);
        accountDetail.gameObject.SetActive(false);
    }
    public void ClearAllInputs()
    {
        loginPasswordField.text = "";
        loginUsernameField.text = "";
        registerPasswordField.text = "";
        registerUsernameField.text = "";
        loginPasswordField.contentType = TMP_InputField.ContentType.Password;
        registerPasswordField.contentType = TMP_InputField.ContentType.Password;
        loginPasswordField.ForceLabelUpdate();
        registerPasswordField.ForceLabelUpdate();
    }

    public void GetProfile()
    {
        string token = PlayerPrefs.GetString("token", "");

        if (string.IsNullOrEmpty(token))
        {
            Debug.Log("No token found");
            return;
        }
        loadingBlocker.SetActive(true);

        var request = new RequestHelper
        {
            Uri = baseLink + profileEndpoint,
            Method = "GET",
            Headers = new System.Collections.Generic.Dictionary<string, string>
            {
                { "Authorization", "Bearer " + token }
            }
        };

        RestClient.Get<TokenResponse>(request)
            .Then(response =>
            {
                loadingBlocker.SetActive(false);

                Debug.Log("Username: " + response.username);
                Debug.Log("Credit: " + response.credit);

                accountDetail.SetActive(true);
                accountName.text = response.username;
                UpdateUI(response.username, response.credit);
                storedBalance = response.credit;
            })
            .Catch(error =>
            {
                loadingBlocker.SetActive(false);

                Debug.LogError("Profile error: " + error);

                // IMPORTANT: token invalid or expired
                PlayerPrefs.DeleteKey("token");
                accountDetail.SetActive(false);
                UpdateUI(null, 0);
            });
    }

    public void ShowPassword()
    {
        if (registerTab.gameObject.activeSelf)
        {
            if (registerPasswordField.contentType == TMP_InputField.ContentType.Password)
            {
                registerPasswordField.contentType = TMP_InputField.ContentType.Standard;
                registerPasswordShow.sprite = showPassword;
            }
            else
            {
                registerPasswordField.contentType = TMP_InputField.ContentType.Password;
                registerPasswordShow.sprite = hidePassword;
            }
            registerPasswordField.ForceLabelUpdate();
        }
        else
        {
            if (loginPasswordField.contentType == TMP_InputField.ContentType.Password)
            {
                loginPasswordField.contentType = TMP_InputField.ContentType.Standard;
                loginPasswordShow.sprite = showPassword;
            }
            else
            {
                loginPasswordField.contentType = TMP_InputField.ContentType.Password;
                loginPasswordShow.sprite = hidePassword;
            }
           loginPasswordField.ForceLabelUpdate();
        }
    }

    public void CheckScale(string scaleID)
    {
        loadingBlocker.SetActive(true);
        RestClient.Get<ScaleResponse>(baseLink + getScaleEndpoint + scaleID).Then(response =>
        {
            loadingBlocker.SetActive(false);
            if (response.deposited)
            {
                Debug.LogError("Failure to get scale with ID \"" + scaleID + "\" : This scale is already deposited! Take off the weight first!");
                modernScalePanel.storedScale = "";
                notificationQRAppear.gameObject.SetActive(true);
                notificationQRAppear.Play("TurnAppear", 0, 0f);
                return;
            }
            cameraQRMenu.Play("PanelDisappear", 0, 0f);
            modernScalePanel.gameObject.SetActive(true);
            modernScalePanel.Initialize(scaleID, storedBalance);
        }).Catch(error =>
        {
            loadingBlocker.SetActive(false);
            Debug.LogError("Failure to get scale with ID \"" + scaleID + "\" : " + error);
            modernScalePanel.storedScale = "";
            notificationQRAppear.gameObject.SetActive(true);
            notificationQRAppear.Play("TurnAppear", 0, 0f);
            QRScanning.Instance.Initialize();
        });
    }

    public void DepositScale()
    {
        if (string.IsNullOrEmpty(modernScalePanel.storedScale) && !modernScalePanel.randomizedValue)
            return;
        if (modernScalePanel.randomizedValue)
        {
            StopScaleConnection();
            return;
        }
        loadingBlocker.SetActive(true);
        string token = PlayerPrefs.GetString("token");

        RestClient.Request<DepositResponse>(
            new RequestHelper
            {
                Uri = baseLink + "/deposit",
                Method = "POST",
                Body = new DepositRequest
                {
                    scaleId = modernScalePanel.storedScale
                },
                Headers = new System.Collections.Generic.Dictionary<string, string>
                {
            {
                "Authorization",
                "Bearer " + PlayerPrefs.GetString("token")
            }
                }
            }
        )
        .Then(response =>
        {
            UpdateUI(null, response.totalCredit);
            storedBalance = response.totalCredit;

            StopScaleConnection();
            loadingBlocker.SetActive(false);
        })
        .Catch(error =>
        {
            Debug.LogError(error);
            loadingBlocker.SetActive(false);
        });
    }

    public void StopScaleConnection()
    {
        modernScalePanel.storedScale = "";
        modernScalePanel.selfAnim.Play("PanelDisappear", 0, 0f);
        cameraQRMenu.gameObject.SetActive(true);
    }

    public void SignInGoogle()
    {
        if (!FirebaseManager.Instance.IsReady)
        {
            Debug.LogError("Firebase not ready");
            return;
        }
        if (Application.platform != RuntimePlatform.Android)
        {
            Debug.Log("Google Sign-In only works on Android.");
            if (PlayerPrefs.GetInt("HasMadeAccount", 0) == 0)
            {
                registerTab.gameObject.SetActive(true);
            }
            else
            {
                loginTab.gameObject.SetActive(true);
            }
            signInPanel.Play("PanelDisappear", 0, 0f);
            return;
        }
        Debug.Log("1. SignInGoogle called");
        loadingBlocker.SetActive(true);
        GoogleSignIn.DefaultInstance
            .SignIn()
            .ContinueWith(OnGoogleLogin);
    }

    void OnGoogleLogin(Task<GoogleSignInUser> task)
    {
        Debug.Log("2. OnGoogleLogin reached");

        if (task.IsFaulted)
        {
            Debug.LogError("Google Sign-In FAILED");
            Debug.LogException(task.Exception);
            loadingBlocker.SetActive(false);
            return;
        }

        if (task.IsCanceled)
        {
            Debug.Log("Google Sign-In CANCELLED");
            loadingBlocker.SetActive(false);
            return;
        }

        if (task.Result == null)
        {
            Debug.LogError("GoogleSignInUser is NULL");
            loadingBlocker.SetActive(false);
            return;
        }

        Debug.Log("Google Sign-In SUCCESS");

        string idToken = task.Result.IdToken;

        if (string.IsNullOrEmpty(idToken))
        {
            Debug.LogError("ID TOKEN IS NULL");
            loadingBlocker.SetActive(false);
            return;
        }

        Debug.Log("ID TOKEN OK");

        var credential =
            GoogleAuthProvider.GetCredential(
                idToken,
                null
            );

        FirebaseManager.Instance.Auth
            .SignInWithCredentialAsync(credential)
            .ContinueWith(OnFirebaseAuth);
    }

    void OnFirebaseAuth(Task<FirebaseUser> task)
    {
        Debug.Log("3. OnFirebaseAuth reached");

        if (task.IsFaulted)
        {
            Debug.LogError("Firebase Auth FAILED");
            Debug.LogException(task.Exception);
            loadingBlocker.SetActive(false);
            return;
        }

        if (task.IsCanceled)
        {
            Debug.Log("Firebase Auth CANCELLED");
            loadingBlocker.SetActive(false);
            return;
        }

        FirebaseUser user = task.Result;

        if (user == null)
        {
            Debug.LogError("FirebaseUser NULL");
            loadingBlocker.SetActive(false);
            return;
        }

        Debug.Log("Firebase Auth SUCCESS");
        Debug.Log("UID: " + user.UserId);
        Debug.Log("Email: " + user.Email);

        user.TokenAsync(false)
            .ContinueWith(OnFirebaseToken);
    }

    void OnFirebaseToken(Task<string> task)
    {
        Debug.Log("4. OnFirebaseToken reached");

        if (task.IsFaulted)
        {
            Debug.LogError("Token FAILED");
            Debug.LogException(task.Exception);
            loadingBlocker.SetActive(false);
            return;
        }

        string firebaseToken = task.Result;

        Debug.Log("TOKEN RECEIVED: " + firebaseToken.Substring(0, 20) + "...");

        RestClient.Post<LoginResponse>(
            baseLink + firebaseEndpoint,
            new FirebaseLoginRequest
            {
                firebaseToken = firebaseToken
            })
        .Then(response =>
        {
            Debug.Log("5. BACKEND LOGIN SUCCESS");

            PlayerPrefs.SetString("token", response.token);
            PlayerPrefs.Save();

            signInPanel.Play("PanelDisappear", 0, 0f);

            GetProfile();
            loadingBlocker.SetActive(false);
        })
        .Catch(error =>
        {
            loadingBlocker.SetActive(false);
            Debug.LogError("BACKEND ERROR: " + error);
        });
    }

    public void UpdateUI(string username = null, int credit = 0)
    {
        if (username != null)
        {
            accountName.text = username;
        }
        foreach (TextMeshProUGUI wallet in everyCurrencyShowcase)
        {
            wallet.text = "<sprite index=0> " + credit.ToString("N0");
        }
    }

    public void PlaySFX(string sfx)
    {
        SoundManager.instance.Play(sfx);
    }
}

[System.Serializable]
public class LoginRequest
{
    public string username;
    public string password;
}

[System.Serializable]
public class LoginResponse
{
    public string token;
}

[System.Serializable]
public class TokenResponse
{
    public bool valid;
    public string username;
    public int credit;
}

[System.Serializable]
public class ScaleResponse
{
    public string scale;
    public int weight;
    public bool deposited;
}

[System.Serializable]
public class DepositRequest
{
    public string scaleId;
}

[System.Serializable]
public class DepositResponse
{
    public int earned;
    public int totalCredit;
}

[System.Serializable]
public class FirebaseLoginRequest
{
    public string firebaseToken;
}

[System.Serializable]
public class PaymentResponse
{
    public bool success;

    public int remaining;
}