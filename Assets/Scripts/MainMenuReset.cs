using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuReset : MonoBehaviour
{
    private void Start()
    {
        // Xóa toàn bộ singleton cũ
        DestroyIfExists(PlayerController.Instance);
        DestroyIfExists(PlayerHealth.Instance);
        DestroyIfExists(ActiveWeapon.Instance);
        DestroyIfExists(Stamina.Instance);
        DestroyIfExists(SceneManagement.Instance);
        DestroyIfExists(PlayerAmmo.Instance);

        // thêm các singleton khác nếu có
    }

    private void DestroyIfExists(MonoBehaviour obj)
    {
        if (obj != null)
            Destroy(obj.gameObject);
    }
}