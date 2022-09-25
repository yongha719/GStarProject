using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestPlayer : MonoBehaviour
{
    public Vector2Int targetPos;

    private Coroutine _moveCo = null;

    private Vector3 dest;
    public float _progress;

    private Tilemap tilemap;
    private List<Node> nodes = new List<Node>();

    private void Start()
    {
        tilemap = GridBuildingSystem.Instance.MainTilemap;
        Move();
    }

    private void Update()
    {
        //dest = tilemap.LocalToCell(dest);
        transform.position = Vector2.Lerp(transform.position, dest, _progress * 2f);
        if (_progress < 1f)
        {
            _progress += Time.deltaTime;
            if (_progress >= 1f)
                _progress = 1f;
        }
    }

    public void Move()
    {
        if (_moveCo != null)
            StopCoroutine(_moveCo);
        _moveCo = StartCoroutine(MoveStep());
    }

    private IEnumerator MoveStep()
    {
        nodes = AStar.PathFinding(Vector2Int.CeilToInt(transform.position), targetPos);
        var cnt = 0;
        Node node = null;

        while (nodes.Count - 1 != cnt++)
        {
            _progress = 0f;
            node = nodes[cnt];
            print(node.Pos);
            dest = node.Pos;
            yield return new WaitForSeconds(2);
        }

    }
    void OnDrawGizmos()
    {
        if (nodes.Count != 0)
            for (int i = 0; i < nodes.Count - 1; i++)
                Gizmos.DrawLine(nodes[i].Pos, nodes[i + 1].Pos);
    }
}
