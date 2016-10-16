using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Grid{
	public float x, z;
	public int obeliskCount;
	public List<Hex> hexes;
	public Grid(float x, float z){
		this.x = x;
		this.z = z;
		hexes = new List<Hex>();
		this.obeliskCount = 0;
	}
}

public struct Hex{
	public Vector2 gridPos;
	public GameObject go;
	public Hex(GameObject go, Vector2 gridPos){
		this.gridPos = gridPos;
		this.go = go;
	}
}

public class GridGenerator : MonoBehaviour {

	public GameObject grass, dirt, sand, water, obelisk;

	private GameObject Player;
	private GameObject hexGridGO;
	public Grid grid;
	private Hex hexGrass;

	private float hexWidth;
	private float hexHeight;
	private float MARGIN = 0.2f;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
		hexGridGO = new GameObject("HexGrid");
		grid = new Grid (0,0);
		setSizes ();

		hexGrass = new Hex ();
	}
	
	// Update is called once per frame
	void Update () {

		//See if we need to generate a hex
		int x = (int)( (Player.transform.position.x / hexWidth) );
		int z = (int)( (Player.transform.position.z / hexHeight) *1.5f);
		bool needHex = true;

		Vector2 h;
		for (int i=-3; i<=3; i++) {
			for(int k=-3; k<=3; k++){
				makeHex (new Vector2(x+i, z+k));
			}
		}
		makeVisible ();
	}

	void makeHex(Vector2 gridPos){
		bool needHex = true;
		for(int i=0; i<grid.hexes.Count; i++) {
			Vector2 h = grid.hexes[i].gridPos;
			if( gridPos.x == h.x && gridPos.y == h.y ) needHex = false;
		}
		if (needHex) {
			Vector3 worldPos = calcWorldCoord(gridPos);
			GameObject type = getType(worldPos.y);

			Hex hex = new Hex (type, gridPos);
			hex.go = (GameObject)Instantiate (type);
			grid.hexes.Add (hex);
			hex.go.transform.position = worldPos;
			hex.go.transform.parent = hexGridGO.transform;
			hex.go.GetComponent<Renderer>().enabled = false;
		}
	}

	void makeVisible(){
		GameObject hex;
		for (int i=0; i<grid.hexes.Count; i++) {
			hex = grid.hexes[i].go;
			if ( (Mathf.Sqrt (Mathf.Pow (hex.transform.position.x - Player.transform.position.x, 2f) 
			                  + Mathf.Pow (hex.transform.position.z - Player.transform.position.z, 2f)) < 15f) && !hex.GetComponent<Renderer>().enabled) {
				hex.GetComponent<Renderer> ().enabled = true;
			}
			if( hex.transform.position.y < (-hexHeight*2 *  Mathf.PerlinNoise (hex.transform.position.x, hex.transform.position.z)+10f) && hex.GetComponent<Renderer>().enabled){
				hex.transform.Translate(0, 30 * Time.deltaTime,0);
			}
		}
	}

	//method used to convert hex grid coordinates to game world coordinates
	private Vector3 calcWorldCoord(Vector2 gridPos)
	{
		//Every second row is offset by half of the tile width
		float offset = 0;
		if (gridPos.y % 2 != 0)
			offset = hexWidth / 2;
		
		float x =  offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = gridPos.y * hexHeight * 0.75f;
		float y = -hexHeight*2 *  Mathf.PerlinNoise (x, z);
		return new Vector3(x, y, z);
	}

	//Method to initialise Hexagon width and height
	void setSizes()
	{
		//renderer component attached to the Hex prefab is used to get the current width and height
		//	plus some margin for aesthetics
		hexWidth = grass.GetComponent<Renderer>().bounds.size.x + MARGIN;
		hexHeight = grass.GetComponent<Renderer>().bounds.size.z + MARGIN;
	}

	GameObject getType(float height){
		if (height < -10)
			return water;
		else if (height < -7) {
			if (Random.Range (0, 10) < 5)
				return sand;
			else
				return dirt;
		} else if (Random.Range (0,10) < 2) {
			if(grid.obeliskCount <= 3){
				grid.obeliskCount++;
				return obelisk;
			}
		}
		return grass;
	}

	bool equalish(float n1, float n2, float margin){
		if (n1 > (n2 - margin) && n1 < (n2 + margin) )
			return true;
		return false;
	}
}