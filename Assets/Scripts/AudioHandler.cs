using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    // The Audio Handler has references to the Audio Files Prefabs
    public AudioSource birds;
    public AudioSource rain;
    public AudioSource sea;
    public AudioSource wind;
    // A reference to the Terrain to be able to get desired positions
    private TerrainHandler terra;
    // A Gameobject with an audiosource to clone and place on the map


    void Start()
    {
        // Find the TerrainHandler
        terra = GameObject.Find("TerrainHandler").GetComponent<TerrainHandler>();
    }

    private void SetBirds()
    {
        Vector3 _newSpot = terra.GetRandomPosition();
        AudioSource _newSource = Instantiate(birds, _newSpot, Quaternion.identity).GetComponent<AudioSource>();
        _newSource.Play(); 
    }


}