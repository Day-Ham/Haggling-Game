using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MarketplaceManager : MonoBehaviour
{
    [Header("Items")]
    public MarketplaceItem[] items;
    private HashSet<MarketplaceItem> viewedItems = new HashSet<MarketplaceItem>();

    private int currentIndex;

    [Header("UI")]
    public GameObject marketplacePanel;

    public Image itemImage;

    public TMP_Text itemName;

    public TMP_Text price;

    public TMP_Text owner;

    public TMP_Text dateListed;

    public TMP_Text watchCount;

    public TMP_Text pageText;
    

    [Header("Model")]
    public Transform modelSpawnPoint;

    private GameObject currentModel;

    void Start()
    {
        DisplayItem();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            marketplacePanel.SetActive(!marketplacePanel.activeSelf);

            if (marketplacePanel.activeSelf)
                DisplayItem();
        }
    }

    public void NextItem()
    {
        currentIndex++;

        if (currentIndex >= items.Length)
            currentIndex = 0;

        DisplayItem();
    }

    public void PreviousItem()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = items.Length - 1;

        DisplayItem();
    }

    void DisplayItem()
    {
        MarketplaceItem item = items[currentIndex];

        viewedItems.Add(item);
        item.watchCount++;

        itemImage.sprite = item.itemSprite;

        itemName.text = item.itemName;

        price.text = "Price: ₱" + item.price;

        owner.text = "Owner: " + item.ownerName;

        dateListed.text = "Listed: " + item.dateListed;

        watchCount.text = "👀 " + item.watchCount + " watching";

        pageText.text = (currentIndex + 1) + " / " + items.Length;

        if(currentModel != null)
            Destroy(currentModel);

        if(item.itemModel != null)
        {
            currentModel = Instantiate(
                item.itemModel,
                modelSpawnPoint.position,
                Quaternion.identity
            );
        }
    }
}