using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    public TutorialState[] tutorialStates;
    public TutorialState endOfTutorial;
    int tutorialIndex;

    public Animator tutorialAnim;
    public TextMeshProUGUI tutorialText;

    public Button skipTutorial;
    public TextMeshProUGUI skipText;

    Button lastButtonForTutorial;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        tutorialIndex = -1;
    }

    public void CheckForTutorialStart(Button firstHighlightButton)
    {
        if (PlayerPrefs.GetInt("FirstTimePlaying", 0) == 0 && MainMenu.Instance.storedBalance < 5)
        {
            tutorialIndex = 0;
            tutorialStates[tutorialIndex].targetHighlight = firstHighlightButton.GetComponent<RectTransform>();
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        if (lastButtonForTutorial != null)
        {
            lastButtonForTutorial.onClick.RemoveListener(StartTutorial);
        }
        if (tutorialIndex >= tutorialStates.Length)
        {
            tutorialAnim.Play("TutorialDisappear", 0, 0f);
            tutorialText.text = endOfTutorial.tutorialText;
            HighlightManager.Instance.EndHighlight();
            tutorialIndex = -1;
            PlayerPrefs.SetInt("FirstTimePlaying", 1);
            return;
        }
        tutorialAnim.gameObject.SetActive(true);
        tutorialAnim.Play("TutorialAppear", 0, 0f);

        TutorialState state = tutorialStates[tutorialIndex];
        tutorialText.text = state.tutorialText;
        HighlightManager.Instance.StartHighlighting(state.targetHighlight, Vector2.zero, state.shape, state.followShapeWidth, state.followShapeHeight);

        if (state.enableSkipTutorial)
        {
            skipText.text = state.skipTutorialText;
            skipTutorial.gameObject.SetActive(true);
        } else
        {
            skipTutorial.gameObject.SetActive(false);
        }

        lastButtonForTutorial = state.targetHighlight.GetComponent<Button>();
        if (lastButtonForTutorial != null && lastButtonForTutorial != skipTutorial)
        {
            lastButtonForTutorial.onClick.AddListener(StartTutorial);
        }
        if (lastButtonForTutorial == skipTutorial)
        {
            lastButtonForTutorial = null;
        }

        tutorialIndex++;
    }

    public void SkipTutorial()
    {
        TutorialState current = GetActiveState();
        foreach (GameObject obj in current.enabledOnSkip)
        {
            Animator isThereAnim = obj.GetComponent<Animator>();
            if (isThereAnim != null)
            {
                isThereAnim.Play("PanelAppear", 0, 0f);
            }
            if (obj == MainMenu.Instance.modernScalePanel.gameObject)
            {
                MainMenu.Instance.modernScalePanel.FakeInitialize();
            }
            obj.SetActive(true);
        }
        foreach (GameObject obj in current.disabledOnSkip)
        {
            Animator isThereAnim = obj.GetComponent<Animator>();
            if (isThereAnim != null)
            {
                isThereAnim.Play("PanelDisappear", 0 ,0f);
            } else
            {
                obj.SetActive(false);
            }
        }
        StartTutorial();
    }

    public TutorialState GetActiveState()
    {
        return tutorialStates[tutorialIndex - 1];
    }

    public bool IsTutorialActive()
    {
        return tutorialIndex >= 0;
    }
}

[System.Serializable]
public class TutorialState
{
    public RectTransform targetHighlight;

    [TextArea(3, 10)]
    public string tutorialText;

    public HighlightManager.HighlightShape shape;

    public bool followShapeWidth;

    public bool followShapeHeight;

    public bool progressAfterScan;

    [Header("Skip Tutorial")]
    public bool enableSkipTutorial;
    [TextArea(3, 10)]
    public string skipTutorialText;
    public GameObject[] enabledOnSkip;
    public GameObject[] disabledOnSkip;
}
