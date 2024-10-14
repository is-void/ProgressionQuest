 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int money;
    public Rigidbody rb;
    public MovementSystem movementSystem;
    public Score CHANGE;
    public readonly Vector3 defaultGravity = new Vector3(0, -9.8F, 0);
    public Vector3 baseGravity = new Vector3(0, -9.8F, 0);

    public static readonly float INITAL_JUMP_FORCE = 200F;
    public static readonly float MAX_JUMP_FORCE = 350F;

    public static readonly float INITIAL_GRAVITY_Y = -20F;

    public int score;
    public int hiScore;
    public int streak;

    public GameManager gameManager;
    public struct Score
    {
        public int visibleScore;
        public int hiScore;
        public int streak;
    }

    void Start()
    {
        money = PlayerPrefs.GetInt("Money");

        hiScore = 0;
        switch(gameManager.gameMode)
        {
            case GameManager.GameMode.FLAPPY:
                hiScore = PlayerPrefs.GetInt("FlapGame_HighScore");
                break;

            case GameManager.GameMode.SPACESHOOTER:
                hiScore = PlayerPrefs.GetInt("SpaceShooter_HighScore");
                break;
        }
        movementSystem.OnSwitch(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!movementSystem.gameOver)
        {
            movementSystem.SystemFixedUpdate();
        }
        
    }

    private void Update()
    {
        if(!movementSystem.gameOver)
        {
            movementSystem.SystemUpdate();
        }
    }
    
    public void Upgrade(string buttonName)
    {
        movementSystem.Upgrade(buttonName);
    }
    
    public void AddMoney(int change)
    {
        money += change;
        PlayerPrefs.SetInt("Money", money);
    }
        
    public void SubtractMoney(int change)
    {
        money -= change;
        PlayerPrefs.SetInt("Money", money);
    }
    
    public void UpdateScore(int change)
    {
        score += change;
        if(hiScore < score)
        {
            hiScore = score;

            switch(gameManager.gameMode)
            {
                case GameManager.GameMode.FLAPPY:
                    PlayerPrefs.SetInt("FlapGame_HighScore", hiScore);
                    break;
                
                case GameManager.GameMode.SPACESHOOTER:
                    PlayerPrefs.SetInt("SpaceShooter_HighScore", hiScore);
                    break;
            }
        }
    }
}

