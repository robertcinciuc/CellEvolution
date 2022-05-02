using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataSerializer : MonoBehaviour
{
	//TODO: Move these stats to a sepparate class
	public static int intToSave;
	public static float floatToSave;
	public static bool boolToSave;

	public static void SaveGame() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat");
		ProgressData data = new ProgressData();
		data.savedInt = intToSave;
		data.savedFloat = floatToSave;
		data.savedBool = boolToSave;
		bf.Serialize(file, data);
		file.Close();
		Debug.Log("Game data saved!");
	}

	public static void LoadGame() {
		if (File.Exists(Application.persistentDataPath + "/MySaveData.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/MySaveData.dat", FileMode.Open);
			ProgressData data = (ProgressData)bf.Deserialize(file);
			file.Close();
			intToSave = data.savedInt;
			floatToSave = data.savedFloat;
			boolToSave = data.savedBool;
			Debug.Log("Game data loaded!");
		} else {
			Debug.LogError("There is no save data!");
		}
	}

	public static void ResetData() {
		if (File.Exists(Application.persistentDataPath + "/MySaveData.dat")) {
			File.Delete(Application.persistentDataPath + "/MySaveData.dat");
			intToSave = 0;
			floatToSave = 0.0f;
			boolToSave = false;
			Debug.Log("Data reset complete!");
		} else {
			Debug.LogError("No save data to delete.");
		}
	}
}