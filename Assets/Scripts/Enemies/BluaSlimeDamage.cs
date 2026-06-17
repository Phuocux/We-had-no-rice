using UnityEngine;

public class BlueSlimeDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnCollisionStay2D(Collision2D collision)
    {
        PlayerHealth player =
            collision.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(
                damage,
                transform
            );
        }
    }
}