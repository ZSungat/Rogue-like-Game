using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController instance;

    public int CurrentCoins;
    public CoinPickUp Coin;

    private void Awake()
    {
        instance = this;
    }


    public void AddCoins(int CoinsToAdd)
    {
        CurrentCoins += CoinsToAdd;

        UIController.instance.UpdateCoins();

        SFXManager.instance.PlaySFXPitched(2);
    }

    public void DropCoin(Vector3 Position, int Value)
    {
        CoinPickUp NewCoin = Instantiate(Coin, Position + new Vector3(.2f, .1f, 0f), Quaternion.identity);
        NewCoin.CoinAmount = Value;
        NewCoin.gameObject.SetActive(true);
    }

    public void SpendCoins(int CoinsToSpend)
    {
        CurrentCoins -= CoinsToSpend;

        UIController.instance.UpdateCoins();
    }
}
