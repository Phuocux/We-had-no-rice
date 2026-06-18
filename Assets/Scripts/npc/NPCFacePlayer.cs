using UnityEngine;

public class NPCFacePlayer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool playerInRange;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Nếu SpriteRenderer nằm ở object con:
        // spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        FacePlayer();
    }

    private void FacePlayer()
    {
        if (PlayerController.Instance == null)
            return;

        float playerX =
            PlayerController.Instance.transform.position.x;

        float npcX =
            transform.position.x;

        // Sprite gốc nhìn sang phải
        if (playerX < npcX)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerInRange = true;

            FacePlayer();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerInRange = false;

            // NPC quay lại hướng mặc định (phải)
            spriteRenderer.flipX = false;
        }
    }
}