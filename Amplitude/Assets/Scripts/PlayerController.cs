using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PitchDetector;

public class PlayerController : MonoBehaviour
{
    public VoiceMeshGenerator voiceMeshGenerator;
    public ListenerComponent listener;
    public float direction = 1f;
    public Vector3 singingOffset;



    public float recordingDelay;
    private float time;
    private bool delayedStartGenerating;

    private bool canSing = true;
    private bool isLookingRight = false;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            canSing = true;
        }
        else
        {
            canSing = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        listener.OnStartSilence.AddListener(StopGenerating);
        listener.OnStopSilence.AddListener(StartGeneratingDelayed);
    }

    private void StopGenerating()
    {
        if (voiceMeshGenerator.isGenerating)
        {
            voiceMeshGenerator.StopGenerating();
        }
    }

    private void StartGenerating()
    {
        if (!voiceMeshGenerator.isGenerating && canSing)
        {
            voiceMeshGenerator.ResetShape();
            voiceMeshGenerator.transform.localScale = new Vector3(isLookingRight ? 1f : -1f, 1f, 1f);
            voiceMeshGenerator.transform.SetPositionAndRotation(transform.position + new Vector3(direction * singingOffset.x, singingOffset.y - listener.pitchHeightSmoothed, singingOffset.z), Quaternion.identity);
            voiceMeshGenerator.StartGenerating();
            canSing = false;
        }
    }

    private void StartGeneratingDelayed()
    {
        time = 0f;
        delayedStartGenerating = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(delayedStartGenerating)
        {
            time += Time.deltaTime;
            if(time > recordingDelay)
            {
                Debug.Log(isLookingRight);
                delayedStartGenerating = false;
                StartGenerating();
            }
        }

        if (Input.GetAxis("Horizontal") < -0.01f && isLookingRight)
        {
            isLookingRight = false;
            this.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (Input.GetAxis("Horizontal") > 0.01f && !isLookingRight)
        {
            isLookingRight = true;
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        /*
        if(Input.GetKeyDown(KeyCode.Q))
        {
            StartGenerating();
        } else if (Input.GetKeyUp(KeyCode.Q))
        {
            StopGenerating();
        }
        */
    }
}
