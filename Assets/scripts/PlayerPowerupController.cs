using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerupController : MonoBehaviour
{
    private int[] flags; 

    /**
     * a list of all the powerups that can affect this player
     * determines if you have a powerup by using a simple bit fiddling formula
     * */
    private int currentPowerups;

    public bool hasPowerup(PowerupType powerup)
    {
        return ((currentPowerups & flags[(int)powerup]) != 0);
    }

    public void addPowerup(PowerupType powerup)
    {
        currentPowerups |= flags[(int)powerup];
    }

    public void removePowerup(PowerupType powerup)
    {
        currentPowerups ^= flags[(int)powerup];
    }

    void Awake()
    {
        flags = new int[(int)PowerupType.POWERUP_COUNT];
        int currentFlag = 1;

        for (int i = 0; i < (int)PowerupType.POWERUP_COUNT; i++)
        {
            flags[i] = currentFlag;
            currentFlag <<= 1;
        }

        this.currentPowerups = 0; 
    }
}