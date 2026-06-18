using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AmmoTrader : MonoBehaviour
{
    [Header("Trade")]
    [SerializeField] private int ammoAmount = 20;
    [SerializeField] private int goldCost = 10;

    [Header("Dialogue")]
    [SerializeField] private TMP_Text dialogueText;

    [Header("Audio")]
    [SerializeField] private AudioClip tradeSuccessSFX;

    private AudioSource audioSource;
    private bool playerInRange;

    private void Start()
    {
        dialogueText.gameObject.SetActive(false);
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!playerInRange)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            BuyAmmo();
        }
    }

    private void BuyAmmo()
    {
        if (EconomyManager.Instance.SpendGold(goldCost))
        {
            PlayerAmmo.Instance.AddAmmo(ammoAmount);
            if (tradeSuccessSFX != null)
            {
                audioSource.PlayOneShot(tradeSuccessSFX);
            }
            dialogueText.text =
                $"Nhận {ammoAmount} đạn!";
        }
        else
        {
            dialogueText.text =
                "Không đủ vàng!";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerInRange = true;

            dialogueText.gameObject.SetActive(true);

            dialogueText.text =
                $"Tôi có thể cho bạn {ammoAmount} đạn\n{goldCost} vàng (E)";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerInRange = false;

            dialogueText.gameObject.SetActive(false);
        }
    }
}