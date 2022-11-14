using UnityEngine;

[CreateAssetMenu(fileName = "CatInfo", menuName = "CatInfo", order = int.MinValue)]
[System.Serializable]
public class CatInfo : ScriptableObject
{
    public string CatName;
    public Sprite CatSprite;
    public CatSkinType CatType;
}