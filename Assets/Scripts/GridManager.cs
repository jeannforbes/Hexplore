using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct Grid{
	public float x, z;
	public List<GameObject> hexes;
	public Grid(float x, float z){
		this.x = x;
		this.z = z;
		hexes = new List<GameObject>();
	}
}

public class GridManager: MonoBehaviour
{
	public GameObject HexGrass, HexMud, HexSand, HexWater;
	public GameObject player;
	public int gridWidthInHexes = 5;
	public int gridHeightInHexes = 5;
	public float viewDistance = 15f;
	
	//Hexagon tile width and height in game world
	private float hexWidth;
	private float hexHeight;
	private int mudChance = 1, waterChance = 3, grassChance = 7, sandChance = 8;

	//Holds the hexagons
	private List<Grid> grids;
	
	//Method to initialise Hexagon width and height
	void setSizes()
	{
		//renderer component attached to the Hex prefab is used to get the current width and height
		hexWidth = HexGrass.GetComponent<Renderer>().bounds.size.x+0.5f;
		hexHeight = HexGrass.GetComponent<Renderer>().bounds.size.z+0.5f;

		grids = new List<Grid> ();
	}
	
	//Method to calculate the position of the first hexagon tile
	//The center of the hex grid is (0,0,0)
	Vector3 calcInitPos()
	{
		Vector3 initPos;
		//the initial position will be in the left upper corner
		initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2,
		                      0,
		                      gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
		
		return initPos;
	}
	
	//method used to convert hex grid coordinates to game world coordinates
	public Vector3 calcWorldCoord(Vector2 gridPos)
	{
		//Position of the first hex tile
		Vector3 initPos = calcInitPos();
		//Every second row is offset by half of the tile width
		float offset = 0;
		if (gridPos.y % 2 != 0)
			offset = hexWidth / 2;
		
		float x =  initPos.x + offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = initPos.z - gridPos.y * hexHeight * 0.75f;
		float y = -10 * Mathf.PerlinNoise (x, z);
		return new Vector3(x, y, z);
	}
	
	//Initialize and positions all the tiles
	Grid createGrid(float xOrigin, float yOrigin)
	{
		Debug.Log ("Making grid");
		//Game object which is the parent of all the hex tiles
		GameObject hexGridGO = new GameObject("HexGrid");

		Grid grid = new Grid(player.transform.position.x % (gridWidthInHexes*hexWidth) * (gridWidthInHexes*hexWidth),
		                     player.transform.position.z % (gridHeightInHexes*hexHeight) * (gridHeightInHexes*hexHeight));
		
		for (float y = yOrigin; y < gridHeightInHexes; y++)
		{
			for (float x = xOrigin; x < gridWidthInHexes; x++)
			{
				//GameObject assigned to Hex public variable is cloned
				GameObject hex = null;
				int r = Random.Range (0,10);
				if( r <= mudChance){
					hex = (GameObject)Instantiate (HexMud);
				}else if( r <= waterChance){
					hex = (GameObject)Instantiate (HexWater);
				}else if( r <= grassChance){
					hex = (GameObject)Instantiate (HexGrass);
				}else{// if(r <= sandChance){
					hex = (GameObject)Instantiate (HexSand);
				} //else {return grid;}
				//Current position in grid
				Vector2 gridPos = new Vector2(x, y);
				hex.transform.position = calcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;
				hex.GetComponent<Renderer>().enabled = false;
				grid.hexes.Add(hex);
			}
		}
		return grid;
	}
	
	//Grid is generated when game starts
	void Start()
	{
		setSizes();
		grids.Add (createGrid (0,0));
	}

	void Update(){
		//Get the player
		player = GameObject.FindGameObjectWithTag ("Player");
		//Check if any hexes need to make themselves visible
		foreach (Grid grid in grids) {
			foreach (GameObject hex in grid.hexes) {
				if ( (Mathf.Sqrt (Mathf.Pow (hex.transform.position.x - player.transform.position.x, 2f) 
					+ Mathf.Pow (hex.transform.position.z - player.transform.position.z, 2f)) < viewDistance) && !hex.GetComponent<Renderer>().enabled) {
					hex.GetComponent<Renderer> ().enabled = true;
				}
				if( hex.transform.position.y < 0 && hex.GetComponent<Renderer>().enabled){
					hex.transform.Translate(0, 30 * Time.deltaTime,0);
				}
			}
		}
		//Check if we need to make another grid of hexes -- WIP!
		if (player.transform.position.x >= (gridWidthInHexes * hexWidth)) {
			//need to determine where to create the new grid, in relation to player's current position
			//grids.Add(createGrid ());
		}
	}
}