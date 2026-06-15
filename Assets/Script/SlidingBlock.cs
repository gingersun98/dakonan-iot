using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ExitArea;

public class SlidingBlock : MonoBehaviour
{
    public static List<SlidingBlock> AllBlocks = new();

    [Header("Grid")]
    public Vector2Int GridPosition;
    public Vector2Int Size = Vector2Int.one;

    [Header("Board")]
    public float CellSize = 1f;

    [Header("Movement")]
    public float MoveDuration = 0.15f;

    [Header("Block State")]
    public bool CanExit;
    public List<ExitArea> ExitCells;
    private bool hasWon;
    private bool isMoving;

    [Header("Cached")]
    public GameObject targetIndicator;
    [HideInInspector] public Animator anim;
    public SpriteRenderer blockSprite;
    private Vector2Int lastMoveDirection;
    private Vector2 swipeStart;
    private bool dragging;
    private const float SwipeThreshold = 50f;
    [HideInInspector] public Vector2Int StartingGridPosition;

    private void Awake()
    {
        AllBlocks.Add(this);
    }

    private void OnDestroy()
    {
        AllBlocks.Remove(this);
    }

    private void Start()
    {
        if (!IsValidSpawnPosition(GridPosition))
        {
            Debug.LogError($"Invalid spawn position for block {name}: {GridPosition}");
            return;
        }
        anim = blockSprite.GetComponent<Animator>();
        anim.Play("BlockIdle", 0, 0f);
        hasWon = false;
        UpdateWorldPosition();
    }

    private void Update()
    {
        targetIndicator.SetActive(CanExit);

        if (isMoving)
            return;

        if (hasWon && CanExit)
        {
            transform.Translate((Vector2)lastMoveDirection * 5f * Time.deltaTime);
            return;
        }

        if (GameManager.Instance.levelTimer > 0 && Time.timeScale > 0f && !GameManager.Instance.revivePanel.gameObject.activeSelf)
        {
            HandleInput();
        }
    }

    #region Input

    private void HandleInput()
    {
#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit =
                Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null &&
                hit.collider.gameObject == gameObject)
            {
                dragging = true;
                swipeStart = Input.mousePosition;
            }
        }

        if (dragging && Input.GetMouseButtonUp(0))
        {
            dragging = false;

            Vector2 swipe =
                (Vector2)Input.mousePosition - swipeStart;

            ProcessSwipe(swipe);
        }

#else

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            Vector2 worldPos =
                Camera.main.ScreenToWorldPoint(touch.position);

            RaycastHit2D hit =
                Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null &&
                hit.collider.gameObject == gameObject)
            {
                dragging = true;
                swipeStart = touch.position;
            }
        }

        if (dragging &&
            (touch.phase == TouchPhase.Ended ||
             touch.phase == TouchPhase.Canceled))
        {
            dragging = false;

            Vector2 swipe = touch.position - swipeStart;

            ProcessSwipe(swipe);
        }

