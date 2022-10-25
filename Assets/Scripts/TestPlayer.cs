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
        StartCoroutine(RandomMove());
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, dest, _progress * 2f); 
        if (_progress < 1f)
        {
            _progress += Time.deltaTime;
            if (_progress >= 1f)
                _progress = 1f;
        }
    }

    IEnumerator RandomMove()
    {
        while (true)
        {
            targetPos = new Vector2Int(Random.Range(0,5), Random.Range(0,5));
            yield return StartCoroutine(MoveStep());
            yield return new WaitForSeconds(2);
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

        int nodesCnt = nodes.Count;

        while (nodesCnt - 1 != cnt++)
        {
            _progress = 0f;
            node = nodes[cnt];
            dest = new Vector2(node.Pos.x, node.Pos.y);
            yield return new WaitForSeconds(1);
        }
    }

    void OnDrawGizmos()
    {
        if (nodes.Count != 0)
            for (int i = 0; i < nodes.Count - 1; i++)
                Gizmos.DrawLine(nodes[i].Pos, nodes[i + 1].Pos);
    }
}
