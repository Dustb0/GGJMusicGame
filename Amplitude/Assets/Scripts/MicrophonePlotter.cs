using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PitchDetector;
using System;

public class MicrophonePlotter : MonoBehaviour
{
    public GameObject objectToPlot;
    public MicrophonePitchDetector detector;
    public float dbThreshold;

    public float scaleMin = 0.3f;
    public float scaleMax = 3f;


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


    // Start is called before the first frame update
    void Start()
    {
        detector.onPitchDetected.AddListener(PlotPitch);
    }

    private void PlotPitch(List<float> pitchList, int samples, float db)
    {
        var duration = (float)samples / (float)detector.micSampleRate;
        if (pitchList.Count > 0 && db > dbThreshold)
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
    }

    // Update is called once per frame
    void Update()
    {
    }
}