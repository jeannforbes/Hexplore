using UnityEngine;
using System.Collections;

public enum HexType
{
    Enemy,
    Treasure,
    Normal
}
public class HexInfo : MonoBehaviour {
    private bool unvisited = true;
    public HexType type;

    //Gets whether this hex has been visited before.
    public bool Unvisited
    {
        get
        {
            return unvisited;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //updates the hex, when the player 'visits' it
    public void visit()
    {
        unvisited = false;
    }
}
