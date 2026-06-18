using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HealerNPC : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [Header("Audio")]
    [SerializeField] private AudioClip healSuccessSFX;

    private AudioSource audioSource;
    private bool playerInRange;
    private bool alreadyUsed;

    private string currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

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

        if (alreadyUsed)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            HealPlayer();
        }
    }

    private void HealPlayer()
    {
        PlayerHealth player = PlayerHealth.Instance;

        int healAmount =
            Mathf.CeilToInt(player.GetMaxHealth() / 2f);

        player.Heal(healAmount);

        alreadyUsed = true;
        if (healSuccessSFX != null)
        {
            audioSource.PlayOneShot(healSuccessSFX);
        }
        dialogueText.text =
            "Xong rồi, hãy cẩn thận nhé.";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<PlayerController>())
            return;

        playerInRange = true;

        dialogueText.gameObject.SetActive(true);

        if (alreadyUsed)
        {
            dialogueText.text =
                "Tôi đã chữa cho bạn rồi.";
        }
        else
        {
            dialogueText.text =
                "Bạn bị thương à.\nĐể tôi chữa cho bạn.\n(E)";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.GetComponent<PlayerController>())
            return;

        playerInRange = false;

        dialogueText.gameObject.SetActive(false);
    }
}