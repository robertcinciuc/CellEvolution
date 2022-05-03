using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsDisplayer : MonoBehaviour
{
    private void OnGUI() {
		if (GUI.Button(new Rect(0, 0, 125, 50), "Raise Integer")) {
			ProgressionData.nbEnemiesKilled++;
		}
		//if (GUI.Button(new Rect(0, 100, 125, 50), "Raise Float")) {
		//	//DataSerializer.floatToSave += 0.1f;
		//}
		//if (GUI.Button(new Rect(0, 200, 125, 50), "Change Bool")) {
		//	//DataSerializer.boolToSave = DataSerializer.boolToSave ? DataSerializer.boolToSave = false : DataSerializer.boolToSave = true;
		//}

		GUI.Label(new Rect(375, 0, 125, 50), "Integer value is " + ProgressionData.nbEnemiesKilled);
        //GUI.Label(new Rect(375, 100, 125, 50), "Float value is " + DataSerializer.floatToSave.ToString("F1"));
        //GUI.Label(new Rect(375, 200, 125, 50), "Bool value is " + DataSerializer.boolToSave);
    }
}
