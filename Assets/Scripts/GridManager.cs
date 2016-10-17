using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Grid{
	public float x, z;
	public List<Hex> hexes;
	public Grid(float x, float z){
		this.x = x;
		this.z = z;
		hexes = new List<Hex>();
	}
}

public struct Hex{
	public Vector2 gridPos; //position in grid coords
	public GameObject go; //the hex's prefab
	public GameObject topDecor; //the top asset's prefab
	public GameObject botDecor; //the bottom asset's prefab
	public bool canBounce;
	public Hex(GameObject go, Vector2 gridPos){
		this.gridPos = gridPos;
		this.go = go;
		this.topDecor = null;
		this.botDecor = null;
		this.canBounce = true;
	}
}

enum Biome{ normal, fall, winter };

public class GridManager : MonoBehaviour {

	public GameObject grass0, grass1, grass2, fallGrass0, fallGrass1, fallGrass2;
	public GameObject mud, ice, snow, water;
	public GameObject cloud , decay;
	public GameObject tree0, tree1, tree2, fallTree0, fallTree1, fallTree2;

	private GameObject Player;
	public GameObject hexGridGO;
	public Grid grid, gridClouds;
	private Hex hexGrass;

	private float hexWidth;
	private float hexHeight;
	private float MARGIN = 0.2f;

	// Use this for initialization
	void Start () {
		Random.seed = ((int)System.DateTime.Now.Millisecond);

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

			Biome hexBiome = getBiome(worldPos.y);
			GameObject type = grass0;
			Hex hex = new Hex();
			if(hexBiome == Biome.normal){
				if(worldPos.y < -9){
					type = water;
				} else if(worldPos.y < -7){
					type = mud;
				} else{
					if( Random.Range (0,10) <= 3) type = grass1;
					else if( Random.Range (0,10) <= 3) type = grass2;
				}
				hex = new Hex (type, gridPos);
				//If grass, choose top decoration
				if(type == grass0 || type == grass1 || type == grass2){
					switch ((int)Random.Range (0,15))
					{
						case 1:
							hex.topDecor = (GameObject)Instantiate (tree0);
							break;
						case 2:
							hex.topDecor = (GameObject)Instantiate (tree1);
							break;
						case 3:
							hex.topDecor = (GameObject)Instantiate (tree2);
							break;
						default: break;
					}
				}
			}
			else if(hexBiome == Biome.fall){
				if(worldPos.y < -10){
					type = ice;
				} else if(worldPos.y < -9){
					type = mud;
				} else{
					if( Random.Range (0,10) <= 3) type = fallGrass0;
					else if( Random.Range (0,10) <= 3) type = fallGrass1;
					else type = fallGrass2;
				}
				hex = new Hex (type, gridPos);
				//If grass, choose top decoration
				if(type == fallGrass0 || type == fallGrass1 || type == fallGrass2){
					switch ((int)Random.Range (0,15))
					{
					case 1:
						hex.topDecor = (GameObject)Instantiate (fallTree0);
						break;
					case 2:
						hex.topDecor = (GameObject)Instantiate (fallTree1);
						break;
					case 3:
						hex.topDecor = (GameObject)Instantiate (fallTree2);
						break;
					default: break;
					}
				}
			} else if(hexBiome == Biome.winter){
				if(worldPos.y < -10)
					type = ice;
				else
					type = snow;
				hex = new Hex (type, gridPos);
			}
			
			hex.go = (GameObject)Instantiate (type);
			
			grid.hexes.Add (hex);
			hex.go.transform.position = worldPos;
			hex.go.transform.parent = hexGridGO.transform;
			hex.go.GetComponent<Renderer>().enabled = false;

			//Hide decor
			if(hex.topDecor){
				hex.topDecor.transform.position = new Vector3(worldPos.x, worldPos.y+5, worldPos.z);
				hex.topDecor.transform.Rotate( Vector3.up * Random.Range (0,360));
				hex.topDecor.transform.parent = hex.go.transform;
				foreach( Renderer hd in hex.topDecor.GetComponentsInChildren<Renderer>())
					hd.enabled = false;
			}
			if(hex.botDecor){
				hex.botDecor.transform.position = new Vector3(worldPos.x, worldPos.y-5, worldPos.z);
				hex.botDecor.transform.Rotate( Vector3.up * Random.Range (0,360));
				hex.botDecor.transform.Rotate (new Vector3(0,0,180f));
				hex.botDecor.transform.parent = hex.go.transform;
				foreach( Renderer hd in hex.botDecor.GetComponentsInChildren<Renderer>())
					hd.enabled = false;
			}
		}

		//Spread the infection!
		//spreadDecay ();
	}

	void makeVisible(){
		GameObject hex;
		for (int i=0; i<grid.hexes.Count; i++) {
			hex = grid.hexes[i].go;
			if(!hex) return;
			if ( (Mathf.Sqrt (Mathf.Pow (hex.transform.position.x - Player.transform.position.x, 2f) 
			                  + Mathf.Pow (hex.transform.position.z - Player.transform.position.z, 2f)) < 20f) && !hex.GetComponent<Renderer>().enabled) {
				hex.GetComponent<Renderer> ().enabled = true;

				//Reveal decor
				if(grid.hexes[i].topDecor){
					foreach( Renderer hd in grid.hexes[i].topDecor.GetComponentsInChildren<Renderer>())
						hd.enabled = true;
				}
				if(grid.hexes[i].botDecor){
					foreach( Renderer hd in grid.hexes[i].botDecor.GetComponentsInChildren<Renderer>())
						hd.enabled = true;
				}
			}
			if( hex.transform.position.y < (-hexHeight*2 *  Mathf.PerlinNoise (hex.transform.position.x, hex.transform.position.z)+10f) && hex.GetComponent<Renderer>().enabled){
				hex.transform.Translate(0, 30 * Time.deltaTime,0);
			}
		}
	}

	//method used to convert hex grid coordinates to game world coordinates
	public Vector3 calcWorldCoord(Vector2 gridPos)
	{
		//Every second row is offset by half of the tile width
		float offset = 0;
		if (gridPos.y % 2 != 0)
			offset = hexWidth / 2;
		
		float x =  offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float z = gridPos.y * hexHeight * 0.75f;
		float y = -hexHeight*2 *  Mathf.PerlinNoise (x,z);
		return new Vector3(x, y, z);
	}

	//Method to initialise Hexagon width and height
	void setSizes()
	{
		//renderer component attached to the Hex prefab is used to get the current width and height
		//	plus some margin for aesthetics
		hexWidth = grass0.GetComponent<Renderer>().bounds.size.x + MARGIN;
		hexHeight = grass0.GetComponent<Renderer>().bounds.size.z + MARGIN;
	}

	void spreadDecay(){
		if (Random.Range (0, 1000) < 2) {
			Hex decayHex = grid.hexes [Random.Range (0, grid.hexes.Count)];
			if(decayHex.go){
				GameObject oldGO = decayHex.go;
				decayHex.go = (GameObject)Instantiate (decay, decayHex.go.transform.position, decayHex.go.transform.rotation);
				Destroy (oldGO);
			}
		}
	}

	Biome getBiome(float height){
		if(Mathf.PerlinNoise (Time.timeSinceLevelLoad*0.1f , 1f) < 0.5f)
			return Biome.normal;
		else if( Mathf.PerlinNoise (Time.timeSinceLevelLoad*0.1f , 1f) < 0.6f)
			return Biome.fall;
		return Biome.winter;
	}

	public static bool equalish(float n1, float n2, float margin){
		if (n1 > (n2 - margin) && n1 < (n2 + margin) )
			return true;
		return false;
	}
}