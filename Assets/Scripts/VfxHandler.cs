using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VfxHandler : MonoBehaviour
{
    public VisualEffect snowEffect;
    public VisualEffect rainEffect;
    private TerrainHandler terra;
    // Start is called before the first frame update
    void Start()
    {
        terra = GameObject.Find("TerrainHandler").GetComponent<TerrainHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
