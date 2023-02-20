
public class CatPlacementWarningUI : WarningUI
{
    CatPlacementUI CatPlacementUI;

    private const string WARNING_TEXT = "을(를)\n배치하시겠습니까?";

    void Start()
    {
        CatPlacementUI = FindObjectOfType<CatPlacementUI>();

        YesButton.onClick.AddListener(() => CloseUIPopup());
        NoButton.onClick.AddListener(() => CloseUIPopup());
    }

    private void OnDisable()
    {
        YesButton.onClick.RemoveAllListeners();
        YesButton.onClick.AddListener(() => CloseUIPopup());
    }

    public void OnClickYesButton(System.Action call)
    {
        YesButton.onClick.AddListener(() => call());
    }

    public void SetWarningData(string CatName)
    {
        WarningText.text = CatName + WARNING_TEXT;
    }
}

