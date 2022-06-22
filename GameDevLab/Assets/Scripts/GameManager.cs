using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Text score;
    private int playerScore = 0;
    public delegate void gameEvent();
    public static event gameEvent onPlayerDeath;
    public static event gameEvent onEnemyDeath;

    // Singleton Pattern
    private static GameManager _instance;
    // Getter
    public static GameManager Instance
    {
        get { return _instance; }
    }

    override public void Awake()
    {
        base.Awake();
        Debug.Log("awake called");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void increaseScore()
    {
        playerScore += 1;
        score.text = "SCORE: " + playerScore.ToString();

        onEnemyDeath();
    }

    public void damagePlayer()
    {
        onPlayerDeath();
    }
}
