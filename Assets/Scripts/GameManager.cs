using EasyTransition;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int ballsPerHole;

    [Header("Board References")]
    public BallContainer playerBase;
    public BallContainer enemyBase;

    public List<BallContainer> playerHoles;
    public List<BallContainer> enemyHoles;

    [Header("Ball Management")]
    public ObjectPooling ballPool;

    private List<GameObject> activeBalls = new();

    [Header("UI")]
    public Toggle turboToggle;

    public GameObject resultPanel;
    public TextMeshProUGUI resultText;

    public Animator turnNotifyAnim;
    public TextMeshProUGUI turnNotifyText;

    [Header("Player & Enemy UI")]
    public Animator playerCornerAnim;
    public Animator enemyCornerAnim;

    public TextMeshPro playerVictoryText;
    public TextMeshPro enemyVictoryText;

    [Header("Animation")]
    public AnimationCurve moveCurve;

    [Header("Inventory")]
    public Transform playerInventory;
    public Transform enemyInventory;

    [Header("Cameras")]
    public GameObject playerVictoryCam;
    public GameObject enemyVictoryCam;

    [Header("Effects")]
    public CinemachineImpulseSource impulseSource;
    public CinemachineImpulseSource explodeSource;

    public ParticleSystem confettiParticle;

    [Header("Managers")]
    public RockPaperScissors rpsManager;

    [Header("Transition")]
    public TransitionSettings transition;

    [Header("Runtime")]
    private bool isPlayerTurn;

    [HideInInspector]
    public Vector3 phoneOffset;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SoundManager.instance.PlayMusic("music");
        StartGame();
    }

    private void Update()
    {
    #if UNITY_ANDROID || UNITY_IOS

        var accel = Accelerometer.current;
        Vector3 value = accel != null ? accel.acceleration.ReadValue() : Vector3.zero;

        phoneOffset = new Vector3(value.x, value.y, 0f);

    #else

        var keyboard = Keyboard.current;

        float x = 0f;
        float y = 0f;

        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed)
                x = -1f;
            else if (keyboard.dKey.isPressed)
                x = 1f;

            if (keyboard.sKey.isPressed)
                y = -1f;
            else if (keyboard.wKey.isPressed)
                y = 1f;
        }

        phoneOffset = new Vector3(x, y, 0f);

