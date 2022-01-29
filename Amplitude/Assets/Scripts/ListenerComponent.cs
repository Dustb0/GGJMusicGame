using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerComponent : MonoBehaviour
{
    public MicrophoneInput input = new MicrophoneInput();

    // Start is called before the first frame update
    void Start()
    {
        MicrophoneInput.InitDeviceList();
        input.OnInitialize();

        Debug.Log(MicrophoneInput.CurrentDeviceName);

        input.StartRecording();
    }

    // Update is called once per frame
    void Update()
    {
        input.Update();

        float volume = input.GetAverageVolume();
        float pitch = input.GetPitch();

        Debug.Log($"Volume: {volume}   Pitch: {pitch}");
    }
}
