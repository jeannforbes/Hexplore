using UnityEngine;
using System.Collections;


public class DisplayGUI : MonoBehaviour {
    private Rect textRect;
    private string guiContent = "";
    private PlayerMovement playerMovement = null;

	// Use this for initialization
	void Start () {
        textRect = new Rect(0.0f, 0.0f, 100.0f, 40.0f);
        playerMovement = (PlayerMovement)(GameObject.FindWithTag("Player").GetComponent("PlayerMovement"));
    }
	
	// Update is called once per frame
	void Update () {
       if(playerMovement != null)
        {
            guiContent = playerMovement.ToString();
        }
    }

   
    //OnGUI is called once per frame to display the GUI
    void OnGUI()
    {
        GUI.TextArea(textRect, guiContent);
    }

}
