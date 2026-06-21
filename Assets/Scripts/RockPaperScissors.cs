using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour
{
    public enum HandType
    {
        Batu = 0,
        Gunting = 1,
        Kertas = 2
    }

    public Sprite[] handSprites;

    public Image playerRenderer;
    public Image enemyRenderer;

    public TextMeshProUGUI playerHandDisplay;
    public TextMeshProUGUI enemyHandDisplay;

    public GameObject[] disableOnSubmit;

    public Animator winResultAnim;
    public TextMeshProUGUI winResultText;

    private HandType playerHand;
    private HandType enemyHand;

    private Animator anim;
    Coroutine handMovement;
    bool hasSubmit;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Initiate()
    {
        playerHand = HandType.Batu;
        hasSubmit = false;
        if (handMovement != null) StopCoroutine(handMovement);
        handMovement = StartCoroutine(SpinningEnemyHand());
        UpdateUI();
    }

    public void ChangeHand(bool isNext)
    {
        int value = (int)playerHand;

        if (isNext) value++;
        else value--;

        if (value > 2) value = 0;
        if (value < 0) value = 2;

        playerHand = (HandType)value;
        UpdateUI();
    }

    void UpdateUI()
    {
        playerRenderer.sprite = handSprites[(int)playerHand];
        if (playerHandDisplay != null)
        playerHandDisplay.text = playerHand.ToString().ToUpper();
    }

    public void AcceptAnswer()
    {
        StartCoroutine(RPSProcess());
    }

    private IEnumerator RPSProcess()
    {
        foreach (var obj in disableOnSubmit)
            obj.SetActive(false);

        // random enemy hand animation
        hasSubmit = true;
        yield return new WaitForSeconds(1.4f);
        StopCoroutine(handMovement);

        enemyHand = (HandType)Random.Range(0, 3);

        enemyRenderer.sprite = handSprites[(int)enemyHand];
        if (enemyHandDisplay != null)
        enemyHandDisplay.text = enemyHand.ToString().ToUpper();

        yield return new WaitForSeconds(0.5f);

        bool playerWin =
            (playerHand == HandType.Batu && enemyHand == HandType.Gunting) ||
            (playerHand == HandType.Kertas && enemyHand == HandType.Batu) ||
            (playerHand == HandType.Gunting && enemyHand == HandType.Kertas);

        bool draw = playerHand == enemyHand;

        if (winResultAnim != null)
        {
            winResultAnim.gameObject.SetActive(true);
            winResultAnim.Play("TurnAppear", 0 ,0f);
        }

        if (draw)
        {
            if (winResultText != null)
            winResultText.text = "<size=42><b>Seri!</b></size><br>Ulangi lagi!";
            SoundManager.instance.Play("drawRPS");
            yield return new WaitForSeconds(1f);
            ResetRPS();
            yield break;
        }

        if (winResultText != null)
            winResultText.text = playerWin ? "<size=42><b>Kamu menang!</b></size><br>Kamu mendapat giliran pertama." : "<size=42><b>Kamu kalah!</b></size><br>Musuh mendapat giliran pertama.";
        if (playerWin) SoundManager.instance.Play("winRPS").pitch = 1f; else SoundManager.instance.Play("winRPS").pitch = 0.8f;

        yield return new WaitForSeconds(1.2f);

        GameManager.Instance.SuccessRPS(playerWin);
        if (anim != null)
        {
            anim.Play("PanelDisappear", 0, 0f);
        } else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator SpinningEnemyHand()
    {
        float t = 0;
        int index = 0;

        while (t < 1.5f)
        {
            t += Time.deltaTime;

            index = (index + 1) % handSprites.Length;

            enemyRenderer.sprite = handSprites[index];
            if (enemyHandDisplay != null)
            enemyHandDisplay.text = ((HandType)index).ToString().ToUpper();
            if (hasSubmit) SoundManager.instance.Play("button");

            yield return new WaitForSeconds((hasSubmit) ? 0.08f : 0.5f);
        }
    }

    void ResetRPS()
    {
        foreach (var obj in disableOnSubmit)
            obj.SetActive(true);
        hasSubmit = false;
        Initiate();
    }
}