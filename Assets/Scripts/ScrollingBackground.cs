using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    public RawImage target;

    public Vector2 scrollSpeed;

    [Header("Color")]
    public bool addColor;
    public Gradient colors;
    public float colorTimeMultiplier;
    public bool pingPongColors;

    private float colorTimer;

    private void Update()
    {
        if (target == null) return;

        // =========================
        // UV SCROLL (POSITION ONLY)
        // =========================
        Rect rect = target.uvRect;

        rect.position += scrollSpeed * Time.deltaTime;

        target.uvRect = rect;

        // =========================
        // COLOR ANIMATION
        // =========================
        if (addColor && colors != null)
        {
            colorTimer += Time.deltaTime * colorTimeMultiplier;

            float t = pingPongColors
                ? Mathf.PingPong(colorTimer, 1f)
                : Mathf.Repeat(colorTimer, 1f);

            target.color = colors.Evaluate(t);
        }
    }
}