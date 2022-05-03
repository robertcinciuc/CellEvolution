using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionData : MonoBehaviour
{
    public static int nbEnemiesKilled = 0;
	public static int nbMeatsEaten = 0;

	public static DataPacket buildDatapacketForStoring() {
		DataPacket data = new DataPacket();
		data.nbEnemiesKilled = nbEnemiesKilled;
		data.nbMeatsEaten= nbMeatsEaten;

		return data;
	}

	public static void loadFromDataPacket(DataPacket data) {
        nbEnemiesKilled = data.nbEnemiesKilled;
		nbMeatsEaten = data.nbMeatsEaten;
    }

    public static void applyReset() {
		nbEnemiesKilled = 0;
		nbMeatsEaten = 0;
	}

}
