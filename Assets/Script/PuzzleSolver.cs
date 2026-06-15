using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolver : MonoBehaviour
{
    public static PuzzleSolver Instance;

    private void Awake()
    {
        Instance = this;
    }

    class State
    {
        public Vector2Int[] positions;
        public int g;
        public int h;

        public int F => g + h;
    }

    class StateComparer : IComparer<State>
    {
        public int Compare(State a, State b)
        {
            int result = a.F.CompareTo(b.F);

            if (result == 0)
                result = a.h.CompareTo(b.h);

            return result;
        }
    }

    struct BlockInfo
    {
        public Vector2Int size;
        public bool canMoveHorizontal;
    }

    BlockInfo[] blocks;
    int boardSize;
    int targetIndex;

    public int Solve(
        List<SlidingBlock> sourceBlocks,
        SlidingBlock target)
    {
        boardSize = GameManager.Instance.cellSize;

        blocks = new BlockInfo[sourceBlocks.Count];

        State start = new State
        {
            positions = new Vector2Int[sourceBlocks.Count],
            g = 0
        };

        for (int i = 0; i < sourceBlocks.Count; i++)
        {
            SlidingBlock b = sourceBlocks[i];

            start.positions[i] = b.GridPosition;

            blocks[i] = new BlockInfo
            {
                size = b.Size,

                // determine orientation
                canMoveHorizontal =
                    b.Size.x > b.Size.y
            };

            if (b == target)
                targetIndex = i;
        }

        start.h = Heuristic(start);

        return AStar(start);
    }

    int AStar(State start)
    {
        SimplePriorityQueue<State> open =
            new SimplePriorityQueue<State>();

        HashSet<ulong> visited =
            new HashSet<ulong>();

        open.Enqueue(start, start.F);

        while (open.Count > 0)
        {
            State current = open.Dequeue();

            ulong hash = Hash(current);

            if (!visited.Add(hash))
                continue;

            if (IsSolved(current))
                return current.g;

            foreach (State next in GetNeighbors(current))
            {
                ulong nextHash = Hash(next);

                if (visited.Contains(nextHash))
                    continue;

                open.Enqueue(next, next.F);
            }
        }

        return -1;
    }

    bool IsSolved(State state)
    {
        Vector2Int pos =
            state.positions[targetIndex];

        Vector2Int size =
            blocks[targetIndex].size;

        return
            pos.x + size.x >= boardSize ||
            pos.y + size.y >= boardSize ||
            pos.x <= 0 ||
            pos.y <= 0;
    }

    int Heuristic(State state)
    {
        Vector2Int targetPos =
            state.positions[targetIndex];

        Vector2Int targetSize =
            blocks[targetIndex].size;

        int rightDistance =
            boardSize -
            (targetPos.x + targetSize.x);

        int leftDistance =
            targetPos.x;

        int topDistance =
            boardSize -
            (targetPos.y + targetSize.y);

        int bottomDistance =
            targetPos.y;

        return Mathf.Min(
            rightDistance,
            leftDistance,
            topDistance,
            bottomDistance);
    }

    IEnumerable<State> GetNeighbors(State current)
    {
        bool[,] grid = BuildGrid(current);

        for (int i = 0; i < blocks.Length; i++)
        {
            BlockInfo block = blocks[i];

            if (block.canMoveHorizontal)
            {
                yield return Move(
                    current,
                    i,
                    Vector2Int.left,
                    grid);

                yield return Move(
                    current,
                    i,
                    Vector2Int.right,
                    grid);
            }
            else
            {
                yield return Move(
                    current,
                    i,
                    Vector2Int.up,
                    grid);

                yield return Move(
                    current,
                    i,
                    Vector2Int.down,
                    grid);
            }
        }
    }

    State Move(
        State current,
        int blockIndex,
        Vector2Int dir,
        bool[,] grid)
    {
        Vector2Int pos =
            current.positions[blockIndex];

        Vector2Int size =
            blocks[blockIndex].size;

        Vector2Int newPos = pos;

        while (true)
        {
            Vector2Int next = newPos + dir;

            if (!CanPlace(
                blockIndex,
                next,
                size,
                current))
                break;

            newPos = next;
        }

        if (newPos == pos)
            return null;

        State nextState = new State
        {
            positions =
                (Vector2Int[])
                current.positions.Clone(),

            g = current.g + 1
        };

        nextState.positions[blockIndex] =
            newPos;

        nextState.h =
            Heuristic(nextState);

        return nextState;
    }

    bool CanPlace(
        int ignoreIndex,
        Vector2Int pos,
        Vector2Int size,
        State state)
    {
        if (pos.x < 0 ||
            pos.y < 0 ||
            pos.x + size.x > boardSize ||
            pos.y + size.y > boardSize)
            return false;

        for (int i = 0; i < state.positions.Length; i++)
        {
            if (i == ignoreIndex)
                continue;

            RectInt a =
                new RectInt(
                    pos,
                    size);

            RectInt b =
                new RectInt(
                    state.positions[i],
                    blocks[i].size);

            if (a.Overlaps(b))
                return false;
        }

        return true;
    }

    bool[,] BuildGrid(State state)
    {
        bool[,] grid =
            new bool[boardSize, boardSize];

        for (int i = 0; i < state.positions.Length; i++)
        {
            Vector2Int pos =
                state.positions[i];

            Vector2Int size =
                blocks[i].size;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    grid[
                        pos.x + x,
                        pos.y + y] = true;
                }
            }
        }

        return grid;
    }

    ulong Hash(State state)
    {
        ulong hash = 0;

        for (int i = 0; i < state.positions.Length; i++)
        {
            hash |=
                ((ulong)state.positions[i].x & 15UL)
                << (i * 8);

            hash |=
                ((ulong)state.positions[i].y & 15UL)
                << (i * 8 + 4);
        }

        return hash;
    }
}

public class SimplePriorityQueue<T>
{
    private List<(T item, int priority)> items = new();

    public int Count => items.Count;

    public void Enqueue(T item, int priority)
    {
        items.Add((item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 1; i < items.Count; i++)
        {
            if (items[i].priority < items[bestIndex].priority)
                bestIndex = i;
        }

        T item = items[bestIndex].item;
        items.RemoveAt(bestIndex);

        return item;
    }
}