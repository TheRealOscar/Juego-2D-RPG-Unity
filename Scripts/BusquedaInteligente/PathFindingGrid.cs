using UnityEngine;

public class PathFindingGrid : MonoBehaviour
{
    public int width = 32;
    public int height = 18;
    public float squareSize = 0.5f;
    public float offset = 0.5f;
    public LayerMask collisionLayer;

    public Node[][] grid;

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new Node[width][];

        Vector3 startPosition = transform.position - new Vector3(width / 2f, height / 2f, 0) + Vector3.one * offset;

        for (int x = 0; x < width; x++)
        {
            grid[x] = new Node[height];
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPosition = startPosition + new Vector3(x, y, 0f);
                bool isObstacle = Physics2D.OverlapBox(worldPosition, Vector2.one * squareSize, 0f, collisionLayer);

                grid[x][y] = new Node(worldPosition, new Vector2(x, y), isObstacle);
            }
        }

        foreach (ChasingEnemy enemy in GetComponentsInChildren<ChasingEnemy>())
        {
            if (enemy != null)
            {
                enemy.grid = grid;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 startPosition = transform.position - new Vector3(width / 2f, height / 2f, 0) + Vector3.one * offset;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 squarePosition = startPosition + new Vector3(i, j, 0f);

                Collider2D[] colliders = Physics2D.OverlapBoxAll(squarePosition, Vector2.one * squareSize, 0f, collisionLayer);

                Gizmos.color = colliders.Length > 0 ? Color.red : Color.blue;
                Gizmos.DrawCube(squarePosition, new Vector3(squareSize, squareSize, 0));
            }
        }
    }
}