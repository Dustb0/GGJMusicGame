using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PitchDetector;

public class PlayerController : MonoBehaviour
{
    public VoiceMeshGenerator voiceMeshGenerator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            voiceMeshGenerator.ResetShape();
            voiceMeshGenerator.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            voiceMeshGenerator.StartGenerating();
        } else if (Input.GetKeyUp(KeyCode.Q))
        {
            voiceMeshGenerator.StopGenerating();
        }
    }
}
