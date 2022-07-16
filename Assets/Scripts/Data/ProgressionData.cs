using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionData : MonoBehaviour
{
    public static int nbEnemiesKilled = 0;
	public static int nbMeatsEaten = 0;
	public PlayerState playerState;
	public GameObject player;
	public UpgradeManager upgradeManager;

    private void Start() {
    }

	public void updateProgressionData(ProgressionDataSerial progressionDataSerial) {
		nbEnemiesKilled = progressionDataSerial.nbEnemiesKilled;
		nbMeatsEaten = progressionDataSerial.nbMeatsEaten;
    }

	public void resetProgressionData() {
		nbEnemiesKilled = 0;
		nbMeatsEaten= 0;
    }
	
}
