using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneEV : MonoBehaviour
{
    public AudioSource changeSceneSound;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            changeSceneSound.PlayOneShot(changeSceneSound.clip);
            // StartCoroutine(ChangeScene("MarioLevel2"));
            StartCoroutine(WaitSoundClip("MarioLevel2"));
        }
    }

    IEnumerator WaitSoundClip(string sceneName)
    {
        yield return new WaitUntil(() => !changeSceneSound.isPlaying);
        StartCoroutine(ChangeScene("MarioLevel2"));

    }
    IEnumerator ChangeScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
