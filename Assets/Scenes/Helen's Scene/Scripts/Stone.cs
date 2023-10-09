using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {            
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("stoneSound"))
            audio.Play();
    }
}
