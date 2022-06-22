using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreMonitor : MonoBehaviour
{
    public IntVariable marioScore;
    public Text text;

    public void UpdateScore()
    {
        text.text = "Score: " + marioScore.Value.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
