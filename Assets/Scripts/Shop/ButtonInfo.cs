using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    public string upgradeKey;
    public GameManager gameManager;
    public bool isMaxedOut = false;
    public int initalCost;
    public int cost;
    public int index, max;


    void Start()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>() != null)
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        else
            Debug.Log("Can't find GameManager");

        upgradeKey = name + "Button";
        if(!PlayerPrefs.HasKey(upgradeKey))
        {
            PlayerPrefs.SetInt(upgradeKey, 0);
            cost = initalCost;
        }
        
        
    }

    private void Update()
    {
        if (index >= max)
            isMaxedOut = true;
    }
    public void BuyUpgrade()
    {
        if (max - index  > 0 && (gameManager.playerController.money - cost > 0))
        {
            PlayerPrefs.SetInt(upgradeKey, index + 1);
            gameManager.playerController.SubtractMoney(cost);
            gameManager.playerController.Upgrade(name);
            UpdateCost();
            if (max - index <= 0)
            {
                isMaxedOut = true;
            }
        } 
        if(max - index <= 0)
        {
            isMaxedOut = true;
        }


    }

    void UpdateCost()
    {
        UpgradeShopDescription shopDescription = GetComponentInChildren<UpgradeShopDescription>();
        Debug.Log(name + shopDescription);
        index = PlayerPrefs.GetInt(upgradeKey);
        cost = initalCost + Mathf.RoundToInt(initalCost * index/ 2f);
        shopDescription.UpdateText(cost);
    }

}
