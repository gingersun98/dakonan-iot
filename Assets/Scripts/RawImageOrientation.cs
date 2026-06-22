using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RawImageOrientation : MonoBehaviour
{
    [Header("Landscape UV Size")]
    [SerializeField] private Vector2 landscapeUV = new Vector2(3, 2);

    [Header("Portrait UV Size")]
    [SerializeField] private Vector2 portraitUV = new Vector2(1, 2);

    private RawImage rawImage;
    private bool lastLandscape;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        UpdateUV();
    }

    private void Update()
    {
        bool isLandscape = Screen.width > Screen.height;

        if (isLandscape != lastLandscape)
        {
            UpdateUV();
        }
    }

    private void UpdateUV()
    {
        bool isLandscape = Screen.width > Screen.height;

        Rect uv = rawImage.uvRect;

        Vector2 size = isLandscape ? landscapeUV : portraitUV;

        uv.width = size.x;
        uv.height = size.y;

        rawImage.uvRect = uv;

        lastLandscape = isLandscape;
    }
}