using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int width;
    public int height;
    public float cellSize = 1f;
    public Color gridColor = Color.black;

    private GameObject gridParent;

    public Vector2 GetCellCenter(int x, int y)
    {
        return new Vector2(x + 0.5f, y + 0.5f) * cellSize;
    }

    public Vector2 GetSnappedPosition(Vector2 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int y = Mathf.RoundToInt(worldPosition.y / cellSize);
        return GetCellCenter(x, y);
    }

    private void Start()
    {
        CreateGridVisual();
    }

    private void CreateGridVisual()
    {
        if (gridParent != null)
        {
            Destroy(gridParent);
        }

        gridParent = new GameObject("GridVisual");
        gridParent.transform.parent = transform;

        for (int x = 0; x <= width; x++)
        {
            DrawLine(new Vector3(x * cellSize, 0, 0), new Vector3(x * cellSize, 0, height * cellSize));
        }

        for (int y = 0; y <= height; y++)
        {
            DrawLine(new Vector3(0, 0, y * cellSize), new Vector3(width * cellSize, 0, y * cellSize));
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObject = new GameObject("GridLine");
        lineObject.transform.parent = gridParent.transform;

        LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = gridColor;
        lineRenderer.endColor = gridColor;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 cellCenter = GetCellCenter(x, y);
                Vector3 gizmoPosition = new Vector3(cellCenter.x, 0, cellCenter.y);
                Gizmos.DrawWireCube(gizmoPosition, new Vector3(cellSize, 0, cellSize));
            }
        }
    }
}
