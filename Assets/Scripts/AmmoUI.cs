using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;

    private void Update()
    {
        Gun gun =
            ActiveWeapon.Instance.CurrentActiveWeapon as Gun;

        if (gun == null)
        {
            ammoText.text = "";
            return;
        }

        ammoText.text =
            $"{gun.GetCurrentAmmo()}/{PlayerAmmo.Instance.reserveAmmo}";
    }
}