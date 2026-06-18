using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomBattleExit : MonoBehaviour
{
    private readonly string[] battleScenes =
    {
        "Bscene1",
        "Bscene3",
        "Bscene4",
        "Bscene5"
    };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.GetComponent<PlayerController>())
            return;

        string currentScene =
            SceneManager.GetActiveScene().name;

        List<string> availableScenes =
            new List<string>(battleScenes);

        availableScenes.Remove(currentScene);

        if (availableScenes.Count == 0)
            return;

        string randomScene =
            availableScenes[
                Random.Range(0, availableScenes.Count)
            ];

        // Lưu vàng trước khi chuyển scene
        if (PlayerAmmo.Instance != null &&
            EconomyManager.Instance != null)
        {
            PlayerAmmo.Instance.savedGold =
                EconomyManager.Instance.GetCurrentGold();
        }

        SceneManager.LoadScene(randomScene);
    }
}