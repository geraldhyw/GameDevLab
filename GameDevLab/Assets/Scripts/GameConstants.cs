using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject
{
    // for scoring system
    int currentScore;
    int currentPlayerHealth;

    // for reset values
    Vector3 gombaSpawnPointStart = new Vector3(2.5f, -0.45f, 0); // hardcoded location
    // .. other reset values

    // for Consume.cs
    public int consumeTimeStep = 10;
    public int consumeLargestScale = 4;

    // for BreakBrick.cs
    public int breakTimeStep = 30;
    public int breakDebrisToruqe = 10;
    public int breakDebrisForce = 10;

    // for Debris.cs
    public int spawnNumberOfDebris = 10;

    // for Rotator.cs
    public int rotatorRotateSpeed = 6;

    // for testing
    public int testValue;

    // for EnemyController.cs
    public float maxOffset = 5.0f;
    public float enemyPatroltime = 2.0f;
    public int groundSurface = -1;

    // for enemy rejoice
    public bool isRejoice = false;

    // Mario basic starting values
    public int playerMaxSpeed = 5;
    public int playerMaxJumpSpeed = 30;
    public int playerDefaultForce = 150;

    // ground
    public float groundDistance = 1.0f;
}
