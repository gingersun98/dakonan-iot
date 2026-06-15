using System;
using UnityEngine;

[Serializable]
public class BoardState
{
    public Vector2Int[] positions;

    public BoardState(int count)
    {
        positions = new Vector2Int[count];
    }

    public BoardState Clone()
    {
        BoardState copy = new BoardState(positions.Length);

        for (int i = 0; i < positions.Length; i++)
            copy.positions[i] = positions[i];

        return copy;
    }

    public string Hash()
    {
        System.Text.StringBuilder sb =
            new System.Text.StringBuilder();

        foreach (var pos in positions)
        {
            sb.Append(pos.x);
            sb.Append(',');
            sb.Append(pos.y);
            sb.Append('|');
        }

        return sb.ToString();
    }
}