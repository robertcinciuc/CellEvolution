using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionData : MonoBehaviour
{
    public static int nbEnemiesKilled = 0;
	public static int nbMeatsEaten = 0;
	public Morphology playerMorphology;
	public PlayerState playerState;
	public GameObject player;
	public UpgradeManager upgradeManager;

    private void Start() {
    }

    public DataPacket buildDatapacketForStoring() {
		DataPacket data = new DataPacket();
		data.nbEnemiesKilled = nbEnemiesKilled;
		data.nbMeatsEaten= nbMeatsEaten;
		data.health = playerState.health;
		data.playerSerialOrgans = playerMorphology.getPlayerSerialOrgans();

		data.playerPosX = player.transform.position.x;
		data.playerPosY = player.transform.position.y;
		data.playerPosZ = player.transform.position.z;
		data.playerRotW = player.transform.rotation.w;
		data.playerRotX = player.transform.rotation.x;
		data.playerRotY = player.transform.rotation.y;
		data.playerRotZ = player.transform.rotation.z;

		return data;
	}

	public void loadFromDataPacket(DataPacket data) {
        nbEnemiesKilled = data.nbEnemiesKilled;
		nbMeatsEaten = data.nbMeatsEaten;
		playerState.sethealth(data.health);
		playerMorphology.addAllOrgans(data.playerSerialOrgans);
		upgradeManager.renderFigure();

		player.transform.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);
		player.transform.rotation = Quaternion.Slerp(
			player.transform.rotation, 
			new Quaternion(data.playerRotW, data.playerRotX, data.playerRotY, data.playerRotZ), 
			Time.deltaTime);
	}

    public void applyReset() {
		nbEnemiesKilled = 0;
		nbMeatsEaten = 0;
		playerState.sethealth(playerState.maxHealth);
		playerMorphology.removeAllOrgans();
		playerMorphology.transform.position = Vector3.zero;
		playerMorphology.transform.rotation = Quaternion.identity;
	}

}
