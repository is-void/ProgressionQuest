using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeShopDescription : MonoBehaviour
{
    public string descriptionText;
    public Text shopText;
    void Start()
    {
        shopText = GetComponent<Text>();
    }

    // Update is called once per frame
    public void UpdateText(int cost)
    {
        if (shopText == null)
            Debug.Log(transform.parent.gameObject.name);
        shopText.text = descriptionText + "\n$" + cost;
    }
}
