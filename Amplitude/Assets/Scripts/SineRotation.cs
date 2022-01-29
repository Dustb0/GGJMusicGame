using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineRotation : MonoBehaviour
{

    public float sineSpeed;
    public float sineAngle;
    // Start is called before the first frame update

    private float time;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * sineSpeed;
        transform.Rotate(0f, 0f, Mathf.Sin(time) * sineAngle);
    }
}