#endif
    }

    private void ProcessSwipe(Vector2 swipe)
    {
        if (swipe.magnitude < SwipeThreshold)
            return;

        Vector2Int direction;

        if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
        {
            direction = swipe.x > 0
                ? Vector2Int.right
                : Vector2Int.left;
        }
        else
        {
            direction = swipe.y > 0
                ? Vector2Int.up
                : Vector2Int.down;
        }

        Slide(direction);
    }

    #endregion

    #region Movement

    private void Slide(Vector2Int direction)
    {
        lastMoveDirection = direction;

        Vector2Int destination = GridPosition;

        while (CanMoveTo(destination + direction))
        {
            destination += direction;
        }

        StartCoroutine(MoveRoutine(destination));
    }

    private bool CanMoveTo(Vector2Int newPos)
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                Vector2Int cell = newPos + new Vector2Int(x, y);

                // BOARD BOUNDS (BOTTOM-LEFT SYSTEM)
                if (!IsInsideBoard(cell))
                    return false;

                // COLLISION
                foreach (SlidingBlock other in AllBlocks)
                {
                    if (other == this)
                        continue;

                    if (other.OccupiesCell(cell))
                        return false;
                }
            }
        }

        return true;
    }

    public bool OccupiesCell(Vector2Int cell)
    {
        return cell.x >= GridPosition.x &&
               cell.x < GridPosition.x + Size.x &&
               cell.y >= GridPosition.y &&
               cell.y < GridPosition.y + Size.y;
    }

    private IEnumerator MoveRoutine(Vector2Int destination)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = GridToWorld(destination);

        float timer = 0f;

        while (timer < MoveDuration)
        {
            timer += Time.deltaTime;

            float t = timer / MoveDuration;

            transform.position = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        transform.position = endPos;

        GridPosition = destination; // keep AFTER movement

        isMoving = false;

        CheckExit();
    }

    #endregion

    #region Helpers

    public void UpdateWorldPosition()
    {
        transform.position = GridToWorld(GridPosition);
    }

    private Vector3 GridToWorld(Vector2Int pos)
    {
        return new Vector3(
            pos.x * CellSize,
            pos.y * CellSize,
            0f
        );
    }

    private void CheckExit()
    {
        if (!CanExit)
        {
            return;
        }

        bool isOnExit = false;
        bool directionMatches = false;

        // 1. Check if block covers ALL required exit cells
        foreach (ExitArea exit in ExitCells)
        {
            bool covered = false;

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Vector2Int cell =
                        GridPosition + new Vector2Int(x, y);

                    if (cell == exit.cell)
                    {
                        covered = true;
                        break;
                    }
                }

                if (covered)
                    break;
            }

            if (!covered)
                return;
        }

        isOnExit = true;

        // 2. Check direction (IMPORTANT FIX)
        directionMatches = IsCorrectDirectionAnyExit();

        // 3. WIN ONLY if both are true
        if (isOnExit && directionMatches)
        {
            Win();
        }
    }

    private void Win()
    {
        Debug.Log("LEVEL COMPLETE!");
        hasWon = true;
        anim.Play("BlockFadeOut", 0, 0f);
        GameManager.Instance.GiveScore();

        GameManager.Instance.bumpSource.GenerateImpulse();
        GameObject obj = VFXManager.instance.PlayVFX("BlowUp");
        obj.transform.localScale = Vector3.one;
        ParticleSystem particle = obj.GetComponent<ParticleSystem>();
        var main = particle.main;
        var trails = particle.trails;
        main.startColor = blockSprite.color;
        trails.colorOverLifetime = blockSprite.color;

        particle.transform.position = targetIndicator.transform.position;
        Invoke("StartGenerateAgain", 0.8f);
    }

    public void StartGenerateAgain()
    {
        GameManager.Instance.HandleDifficulty();
    }

    private bool IsCorrectDirection(ExitArea.ExitDirection dir)
    {
        switch (dir)
        {
            case ExitArea.ExitDirection.Up:
                return lastMoveDirection == Vector2Int.up;

            case ExitArea.ExitDirection.Down:
                return lastMoveDirection == Vector2Int.down;

            case ExitArea.ExitDirection.Left:
                return lastMoveDirection == Vector2Int.left;

            case ExitArea.ExitDirection.Right:
                return lastMoveDirection == Vector2Int.right;
        }

        return false;
    }

    private bool IsCorrectDirectionAnyExit()
    {
        foreach (ExitArea exit in ExitCells)
        {
            if (IsCorrectDirection(exit.direction))
                return true;
        }

        return false;
    }

    private bool IsInsideBoard(Vector2Int cell)
    {
        bool inside =
            cell.x >= 0 && cell.x < GameManager.Instance.cellSize &&
            cell.y >= 0 && cell.y < GameManager.Instance.cellSize;

        return inside;
    }

    private bool IsValidSpawnPosition(Vector2Int pos)
    {
        return pos.x >= 0 &&
               pos.y >= 0 &&
               pos.x + Size.x <= GameManager.Instance.cellSize &&
               pos.y + Size.y <= GameManager.Instance.cellSize;
    }

    public bool InstantSlide(Vector2Int direction)
    {
        Vector2Int start = GridPosition;
        Vector2Int destination = GridPosition;

        while (CanMoveTo(destination + direction))
        {
            destination += direction;
        }

        GridPosition = destination;
        transform.position = GridToWorld(destination);

        return destination != start;
    }

    #endregion
}

[System.Serializable]
public struct ExitArea
{
    public enum ExitDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    public Vector2Int cell;
    public ExitDirection direction;
}