using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public abstract class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    [Header("Building Info")]
    public string BuildingName;
    public int Level = 1;

    #region Deploying


    public bool isDeploying;
    public virtual bool IsDeploying
    {
        get
        {
            return isDeploying;
        }

        set
        {
            isDeploying = value;
        }
    }

    [SerializeField] protected string DefaultPlacingPrice;
    public virtual string PlacingPrice { get; }


    [HideInInspector] public BuildingBuyInfoUI BuildingInfo;


    [Header("Deploying")]
    [Tooltip("배치 가능한 고양이 수")] public int MaxDeployableCat;

    private GameObject BuildingSprteObj;
    [HideInInspector] public bool FirstTimeInstallation = true;
    [SerializeField] protected SpriteRenderer SpriteRenderer;
    [HideInInspector] public Sprite BuildingSprite;

    [Space(5f)]
    [SerializeField] protected GameObject DeployingUIParent;
    [SerializeField] private Button DecideButton;
    [SerializeField] private Button CancelButton;
    [SerializeField] private Button RotateButton;
    #endregion

    public CatPlacementUI CatPlacement;
    [HideInInspector] public BuildingLevelUpUI BuildingLevelUpUI;

    protected RectTransform ParticleCanvasRt;
    protected Camera Camera;

    protected GridBuildingSystem GridBuildingSystem;

    protected virtual void Start()
    {
        GridBuildingSystem = GridBuildingSystem.Instance;

        Camera = Camera.main;

        BuildingSprteObj = SpriteRenderer.gameObject;
        BuildingSprite = SpriteRenderer.sprite;

        ParticleCanvasRt = GameObject.Find("ParticleCanvas").transform as RectTransform;

        DecideButton?.onClick.AddListener(() =>
        {
            GridBuildingSystem.Place();
        });

        CancelButton?.onClick.AddListener(() =>
        {
            GridBuildingSystem.BuildingClear(true);
            Destroy(gameObject);
        });

        RotateButton?.onClick.AddListener(() =>
        {
            SpriteRenderer.flipX = !SpriteRenderer.flipX;
        });
    }

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        return GridBuildingSystem.CanTakeArea(areaTemp);
    }


    public virtual void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;

        GridBuildingSystem.TakeArea(areaTemp);

        IsDeploying = false;

        if (FirstTimeInstallation)
        {
            FirstTimeInstallation = false;
            StartCoroutine(BuildingInstalltionEffect());

            //UIManager.Instance.SetBuildingLevelUp();
        }

        SoundManager.Instance.PlaySoundClip("SFX_Building", SoundType.SFX);
    }

    protected virtual IEnumerator BuildingInstalltionEffect()
    {
        BuildingSprteObj.transform.localScale = new Vector3(0.03f, 0.03f, 1f);
        GridBuildingSystem.BuildingInstallEffectPlay(transform.localPosition);

        yield return BuildingSprteObj.transform.DOScale(new Vector3(0.1f, 0.1f, 1f), 0.4f).WaitForCompletion();
    }

    protected bool IsPointerOverGameObject()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Check mouse
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("touch");

            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        return false;
    }
}
