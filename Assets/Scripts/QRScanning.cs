using Proyecto26;
using QRCodeShareMain;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class QRScanning : MonoBehaviour
{
    public static QRScanning Instance {  get; private set; }
    public GameObject askForCameraButton;
    public WebCamTexture camTexture;
    public RawImage targetImage;
    private float scanCooldown = 0f;
    private Texture2D scanTexture;
    Coroutine checkPermissionRoutine = null;
    bool canScan;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        if (camTexture == null)
        {
            Initialize();
        } else
        {
            camTexture.Play();
            StartCoroutine(WaitForFreshFrames());
            StartCoroutine(FixCameraOrientation());
        }
    }

    private void OnDisable()
    {
        if (camTexture != null)
            camTexture.Stop();
        if (checkPermissionRoutine != null) StopCoroutine(checkPermissionRoutine);
        StopAllCoroutines();
    }

    public void Initialize()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            if (checkPermissionRoutine != null) StopCoroutine(checkPermissionRoutine);
            checkPermissionRoutine = StartCoroutine(StartCamera());
            return;
        }

        if (camTexture != null)
        {
            camTexture.Play();
            StartCoroutine(WaitForFreshFrames());
            StartCoroutine(FixCameraOrientation());
            return;
        }

        WebCamDevice[] devices =
            WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.LogError("No camera found.");
            return;
        }

#if UNITY_ANDROID || UNITY_IOS

        bool foundRearCamera = false;

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                camTexture =
                    new WebCamTexture(
                        devices[i].name);

                foundRearCamera = true;
                break;
            }
        }

        if (!foundRearCamera)
        {
            camTexture =
                new WebCamTexture(
                    devices[0].name);
        }

#else

camTexture =
    new WebCamTexture(
        devices[0].name);

#endif

        targetImage.texture = camTexture;
        targetImage.material.mainTexture = camTexture;

        camTexture.Play();
        StartCoroutine(WaitForFreshFrames());
        StartCoroutine(FixCameraOrientation());
    }

    IEnumerator WaitForFreshFrames()
    {
        canScan = false;

        yield return new WaitForSeconds(1f);

        canScan = true;
    }

    IEnumerator StartCamera()
    {
        while (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            yield return null;
        }

        WebCamDevice[] devices =
            WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.LogError("No camera found.");
            yield break;
        }

    #if UNITY_ANDROID || UNITY_IOS

        bool foundRearCamera = false;

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                camTexture =
                    new WebCamTexture(
                        devices[i].name);

                foundRearCamera = true;
                break;
            }
        }

        if (!foundRearCamera)
        {
            camTexture =
                new WebCamTexture(
                    devices[0].name);
        }

    #else

    camTexture =
        new WebCamTexture(
            devices[0].name);

    #endif

        targetImage.texture = camTexture;
        targetImage.material.mainTexture = camTexture;

        camTexture.Play();
        StartCoroutine(WaitForFreshFrames());
        StartCoroutine(FixCameraOrientation());
        checkPermissionRoutine = null;
    }

    public void AskForCamera()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
    }

    IEnumerator FixCameraOrientation()
    {
        yield return new WaitForSeconds(0.2f);

        if (camTexture == null)
            yield break;
        targetImage.rectTransform.localEulerAngles =
            new Vector3(
                0,
                0,
                -camTexture.videoRotationAngle
            );

        if (camTexture.videoVerticallyMirrored)
        {
            targetImage.rectTransform.localScale =
                new Vector3(1, -1, 1);
        }
    }

    void Update()
    {
        if (!canScan)
            return;
        if (MainMenu.Instance.loadingBlocker.activeSelf || MainMenu.Instance.modernScalePanel.gameObject.activeSelf)
        {
            if (camTexture != null && camTexture.isPlaying)
                camTexture.Pause();
            return;
        } else
        {
            if (camTexture != null && !camTexture.isPlaying)
            {
                camTexture.Play();
                StartCoroutine(WaitForFreshFrames());
                StartCoroutine(FixCameraOrientation());
            }
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            askForCameraButton.SetActive(true);
            return;
        }
        askForCameraButton.SetActive(false);

        if (camTexture == null || !camTexture.isPlaying)
            return;

        scanCooldown -= Time.deltaTime;

        if (scanCooldown > 0)
            return;

        // scan every 0.5 seconds (prevents overload)
        scanCooldown = 0.5f;

        scanTexture = new Texture2D(
            camTexture.width,
            camTexture.height,
            TextureFormat.RGBA32,
            false
        );

        scanTexture.SetPixels32(
            camTexture.GetPixels32()
        );

        scanTexture.Apply();

        string result = QRCodeShare.ReadQRCodeImage(scanTexture);

        if (!string.IsNullOrEmpty(result))
        {
            Debug.Log("QR FOUND: " + result);

            OnQRDetected(result);
        }
    }

    void OnQRDetected(string scaleId)
    {
        camTexture.Stop();
        MainMenu.Instance.CheckScale(scaleId);
    }
}
