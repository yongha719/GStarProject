
public class CatPlacementWarning : Warning
{
    CatPlacement CatPlacement;

    private const string WARNING_TEXT = "을(를)\n배치하시겠습니까?";

    void Start()
    {
        CatPlacement = FindObjectOfType<CatPlacement>();

        YesButton.onClick.AddListener(() =>
        {

        });

        NoButton.onClick.AddListener(() =>
        {
            WarningUISetActive(false);
        });
    }

    public void OnClickYesButton(System.Action call)
    {
        YesButton.onClick.RemoveAllListeners();
        YesButton.onClick.AddListener(() => call());
    }

    public void SetWaringData(string CatName)
    {
        WarningText.text = CatName + WARNING_TEXT;
    }
}

