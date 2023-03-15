using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Reflection;
using System.Linq;

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
    Stove,                   // 화로질
    Sewing,               // 뜨개질 
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
    public RuntimeAnimatorController CatAnimator;
}

[System.Serializable]
public enum CatState
{
    NotProducting = 0,      // 아무것도 안하는 중
    Idle = 0,               // 걷는 중
    Moving,                 // 이동하는중
    Working,                // 일하는 중
    Resting,                // 휴식하는 중
}


public class Cat : MonoBehaviour
{
    public SpriteRenderer SpriteRenderer;

    public CatData catData = new CatData();
    [Space]
    private CatState catState = CatState.NotProducting;
    public CatState CatState
    {
        get => catState;

        set
        {
            catState = value;

            Animator.SetInteger("State", (int)catState);
        }
    }
    public string BuildingName;

    public Animator Animator;
    public RuntimeAnimatorController AnimatorController => Animator.runtimeAnimatorController;

    #region Astar Move

    public Vector2Int targetPos;
    private Vector3 dest;
    private bool done;

    public int canMoveArea;
    // z fighting 방지를 위해 z 값 조절을 위한 변수
    private float PosZ;

    private List<Node> nodes = new List<Node>();

    public Coroutine RandomMoveCoroutine;
    public Coroutine MoveCoroutine;
    public bool StopMove;

    public bool IsWorking;
    public bool IsResting;
    public bool GoWorking;
    public bool GoResting;

    // 도착했는지 체크
    private bool IsArrived;

    #endregion

    // 골드 생산 횟수
    public int NumberOfGoldProduction;
    // 에너지 생산 횟수
    public int NumberOfEnergyProduction;

    /// <summary>
    /// 능력 등급별 생산 감소 퍼센트
    /// </summary>
    public int PercentageReductionbyRating => catData.AbilityRating switch
    {
        1 => 10,
        2 => 15,
        3 => 20,
        _ => throw new System.Exception("고양이 능력 등급 값이 존재하지 않는 값임")
    };

    public GoldProductionBuilding goldBuilding;
    public int catNum;

    private CatManager CatManager;

    void Start()
    {
        CatManager = CatManager.Instance;

        SpriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();

        SpriteRenderer.sprite = catData.CatSprite;

        canMoveArea = (int)GridBuildingSystem.Instance.ViliageAreaSize.x;

        Animator.runtimeAnimatorController = catData.CatAnimator;
        catData.Cat = this;

        transform.position = transform.position + (Vector3.back * 0.01f * CatManager.CatList.IndexOf(this));
        PosZ = transform.position.z;

        RandomMoveCoroutine = StartCoroutine(RandomMove());
    }

    private void Update()
    {
        // 이동 로직
        if (done)
        {

            SpriteRenderer.flipX = (transform.position.x < dest.x);

            if (transform.position.y < dest.y)
            {
                Animator.SetBool("Isback", true);
                SpriteRenderer.flipX = !SpriteRenderer.flipX;
            }
            else
            {
                Animator.SetBool("Isback", false);
            }

            if (transform.position.y == dest.y)
                transform.DOMove(dest, 1.4f).SetEase(Ease.Linear);
            else
                transform.DOMove(dest, 1f).SetEase(Ease.Linear);

            done = false;
        }
    }

    #region 이동

    public IEnumerator RandomMove()
    {
        while (true)
        {
            targetPos = RandomPos();

            MoveCoroutine = null;
            MoveCoroutine = StartCoroutine(MoveStep());
            yield return MoveCoroutine;
        }
    }

    public IEnumerator Move(Vector3 Pos)
    {
        var pos = new Vector2(Pos.x, Pos.y * 2);
        targetPos = Vector2Int.CeilToInt(Pos);

        MoveCoroutine = null;
        MoveCoroutine = StartCoroutine(MoveStep());
        yield return MoveCoroutine;
    }

    /// <summary>
    /// 영역안에서 움직일 수 있는 랜덤 좌표 
    /// </summary>
    private Vector2Int RandomPos()
    {
        var randomPos = new Vector2Int(Random.Range(-canMoveArea + 1, canMoveArea), Random.Range(-canMoveArea + 1, canMoveArea));

        while (true)
        {
            if (GridBuildingSystem.Instance.WallCheck(randomPos) == false)
            {
                return randomPos;
            }
            else
            {
                randomPos = new Vector2Int(Random.Range(-canMoveArea + 1, canMoveArea), Random.Range(-canMoveArea + 1, canMoveArea));
            }
        }
    }

    private IEnumerator MoveStep()
    {
        nodes = AStar.PathFinding(Vector2Int.CeilToInt(transform.position), targetPos);

        int cnt = 0;

        int nodesCnt = nodes.Count;

        while (cnt++ != nodesCnt - 1)
        {

            done = true;
            Node node = nodes[cnt];
            dest = new Vector3(node.Pos.x, node.Pos.y, PosZ);

            CatState = CatState.Moving;
            if (StopMove)
            {
                break;
            }

            yield return new WaitForSeconds(1);

        }

        StopMove = false;
        CatState = CatState.Idle;
        yield return new WaitForSeconds(2);
    }

