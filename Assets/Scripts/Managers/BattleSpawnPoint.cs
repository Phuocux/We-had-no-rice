using UnityEngine;

public class BattleSpawnPoint : MonoBehaviour
{
    private void Start()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.transform.position =
                Vector3.zero;
        }
    }
}