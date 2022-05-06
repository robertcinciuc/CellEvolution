using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionData : MonoBehaviour
{
    public static int nbEnemiesKilled = 0;
	public static int nbMeatsEaten = 0;
	
	public PlayerState playerState;

	public DataPacket buildDatapacketForStoring() {
		DataPacket data = new DataPacket();
		data.nbEnemiesKilled = nbEnemiesKilled;
		data.nbMeatsEaten= nbMeatsEaten;
		data.health = playerState.health;

        return data;
	}

	public void loadFromDataPacket(DataPacket data) {
        nbEnemiesKilled = data.nbEnemiesKilled;
		nbMeatsEaten = data.nbMeatsEaten;
		playerState.sethealth(data.health);
    }

    public void applyReset() {
		nbEnemiesKilled = 0;
		nbMeatsEaten = 0;
		playerState.sethealth(playerState.maxHealth);
	}

}