using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CentralManager : MonoBehaviour
{
    public GameObject gameManagerObject;
    private GameManager gameManager;

    public GameObject powerupManagerObject;
    private PowerupManager powerupManager;

    public GameObject spawnManagerObject;
    private SpawnManager spawnManager;

    public static CentralManager centralManagerInstance;

    void Awake()
    {
        centralManagerInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManagerObject.GetComponent<GameManager>();
        powerupManager = powerupManagerObject.GetComponent<PowerupManager>();
        spawnManager = spawnManagerObject.GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void increaseScore()
    {
        gameManager.increaseScore();
    }

    public void damagePlayer()
    {
        gameManager.damagePlayer();
    }

    public void consumePowerup(KeyCode k, GameObject g)
    {
        powerupManager.consumePowerup(k, g);
    }

    public void addPowerup(Texture t, int i, ConsumableInterface c)
    {
        powerupManager.addPowerup(t, i, c);
    }

    public void spawnOneRandom()
    {
        spawnManager.spawnOneRandom();
    }

    public void changeScene()
    {
        StartCoroutine(LoadYourAsyncScene("MarioLevel2"));
    }

    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        // The application loads the scene in the background as the current scene runs.
        // This is particularly good for creating loading screens.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
