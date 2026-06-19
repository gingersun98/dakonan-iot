using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallContainer : MonoBehaviour
{
    #region State

    [Header("State")]
    public bool isPlayerSide;
    public bool isSelected;

    #endregion

    #region UI

    [Header("UI")]
    public GameObject playerIndicator;
    public GameObject enemyIndicator;

    public TextMeshPro[] displayText;

    public GameObject selectedIndicator;

    #endregion

    #region Ball Storage

    [Header("Ball Storage")]
    List<GameObject> ballInside = new List<GameObject>();

    #endregion

    #region Animation

    [Header("Animation")]

    float maxJitterAngle = 60f;
    float recoverySpeed = 10f;

    Quaternion[] originalRotations;

    #endregion

    private void Start()
    {
        originalRotations = new Quaternion[displayText.Length];

        for (int i = 0; i < displayText.Length; i++)
        {
            originalRotations[i] = displayText[i].transform.localRotation;
        }
    }

    private void Update()
    {
        selectedIndicator.SetActive(isSelected && isPlayerSide);
    }

    public void TriggerJitter()
    {
        for (int i = 0; i < displayText.Length; i++)
        {
            StartCoroutine(DoJitter(displayText[i].transform, originalRotations[i]));
        }
    }

    private IEnumerator DoJitter(Transform whatToMove, Quaternion originalRotation)
    {
        // 1. Pick a random direction: left (negative) or right (positive)
        // We use a range that excludes 0 so it always snaps noticeably
        float randomSign = Random.value > 0.5f ? 1f : -1f;
        float randomAngle = Random.Range(maxJitterAngle * 0.5f, maxJitterAngle) * randomSign;

        // 2. Snap instantly to the jittered rotation
        // Note: For 2D / Top-down, use Vector3.forward (Z-axis). For 3D characters, use Vector3.up (Y-axis).
        Quaternion jitteredRotation = originalRotation * Quaternion.AngleAxis(randomAngle, Vector3.forward);
        whatToMove.localRotation = jitteredRotation;

        // 3. Smoothly rotate back to the original rotation
        while (Quaternion.Angle(whatToMove.localRotation, originalRotation) > 0.1f)
        {
            whatToMove.localRotation = Quaternion.Slerp(
                whatToMove.localRotation,
                originalRotation,
                Time.deltaTime * recoverySpeed
            );

            yield return null; // Wait for the next frame
        }

        // Snap precisely to the end to prevent micro-movements
        whatToMove.localRotation = originalRotation;
    }

    public void InsertBall(GameObject ball)
    {
        ball.transform.SetParent(transform, true);
        ballInside.Add(ball);
        TriggerJitter();
        UpdateUI();
    }

    public GameObject[] TakeAllBalls()
    {
        GameObject[] newArray = ballInside.ToArray();
        ballInside.Clear();
        UpdateUI();
        return newArray;
    }

    public int CheckBallCount()
    {
        return ballInside.Count;
    }

    public void OnClicked()
    {
        if (!isPlayerSide || CheckBallCount() <= 0)
            return;
        SoundManager.instance.Play("click");
        if (!isSelected)
        {
            GameManager.Instance.DeselectAll();
            isSelected = true;
        } else
        {
            StartCoroutine(GameManager.Instance.StartPlayerTurn(this, false));
        }
    }

    public void UpdateUI()
    {
        if (CheckBallCount() <= 0)
        {
            playerIndicator.gameObject.SetActive(false);
            enemyIndicator.gameObject.SetActive(false);
            return;
        }
        playerIndicator.gameObject.SetActive(isPlayerSide);
        enemyIndicator.gameObject.SetActive(!isPlayerSide);

        foreach(TextMeshPro text in displayText)
        {
            text.text = CheckBallCount().ToString();
        }
    }
}
