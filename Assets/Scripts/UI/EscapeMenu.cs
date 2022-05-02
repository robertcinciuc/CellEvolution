using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    private bool inGame = true;
    void Start(){
        
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)) {
            inGame = !inGame;
        }
    }

    private void OnGUI() {
        if (!inGame) {
            if (GUI.Button(new Rect(0, 300, 125, 50), "Save Your Game")) {
                DataSerializer.SaveGame();
            }
            if (GUI.Button(new Rect(0, 400, 125, 50), "Load Your Game")) {
                DataSerializer.LoadGame();
            }
            if (GUI.Button(new Rect(0, 500, 125, 50), "Reset Save Data")) {
                DataSerializer.ResetData();
            }
        }
    }


}
