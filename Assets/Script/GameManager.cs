using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [Header("General")]
    public int cellSize = 5; //5x5
    public int levelDifficulty = 100;
    public BlockData[] blockLists;

    int currentLevel = 0;
    int score;
    float displayedScore;

    [Header("Increasing Difficulty - Timer")]
    public float defaultTimer = 120;
    public float bonusDefaultTimer = 45;
    public float levelTimer; // if 0, GAME IS OVER
    float displayedTimer;
    float bonusScoreTimer; // if 0, player get 0 bonus score

    [Header("Increasing Difficulty - Score")]
    public int baseScorePerWon = 1000;
    public int bonusScore = 500;

    [Header("Increasing Difficulty - Generation Level")]
    public int expandBoardEveryLevel = 10;
    public int blockAmountEveryLevel = 10;
    public int blockAmountIncrease = 3;
    public int maxBlockAmountEveryLevel = 30;
    public int maxBlockAmountIncrease = 2;
    public int increaseLevelDifficultyEveryLevel = 10;
    public int levelDifficultyAmount = 50;

    [Header("Increasing Difficulty - Timer Level")]
    //TIMER RELATED
    public int timerIncreaseEveryLevel = 5;
    public float timerIncreaseAmount = 120;
    [Header("Increasing Difficulty - Bonus Level")]
    //BONUS TIMER RELATED
    public int bonusTimerIncreaseEveryLevel = 10;
    public int bonusScoreIncreaseEveryLevel = 10;
    public int bonusTimerIncreaseAmount = 15;
    public int bonusScoreIncreaseAmount = 500;

    [Header("Combo")]
    public int comboCount = 0;
    public RawImage background;
    public float defaultBackgroundSpeed = 1;
    public float maxBackgroundSpeed = 10;
    public float scoreGainPerCombo = 0.2f;
    public float defaultComboTimer = 20;
    public float comboTimerIncrement = 1;
    public TextMeshProUGUI comboText;
    public Animator comboAnim;
    float comboTimer;
    float scoreGainMultiplier = 1.0f;
    float backgroundSpeed;

    [Header("Technical")]
    public Volume lowTimerEffects;
    public GameObject exitHints;
    List<GameObject> exitHintActive = new();
    public SpriteRenderer boardSpriteRend;
    public CinemachineCamera mainCamera;
    public CinemachineImpulseSource impulseSource;
    public CinemachineImpulseSource bumpSource;
    public Slider bonusScoreSlider;
    public Image bonusScoreImage;
    public Gradient bonusScoreGradient;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public GameObject losePanel;
    public TextMeshProUGUI resultScreenText;
    public Animator pauseScreen;
    public RevivePanel revivePanel;
    private Material materialInstance;
    bool pauseBonusScoreTimer = false;
    float gameTimer = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        materialInstance = timerText.fontMaterial;
        currentLevel = 0;
        score = 0;
        displayedScore = 0;
        displayedTimer = defaultTimer;
        levelTimer = defaultTimer;
        pauseBonusScoreTimer = true;
        scoreGainMultiplier = 1;
        EnsureBlocksFitBoard();
        HandleDifficulty();
    }

    private void Update()
    {
        if (revivePanel.gameObject.activeSelf)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        backgroundSpeed = Mathf.Clamp(defaultBackgroundSpeed * comboCount, 0, maxBackgroundSpeed);

        background.uvRect = new Rect(
        background.uvRect.position + (Vector2.right * (backgroundSpeed * Time.deltaTime)),
        background.uvRect.size
        );

        if (comboCount > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                comboCount = 0;
                scoreGainMultiplier = 1;
                comboText.text = "COMBO<br><color=yellow><size=144.5>0</size><br>x1.0 Bonus";
                comboAnim.Play("ComboDisappearText", 0, 0f);
            }
        }

        levelTimer -= Time.deltaTime;
        gameTimer += Time.deltaTime;
        if (!pauseBonusScoreTimer)
            bonusScoreTimer -= Time.deltaTime;

        displayedScore = Mathf.MoveTowards(
            displayedScore,
            score,
            Mathf.Max(100f, Mathf.Abs(score - displayedScore) * 5f) * Time.deltaTime
        );
        scoreText.text = Mathf.RoundToInt(displayedScore).ToString("N0");

        displayedTimer = Mathf.MoveTowards(
            displayedTimer,
            levelTimer,
            10f * Time.deltaTime
        );
        if (displayedTimer < levelTimer)
        {
            timerText.color = Color.green;
        } else
        {
            if (displayedTimer < 30)
            {
                timerText.color = Color.red;
                lowTimerEffects.weight = Mathf.MoveTowards(lowTimerEffects.weight, 1f, 2 * Time.deltaTime);
                materialInstance.SetFloat(ShaderUtilities.ID_GlowPower, Mathf.MoveTowards(materialInstance.GetFloat(ShaderUtilities.ID_GlowPower), 0.25f, Time.deltaTime));
            } else
            {
                timerText.color = Color.white;
                lowTimerEffects.weight = Mathf.MoveTowards(lowTimerEffects.weight, 0f, 2 * Time.deltaTime);
                materialInstance.SetFloat(ShaderUtilities.ID_GlowPower, Mathf.MoveTowards(materialInstance.GetFloat(ShaderUtilities.ID_GlowPower), 0f, Time.deltaTime));
            }
        }
        TimeSpan time = TimeSpan.FromSeconds(displayedTimer);
        timerText.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00") + "<size=30>." + time.Milliseconds.ToString("000");

        bonusScoreSlider.maxValue = bonusDefaultTimer;
        bonusScoreSlider.value = bonusScoreTimer;
        bonusScoreImage.color = bonusScoreGradient.Evaluate(GetScoreMultiplier(bonusScoreTimer, bonusDefaultTimer));

        if (levelTimer <= 0)
        {
            print("Game over!");
            pauseBonusScoreTimer = true;
            TimeSpan lastTime = TimeSpan.FromSeconds(gameTimer);
            resultScreenText.text = "[ Final Score : " + score.ToString("N0") + " ]<br>[ Time Taken : " + lastTime.Hours.ToString("00") + "h " + lastTime.Minutes.ToString("00") + "m " + lastTime.Seconds.ToString("00") + "s ]<br>[ Level Reached : " + currentLevel.ToString("N0") + " ]";
            timerText.text = "00:00<size=30>.000";
            revivePanel.Initialize();
        }
    }

    IEnumerator ClearAllBlocks()
    {
        impulseSource.GenerateImpulse(new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
        foreach (SlidingBlock block in SlidingBlock.AllBlocks.ToArray())
        {
            if (!block.CanExit)
            {
                //block.anim.Play("BlockFadeOut", 0, 0);
                GameObject obj = VFXManager.instance.PlayVFX("BlowUp");
                obj.transform.localScale = Vector3.one;
                ParticleSystem particle = obj.GetComponent<ParticleSystem>();
                var main = particle.main;
                var trails = particle.trails;
                main.startColor = block.blockSprite.color;
                trails.colorOverLifetime = block.blockSprite.color;

                particle.transform.position = block.targetIndicator.transform.position;
            }
            Destroy(block.gameObject);
        }
        yield return new WaitForEndOfFrame();
        GenerateLevel(false);
    }

    public void GiveScore()
    {
        pauseBonusScoreTimer = true;

        float currentTimer = bonusScoreTimer > 0 ? bonusScoreTimer : 0;
        float multiplier = GetScoreMultiplier(currentTimer, bonusDefaultTimer);
        int finalScore = Mathf.RoundToInt(bonusScore * multiplier);

        score += Mathf.RoundToInt((baseScorePerWon + finalScore) * scoreGainMultiplier);
        GainCombo();
    }

    public void GainCombo()
    {
        comboCount++;
        scoreGainMultiplier = 1 + (comboCount * scoreGainPerCombo);
        comboTimer = defaultComboTimer + (comboTimerIncrement * (comboCount - 1));
        comboText.gameObject.SetActive(true);
        comboText.text = "COMBO<br><color=yellow><size=144.5>" + comboCount + "</size><br>x" + scoreGainMultiplier.ToString("0.0") + " Bonus";
        comboAnim.Play("ComboGainText", 0, 0f);
    }

    public void HandleDifficulty()
    {
        currentLevel += 1;
        if (currentLevel % expandBoardEveryLevel == 0)
        {
            cellSize++;
            boardSpriteRend.size = Vector2.one * cellSize;
            boardSpriteRend.transform.Translate(0.5f, 0.5f, 0);
            mainCamera.Lens.OrthographicSize += 1;
        }
        if (currentLevel % maxBlockAmountEveryLevel == 0)
        {
            for (int i = 0; i < blockAmountIncrease; i++)
            {
                blockLists[UnityEngine.Random.Range(0, blockLists.Length)].maxAllowedAmount++;
            }
        }
        if (currentLevel % blockAmountEveryLevel == 0)
        {
            for (int i = 0; i < blockAmountIncrease; i++)
            {
                BlockData selected = GetWeightedBlock();
                if (selected.allowedAmount < selected.maxAllowedAmount)
                {
                    selected.allowedAmount++;
                }
            }
        }
        if (currentLevel % timerIncreaseEveryLevel == 0)
        {
            levelTimer += timerIncreaseAmount;
        }
        if (currentLevel % bonusTimerIncreaseEveryLevel == 0)
        {
            bonusDefaultTimer += bonusTimerIncreaseAmount;
        }
        if (currentLevel % bonusScoreIncreaseEveryLevel == 0)
        {
            bonusScore += bonusScoreIncreaseAmount;
        }
        if (currentLevel % increaseLevelDifficultyEveryLevel == 0)
        {
            levelDifficulty += levelDifficultyAmount;
        }

        levelText.text = "Level " + currentLevel;
        GenerateLevel(false);
    }

    public void GenerateLevel(bool forceRemoval, int retryCount = 0)
    {
        if (retryCount > 20)
        {
            Debug.LogError(
                "Failed to generate level after 20 attempts."
            );
            return;
        }
        EnsureBlocksFitBoard();

        // Clear up exit hints
        foreach (GameObject obj in exitHintActive)
        {
            Destroy(obj);
        }
        exitHintActive = new();
        // Clear up previous blocks (if exist)
        if (SlidingBlock.AllBlocks.Count > 0)
        {
            print("There's still blocks while trying to run GenerateLevel(), removing them...");
            if (!forceRemoval)
            {
                StartCoroutine(ClearAllBlocks());
                return;
            } else
            {
                foreach (SlidingBlock block in SlidingBlock.AllBlocks.ToArray())
                {
                    Destroy(block.gameObject);
                }
            }
        }

        // Set a target block using a randomized block.
        bool[,] occupied = new bool[cellSize, cellSize];
        // Find the edge of the board. Make sure it fits the block size target. This is used as Exit Cells. Also generate the hints for it
        ExitArea.ExitDirection exitSide = (ExitArea.ExitDirection)UnityEngine.Random.Range(0, 4);
        Vector2Int targetPos = Vector2Int.zero;
        int x = 0;
        int y = 0;
        List<BlockData> generationPool = new();
        foreach (BlockData block in blockLists)
        {
            for (int i = 0; i < block.allowedAmount; i++)
            {
                generationPool.Add(block);
            }
        }

        for (int i = generationPool.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);

            (generationPool[i], generationPool[j]) =
                (generationPool[j], generationPool[i]);
        }
        int targetIndex = UnityEngine.Random.Range(0, generationPool.Count);

        BlockData targetBlock = generationPool[targetIndex];
        generationPool.RemoveAt(targetIndex);
        switch (exitSide)
        {
            case ExitArea.ExitDirection.Up:
                x = UnityEngine.Random.Range(0, cellSize - targetBlock.size.x + 1);
                targetPos = new Vector2Int(x, cellSize - targetBlock.size.y);
                break;
            case ExitArea.ExitDirection.Down:
                x = UnityEngine.Random.Range(0, cellSize - targetBlock.size.x + 1);
                targetPos = new Vector2Int(x, 0);
                break;
            case ExitArea.ExitDirection.Left:
                y = UnityEngine.Random.Range(0, cellSize - targetBlock.size.y + 1);
                targetPos = new Vector2Int(0, y);
                break;
            case ExitArea.ExitDirection.Right:
                y = UnityEngine.Random.Range(0, cellSize - targetBlock.size.y + 1);
                targetPos = new Vector2Int(cellSize - targetBlock.size.x,y);
                break;
        }

        // Place the target block in the Exit Cells.
        GameObject targetObj = Instantiate(targetBlock.prefab);
        SlidingBlock target = targetObj.GetComponent<SlidingBlock>();
        target.GridPosition = targetPos;
        target.StartingGridPosition = targetPos;
        target.Size = targetBlock.size;
        target.blockSprite.color = UnityEngine.Random.ColorHSV(
            0f, 1f,   // Hue
            0.7f, 1f, // Saturation
            0.8f, 1f  // Value (brightness)
        );
        target.UpdateWorldPosition();
        target.CanExit = false;
        target.ExitCells = new();
        switch (exitSide)
        {
            case ExitArea.ExitDirection.Up:
                for (int i = 0; i < target.Size.x; i++)
                {
                    AddExitCell(
                        target,
                        new Vector2Int(targetPos.x + i, cellSize - 1),
                        exitSide);
                }
                break;

            case ExitArea.ExitDirection.Down:
                for (int i = 0; i < target.Size.x; i++)
                {
                    AddExitCell(
                        target,
                        new Vector2Int(targetPos.x + i, 0),
                        exitSide);
                }
                break;

            case ExitArea.ExitDirection.Left:
                for (int i = 0; i < target.Size.y; i++)
                {
                    AddExitCell(
                        target,
                        new Vector2Int(0, targetPos.y + i),
                        exitSide);
                }
                break;

            case ExitArea.ExitDirection.Right:
                for (int i = 0; i < target.Size.y; i++)
                {
                    AddExitCell(
                        target,
                        new Vector2Int(cellSize - 1, targetPos.y + i),
                        exitSide);
                }
                break;
        }
        MoveTargetAwayFromExit(target);
        for (int sizeX = 0; sizeX < target.Size.x; sizeX++)
        {
            for (int sizeY = 0; sizeY < target.Size.y; sizeY++)
            {
                occupied[targetPos.x + sizeX, targetPos.y + sizeY] = true;
            }
        }

        // Place other blocks in random cells. Make sure it fits according to their size, and doesn't overlap other generated blocks.
        foreach (BlockData data in generationPool)
        {
            bool found = false;
            Vector2Int spawnPos = Vector2Int.zero;

            for (int attempt = 0; attempt < 100; attempt++)
            {
                spawnPos = new Vector2Int(UnityEngine.Random.Range(0, cellSize - data.size.x + 1), UnityEngine.Random.Range(0, cellSize - data.size.y + 1));
                bool valid = true;

                for (int validX = 0; validX < data.size.x; validX++)
                {
                    for (int validY = 0; validY < data.size.y; validY++)
                    {
                        if (occupied[spawnPos.x + validX, spawnPos.y + validY])
                        {
                            valid = false;
                            break;
                        }
                    }
                }
                if (valid)
                {
                    found = true;
                    break;
                }
            }
            if (found)
            {
                GameObject obj = Instantiate(data.prefab);

                SlidingBlock block = obj.GetComponent<SlidingBlock>();

                block.GridPosition = spawnPos;
                block.Size = data.size;
                block.CanExit = false;
                block.blockSprite.color = UnityEngine.Random.ColorHSV(
                    0f, 1f,   // Hue
                    0.7f, 1f, // Saturation
                    0.8f, 1f  // Value (brightness)
                );
                block.UpdateWorldPosition();
                for (int checkX = 0; checkX < data.size.x; checkX++)
                    for (int checkY = 0; checkY < data.size.y; checkY++)
                        occupied[spawnPos.x + checkX,spawnPos.y + checkY] = true;
            }
        }

        // Move blocks in specific amount of steps to generate new solvable level.
        Vector2Int[] dirs =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        int successfulMoves = 0;
        int attempts = 0;
        int maxAttempts = levelDifficulty * 50;
        int targetMoveCount = 0;
        SlidingBlock previousBlock = null;
        Vector2Int previousDir = Vector2Int.zero;
        HashSet<string> visitedStates = new();
        visitedStates.Add(GetBoardHash());

        while (successfulMoves < levelDifficulty &&
               attempts < maxAttempts)
        {
            attempts++;

            SlidingBlock block =
                SlidingBlock.AllBlocks[
                    UnityEngine.Random.Range(
                        0,
                        SlidingBlock.AllBlocks.Count)];

            Vector2Int dir = dirs[UnityEngine.Random.Range(0, dirs.Length)];

            if (block == previousBlock && dir == -previousDir)
            {
                continue;
            }

            if (block.InstantSlide(dir))
            {
                string hash = GetBoardHash();

                if (!visitedStates.Add(hash))
                {
                    continue;
                }

                successfulMoves++;

                if (block == target)
                {
                    targetMoveCount++;
                }
            }
            previousBlock = block;
            previousDir = dir;
        }
        /*
        if (Mathf.Abs(target.GridPosition.x - target.StartingGridPosition.x) + Mathf.Abs(target.GridPosition.y -target.StartingGridPosition.y) < 2)
        {
            Debug.Log("Target barely moved. Regenerating...");
            GenerateLevel(true, retryCount + 1);
            return;
        }
        /*
        if (attempts >= maxAttempts)
        {
            print("Generation is too tight! Regenerating level...");
            GenerateLevel(true, retryCount + 1);
            return;
        }
        */
        /*
        int depth = PuzzleSolver.Instance.Solve(SlidingBlock.AllBlocks, target);
        if (depth == -1)
        {
            Debug.Log("This level is unsolvable, retrying... : " + depth);
            GenerateLevel(true, retryCount + 1);
            return;
        }
        if (depth < 3 && retryCount == 19)
        {
            Debug.Log("This level is too easy, retrying... : " + depth);
            GenerateLevel(true, retryCount + 1);
            return;
        } else if (retryCount == 19)
        {
            Debug.Log("Level is too easy, but retry count is nearing its limit. Continuing it anyway...");
        }
        */
        /*
        if (target.GridPosition == target.StartingGridPosition)
        {
            Debug.Log("Target never moved from exit. Regenerating...");
            GenerateLevel(true, retryCount + 1);
            return;
        }
        */

        //Reactivate target block for safety
        target.CanExit = true;
        bonusScoreTimer = bonusDefaultTimer;
        print("Level generated! Set difficulty : " + levelDifficulty + " | Board size : " + cellSize + "x" + cellSize + " | Retry Count : " + retryCount);
        pauseBonusScoreTimer = false;
    }

    private void AddExitCell(SlidingBlock target, Vector2Int cell, ExitArea.ExitDirection direction)
    {
        target.ExitCells.Add(new ExitArea
        {
            cell = cell,
            direction = direction
        });

        if ((direction == ExitArea.ExitDirection.Up && cell.y == cellSize - 1) || (direction == ExitArea.ExitDirection.Down && cell.y == 0) || (direction == ExitArea.ExitDirection.Left && cell.x == 0) || (direction == ExitArea.ExitDirection.Right && cell.x == cellSize - 1))
        {
            GameObject hint = Instantiate(exitHints);
            hint.transform.position = new Vector3(cell.x, cell.y, 0);
            switch (direction)
            {
                case ExitArea.ExitDirection.Up:
                    hint.transform.Translate(0, 1, 0); hint.transform.rotation = Quaternion.Euler(0f, 0f, 90f); break;
                case ExitArea.ExitDirection.Down:
                    hint.transform.Translate(0, -1, 0); hint.transform.rotation = Quaternion.Euler(0f, 0f, -90f); break;
                case ExitArea.ExitDirection.Left:
                    hint.transform.Translate(-1, 0, 0); hint.transform.rotation = Quaternion.Euler(0f, 0f, 180f); break;
                case ExitArea.ExitDirection.Right:
                    hint.transform.Translate(1, 0, 0); hint.transform.rotation = Quaternion.Euler(0f, 0f, 0f); break;
            }
            exitHintActive.Add(hint);
        }
    }

    public float GetScoreMultiplier(float currentTime, float maxTime)
    {
        float percent = currentTime / maxTime;

        if (percent >= 0.8f)
            return 1f;

        return percent / 0.8f;
    }

    public void PauseGame()
    {
        if (pauseScreen.gameObject.activeSelf)
        {
            pauseScreen.Play("PanelDisappear", 0, 0f);
            Time.timeScale = 1f;
        } else
        {
            pauseScreen.gameObject.SetActive(true);
            pauseScreen.Play("PanelAppear", 0, 0f);
            Time.timeScale = 0f;
        }
    }

    private BlockData GetWeightedBlock()
    {
        float totalWeight = 0f;

        foreach (BlockData block in blockLists)
        {
            totalWeight += block.upgradeChance;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        float currentWeight = 0f;

        foreach (BlockData block in blockLists)
        {
            currentWeight += block.upgradeChance;

            if (randomValue <= currentWeight)
            {
                return block;
            }
        }

        return blockLists[0];
    }

    private void EnsureBlocksFitBoard()
    {
        int boardArea = cellSize * cellSize;

        while (true)
        {
            int requiredArea = 0;

            foreach (BlockData block in blockLists)
            {
                requiredArea +=
                    block.size.x *
                    block.size.y *
                    block.allowedAmount;
            }

            if (requiredArea <= boardArea - 4)
                break;

            List<BlockData> removable = new();

            foreach (BlockData block in blockLists)
            {
                if (block.allowedAmount > 0)
                {
                    removable.Add(block);
                }
            }

            if (removable.Count == 0)
            {
                Debug.LogError(
                    "Board is too small to fit any blocks."
                );
                break;
            }

            BlockData selected =
                removable[UnityEngine.Random.Range(
                    0,
                    removable.Count)];

            selected.allowedAmount--;

            Debug.Log(
                $"Reduced {selected.prefab.name} to {selected.allowedAmount}");
        }
    }

    string GetBoardHash()
    {
        System.Text.StringBuilder sb =
            new System.Text.StringBuilder();

        foreach (SlidingBlock block in SlidingBlock.AllBlocks)
        {
            sb.Append(block.GridPosition.x);
            sb.Append(',');
            sb.Append(block.GridPosition.y);
            sb.Append('|');
        }

        return sb.ToString();
    }

    public void MoveTargetAwayFromExit(SlidingBlock target)
    {
        if (target.ExitCells == null || target.ExitCells.Count == 0)
        {
            print("Exit cells is null or empty");
            return;
        }

        Vector2Int dir = Vector2Int.zero;

        switch (target.ExitCells[0].direction)
        {
            case ExitArea.ExitDirection.Up:
                dir = Vector2Int.down;
                break;

            case ExitArea.ExitDirection.Down:
                dir = Vector2Int.up;
                break;

            case ExitArea.ExitDirection.Left:
                dir = Vector2Int.right;
                break;

            case ExitArea.ExitDirection.Right:
                dir = Vector2Int.left;
                break;
        }

        target.InstantSlide(dir);
    }
}

[System.Serializable]
public class BlockData
{
    public GameObject prefab;
    public Vector2Int size;
    public int allowedAmount;
    public int maxAllowedAmount = 10;
    [Range(0f, 1f)]
    public float upgradeChance;
}
