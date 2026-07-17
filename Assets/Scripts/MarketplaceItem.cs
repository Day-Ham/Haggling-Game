using UnityEngine;

[CreateAssetMenu(fileName = "Marketplace Item", menuName = "Marketplace/Item")]
public class MarketplaceItem : ScriptableObject
{
    public string itemName;

    public Sprite itemSprite;

    public GameObject itemModel;

    public int price;

    public string ownerName;

    public string dateListed;

    public int watchCount;
}