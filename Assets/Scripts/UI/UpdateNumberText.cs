using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateNumberText : MonoBehaviour
{
    public PlayerController player;
    public TextMeshProUGUI meshText;
    public string text;
    public enum ScoreType
    {
        SCORE,
        HISCORE,
        MONEY,
        STREAK
    }
    public ScoreType mode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(mode)
        {
            case ScoreType.SCORE:
                meshText.text = text + player.score;
                break;
            case ScoreType.HISCORE:
                meshText.text = text + player.hiScore;
                break;
            case ScoreType.STREAK:
                meshText.text = text + player.streak;
                break;
            case ScoreType.MONEY:
                meshText.text = text + player.money;
                break;
        }
        
    }
}