    #endregion

    #region 고양이 일

    /// <summary>
    /// 고양이 휴식 
    /// 골드 10번 생산했을시 호출
    /// 에너지 생산 건물로 가서 에너지 생산
    /// </summary>
    public void GoToRest(Vector3 buildingPos)
    {
        StopCoroutine(RandomMoveCoroutine);
        RandomMoveCoroutine = null;

        StartCoroutine(Move(buildingPos));


        GoWorking = false;
        IsWorking = false;
        GoResting = true;
    }

    /// <summary>
    /// 일 끝나고 자유롭게 움직이게하는 함수
    /// </summary>
    public void FinishWork()
    {
        // 얘네는 건물에서 일할때 모션이 없고 들어가서 일하는 컨셉이라
        // 꺼져있기 때문에 다시 켜줘야 함
        switch (goldBuilding.buildingType)
        {
            case GoldBuildingType.GoldMine:
            case GoldBuildingType.PotatoFarming:
            case GoldBuildingType.PowerPlant:
                gameObject.SetActive(true);
                break;
        }

        StopCoroutine(MoveCoroutine);
        MoveCoroutine = null;
        RandomMoveCoroutine = StartCoroutine(RandomMove());

        GoWorking = true;
        IsWorking = false;

        CatState = CatState.Moving;
        Animator.SetInteger("State", (int)catState);
    }

    /// <summary>
    /// 일해라 고양이
    /// 골드 생산
    /// </summary>
    public void GoToWork(Vector3 buildingPos)
    {
        NumberOfEnergyProduction = 0;

        GoWorking = true;
        GoResting = false;
        IsResting = false;

        if (MoveCoroutine != null)
        {
            StopCoroutine(MoveCoroutine);
            MoveCoroutine = null;
        }

        StartCoroutine(Move(buildingPos));

        GoWorking = true;
    }

    #endregion

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out GoldProductionBuilding goldbuilding) && goldBuilding != goldbuilding)
        {
            IsWorking = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsWorking || IsResting)
            return;

        print("박치기");

        if (GoWorking && collision.TryGetComponent(out GoldProductionBuilding goldbuilding))
        {
            transform.DOKill();
            Animator.SetBool("Isback", false);

            SpriteRenderer.flipX = false;

            GoWorking = false;
            IsWorking = true;


            StopCoroutine(MoveCoroutine);

            SetPos();

            done = false;
            Animator.SetInteger("WorkState", (int)goldbuilding.buildingType);

            CatState = CatState.Working;
            return;
        }

        if (GoResting && collision.TryGetComponent(out EnergyProductionBuilding energybuilding))
        {
            transform.DOKill();
            StopMove = true;

            SpriteRenderer.flipX = false;
            var pos = energybuilding.transform.position;

            GoResting = false;
            IsResting = true;
            transform.position = new Vector3(pos.x, pos.y + 0.675f, PosZ);

            energybuilding.ChangeCat(catData);
        }
    }

    void SetPos()
    {
        switch (goldBuilding.buildingType)
        {
            case GoldBuildingType.IceFishing:
                transform.position = goldBuilding.transform.position + new Vector3(0.16f, 1f, 0);
                break;
            case GoldBuildingType.GoldMine:
            case GoldBuildingType.PotatoFarming:
            case GoldBuildingType.PowerPlant:
                gameObject.SetActive(false);
                break;
            case GoldBuildingType.FirewoodChopping:
                transform.position = goldBuilding.transform.position + new Vector3(0.13f, 0.8f, 0);
                break;
            case GoldBuildingType.BlastFurnace:
                transform.position = goldBuilding.transform.position + new Vector3(0.4f, 0.7f, 0);
                break;
            case GoldBuildingType.WinterClothesWorkshop:
                int index = goldBuilding.PlacedInBuildingCats.IndexOf(this);
                if (index == 0)
                {
                    transform.position = transform.position + new Vector3(0.4f, 0.8f);
                    Animator.SetInteger("Clothes", index);
                }
                else
                {
                    transform.position = transform.position + new Vector3(-0.24f, 0.8f);
                    Animator.SetInteger("Clothes", index);
                }
                break;
            case GoldBuildingType.Cauldron:
                transform.position = transform.position + new Vector3(-0.026f, 0.86f);
                break;
            case GoldBuildingType.End:
                break;
        }
    }
    void OnDrawGizmos()
    {
        if (nodes.Count != 0)
            for (int i = 0; i < nodes.Count - 1; i++)
                Gizmos.DrawLine(nodes[i].Pos, nodes[i + 1].Pos);
    }
}
