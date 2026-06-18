using TMPro;
using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>
{
    [SerializeField] private TMP_Text goldText;

    private int currentGold = 0;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";

    private void Start()
    {
        if (PlayerAmmo.Instance != null)
        {
            currentGold = PlayerAmmo.Instance.savedGold;
        }
        RefreshUI();
    }
    protected override void Awake()
    {
        base.Awake();

        Debug.Log(
            $"EconomyManager Awake | ID={GetInstanceID()} | Gold={currentGold}"
        );
    }
    // Giữ tương thích với Pickup cũ
    public void SetGold(int amount)
    {
        currentGold = amount;
        RefreshUI();
    }
    public void UpdateCurrentGold()
    {
        AddGold(1);
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        RefreshUI();
    }

    public bool SpendGold(int amount)
    {
        if (currentGold < amount)
            return false;

        currentGold -= amount;
        RefreshUI();

        return true;
    }

    public int GetCurrentGold()
    {
        return currentGold;
    }

    private void RefreshUI()
    {
        if (goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT)
                .GetComponent<TMP_Text>();
        }

        goldText.text = currentGold.ToString("D3");
    }
}