#endif

        if (!isPlayerTurn)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TrySelectContainer(Mouse.current.position.ReadValue());
        }

        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                TrySelectContainer(touch.position.ReadValue());
            }
        }
    }

    private void TrySelectContainer(Vector2 screenPos)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector2 worldPoint = cam.ScreenToWorldPoint(screenPos);

        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider == null)
            return;

        BallContainer container =
            hit.collider.GetComponentInParent<BallContainer>();

        if (container == null)
            return;

        container.OnClicked();
    }

    public void StartGame()
    {
        ClearBalls();

        int totalBall = ballsPerHole * 7;

        for (int i = 0; i < totalBall; i++)
        {
            AddBall(playerBase);
            AddBall(enemyBase);
        }

        isPlayerTurn = false;

        resultPanel.SetActive(false);
        if (playerVictoryCam != null)
        playerVictoryCam.SetActive(false);
        if (enemyVictoryCam != null)
        enemyVictoryCam.SetActive(false);

        if (enemyCornerAnim != null)
        enemyCornerAnim.Play("CornerEnemyDisappear", 0);
        if (playerCornerAnim != null)
        playerCornerAnim.Play("CornerProfileDisappear", 0);

        playerVictoryText.gameObject.SetActive(false);
        enemyVictoryText.gameObject.SetActive(false);

        if (rpsManager != null)
        {
            rpsManager.gameObject.SetActive(true);
            rpsManager.Initiate();
        } else
        {
            SuccessRPS(true);
        }
    }

    public void SuccessRPS(bool playerWon)
    {
        isPlayerTurn = playerWon;

        GameObject[] playerBalls = playerBase.TakeAllBalls();
        GameObject[] enemyBalls = enemyBase.TakeAllBalls();

        float delayStep = turboToggle.isOn ? 0.01f : 0.05f;
        float travelTime = turboToggle.isOn ? 0.05f : 0.2f;

        for (int i = 0; i < playerBalls.Length; i++)
        {
            GameObject ball = playerBalls[i];

            BallContainer targetHole =
                playerHoles[i % playerHoles.Count];

            GravityToTarget gravity =
                ball.GetComponent<GravityToTarget>();

            MoveToTarget(
                ball.transform,
                targetHole.transform,
                travelTime,
                i * delayStep,
                () =>
                {
                    gravity.Initialize(targetHole.transform);
                    targetHole.InsertBall(ball);
                    SoundManager.instance.Play("tap");
                });
        }

        for (int i = 0; i < enemyBalls.Length; i++)
        {
            GameObject ball = enemyBalls[i];

            BallContainer targetHole =
                enemyHoles[i % enemyHoles.Count];

            GravityToTarget gravity =
                ball.GetComponent<GravityToTarget>();

            bool isLastBall = i == enemyBalls.Length - 1;

            MoveToTarget(
                ball.transform,
                targetHole.transform,
                travelTime,
                i * delayStep,
                () =>
                {
                    gravity.Initialize(targetHole.transform);
                    targetHole.InsertBall(ball);

                    if (isLastBall)
                    {
                        if (rpsManager != null)
                        rpsManager.gameObject.SetActive(false);

                        if (isPlayerTurn)
                        {
                            SwitchPlayerTurn();
                        }
                        else
                        {
                            SwitchEnemyTurn();
                        }
                    }
                });
        }
    }

    public void AddBall(BallContainer hole)
    {
        GameObject ball = ballPool.GetObject();

        ball.transform.SetParent(hole.transform);
        ball.transform.localPosition = Vector3.zero;

        GravityToTarget gravity = ball.GetComponent<GravityToTarget>();

        if (gravity != null)
        {
            gravity.Initialize(hole.transform);
        }

        hole.InsertBall(ball);

        if (!activeBalls.Contains(ball))
        {
            activeBalls.Add(ball);
        }
    }

    public void ClearBalls()
    {
        foreach (BallContainer hole in playerHoles)
        {
            GameObject[] balls = hole.TakeAllBalls();

            foreach (GameObject ball in balls)
            {
                ballPool.ReturnObject(ball);
                activeBalls.Remove(ball);
            }
        }

        foreach (BallContainer hole in enemyHoles)
        {
            GameObject[] balls = hole.TakeAllBalls();

            foreach (GameObject ball in balls)
            {
                ballPool.ReturnObject(ball);
                activeBalls.Remove(ball);
            }
        }

        foreach (GameObject ball in playerBase.TakeAllBalls())
        {
            ballPool.ReturnObject(ball);
            activeBalls.Remove(ball);
        }

        foreach (GameObject ball in enemyBase.TakeAllBalls())
        {
            ballPool.ReturnObject(ball);
            activeBalls.Remove(ball);
        }

        foreach (GameObject ball in activeBalls)
        {
            if (ball != null && ball.activeInHierarchy)
            {
                ballPool.ReturnObject(ball);
            }
        }

        activeBalls.Clear();
    }

    public IEnumerator StartPlayerTurn(BallContainer containerToTake, bool hasSpunBefore)
    {
        isPlayerTurn = false;
        DeselectAll();

        bool isPlayerSide = containerToTake.isPlayerSide;

        GameObject[] takenBalls = containerToTake.TakeAllBalls();

        activeBalls = new List<GameObject>(takenBalls);

        foreach (GameObject ball in activeBalls)
        {
            MoveToTarget(
                ball.transform,
                playerInventory,
                0.2f,
                0f,
                null
            );

            GravityToTarget g = ball.GetComponent<GravityToTarget>();
            if (g != null)
                g.Initialize(playerInventory);
        }
        SoundManager.instance.Play("pickUp");

        yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 1f);

        bool onPlayerSide = containerToTake.isPlayerSide;
        int holeIndexProgress = onPlayerSide ? playerHoles.IndexOf(containerToTake) + 1 : enemyHoles.IndexOf(containerToTake) + 1;

        bool hasSpunAround = hasSpunBefore;

        BallContainer currentTarget = null;

        int i = 0;
        float pitchProgress = 1f;

        while (i < activeBalls.Count)
        {
            GameObject ball = activeBalls[i];

            BallContainer nextContainer = null;

            if (onPlayerSide)
            {
                if (holeIndexProgress >= playerHoles.Count)
                {
                    nextContainer = playerBase;

                    onPlayerSide = false;
                    holeIndexProgress = 0;
                }
                else
                {
                    nextContainer = playerHoles[holeIndexProgress];
                    holeIndexProgress++;
                }
            }
            else
            {
                if (holeIndexProgress >= enemyHoles.Count)
                {
                    nextContainer = playerHoles[0];
                    onPlayerSide = true;
                    holeIndexProgress = 1;

                    hasSpunAround = true;
                }
                else
                {
                    nextContainer = enemyHoles[holeIndexProgress];
                    holeIndexProgress++;
                }
            }

            currentTarget = nextContainer;

            bool wasEmpty = nextContainer.CheckBallCount() == 0;

            MoveToTarget(
                ball.transform,
                nextContainer.transform,
                0.25f,
                0f,
                null
            );

            GravityToTarget g2 = ball.GetComponent<GravityToTarget>();
            if (g2 != null)
                g2.Initialize(nextContainer.transform);

            nextContainer.InsertBall(ball);

            SoundManager.instance.Play("drop").pitch = pitchProgress;

            pitchProgress += 0.1f;

            i++;

            yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 0.15f);
        }

        yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 1f);

        if (currentTarget == playerBase)
        {
            SwitchPlayerTurn();
            yield break;
        }

        if (!currentTarget.isPlayerSide)
        {
            if (currentTarget.CheckBallCount() <= 1)
            {
                SwitchEnemyTurn();
                yield break;
            } else
            {
                StartCoroutine(StartPlayerTurn(currentTarget, hasSpunBefore));
                yield break;
            }
        }

        if (currentTarget.isPlayerSide)
        {
            bool wasEmpty = currentTarget.CheckBallCount() == 1;

            if (wasEmpty && hasSpunAround)
            {
                int index = playerHoles.IndexOf(currentTarget);
                int mirroredIndex = playerHoles.Count - 1 - index;
                BallContainer opposite = enemyHoles[mirroredIndex];
                if (opposite.CheckBallCount() > 0)
                {
                    GameObject[] capturedOpposite = opposite.TakeAllBalls();
                    GameObject[] capturedSelf = currentTarget.TakeAllBalls();

                    StartCoroutine(CaptureToBase(capturedOpposite, playerBase));
                    StartCoroutine(CaptureToBase(capturedSelf, playerBase));
                }

                SoundManager.instance.Play("capture");
                if (impulseSource != null)
                impulseSource.GenerateImpulse();
                yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 1f);

                SwitchEnemyTurn();
                yield break;
            }

            if (currentTarget.CheckBallCount() > 1)
            {
                StartCoroutine(StartPlayerTurn(currentTarget, hasSpunBefore));
                yield break;
            }

            SwitchEnemyTurn();
            yield break;
        }
    }

    private void DecideEnemyHoles()
    {
        List<BallContainer> validHoles = new List<BallContainer>();

        foreach (BallContainer hole in enemyHoles)
        {
            if (hole.CheckBallCount() > 0)
                validHoles.Add(hole);
        }

        if (validHoles.Count == 0)
            return;

        float bestScore = float.MinValue;
        List<BallContainer> best = new List<BallContainer>();

        foreach (BallContainer hole in validHoles)
        {
            BallAheadResult r = CheckBallAhead(hole, hole.CheckBallCount());

            float score = 0f;

            // ✔ capture = strongest move
            if (r.canCapture)
                score += 200f;

            // ✔ extra turn (base landing)
            if (r.landedOnBase)
                score += 120f;

            // ✔ chain potential
            if (r.causesRelay)
                score += 80f;

            // ❌ dangerous empty landing without capture
            if (r.wasEmptyLanding && !r.canCapture)
                score -= 150f;

            // small preference: more balls = longer influence
            score += hole.CheckBallCount() * 2f;

            if (score > bestScore)
            {
                bestScore = score;
                best.Clear();
                best.Add(hole);
            }
            else if (Mathf.Approximately(score, bestScore))
            {
                best.Add(hole);
            }
        }

        BallContainer chosen =
            best[UnityEngine.Random.Range(0, best.Count)];

        StartCoroutine(StartEnemyTurn(chosen, false));
    }

    public IEnumerator StartEnemyTurn(BallContainer containerToTake, bool hasSpunBefore)
    {
        isPlayerTurn = false;
        DeselectAll();
        yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 0.8f);

        GameObject[] takenBalls = containerToTake.TakeAllBalls();

        activeBalls = new List<GameObject>(takenBalls);

        foreach (GameObject ball in activeBalls)
        {
            MoveToTarget(
                ball.transform,
                enemyInventory,
                0.2f,
                0f,
                null
            );

            GravityToTarget g = ball.GetComponent<GravityToTarget>();
            if (g != null)
                g.Initialize(enemyInventory);
        }
        SoundManager.instance.Play("pickUp");

        yield return new WaitForSeconds(turboToggle.isOn ? 0.5f : 1f);

        bool onEnemySide = !containerToTake.isPlayerSide;
        int holeIndexProgress = onEnemySide ? enemyHoles.IndexOf(containerToTake) + 1 : playerHoles.IndexOf(containerToTake) + 1;

        bool hasSpunAround = hasSpunBefore;

        BallContainer currentTarget = null;

        int i = 0;
        float pitchProgress = 1f;

        while (i < activeBalls.Count)
        {
            GameObject ball = activeBalls[i];

            BallContainer nextContainer = null;

            if (onEnemySide)
            {
                if (holeIndexProgress >= enemyHoles.Count)
                {
                    nextContainer = enemyBase;

                    onEnemySide = false;
                    holeIndexProgress = 0;
                }
                else
                {
                    nextContainer = enemyHoles[holeIndexProgress];
                    holeIndexProgress++;
                }
            }
            else
            {
                if (holeIndexProgress >= playerHoles.Count)
                {
                    nextContainer = enemyHoles[0];
                    onEnemySide = true;
                    holeIndexProgress = 1;

                    hasSpunAround = true;
                }
                else
                {
                    nextContainer = playerHoles[holeIndexProgress];
                    holeIndexProgress++;
                }
            }

            currentTarget = nextContainer;

            bool wasEmptyBefore = nextContainer.CheckBallCount() == 0;

            MoveToTarget(
                ball.transform,
                nextContainer.transform,
                0.25f,
                0f,
                null
            );

            GravityToTarget g2 = ball.GetComponent<GravityToTarget>();
            if (g2 != null)
                g2.Initialize(nextContainer.transform);

            nextContainer.InsertBall(ball);

            SoundManager.instance.Play("drop").pitch = pitchProgress;

            pitchProgress += 0.1f;
            i++;

            yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 0.15f);
        }

        yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 1f);

        if (currentTarget == enemyBase)
        {
            SwitchEnemyTurn();
            yield break;
        }

        if (currentTarget.isPlayerSide == false)
        {
            bool wasEmpty = currentTarget.CheckBallCount() == 1;

            int index = enemyHoles.IndexOf(currentTarget);

            int mirroredIndex = enemyHoles.Count - 1 - index;

            BallContainer opposite = playerHoles[mirroredIndex];

            if (wasEmpty && hasSpunAround && opposite.CheckBallCount() > 0)
            {
                GameObject[] capturedOpposite = opposite.TakeAllBalls();
                GameObject[] capturedSelf = currentTarget.TakeAllBalls();

                StartCoroutine(CaptureToBase(capturedOpposite, enemyBase));
                StartCoroutine(CaptureToBase(capturedSelf, enemyBase));
                SoundManager.instance.Play("capture");
                if (impulseSource != null)
                impulseSource.GenerateImpulse();
                yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 1f);
                SwitchPlayerTurn();
                yield break;
            }

            if (currentTarget.CheckBallCount() > 1)
            {
                StartCoroutine(StartEnemyTurn(currentTarget, hasSpunAround));
                yield break;
            }

            SwitchPlayerTurn();
            yield break;
        }

        else
        {
            bool wasEmpty = currentTarget.CheckBallCount() == 1;

            if (wasEmpty)
            {
                SwitchPlayerTurn();
                yield break;
            }

            if (currentTarget.CheckBallCount() > 1)
            {
                StartCoroutine(StartEnemyTurn(currentTarget, hasSpunAround));
                yield break;
            }

            SwitchPlayerTurn();
            yield break;
        }
    }

    private void SwitchPlayerTurn()
    {
        if (CheckWin())
            return;
        int ballAmount = 0;
        foreach (BallContainer ball in playerHoles)
        {
            ballAmount += ball.CheckBallCount();
        }
        if (ballAmount <= 0)
        {
            SwitchEnemyTurn();
            return;
        }
        isPlayerTurn = true;

        if (playerCornerAnim != null)
            playerCornerAnim.Play("CornerProfileAppear");
        if (enemyCornerAnim != null)
            enemyCornerAnim.Play("CornerEnemyDisappear");

        SoundManager.instance.Play("turnChange");
        StartNotify("Giliran Pemain");
    }

    private void SwitchEnemyTurn()
    {
        if (CheckWin())
            return;
        int ballAmount = 0;
        foreach (BallContainer ball in enemyHoles)
        {
            ballAmount += ball.CheckBallCount();
        }
        if (ballAmount <= 0)
        {
            SwitchPlayerTurn();
            return;
        }
        isPlayerTurn = false;

        if (enemyCornerAnim != null)
            enemyCornerAnim.Play("CornerEnemyAppear");
        if (playerCornerAnim != null)
            playerCornerAnim.Play("CornerProfileDisappear");

        SoundManager.instance.Play("turnChange");
        StartNotify("Giliran Musuh");
        DecideEnemyHoles();
    }

    public void StartNotify(string notify)
    {
        if (turnNotifyText != null)
            turnNotifyText.text = notify;

        if (turnNotifyAnim != null)
        {
            turnNotifyAnim.gameObject.SetActive(true);
            turnNotifyAnim.Play("TurnAppear", 0, 0);
        }
    }

    public bool CheckWin()
    {
        foreach (BallContainer hole in playerHoles)
        {
            if (hole.CheckBallCount() > 0)
                return false;
        }

        foreach (BallContainer hole in enemyHoles)
        {
            if (hole.CheckBallCount() > 0)
                return false;
        }

        int playerScore = playerBase.CheckBallCount();
        int enemyScore = enemyBase.CheckBallCount();

        if (playerScore > enemyScore)
        {
            StartCoroutine(CountWinningBall(true));
        }
        else if (enemyScore > playerScore)
        {
            StartCoroutine(CountWinningBall(false));
        }
        else
        {
            resultPanel.SetActive(true);
            resultText.text = "Seri!";
        }
        return true;
    }

    private IEnumerator CountWinningBall(bool isPlayer)
    {
        BallContainer winnerBase =
            isPlayer ? playerBase : enemyBase;

        TextMeshPro winnerText =
            isPlayer ? playerVictoryText : enemyVictoryText;

        GameObject winnerCam =
            isPlayer ? playerVictoryCam : enemyVictoryCam;

        if (winnerCam != null)
        winnerCam.SetActive(true);
        winnerText.text = "0";
        winnerText.gameObject.SetActive(true);

        float pitchProgress = 1f;
        float originalTime = 0.08f;

        int normalValue = winnerBase.CheckBallCount();

        winnerText.text = "0";

        GameObject[] balls = winnerBase.TakeAllBalls();

        int currentCount = 0;

        foreach (GameObject ball in balls)
        {
            ballPool.ReturnObject(ball);

            currentCount++;

            winnerText.text = currentCount.ToString();

            AudioSource source =
                SoundManager.instance.Play("drop");

            if (source != null)
            {
                source.pitch = pitchProgress;
            }

            pitchProgress += 0.02f;

            yield return new WaitForSeconds(originalTime);

            originalTime *= 0.96f;

            if (originalTime < 0.01f)
            {
                originalTime = 0.01f;
            }
        }

        if (confettiParticle != null)
        {
            confettiParticle.transform.position = winnerBase.transform.position;
            confettiParticle.gameObject.SetActive(true);
            confettiParticle.Play();
        }

        SoundManager.instance.Play("successCounting");

        if (explodeSource != null)
            explodeSource.GenerateImpulse();

        yield return new WaitForSeconds(2f);

        resultPanel.SetActive(true);
        resultText.text =
            isPlayer ? "Pemain menang!" : "Pemain kalah!";
    }

    public BallAheadResult CheckBallAhead(BallContainer starter, int amount)
    {
        BallAheadResult result = new BallAheadResult();

        bool onPlayerSide = starter.isPlayerSide;

        int index = onPlayerSide
            ? playerHoles.IndexOf(starter)
            : enemyHoles.IndexOf(starter);

        BallContainer current = starter;

        for (int i = 0; i < amount; i++)
        {
            index++;

            if (onPlayerSide)
            {
                if (index >= playerHoles.Count)
                {
                    current = playerBase;

                    onPlayerSide = false;
                    index = 0;
                }
                else
                {
                    current = playerHoles[index];
                }
            }
            else
            {
                if (index >= enemyHoles.Count)
                {
                    current = enemyBase;

                    onPlayerSide = true;
                    index = 0;
                }
                else
                {
                    current = enemyHoles[index];
                }
            }
        }


        result.isOnPlayerSide = current.isPlayerSide;

        result.landedOnBase =
            (current == playerBase || current == enemyBase);

        result.wasEmptyLanding = current.CheckBallCount() == 0;

        result.canCapture = false;

        if (!result.landedOnBase && result.wasEmptyLanding)
        {
            if (current.isPlayerSide)
            {
                int idx = playerHoles.IndexOf(current);

                if (idx >= 0)
                {
                    int mirroredIndex = playerHoles.Count - 1 - idx;

                    if (enemyHoles[mirroredIndex].CheckBallCount() > 0)
                    {
                        result.canCapture = true;
                    }
                }
            }
            else
            {
                int idx = enemyHoles.IndexOf(current);

                if (idx >= 0)
                {
                    int mirroredIndex = enemyHoles.Count - 1 - idx;

                    if (playerHoles[mirroredIndex].CheckBallCount() > 0)
                    {
                        result.canCapture = true;
                    }
                }
            }
        }

        result.causesRelay = (!result.landedOnBase && current.CheckBallCount() > 0);


        result.amountOfBall = current.CheckBallCount();

        return result;
    }

    public void DeselectAll()
    {
        foreach (BallContainer hole in playerHoles)
            hole.isSelected = false;

        foreach (BallContainer hole in enemyHoles)
            hole.isSelected = false;
    }

    public void SceneTravel(int index)
    {
        TransitionManager.Instance().Transition(index, transition, 0);
    }

    public void MoveToTarget(Transform movedObject, Transform target, float travelTime, float delay = 0f, Action onComplete = null)
    {
        StartCoroutine(
            MoveCoroutine(
                movedObject,
                target,
                travelTime,
                delay,
                onComplete));
    }

    public void MoveToTarget(Transform movedObject, Vector3 target, float travelTime, float delay = 0f, Action onComplete = null)
    {
        StartCoroutine(
            MoveCoroutine(
                movedObject,
                target,
                travelTime,
                delay,
                onComplete));
    }

    private IEnumerator MoveCoroutine(Transform movedObject, Transform target, float travelTime, float delay, Action onComplete = null)
    {
        if (turboToggle.isOn)
        {
            delay = 0f;
            travelTime = 0.1f;
        }
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Vector3 startPos = movedObject.position;
        Vector3 endPos = target.position;

        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / travelTime);

            if (moveCurve != null)
                t = moveCurve.Evaluate(t);

            movedObject.position =
                Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        movedObject.position = endPos;

        onComplete?.Invoke();
    }

    private IEnumerator MoveCoroutine(Transform movedObject, Vector3 target, float travelTime, float delay, Action onComplete = null)
    {
        if (turboToggle.isOn)
        {
            delay = 0f;
            travelTime = 0.1f;
        }
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Vector3 startPos = movedObject.position;
        Vector3 endPos = target;

        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / travelTime);

            if (moveCurve != null)
                t = moveCurve.Evaluate(t);

            movedObject.position =
                Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        movedObject.position = endPos;

        onComplete?.Invoke();
    }

    public void PlaySFX(string name)
    {
        SoundManager.instance.Play(name);
    }

    private IEnumerator CaptureToBase(GameObject[] balls, BallContainer baseContainer)
    {
        foreach (GameObject b in balls)
        {
            GravityToTarget g = b.GetComponent<GravityToTarget>();

            MoveToTarget(
                b.transform,
                baseContainer.transform,
                0.25f,
                0f,
                null
            );

            if (g != null)
                g.Initialize(baseContainer.transform);

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(turboToggle.isOn ? 0.1f : 0.3f);

        foreach (GameObject b in balls)
        {
            baseContainer.InsertBall(b);
        }
    }
}