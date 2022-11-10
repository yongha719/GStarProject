using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Animations;
using System.Runtime.InteropServices;

/// <summary>
/// 골드 생산 건물 고양이 능력
/// </summary>

[System.Serializable]
public enum GoldAbilityType
{
    Fishing,                // 낚시
    Mining,                 // 광질
    Axing,                  // 도끼질
    Farming,                // 농사
    Kiln,                   // 가마질
    Knitting,               // 뜨개질 
    Boiling,                // 끓이기
    GeneratorOperating,     // 발전기
    End
}

[System.Serializable]
public enum CatSkinType
{
    MufflerCat,        // 목도리 고양이
    beanieCat,         // 비니 고양이
    MufflerBlackCat,   // 검은 목도리 고양이
    PinkCloakCat,      // 분홍 망토 고양이
    WhiteCat,          // 흰 고양이
    RedScarfCat,       // 빨간 스카프 고양이
    End
}

[System.Serializable]
public class CatData
{
    public Cat Cat;                               // 고양이
    public string Name;                           // 고양이 이름
    public int AbilityRating;                     // 능력 등급
    public GoldAbilityType GoldAbilityType;       // 능력 타입
    public Sprite AbilitySprite;                  // 능력 이미지
    public CatSkinType CatSkinType;               // 스킨 종류
    public Sprite CatSprite;                      // 스킨 이미지
}

[System.Serializable]
public enum CatState
{
    NotProducting,          // 아무것도 안하는 중
    Idle,                   // 걷는 중
    Working,                // 일하는 중
    Resting,                // 휴식하는 중
}


public class Cat : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;

    public CatData catData = new CatData();
    [Space]
    public CatState CatState = CatState.NotProducting;
    public string BuildingName;

    public Animator Animator;

    #region Astar Move

    public Vector2Int targetPos;
    private Vector3 dest;
    private bool done;

    public int canMoveArea;
    // z fighting 방지를 위해 z 값 조절을 위한 변수
    private float PosZ;

    private List<Node> nodes = new List<Node>();

    #endregion
    // 골드 생산 횟수
    public int NumberOfGoldProduction;
    // 에너지 생산 횟수
    public int NumberOfEnergyProduction;

    // 능력 등급별 생산 감소 퍼센트
    public int PercentageReductionbyRating => catData.AbilityRating switch
    {
        1 => 10,
        2 => 15,
        3 => 20,
        _ => throw new System.Exception("고양이 능력 등급 값이 존재하지 않는 값임")
    };

    private CatManager CatManager;
    private GridBuildingSystem GridBuildingSystem;

    void Start()
    {
        CatManager = CatManager.Instance;
        GridBuildingSystem = GridBuildingSystem.Instance;

        canMoveArea = (int)GridBuildingSystem.ViliageAreaSize.x;

        catData.Cat = this;

        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();

        SpriteRenderer.sprite = catData.CatSprite;
        Animator.runtimeAnimatorController = CatManager.Instance.CatAnimators[(int)catData.CatSkinType];

        StartCoroutine(RandomMove());
    }

    private void Update()
    {
        // 이동 로직
        if (done)
        {
            SpriteRenderer.flipX = (transform.position.x < dest.x);

            if (transform.position.y == dest.y)
                transform.DOMove(dest, 1.4f).SetEase(Ease.Linear);
            else
                transform.DOMove(dest, 1f).SetEase(Ease.Linear);
            done = false;
        }
    }

    #region 이동

    IEnumerator RandomMove()
    {
        while (true)
        {
            targetPos = RandomPos();

            yield return StartCoroutine(MoveStep());
        }
    }

    /// <summary>
    /// 영역안에서 움직일 수 있는 랜덤 좌표 
    /// </summary>
    private Vector2Int RandomPos() => new Vector2Int(Random.Range(-canMoveArea + 1, canMoveArea), Random.Range(-canMoveArea + 1, canMoveArea));

    private IEnumerator MoveStep()
    {
        nodes = AStar.PathFinding(Vector2Int.CeilToInt(transform.position), targetPos);
        var cnt = 0;
        Node node = null;

        int nodesCnt = nodes.Count;

        while (nodesCnt - 1 != cnt++)
        {
            done = true;
            node = nodes[cnt];
            dest = new Vector3(node.Pos.x, node.Pos.y, PosZ);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(2);
    }

    #endregion

    #region 고양이 일

    /// <summary>
    /// 고양이 휴식 
    /// 골드 10번 생산했을시 호출
    /// 에너지 생산 건물로 가서 에너지 생산
    /// </summary>
    public void GoToRest()
    {

    }

    /// <summary>
    /// 일해라 고양이
    /// 골드 생산
    /// </summary>
    public void GoToWork()
    {

    }

    #endregion
}
