using UnityEngine;

public class PlayerAmmo : Singleton<PlayerAmmo>
{
    [Header("Ammo Reserve")]
    public int reserveAmmo = 90;

    [Header("Magazine")]
    public int magazineAmmo = 12;
    [Header("Economy Backup")]
    public int savedGold = 0;
    public bool HasAmmo(int amount)
    {
        return reserveAmmo >= amount;
    }

    public void RemoveAmmo(int amount)
    {
        reserveAmmo -= amount;

        if (reserveAmmo < 0)
            reserveAmmo = 0;
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
    }
}