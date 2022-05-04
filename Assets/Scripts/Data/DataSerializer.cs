using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataSerializer : MonoBehaviour
{
	public ProgressionData progressionData;

	public void SaveGame() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat");
        DataPacket data = progressionData.buildDatapacketForStoring();
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
			progressionData.loadFromDataPacket(data);
			Debug.Log("Game data loaded!");
		} else {
			Debug.LogError("There is no save data!");
		}
	}

	public void ResetData() {
		if (File.Exists(Application.persistentDataPath + "/MySaveData.dat")) {
			File.Delete(Application.persistentDataPath + "/MySaveData.dat");
			progressionData.applyReset();
			Debug.Log("Data reset complete!");
		} else {
			Debug.LogError("No save data to delete.");
		}
	}
}