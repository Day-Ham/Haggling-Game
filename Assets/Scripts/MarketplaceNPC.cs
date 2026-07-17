using UnityEngine;
using UnityEngine.InputSystem;

public class MarketplaceNPC : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private MarketplaceManager marketplaceManager;
    [SerializeField] private GameObject talkPrompt;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;

    private void Start()
    {
        SetPromptVisible(false);
    }

    private void Update()
    {
        if (player == null || marketplaceManager == null)
            return;

        float distanceSquared = (player.position - transform.position).sqrMagnitude;
        bool playerIsClose = distanceSquared <= interactionDistance * interactionDistance;
        bool canTalk = playerIsClose && !marketplaceManager.IsOpen;

        SetPromptVisible(canTalk);

        if (canTalk && Keyboard.current?.eKey.wasPressedThisFrame == true)
            marketplaceManager.OpenMarketplace();
    }

    private void OnDisable()
    {
        SetPromptVisible(false);
    }

    private void SetPromptVisible(bool isVisible)
    {
        if (talkPrompt != null && talkPrompt.activeSelf != isVisible)
            talkPrompt.SetActive(isVisible);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}