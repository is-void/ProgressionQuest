using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInfo : MonoBehaviour
{
    public ButtonInfo[] buttons;
    public bool isMaxedOut;
    private ShopHolder shopHolder;
    void Start()
    {
        shopHolder = transform.parent.GetComponent<ShopHolder>();

        buttons = GetComponentsInChildren<ButtonInfo>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bool check = true;
        foreach(ButtonInfo button in buttons)
        {
            if(!button.isMaxedOut)
            {
                check = false;
            }
        }
        isMaxedOut = check;
        if(isMaxedOut)
        {
            shopHolder.UpdateUI();
        }
    }
}
