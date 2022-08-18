using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTimeExtended : MonoBehaviour
{
    public float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy (gameObject, lifeTime);
        GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1.25f);
    }
}
