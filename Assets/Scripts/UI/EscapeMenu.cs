using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    public DataSerializer dataSerializer;

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
                dataSerializer.SaveGame();
            }
            if (GUI.Button(new Rect(0, 400, 125, 50), "Load Your Game")) {
                dataSerializer.LoadGame();
            }
            if (GUI.Button(new Rect(0, 500, 125, 50), "Reset Save Data")) {
                dataSerializer.ResetData();
            }
        }
    }


}
