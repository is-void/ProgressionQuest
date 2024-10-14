using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    
    public ShopHolder shopHolder;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Upgrade(GameObject button)
    {
        button.GetComponent<ButtonInfo>().BuyUpgrade();
        shopHolder.UpdateUI();
        shopHolder.SetShop();
    }
}
