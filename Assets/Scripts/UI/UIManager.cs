using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GameUI;
    public GameObject GameOverUI;
    public GameObject ShopUI;
    public ShopHolder shopHolder;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetUI(GameObject uiToShow)
    {
        GameOverUI.SetActive(false);
        GameUI.SetActive(false);
        ShopUI.SetActive(false);
        uiToShow.SetActive(true);
    }
    
}
