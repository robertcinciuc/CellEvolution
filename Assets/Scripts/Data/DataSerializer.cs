using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataSerializer : MonoBehaviour
{
	public GameObject player;
	public ProgressionData progressionData;
	public TerrainRenderer terrainRenderer;
	public UpgradeManager upgradeManager;

	public void SaveGame() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat");
        DataPacket data = new DataPacket(progressionData, player);
		bf.Serialize(file, data);
        file.Close();

        Debug.Log("Game data saved!");
    }

    public void LoadGame() {
		if (File.Exists(Application.persistentDataPath + "/MySaveData.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
			DataPacket data = (DataPacket)bf.Deserialize(file);
			file.Close();

			ProgressionDataSerial progressionDataSerial = data.progressionDataSerial;
			Vector3 playerPos = new Vector3(progressionDataSerial.playerPosX, progressionDataSerial.playerPosY, progressionDataSerial.playerPosZ);
			terrainRenderer.renderCurrentPlane(playerPos);
			loadFromDataPacket(data);

			Debug.Log("Game data loaded!");
		} else {
			Debug.LogError("There is no save data!");
		}
	}

	public void ResetData() {
		if (File.Exists(Application.persistentDataPath + "/MySaveData.dat")) {
			File.Delete(Application.persistentDataPath + "/MySaveData.dat");
			applyReset();
			Debug.Log("Data reset complete!");
		} else {
			Debug.LogError("No save data to delete.");
		}
	}

	private void loadFromDataPacket(DataPacket data) {
		progressionData.updateProgressionData(data.progressionDataSerial);
        PlayerState playerState = player.GetComponent<PlayerState>();
		playerState.updatePlayerState(data.progressionDataSerial.playerStateSerial);
        Morphology morphology = player.GetComponent<Morphology>();
		morphology.updateMorphology(data.morphologySerial);
		upgradeManager.renderFigure();
	}

	private void applyReset() {
		progressionData.resetProgressionData();
		PlayerState playerState = player.GetComponent<PlayerState>();
		playerState.resetPlayerState();
		Morphology morphology = player.GetComponent<Morphology>();
		morphology.resetMorphology();
		upgradeManager.renderFigure();
	}
}