using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VfxHandler : MonoBehaviour
{
    // The Vfx Handler has references to the Visual Effect Prefabs
    // It needs to use UnityEngine.VFX to accept Visual Effects
    public VisualEffect snowEffect;
    public VisualEffect rainEffect;
    // A reference to the Terrain to be able to get desired positions
    private TerrainHandler terra;


    void Start()
    {
        // Find the TerrainHandler
        terra = GameObject.Find("TerrainHandler").GetComponent<TerrainHandler>();
        SetSnow();
    }

    private void SetSnow()
    {
        List<Vector3> peaks = terra.GetMountainPeaks();
        for( int i = 0; i < peaks.Count; i++)
        {
            Debug.Log("we reach the vfx setter");
            Vector3 _currentPos = peaks[i];
            // Instantiate the relevant Vfx
            VisualEffect _newEffect = Instantiate(snowEffect, _currentPos , Quaternion.identity);
            // Setting the Vfx Position via the exposed variables of the Visual Effect
            _newEffect.SetVector3("Position", _currentPos);
        }
    }


}
