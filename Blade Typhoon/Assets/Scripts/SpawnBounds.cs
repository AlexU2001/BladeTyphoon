using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SpawnBounds : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private float _spawnRadius = 5f;

    private Vector3 selectedPosition = Vector3.zero;
    private void Start()
    {
        SpawnEnemy();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 200, 50), "Spawnn Object"))
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (_enemy == null)
        {
            Debug.LogWarning("Object to instantiate is null");
            return;
        }
        if (AreaOfCircle() > AreaOfPoints() / 2)
        {
            Debug.LogError("Spawn radius is too large");
            return;
        }

        selectedPosition = RandomPosition(_points[0].position, _points[1].position);

        Collider2D[] hits = Physics2D.OverlapCircleAll(selectedPosition, _spawnRadius);
        while (hits != null && HasPlayer(hits))
        {
            Debug.Log("Trying again");
            selectedPosition = RandomPosition(_points[0].position, _points[1].position);
            hits = Physics2D.OverlapCircleAll(selectedPosition, _spawnRadius);
        }

        Instantiate(_enemy, selectedPosition, Quaternion.identity);
    }

    private bool HasPlayer(Collider2D[] hits)
    {
        foreach (var hit in hits)
        {
            if(hit.CompareTag("Player"))
                return true;
        }
        return false;
    }

    private Vector2 RandomPosition(Vector2 pos1, Vector2 pos2)
    {
        return new Vector2(Random.Range(pos1.x, pos2.x), Random.Range(pos1.y, pos2.y));
    }

    private Vector3[] AddCornerPoints(Vector2 pos1, Vector2 pos2)
    {
        Vector3[] points = new Vector3[4];
        points[0] = pos1;
        points[1] = new Vector2(pos2.x, pos1.y);
        points[2] = pos2;
        points[3] = new Vector2(pos1.x, pos2.y);
        return points;
    }

    private float AreaOfPoints()
    {
        float width = Mathf.Abs(_points[0].position.y - _points[1].position.y);
        float length = Mathf.Abs(_points[0].position.x - _points[1].position.x);
        return width * length;
    }

    private float AreaOfCircle()
    {
        return (_spawnRadius * _spawnRadius) * Mathf.PI;
    }

    private void DrawBox()
    {
        if (_points.Length < 2)
            return;

        Vector3[] points = AddCornerPoints(_points[0].position, _points[1].position);
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(points[i % 4], points[(i + 1) % 4]);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(selectedPosition, _spawnRadius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        DrawBox();
    }
}
