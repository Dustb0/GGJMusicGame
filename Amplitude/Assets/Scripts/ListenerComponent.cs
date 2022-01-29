using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerComponent : MonoBehaviour
{
    public MicrophoneInput input = new MicrophoneInput();

    // Start is called before the first frame update
    void Start()
    {
        input.Initialize();

        Debug.Log(MicrophoneInput.CurrentDeviceName);

        input.StartRecording();
    }

    // Update is called once per frame
    void Update()
    {
        input.Update();

        float volume = MicrophoneInput.GetAverageVolume();
        float pitch = MicrophoneInput.GetPitch();

        Debug.Log($"{MicrophoneInput.CurrentDeviceName} Volume: {volume}   Pitch: {pitch}");
    }
}
