using UnityEngine;

public class GunBanditAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectRange = 8f;
    [SerializeField] private float keepDistance = 4f;

    [Header("Wander")]
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float wanderWaitTime = 2f;
    [SerializeField] private float wanderSpeedMultiplier = 0.5f;

    private Rigidbody2D rb;
    private Transform player;
    private SpriteRenderer spriteRenderer;

    private Vector2 spawnPosition;
    private Vector2 wanderTarget;
    private float wanderTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (PlayerController.Instance != null)
            player = PlayerController.Instance.transform;

        spawnPosition = transform.position;
        ChooseNewWanderPoint();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        float distance =
            Vector2.Distance(transform.position, player.position);

        // Có người chơi -> di chuyển giống BlueSlime
        if (distance <= detectRange)
        {
            MoveAroundPlayer(distance);
        }
        // Không có người chơi -> đi lang thang
        else
        {
            Wander();
        }
    }

    private void MoveAroundPlayer(float distance)
    {
        Vector2 direction =
            (player.position - transform.position).normalized;

        if (spriteRenderer != null)
            spriteRenderer.flipX = direction.x < 0;

        // Nếu ở xa thì tiến lại gần
        if (distance > keepDistance)
        {
            rb.MovePosition(
                rb.position +
                direction * moveSpeed * Time.fixedDeltaTime
            );
        }
        // Nếu ở gần quá thì lùi ra
        else if (distance < keepDistance - 1f)
        {
            rb.MovePosition(
                rb.position -
                direction * moveSpeed * Time.fixedDeltaTime
            );
        }
    }

    private void Wander()
    {
        Vector2 direction = wanderTarget - rb.position;

        if (direction.magnitude < 0.2f)
        {
            wanderTimer -= Time.fixedDeltaTime;

            if (wanderTimer <= 0f)
            {
                ChooseNewWanderPoint();
            }

            return;
        }

        direction.Normalize();

        if (spriteRenderer != null)
            spriteRenderer.flipX = direction.x < 0;

        rb.MovePosition(
            rb.position +
            direction *
            (moveSpeed * wanderSpeedMultiplier) *
            Time.fixedDeltaTime
        );
    }

    private void ChooseNewWanderPoint()
    {
        wanderTarget =
            spawnPosition +
            Random.insideUnitCircle * wanderRadius;

        wanderTimer = wanderWaitTime;
    }
}