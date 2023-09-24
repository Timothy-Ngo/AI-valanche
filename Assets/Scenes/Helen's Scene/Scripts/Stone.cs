using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 endPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(startPosition.x + 2, startPosition.y, startPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        

        //transform.position = MathParabola.Parabola(transform.position, endPosition, 
    }
}
