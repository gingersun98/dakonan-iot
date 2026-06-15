using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevivePanel : MonoBehaviour
{
    public static RevivePanel Instance {  get; private set; }
    public int maxRevive = 3;
    int currentRevive = 0;

    public float defaultTimer = 5;
    float timer;
    bool freezeTimer;

    public Slider timerDisplay;
    public TextMeshProUGUI timerDisplayText;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        if (currentRevive >= maxRevive)
        {
            GiveUp();
            return;
        }
        timer = defaultTimer;
        timerDisplay.maxValue = defaultTimer;
        timerDisplay.value = defaultTimer;
        freezeTimer = false;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!freezeTimer)
        {
            timer -= Time.deltaTime;
            timerDisplayText.text = timer.ToString("0.0") + "s";
            timerDisplay.value = timer;
            if (timer <= 0)
            {
                GiveUp();
            }
        }
    }

    public void GiveUp()
    {
        freezeTimer = true;
        GameManager.Instance.losePanel.SetActive(true);
        GameManager.Instance.enabled = false;
        gameObject.SetActive(false);
        AdsManager.Instance.ShowInterstitial();
        PlayerPrefs.SetInt("AdsCooldown", PlayerPrefs.GetInt("AdsCooldown", 0) + 1);
    }

    public void TryToRevive()
    {
        freezeTimer = true;
        AdsManager.Instance.ShowRewarded("Revive");
    }

    public void FailedRevive()
    {
        freezeTimer = false;
    }

    public void Revive()
    {
        freezeTimer = true;
        currentRevive++;
        GameManager.Instance.levelTimer = 66;
        gameObject.SetActive(false);
    }
}
