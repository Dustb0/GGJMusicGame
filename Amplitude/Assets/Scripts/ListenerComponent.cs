using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PitchDetector;
using UnityEngine.Events;

public class ListenerComponent : MonoBehaviour
{
    private float pitch_new;

    public float pitch { get; private set; }
    public float pitchHeight { get; private set; }

    public float pitchHeightSmoothed { get; private set; }
    public float pitchTangent { get; private set; }

    public float volume { get; private set; }
    public float volumeSmoothed { get; private set; }

    public int midiMax;
    public int midiMin;

    public GameObject objectToPlot;
    public float transpose;
    public MicrophonePitchDetector detector;
    public float dbSensitivityThreshold;

    public float pitchSmoothing = 2.0f;

    public bool isSilent { get; private set; }
    public float timeToSilence = 0.1f;
    public bool waitForSpaceBeforeStart;
    public UnityEvent OnStartSilence;
    public UnityEvent OnStopSilence;

    private float timeSilent = 0f;
    private float timeNotSilent;

    private bool isRecording;

    private Queue<PitchTime> drawQueue = new Queue<PitchTime>();
    private float appTime = 0f;
    private float analysisTime = 0f;

    // Class that holds pitch value and its duration
    private class PitchTime
    {
        public PitchTime(float pitch, float dt, float db)
        {
            this.pitch = pitch;
            this.dt = dt;
            this.db = Mathf.Max(-db, 1f);
        }
        public float pitch;
        public float dt;
        public float db;
    };

    public float MidiToScreenY(float midiVal)
    {
        if (float.IsInfinity(midiVal))
        {
            midiVal = RAPTPitchDetectorExtensions.HerzToMidi(1f);
        }
        float viewHeight = 2 * Camera.main.orthographicSize;
        float dy = viewHeight / (float)((midiMax + transpose) - (midiMin + transpose));
        float middleMidi = 0.5f * (float)((midiMax + transpose) + (midiMin + transpose));
        return dy * (midiVal + 0.5f - middleMidi);
        /*
         *         float viewHeight = 2 * Camera.main.orthographicSize;
        float dy = viewHeight / (float)(midiMax - midiMin);
        float middleMidi = 0.5f * (float)(midiMax + midiMin);
        return dy * (midiVal + 0.5f - middleMidi);
        */
    }


    // Start is called before the first frame update
    void Start()
    {
        detector.onPitchDetected.AddListener(PlotPitch);
    }

    private void PlotPitch(List<float> pitchList, int samples, float db)
    {
        var duration = (float)samples / (float)detector.micSampleRate;
        if (pitchList.Count > 0 && db > dbSensitivityThreshold)
        {
            foreach (var pitchVal in pitchList)
            {
                drawQueue.Enqueue(new PitchTime(pitchVal, duration / pitchList.Count, db));
            }
        }
        else
        {
            drawQueue.Enqueue(new PitchTime(0f, duration, db));
        }

        /*
        float pitchheightLast = pitchHeight;
        pitchHeight = Unity.Mathematics.math.remap(inputPitchRange.x, inputPitchRange.y, outputHeightRange.x, outputHeightRange.y, pitch);
        pitchTangent = pitchHeight - pitchheightLast;*/
    }

    // Update is called once per frame
    void Update()
    {
        if((!waitForSpaceBeforeStart || Input.GetKey(KeyCode.Space)) && !isRecording)
        {
            detector.ToggleRecord();
            isRecording = true;
        }

        if (!detector.Record)
        {
            return;
        }
        appTime += Time.deltaTime;
        bool silenceDetected = true;

        while (analysisTime < appTime && drawQueue.Count > 0)
        {
            var item = drawQueue.Dequeue();
            var midi = RAPTPitchDetectorExtensions.HerzToFloatMidi(item.pitch);
            if (!float.IsInfinity(midi))
            {
                float pitchHeightPrevious = pitchHeight;
                pitchHeight = MidiToScreenY(midi);
                pitchHeightSmoothed = pitchHeightPrevious + (pitchHeight - pitchHeightPrevious) / pitchSmoothing;
                pitch = item.pitch;

                float volumePrevious = volume;
                volume = 0f-item.db;
                volumeSmoothed = volumePrevious + (volume - volumePrevious) / pitchSmoothing;

                silenceDetected = false;
                timeSilent = 0f;

                if(isSilent)
                {
                    isSilent = false;
                    OnStopSilence.Invoke();
                }
            }
            else
            {
                // Do something when there is no pitch detected
            }
            analysisTime += item.dt;
        }
        if (silenceDetected)
        {
            
            timeSilent += Time.deltaTime;
            if (timeSilent > timeToSilence)
            {
                isSilent = true;
                OnStartSilence.Invoke();
            }
        }
    }
}
