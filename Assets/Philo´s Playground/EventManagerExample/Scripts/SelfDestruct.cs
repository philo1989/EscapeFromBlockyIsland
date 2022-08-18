using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class SelfDestruct : MonoBehaviour
{
    public GameObject explosion;

    private float shake = 0.2f;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("Destroy", Destroy);
    } 
    
    private void OnDisable()
    {
        EventManager.StopListening("Destroy", Destroy);
    }
    
    private void Destroy()
    {
        EventManager.StopListening("Destroy", Destroy);
        StartCoroutine(DestroyNow());
    }

    IEnumerator DestroyNow()
    {
        yield return new WaitForSeconds (Random.Range (0.0f, 1.0f));
        audioSource.pitch = Random.Range (0.75f, 1.75f);
        audioSource.Play();
        float startTime = 0;
        float shakeTime = Random.Range(1.0f, 3.0f);
        while(startTime < shakeTime)
        {
            transform.Translate(Random.Range(-shake, shake), 0, Random.Range(-shake, shake));
            transform.Rotate(0.0f, Random.Range(-shake * 100, shake * 100), 0.0f);
            startTime += Time.deltaTime;
            yield return null;
        }

        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
