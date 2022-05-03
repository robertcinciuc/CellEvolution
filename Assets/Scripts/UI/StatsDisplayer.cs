using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsDisplayer : MonoBehaviour
{
    private void OnGUI() {
		GUI.Label(new Rect(0, 0, 200, 50), "Number of enemies killed: " + ProgressionData.nbEnemiesKilled);
        GUI.Label(new Rect(0, 100, 200, 50), "Number of meats eaten: " + ProgressionData.nbMeatsEaten);
    }
}
