using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Checkpoint : MonoBehaviour
{
    public CinemachineVirtualCamera camera;
    public GameObject cameraTarget;
    public float transitionTime = 1f;

    private float time = 0f;
    private Vector3 from;
    private Vector3 to;
    private bool doTransition = false;

    // Start is called before the first frame update
    void Start()
    {
        //camera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //camera.ForceCameraPosition(cameraTarget.transform.position, Quaternion.identity);
        from = camera.transform.position;
        to = cameraTarget.transform.position;
        doTransition = true;
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (doTransition)
        {
            Debug.Log(time);
            time += Time.deltaTime;
            float alpha = time / transitionTime;
            alpha = 1f-Mathf.Pow(1f - alpha, 2f);
            
            if (time > transitionTime)
            {
                camera.ForceCameraPosition(new Vector3(Mathf.Lerp(from.x, to.x, transitionTime), Mathf.Lerp(from.y, to.y, transitionTime), from.z), Quaternion.identity);
                doTransition = false;
            }
            else
            {
                camera.ForceCameraPosition(new Vector3(Mathf.Lerp(from.x, to.x, alpha), Mathf.Lerp(from.y, to.y, alpha), from.z), Quaternion.identity);
            }
        }
    }
}
