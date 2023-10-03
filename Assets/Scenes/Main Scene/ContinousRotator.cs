using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinousRotator : MonoBehaviour
{
    public float turnRate = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles +=  new Vector3(0, turnRate * Time.deltaTime, 0);
        
    }
}
