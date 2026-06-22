using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }

    public FirebaseAuth Auth { get; private set; }

    public bool IsReady { get; private set; }

    private async void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        #if UNITY_ANDROID && !UNITY_EDITOR
        await InitializeFirebase();
        #endif
    }

    private async Task InitializeFirebase()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        var dependencyStatus =
            await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus != DependencyStatus.Available)
        {
            Debug.LogError(
                "Firebase dependencies unavailable: " +
                dependencyStatus);

            return;
        }

        Auth = FirebaseAuth.DefaultInstance;

        try
        {
            GoogleSignIn.Configuration =
                new GoogleSignInConfiguration
                {
                    WebClientId =
                        "210108637147-emqp98knuad467nqri7l9unodg5nbo31.apps.googleusercontent.com",

                    RequestIdToken = true
                };
        }
        catch (System.Exception e)
        {
            // Happens if GoogleSignIn.DefaultInstance
            // was already created before configuration.
            Debug.LogWarning(
                "Google Sign-In already configured: " +
                e.Message);
        }

        IsReady = true;

        Debug.Log("Firebase Manager Ready");
        #endif
    }
}