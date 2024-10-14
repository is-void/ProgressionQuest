using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopHolder : MonoBehaviour
{
    //Get the shops. If the 1st shops are fully upgraded, hide them and switch to the next shop.
    public ShopInfo[] shops;
    public int shopIndex = 0;
    public string playerPref;
    // Start is called before the first frame update
    void Start()
    {
        shops = GetComponentsInChildren<ShopInfo>();
        shopIndex = PlayerPrefs.GetInt(playerPref);
        UpdateUI();
    }

    public void UpdateUI()
    {
        
        SetShop();
        if (shops != null && shops[shopIndex].isMaxedOut && shopIndex+1<shops.Length)
        {
            shopIndex++;
        }
            

        PlayerPrefs.SetInt(playerPref, shopIndex);
        SetShop();
    }
    public void SetShop()
    {
        foreach(ShopInfo shop in shops)
        {
            shop.gameObject.SetActive(false);
        }
        shops[shopIndex].gameObject.SetActive(true);
    }
}
