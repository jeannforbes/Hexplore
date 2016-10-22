using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WeatherScript : MonoBehaviour {
    public enum weatherType
    {
        clear,
        overcast,
        light_snow,
        heavy_snow,
        light_rain,
        heavy_rain
    }

    public Material clearSky, overcastSky;

    public GameObject lightSnow, heavySnow,lightRain,heavyRain;

    private float timeSinceLastUpdate = 0;
    private float timeBetweenUpdates = 10;
    private float nextRandom;

    private weatherType weather = weatherType.clear;
    //private Camera cCamera;
    //private Material skybox;

    private GameObject player;
    private Vector3 weatherVector;
    private Dictionary<weatherType, GameObject> weatherRef;
    private GameObject precipGO;

    // Use this for initialization
    void Start() {
        //skybox = RenderSettings.skybox;

        player = GameObject.FindGameObjectWithTag("Player");
        Random.seed = (int)Time.time;

        weatherRef = new Dictionary<global::WeatherScript.weatherType, GameObject>();
        weatherRef[weatherType.light_snow] = lightSnow;
        weatherRef[weatherType.heavy_snow] = heavySnow;
        weatherRef[weatherType.light_rain] = lightRain;
        weatherRef[weatherType.heavy_rain] = heavyRain;
    }
	
	// Update is called once per frame
	void Update () {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        weatherVector = player.transform.position;
        weatherVector.y = 30f;
        this.gameObject.transform.position = weatherVector;

        timeSinceLastUpdate += Time.deltaTime;

        if(timeSinceLastUpdate >= timeBetweenUpdates)
        {
            //print("Type: " + weather);
            timeSinceLastUpdate = 0;

            if(Random.Range(0,1.0f) > 0.7)
            {
                //print("weather changing");
                nextRandom = Random.Range(0, 1.0f);
                switch (weather)
                {
                    case weatherType.clear:
                        setWeather(weatherType.overcast);
                        break;
                    case weatherType.overcast:
                        if (nextRandom <= 0.4f)
                        {
                            setWeather(weatherType.clear);
                        }
                        else if(nextRandom <= 0.7f)
                        {
                            setWeather(weatherType.light_snow);
                        }
                        else
                        {
                            setWeather(weatherType.light_rain);
                        }
                        break;
                    case weatherType.light_snow:
                        if(nextRandom <= 0.7f)
                        {
                            setWeather(weatherType.overcast);
                        }
                        else
                        {
                            setWeather(weatherType.heavy_snow);
                        }
                        break;
                    case weatherType.heavy_snow:
                        setWeather(weatherType.light_snow);
                        break;
                    case weatherType.light_rain:
                        if (nextRandom <= 0.7f)
                        {
                            setWeather(weatherType.overcast);
                        }
                        else
                        {
                            setWeather(weatherType.heavy_rain);
                        }
                        break;
                    case weatherType.heavy_rain:
                        setWeather(weatherType.light_rain);
                        break;
                    default:
                        setWeather(weatherType.clear);
                        break;
                }
            }
        }
	}

    public void setWeather(weatherType type)
    {
        //print("New type: " + type);
        weather = type;

        //removes all children
        //children destruction method from http://answers.unity3d.com/questions/611850/destroy-all-children-of-object.html
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if(weather != weatherType.clear && weather != weatherType.overcast)
        {
            precipGO = Instantiate(weatherRef[weather]);
            precipGO.transform.SetParent(this.transform);
            precipGO.transform.localPosition = Vector3.zero;
        }
        else
        {
            if(weather == weatherType.clear)
            {
                RenderSettings.skybox = clearSky;
            }else
            {
                RenderSettings.skybox = overcastSky;
            }
        }
    }

}
