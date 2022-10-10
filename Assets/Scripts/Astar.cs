using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

    public bool isWall;
    public bool isDiagonal;
    public Node ParentNode;

    public Vector2 Pos
    {
        get
        {
            return new Vector2(x * 0.5f, y * 0.25f);
        }
    }
    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F => G + H;
}


public class AStar : MonoBehaviour
{
    public static Vector2Int bottomLeft, topRight;
    public static List<Node> FinalNodeList;
    public static bool allowDiagonal = true;
    public static bool dontCrossCorner;

    static Node[,] NodeArray;
    static Node StartNode, TargetNode, CurNode;
    static List<Node> OpenList, ClosedList;

    private void Awake()
    {
    }


    public static List<Node> PathFinding(Vector2Int startPos, Vector2Int targetPos)
    {
        startPos *= 2;
        targetPos *= 2;
        print($"sp{startPos} tp{targetPos}");
        print(startPos.x < targetPos.x);
        print(startPos.y < targetPos.y);
        bottomLeft = (startPos.x <= targetPos.x && startPos.y <= targetPos.y) ? startPos : targetPos;
        topRight = bottomLeft == startPos ? targetPos : startPos;
        print($"bl {bottomLeft} tr {topRight}");

        // NodeArray의 크기 정해주고, isWall, x, y 대입
        int sizeX = topRight.x - bottomLeft.x + 1;
        int sizeY = topRight.y - bottomLeft.y + 1;
        NodeArray = new Node[sizeX, sizeY];

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                //foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i + bottomLeft.x, j + bottomLeft.y), 0.4f))
                //    if (col.gameObject.layer == LayerMask.NameToLayer("Wall")) isWall = true;

                NodeArray[i, j] = new Node(isWall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }


        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        StartNode = NodeArray[startPos.x - bottomLeft.x, startPos.y - bottomLeft.y];
        TargetNode = NodeArray[targetPos.x - bottomLeft.x, targetPos.y - bottomLeft.y];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();


        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H)
                    CurNode = OpenList[i];

            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);


            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                CheckList();

                return FinalNodeList;
            }


            // ↗↖↙↘
            if (allowDiagonal)
            {
                OpenListAdd(CurNode.x + 1, CurNode.y + 1, true);
                OpenListAdd(CurNode.x - 1, CurNode.y + 1, true);
                OpenListAdd(CurNode.x - 1, CurNode.y - 1, true);
                OpenListAdd(CurNode.x + 1, CurNode.y - 1, true);
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1, false);
            OpenListAdd(CurNode.x + 1, CurNode.y, false);
            OpenListAdd(CurNode.x, CurNode.y - 1, false);
            OpenListAdd(CurNode.x - 1, CurNode.y, false);
        }
        return null;
    }

    static void CheckList()
    {
        for (int i = 0; i < FinalNodeList.Count - 1; i++)
        {
            if (FinalNodeList[i].isDiagonal == false && FinalNodeList[i + 1].isDiagonal == false)
                if (FinalNodeList[i].x == FinalNodeList[i + 1].x || FinalNodeList[i].y == FinalNodeList[i + 1].y)
                    FinalNodeList.RemoveAt(i);
        }
    }

    static void OpenListAdd(int checkX, int checkY, bool isdiagonal)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.y && checkY < topRight.y + 1
            && !NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y].isWall
            && ClosedList.Contains(NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y]) == false)
        {
            // 대각선 허용시, 벽 사이로 통과 안됨
            if (allowDiagonal)
                if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall
                    && NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall)
                    return;

            // 코너를 가로질러 가지 않을시, 이동 중에 수직수평 장애물이 있으면 안됨
            if (dontCrossCorner)
                if (NodeArray[CurNode.x - bottomLeft.x, checkY - bottomLeft.y].isWall
                    || NodeArray[checkX - bottomLeft.x, CurNode.y - bottomLeft.y].isWall)
                    return;


            // 이웃노드에 넣고, 직선은 56, 대각선은 50비용
            Node NeighborNode = NodeArray[checkX - bottomLeft.x, checkY - bottomLeft.y];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 50 : 56);

            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < NeighborNode.G || OpenList.Contains(NeighborNode) == false)
            {
                NeighborNode.G = MoveCost;
                NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                NeighborNode.ParentNode = CurNode;
                NeighborNode.isDiagonal = isdiagonal;

                OpenList.Add(NeighborNode);
            }
        }
    }
}
