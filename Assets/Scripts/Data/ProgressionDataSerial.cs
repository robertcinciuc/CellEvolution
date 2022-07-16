using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProgressionDataSerial {

	public int nbEnemiesKilled;
	public int nbMeatsEaten;
	public PlayerStateSerial playerStateSerial;
	public float playerPosX;
	public float playerPosY;
	public float playerPosZ;
	public float playerRotW;
	public float playerRotX;
	public float playerRotY;
	public float playerRotZ;

	public ProgressionDataSerial(ProgressionData progressionData, GameObject player) {
		this.nbEnemiesKilled = ProgressionData.nbEnemiesKilled;
		this.nbMeatsEaten = ProgressionData.nbMeatsEaten;
		this.playerStateSerial = new PlayerStateSerial(progressionData.playerState);
		playerPosX = player.transform.position.x;
		playerPosY = player.transform.position.y;
		playerPosZ = player.transform.position.z;
		playerRotW = player.transform.rotation.w;
		playerRotX = player.transform.rotation.x;
		playerRotY = player.transform.rotation.y;
		playerRotZ = player.transform.rotation.z;
}

}
