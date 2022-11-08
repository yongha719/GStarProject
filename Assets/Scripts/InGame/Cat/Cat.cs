using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Experimental.Animations;

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
    public CatState CatState = CatState.NotProducting;
    public string BuildingName;

    public Animator Animator;

    public Vector2Int targetPos;

    private Coroutine _moveCo = null;

    private Vector3 dest;
    public float _progress;
    private bool done;

    private List<Node> nodes = new List<Node>();

    // 골드 생산 횟수
    public int NumberOfGoldProduction;
    // 에너지 생산 횟수
    public int NunberOfEnergyProduction;

    // 등급별 생산 감소 퍼센트
    public int PercentageReductionbyGrade => catData.AbilityRating switch
    {
        1 => 10,
        2 => 15,
        3 => 20,
        _ => throw new System.Exception("Cat Ability Rating that does not exist")
    };


    void Start()
    {
        catData.Cat = this;

        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
         
        SpriteRenderer.sprite = catData.CatSprite;
        Animator.runtimeAnimatorController = CatManager.Instance.CatAnimators[(int)catData.CatSkinType];

        StartCoroutine(RandomMove());
    }

    private void Update()
    {
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

    IEnumerator RandomMove()
    {
        while (true)
        {
            targetPos = new Vector2Int(Random.Range(-3, 4), Random.Range(-3, 4));

            yield return StartCoroutine(MoveStep());
        }
    }
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
            dest = new Vector2(node.Pos.x, node.Pos.y);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(2);
    }

    public void SetData()
    {
        


    }

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
}
