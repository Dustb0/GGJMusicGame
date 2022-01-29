using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerComponent : MonoBehaviour
{
    public MicrophoneInput input = new MicrophoneInput();
    public int sampleCount = 256;

    private float pitch_new;

    public float pitch { get; private set; }
    public float pitchHeight { get; private set; }
    public float pitchTangent { get; private set; }

    public float volume { get; private set; }
    public float volumeHeight { get; private set; }
    public float volumeTangent { get; private set; }

    public Vector2 inputVolumeRange;
    public Vector2 inputPitchRange;
    public Vector2 outputHeightRange;



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

        float pitchheightLast = Unity.Mathematics.math.remap(inputPitchRange.x, inputPitchRange.y, outputHeightRange.x, outputHeightRange.y, pitch);
        pitch_new = MicrophoneInput.GetPitch(sampleCount);
        pitch = pitch_new - ((pitch_new - pitch) / 1.1f);
        pitchHeight = Unity.Mathematics.math.remap(inputPitchRange.x, inputPitchRange.y, outputHeightRange.x, outputHeightRange.y, pitch);
        pitchTangent = pitchHeight - pitchheightLast;

        float volumeheightLast = Unity.Mathematics.math.remap(inputVolumeRange.x, inputVolumeRange.y, outputHeightRange.x, outputHeightRange.y, volume);
        volume = MicrophoneInput.GetAverageVolume(sampleCount);
        volumeHeight = Unity.Mathematics.math.remap(inputVolumeRange.x, inputVolumeRange.y, outputHeightRange.x, outputHeightRange.y, volume);
        volumeTangent = volumeHeight - volumeheightLast;

        Debug.Log(pitch);
    }
}
