using UnityEngine;
using System.Collections;

public class DisplayGUI : MonoBehaviour {
    private Rect textRect;
    private string healthScore = "";
    private PlayerMovement playerMovement;

    public GameObject player;
    

	// Use this for initialization
	void Start () {
        textRect = new Rect(0.0f, 0.0f, 100.0f, 40.0f);
        playerMovement = (PlayerMovement)player.GetComponent("PlayerMovement");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //OnGUI is called once per frame to display the GUI
    void OnGUI()
    {
        healthScore = "Health: " + playerMovement.Health + "\nScore : " + playerMovement.Score;

        GUI.TextArea(textRect, healthScore);
    }
}
