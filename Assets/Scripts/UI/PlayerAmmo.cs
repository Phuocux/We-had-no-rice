using UnityEngine;

public class PlayerAmmo : MonoBehaviour
{
    public static PlayerAmmo Instance;

    [Header("Ammo")]
    public int reserveAmmo = 90;

    private void Awake()
    {
        Instance = this;
    }

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