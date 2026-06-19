using TMPro;
using UnityEngine;

public class EnemyDialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float detectRange = 6f;
    [SerializeField] private float dialogueInterval = 5f;

    [TextArea]
    [SerializeField]
    private string[] dialogues =
    {
        "Mày Tày!",
        "Arhhhhhhh!!!",
        "Khốn!!",
        "Thịt nó!",
        "Đứng lại!",
        "Xử nó!"
    };

    private Transform player;
    private float timer;

    private void Start()
    {
        if (PlayerController.Instance != null)
            player = PlayerController.Instance.transform;

        timer = dialogueInterval;

        if (dialogueText != null)
            dialogueText.text = "";
    }

    private void Update()
    {
        if (player == null || dialogueText == null)
            return;

        float distance =
            Vector2.Distance(transform.position,
                             player.position);

        if (distance <= detectRange)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                ShowRandomDialogue();
                timer = dialogueInterval;
            }
        }
        else
        {
            dialogueText.text = "";
            timer = dialogueInterval;
        }
    }

    private void ShowRandomDialogue()
    {
        dialogueText.text =
            dialogues[Random.Range(0, dialogues.Length)];

        CancelInvoke();
        Invoke(nameof(ClearDialogue), 2f);
    }

    private void ClearDialogue()
    {
        dialogueText.text = "";
    }
}