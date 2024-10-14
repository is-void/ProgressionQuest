using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerController playerController;
    public Toggle shouldUseCustomValues;
    public InputField modeField;
    public GameObject errorPanel;

    public enum GameMode
    {
        MENU,
        FLAPPY,
        SPACESHOOTER,
        PLATFORMER,
        FPS
    }
    public enum State
    {
        MAINMENU,
        RUNNING,
        PAUSE,
        SHOP,
        GAMEOVER
    }
    public State gameState;
    public State lastState;

    public GameMode gameMode;

    public UIManager uiManager;

    void Start()
    {

        ResetPlayerPrefs();
        if(gameMode != GameMode.MENU)
            playerController = GameObject.Find("Player").GetComponentInChildren<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(lastState != gameState)
        {
            SwitchState();

        }
        lastState = gameState;
    }

    void SwitchState()
    {
        Debug.Log("Changing State");
        switch(gameMode)
        {
            case GameMode.FLAPPY:
                FlappyStates();
                break;
            case GameMode.SPACESHOOTER:
                SpaceShooterStates();
                break;
            default:
                break;


        }
       
    }

    void FlappyStates()
    {
        switch (gameState)
        {
            case State.GAMEOVER:
                uiManager.SetUI(uiManager.GameOverUI);
                break;
            case State.RUNNING:
                uiManager.SetUI(uiManager.GameUI);
                RestartFlapGame();
                break;
            case State.SHOP:
                uiManager.SetUI(uiManager.ShopUI);
                break;
            case State.MAINMENU:
                SceneManager.LoadScene(0);
                break;

            default:
                break;

        }
    }
    void SpaceShooterStates()
    {
        switch (gameState)
        {
            case State.GAMEOVER:
                uiManager.SetUI(uiManager.GameOverUI);
                break;
            case State.RUNNING:
                uiManager.SetUI(uiManager.GameUI);
                RestartSpaceShooter();
                break;
            case State.SHOP:
                uiManager.SetUI(uiManager.ShopUI);
                break;
            case State.MAINMENU:
                SceneManager.LoadScene(0);
                break;

            default:
                break;

        }
    }
    public void GoToNextScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("sixth");

    }

    public void GoToCurrentGame()
    {
        if(shouldUseCustomValues.isOn)
        {
            if(modeField.text != "" && (int.Parse(modeField.text) > 0 && int.Parse(modeField.text) < 5))
            {
                SceneManager.LoadScene(int.Parse(modeField.text));
            } else
            {
                if(!errorPanel.activeSelf)
                    StartCoroutine(ShowErrorMessage());
            }
        } else
        {
            if (PlayerPrefs.HasKey("Gamemode"))
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("Gamemode"));
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
        
    }

    IEnumerator ShowErrorMessage()
    {
        errorPanel.SetActive(true);
        yield return new WaitForSeconds(4f);
        errorPanel.SetActive(false);
    }    

    public void setState()
    {

    }
    public void RestartFlapGame()
    {
        PlayerPrefs.Save();
        gameState = State.RUNNING;
        SceneManager.LoadScene(1);
    }
    public void RestartSpaceShooter()
    {
        PlayerPrefs.Save();
        gameState = State.RUNNING;
        SceneManager.LoadScene(2);
    }

    public void GoToShop()
    {
        gameState = State.SHOP;
    }

    public void OnGameOver()
    {

    }

    public void ResetPlayerPrefs()
    {
        bool ogActive = false; 
        if (uiManager!=null)
        {
            ogActive = uiManager.ShopUI.activeInHierarchy;
            if (!ogActive)
                uiManager.ShopUI.SetActive(true);
        }
            


        if (!PlayerPrefs.HasKey("SPACESHOOTER_HealthUpgrades"))
        {
            PlayerPrefs.SetFloat("SPACESHOOTER_HealthUpgrades", 0);
        }

        if (!PlayerPrefs.HasKey("SPACESHOOTER_WeaponIndex"))
        {
            PlayerPrefs.SetFloat("SPACESHOOTER_WeaponIndex", 0);
        }

        if (!(PlayerPrefs.HasKey("FlappyJumpForce")))
        {
            PlayerPrefs.SetFloat("FlappyJumpForce", PlayerController.INITAL_JUMP_FORCE);
        }
        if (!(PlayerPrefs.HasKey("GravityYVal")))
        {
            PlayerPrefs.SetFloat("GravityYVal", PlayerController.INITIAL_GRAVITY_Y);
        }
        if(!(PlayerPrefs.HasKey("FlappyLevel")))
        {
            PlayerPrefs.SetInt("FlappyLevel", 1);
        }
        if( !(PlayerPrefs.HasKey("Max Streak")))
        {
            PlayerPrefs.SetInt("Max Streak", 0);
        }
        if( !(PlayerPrefs.HasKey("Use Precise Movement")))
        {
            PlayerPrefs.SetInt("Use Precise Movement", 0);
        }
        if( !(PlayerPrefs.HasKey("Money")))
        {
            PlayerPrefs.SetInt("Money", 0);
        }

        if( !(PlayerPrefs.HasKey("Starting Phase")))
        {
            PlayerPrefs.SetInt("Starting Phase", 1);
        }
        
        if(uiManager != null)
        {
            foreach (ShopHolder shopHolder in GameObject.Find("ShopUI").GetComponentsInChildren<ShopHolder>())
            {
                if (!PlayerPrefs.HasKey(shopHolder.playerPref))
                    PlayerPrefs.SetInt(shopHolder.playerPref, 0);
            }

            uiManager.ShopUI.SetActive(ogActive);
        }
        
    }

}