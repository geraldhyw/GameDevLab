using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerupIndex
{
    ORANGEMUSHROOM = 0,
    REDMUSHROOM = 1
}

public class PowerupManagerEV : MonoBehaviour
{
    // reference of all player stats affected
    public IntVariable marioJumpSpeed;
    public IntVariable marioMaxSpeed;
    public PowerupInventory powerupInventory;
    public List<GameObject> powerupIcons;

    void Start()
    {
        if (!powerupInventory.gameStarted)
        {
            powerupInventory.gameStarted = true;
            powerupInventory.Setup(powerupIcons.Count);
            resetPowerup();
        }
        else
        {
            // re-render the contents of the powerup from the previous time
            for (int i = 0; i < powerupInventory.Items.Count; i++)
            {
                Powerup p = powerupInventory.Get(i);
                if (p != null)
                {
                    AddPowerupUI(i, p.powerupTexture);
                }
            }
        }
    }
        
    public void resetPowerup()
    {
        for (int i = 0; i < powerupIcons.Count; i++)
        {
            powerupIcons[i].SetActive(false);
        }
    }
        
    void AddPowerupUI(int index, Texture t)
    {
        powerupIcons[index].GetComponent<RawImage>().texture = t;
        powerupIcons[index].SetActive(true);
    }

    void RemovePowerupUI(int index)
    {
        if (index < powerupIcons.Count) {
            powerupIcons[index].SetActive(false);
            powerupInventory.Remove(index);
        }
    }

    public void AddPowerup(Powerup p)
    {
        powerupInventory.Add(p, (int)p.index);
        AddPowerupUI((int)p.index, p.powerupTexture);
    }

    public void OnApplicationQuit()
    {
        ResetValues();
    }

    public void ResetValues()
    {
        powerupInventory.Remove(0);
        powerupInventory.Remove(1);

    }

    // attemptconsumepoweriup

    public void AttemptConsumePowerup(KeyCode k)
    {
        // update IntVariable
        if (k == KeyCode.Z) {
            if (powerupInventory.Get(0) != null)
            {
                marioMaxSpeed.SetValue(marioMaxSpeed.Value + 10);
                StartCoroutine(removeRunEffect());
                RemovePowerupUI(0);
            }
            // marioJumpSpeed += 10;
        }

        if (k == KeyCode.X) {
            // marioMaxSpeed += 10;

            if (powerupInventory.Get(1) != null)
            {
                marioJumpSpeed.SetValue(marioJumpSpeed.Value + 10);
                StartCoroutine(removeJumpEffect());
                RemovePowerupUI(1);
            }
        }
    }

    IEnumerator removeJumpEffect()
    {
        yield return new WaitForSeconds(5.0f);
        marioJumpSpeed.SetValue(marioJumpSpeed.Value-10);

    }

    IEnumerator removeRunEffect()
    {
        yield return new WaitForSeconds(5.0f);
        marioMaxSpeed.SetValue(marioMaxSpeed.Value-10);

    }
}
