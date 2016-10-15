using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager: MonoBehaviour
{
	public GameObject HexGrass, HexMud, HexSand, HexWater;
	public GameObject player;
	public float viewDistance = 15f;
	
	//Hexagon tile width and height in game world
	private float hexWidth;
	private float hexHeight;
	private int mudChance = 1, waterChance = 3, grassChance = 7, sandChance = 8;

	private int gridWidthInHexes = 5;
	private int gridHeightInHexes = 5;
	private int leftWidthInHexes = 0;
	private int topHeightInHexes = 0;

	//Holds the hexagons
	private List<Grid> grids;
	private Grid currentGrid;
	private GameObject hexGridGO;
	
	//Method to initialise Hexagon width and height
/*	void setSizes()
	{
		//renderer component attached to the Hex prefab is used to get the current width and height
		//	plus some margin for aesthetics
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
	private Vector3 calcWorldCoord(Vector2 gridPos)
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
		Grid grid = new Grid(xOrigin, yOrigin);
		
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
				//hex.GetComponent<Renderer>().enabled = false;
				grid.hexes.Add(hex);
			}
		}
		return grid;
	}

	void extendGrid(Grid grid, string direction){
		//Debug.Log (direction);
		switch (direction) {
		case"left":
			leftWidthInHexes++;
			for (float z = -topHeightInHexes; z < gridHeightInHexes; z++)
			{
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
				Vector2 gridPos = new Vector2(-leftWidthInHexes, z);
				hex.transform.position = calcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;
				grid.hexes.Add(hex);
			}
			break;
		case "right":
			gridWidthInHexes++;
			for (float z = -topHeightInHexes; z < gridHeightInHexes; z++)
			{
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
				Vector2 gridPos = new Vector2(gridWidthInHexes, z);
				hex.transform.position = calcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;
				grid.hexes.Add(hex);
			}
			break;
		case "up":
			topHeightInHexes++;
			for (float x = -leftWidthInHexes; x < gridWidthInHexes; x ++)
			{
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
				Vector2 gridPos = new Vector2(x, -topHeightInHexes);
				hex.transform.position = calcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;
				grid.hexes.Add(hex);
			}
			break;
		case "down":
			gridHeightInHexes++;
			for (float x = -leftWidthInHexes; x < gridWidthInHexes; x ++)
			{
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
				Vector2 gridPos = new Vector2(x, gridHeightInHexes);
				hex.transform.position = calcWorldCoord(gridPos);
				hex.transform.parent = hexGridGO.transform;
				grid.hexes.Add(hex);
			}
			break;
		}
	}
	
	//Grid is generated when game starts
	void Start()
	{
		Random.seed = 42;
		setSizes();
		//Game object which is the parent of all the hex tiles
		hexGridGO = new GameObject("HexGrid");
		grids.Add (createGrid (0,0));
	}

	void Update(){
		//Get the player
		player = GameObject.FindGameObjectWithTag ("Player");
		//Check if any hexes need to make themselves visible
		/*foreach (Grid grid in grids) {
			foreach (GameObject hex in grid.hexes) {
				if ( (Mathf.Sqrt (Mathf.Pow (hex.transform.position.x - player.transform.position.x, 2f) 
					+ Mathf.Pow (hex.transform.position.z - player.transform.position.z, 2f)) < viewDistance) && !hex.GetComponent<Renderer>().enabled) {
					hex.GetComponent<Renderer> ().enabled = true;
				}
				if( hex.transform.position.y < 0 && hex.GetComponent<Renderer>().enabled){
					hex.transform.Translate(0, 30 * Time.deltaTime,0);
				}
			}
		}*/

		//if(player.transform.position.x > calcWorldCoord(new Vector2(gridWidthInHexes, 0)).x) extendGrid (grids[0],"right");
		//if(player.transform.position.x < calcWorldCoord(new Vector2(-leftWidthInHexes, 0)).x) extendGrid (grids[0],"left");
		//Debug.Log (player.transform.position.z + "," + calcWorldCoord (new Vector2 (0, gridHeightInHexes)).z);
		//if(player.transform.position.z < calcWorldCoord(new Vector2(0, gridHeightInHexes)).z) extendGrid (grids[0],"down");
		//if(player.transform.position.z < calcWorldCoord(new Vector2(0, -topHeightInHexes)).z) extendGrid (grids[0],"up");
	//}
}