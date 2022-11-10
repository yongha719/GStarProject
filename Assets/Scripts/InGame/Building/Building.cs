using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Runtime.ConstrainedExecution;

public abstract class Building : MonoBehaviour
{
    public bool Placed { get; private set; }
    public BoundsInt area;

    [Header("Building Info")]
    public string BuildingName;
    public int Rating = 1;


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

    [SerializeField] protected string DefaultConstructionCost;
    public virtual string ConstructionCost { get; }


    [HideInInspector] public BuildingInfo BuildingInfo;


    [Header("Deploying")]
    [Tooltip("배치 가능한 고양이 수")] public int MaxDeployableCat;

    private GameObject BuildingSprte;
    [HideInInspector] public bool FirstTimeInstallation;
    [SerializeField] protected SpriteRenderer SpriteRenderer;

    [Space(5f)]
    [SerializeField] protected GameObject DeployingUIParent;
    [SerializeField] private Button InstallationButton;
    [SerializeField] private Button DemolitionButton;
    [SerializeField] private Button RotateButton;
    #endregion
    public CatPlacement CatPlacement;

    protected RectTransform CanvasRt;
    protected Camera Camera;

    protected GridBuildingSystem GridBuildingSystem;

    protected virtual void Start()
    {
        GridBuildingSystem = GridBuildingSystem.Instance;

        Camera = Camera.main;

        BuildingSprte = SpriteRenderer?.gameObject;

        CanvasRt = GameObject.Find("ParticleCanvas").transform as RectTransform;

        InstallationButton.onClick.AddListener(() =>
        {
            GridBuildingSystem.Place();
        });

        DemolitionButton?.onClick.AddListener(() =>
        {
            GridBuildingSystem.BuildingClear();
            BuildingInfo.BuildingInstalltionUI.SetActive(true);
            Destroy(gameObject);
        });

        RotateButton.onClick.AddListener(() =>
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
        }
    }

    protected virtual IEnumerator BuildingInstalltionEffect()
    {
        BuildingSprte.transform.localScale = new Vector3(0.03f, 0.03f, 1f);
        GridBuildingSystem.BuildingInstallEffectPlay(transform.localPosition);

        yield return BuildingSprte.transform.DOScale(new Vector3(0.1f, 0.1f, 1f), 0.4f).WaitForCompletion();
    }

}
