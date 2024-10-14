using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Delete Data")]
    public bool deletePlayerPrefs;
    public bool deleteFlappyJumpForce;
    public bool deleteGravityYVal;
    public int flappyStartingPhase;

    [Header("Permissions")]
    public bool allowCustomValues; 

    [Header("Assign Values")]
    public int money;
    public float gameSpeed;
    private GameManager gameManager;

    
    void Start()
    {
        if(allowCustomValues)
        {
            PlayerPrefs.SetInt("Money", money);
            Time.timeScale = gameSpeed;
        }
        else
        {
            Time.timeScale = 1f;
        }
        

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        if (deletePlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            gameManager.ResetPlayerPrefs();
        }
        else
        {
            if (deleteFlappyJumpForce)
            {
                PlayerPrefs.DeleteKey("FlappyJumpForce");
                gameManager.ResetPlayerPrefs();
            }
            if (deleteGravityYVal)
            {
                PlayerPrefs.DeleteKey("GravityYVal");
                gameManager.ResetPlayerPrefs();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        flappyStartingPhase = PlayerPrefs.GetInt("Starting Phase");
        if (allowCustomValues)
        {
            PlayerPrefs.SetInt("Money", money);
            Time.timeScale = gameSpeed;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (deletePlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
            gameManager.ResetPlayerPrefs();
        }
        else
        {
            if (deleteFlappyJumpForce)
            {
                PlayerPrefs.DeleteKey("FlappyJumpForce");
                gameManager.ResetPlayerPrefs();
            }
            if(deleteGravityYVal)
            {
                PlayerPrefs.DeleteKey("GravityYVal");
                gameManager.ResetPlayerPrefs();
            }
           
        }
        
    }
}
