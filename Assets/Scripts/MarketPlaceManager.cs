using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketplaceManager : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private MarketplaceItem[] items;

    private readonly HashSet<MarketplaceItem> viewedItems = new HashSet<MarketplaceItem>();
    private int currentIndex;

    [Header("UI")]
    [SerializeField] private GameObject marketplacePanel;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text price;
    [SerializeField] private TMP_Text owner;
    [SerializeField] private TMP_Text dateListed;
    [SerializeField] private TMP_Text watchCount;
    [SerializeField] private TMP_Text pageText;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("Model")]
    [SerializeField] private Transform modelSpawnPoint;

    private GameObject currentModel;

    public bool IsOpen => marketplacePanel != null && marketplacePanel.activeSelf;

    private void Start()
    {
        if (marketplacePanel != null)
            marketplacePanel.SetActive(false);
    }

    public void OpenMarketplace()
    {
        if (marketplacePanel == null || items == null || items.Length == 0)
        {
            Debug.LogWarning("Marketplace is missing its panel or item list.", this);
            return;
        }

        marketplacePanel.SetActive(true);

        if (playerController != null)
            playerController.SetInputEnabled(false);

        DisplayItem();
    }

    public void CloseMarketplace()
    {
        if (marketplacePanel != null)
            marketplacePanel.SetActive(false);

        if (currentModel != null)
        {
            Destroy(currentModel);
            currentModel = null;
        }

        if (playerController != null)
            playerController.SetInputEnabled(true);
    }

    public void NextItem()
    {
        if (items == null || items.Length == 0)
            return;

        currentIndex = (currentIndex + 1) % items.Length;
        DisplayItem();
    }

    public void PreviousItem()
    {
        if (items == null || items.Length == 0)
            return;

        currentIndex--;

        if (currentIndex < 0)
            currentIndex = items.Length - 1;

        DisplayItem();
    }

    private void DisplayItem()
    {
        if (items == null || items.Length == 0)
            return;

        MarketplaceItem item = items[currentIndex];

        // Count this player only once per item during the current play session.
        if (viewedItems.Add(item))
            item.watchCount++;

        itemImage.sprite = item.itemSprite;
        itemName.text = item.itemName;
        price.text = "Price: ₱" + item.price;
        owner.text = "Owner: " + item.ownerName;
        dateListed.text = "Listed: " + item.dateListed;
        watchCount.text = "👀 " + item.watchCount + " watching";
        pageText.text = $"{currentIndex + 1} / {items.Length}";

        if (currentModel != null)
            Destroy(currentModel);

        if (item.itemModel != null && modelSpawnPoint != null)
        {
            currentModel = Instantiate(
                item.itemModel,
                modelSpawnPoint.position,
                modelSpawnPoint.rotation,
                modelSpawnPoint
            );
        }
    }
}