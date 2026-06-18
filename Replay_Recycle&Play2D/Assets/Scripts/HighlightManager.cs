using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightManager : MonoBehaviour
{
	public enum HighlightShape
	{
		CIRCLE = 0,
		SQUARE = 1
	}

	public RectTransform highlight;
	public Image highlightImage;
	public List<Button> everySingleButton;
	private List<bool> buttonLastState;

	[Header("Shapes")]
	public Sprite circleShape;
	public Sprite squareShape;

	private RectTransform targetPos;
    private Vector2 currentOffset;
    private bool followWidth;
    private bool followHeight;
    private bool isHighlighting;
    private const float SIZE_PADDING = 0f;
    public static HighlightManager Instance { get; private set; }

	private void Awake()
	{
        Instance = this;

        buttonLastState = new List<bool>();

        foreach (Button button in everySingleButton)
        {
            buttonLastState.Add(button.interactable);
        }

        highlight.gameObject.SetActive(false);
    }

    public void StartHighlighting(
        RectTransform target,
        Vector2 offset,
        HighlightShape shape = HighlightShape.SQUARE,
        bool followTargetWidth = false,
        bool followTargetHeight = false)
    {
        targetPos = target;
        currentOffset = offset;

        followWidth = followTargetWidth;
        followHeight = followTargetHeight;

        isHighlighting = true;

        highlight.gameObject.SetActive(true);

        highlightImage.sprite = shape == HighlightShape.CIRCLE
            ? circleShape
            : squareShape;

        CopyRectTransformSettings(target, highlight);
        // Position immediately
        highlight.position = target.position + (Vector3)offset;

        if (followWidth)
        {
            highlight.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                target.rect.width + SIZE_PADDING);
        }

        if (followHeight)
        {
            highlight.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                target.rect.height + SIZE_PADDING);
        }

        // Disable buttons
        foreach (Button button in everySingleButton)
        {
            button.interactable = false;
        }
        Button targetButton = target.GetComponent<Button>();
        if (targetButton != null)
        {
            targetButton.interactable = true;
        }
    }

    private void Update()
    {
        if (!isHighlighting || targetPos == null)
            return;

        highlight.position = targetPos.position + (Vector3)currentOffset;

        if (followWidth)
        {
            highlight.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                targetPos.rect.width + SIZE_PADDING);
        }

        if (followHeight)
        {
            highlight.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                targetPos.rect.height + SIZE_PADDING);
        }
    }

    public void EndHighlight()
    {
        isHighlighting = false;
        targetPos = null;

        highlight.gameObject.SetActive(false);

        for (int i = 0; i < everySingleButton.Count; i++)
        {
            everySingleButton[i].interactable = buttonLastState[i];
        }
    }

    public static void CopyRectTransformSettings(
    RectTransform source,
    RectTransform target)
    {
        target.anchorMin = source.anchorMin;
        target.anchorMax = source.anchorMax;
        target.pivot = source.pivot;
        target.anchoredPosition = source.anchoredPosition;
        target.sizeDelta = source.sizeDelta;
    }
}
