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

	
}
