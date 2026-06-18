using UnityEngine;

[ExecuteAlways]
public class SpriteGridLayout : MonoBehaviour
{
    public Vector2 cellSize = new Vector2(1f, 1f);
    public Vector2 spacing = new Vector2(0.1f, 0.1f);

    public int columns = 4;

    public bool centerGrid = false;

    [ContextMenu("Refresh Grid")]
    public void RefreshGrid()
    {
        int childCount = transform.childCount;

        int rows = Mathf.CeilToInt((float)childCount / columns);

        Vector2 offset = Vector2.zero;

        if (centerGrid)
        {
            float width = (columns - 1) * (cellSize.x + spacing.x);
            float height = (rows - 1) * (cellSize.y + spacing.y);

            offset = new Vector2(-width / 2f, height / 2f);
        }

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            int row = i / columns;
            int col = i % columns;

            Vector3 pos = new Vector3(
                col * (cellSize.x + spacing.x),
                -row * (cellSize.y + spacing.y),
                0f
            );

            child.localPosition = pos + (Vector3)offset;
        }
    }

    private void OnValidate()
    {
        RefreshGrid();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            RefreshGrid();
#endif
    }
}