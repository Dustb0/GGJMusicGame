using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Captures sound-input and calculates the average loudness and pitch
/// </summary>
public class MicrophoneInput 
{
	#region " Constants "
    
    // minimum amplitude to extract pitch
    private const float PITCH_TRESHHOLD = 0.02f;

	#endregion

	#region " Private Fields "
    
    // Reference to the audiosource
    private static AudioSource audioSource;

    // The current device index
    private static int microphoneDeviceIndex = 0;

    // Current device name
    private static string currentDeviceName;

    // Currently available devices (chached list)
    private static string[] availableDevices;

    private bool m_micStartingUp;

    private static float s_loudestVolume;
    private static float s_highestPitch;

	#endregion

    #region " Properties "

    public static int CurrentDeviceIndex { get { return microphoneDeviceIndex; } 
        set { 
            microphoneDeviceIndex = value;
            // Fetch device name, kinda costy so only do it when changing microphone
            currentDeviceName = availableDevices[microphoneDeviceIndex]; 
        } }

    public static string CurrentDeviceName 
    { 
        get 
        {
            if (availableDevices != null && availableDevices.Length > 0) return currentDeviceName;
            else return "None";
        } 
    }

    public static bool IsRecording { get { return Microphone.IsRecording(CurrentDeviceName); } }

    public static float LoudestVolume { get { return s_loudestVolume; } }

    public static float HighestPitch { get { return s_highestPitch; } }

    public static int AvailableMics { get { return availableDevices.Length; } }

    #endregion

    #region " Capture Device "

    public void OnInitialize() 
	{
        // Fetch reference to audiosource
        audioSource = Camera.main.GetComponent<AudioSource>();

        // Init statistics
        s_loudestVolume = 0;
        s_highestPitch = 0;

        InitDeviceList();
	}

    public static void InitDeviceList()
    {
        // Fetch available devices & current once
        availableDevices = Microphone.devices;
        currentDeviceName = availableDevices.Length > 0 ? availableDevices[microphoneDeviceIndex] : "None";
    }

    public void Update() 
	{
        if (Microphone.GetPosition(CurrentDeviceName) > 0)
        {
            audioSource.Play();
            m_micStartingUp = false;
        }
    }

    public void Reset()
    {
        StopRecording();
        StartRecording();
    }

	#endregion

    #region " Public Methods "

    /// <summary>
    /// Starts listening to microphone input
    /// </summary>
    public void StartRecording()
    {
        // If we're already recording, we don't need to start again
        if (IsRecording) return; 

        // Start recording
        audioSource.clip = Microphone.Start(CurrentDeviceName, true, 1, 44100);
        audioSource.loop = true;

        // Wait until recording has started
        m_micStartingUp = true;
    }

    public void StopRecording()
    {
        Microphone.End(CurrentDeviceName);
        audioSource.Stop();
    }

    /// <summary>
    /// Returns the english musical notation of the current highest pitch (stat-value)
    /// </summary>
    public string GetHighestPitchNote()
    {
        return GetPitchNote(HighestPitch);
    }

    public string GetPitchNote(float pitch)
    {
        if (pitch >= 4186) return "C8";
        else if (pitch >= 3951) return "B7";
        else if (pitch >= 3729) return "A#7/Bb7";
        else if (pitch >= 3520) return "A7";
        else if (pitch >= 3322) return "G#7/Ab7";
        else if (pitch >= 3135) return "G7";
        else if (pitch >= 2959) return "F#7/Gb7";
        else if (pitch >= 2793) return "F7";
        else if (pitch >= 2637) return "E7";
        else if (pitch >= 2489) return "D#7/Eb7";
        else if (pitch >= 2349) return "D7";
        else if (pitch >= 2217) return "C#7/Db7";
        else if (pitch >= 2093) return "C7";
        else if (pitch >= 1975) return "B6";
        else if (pitch >= 1864) return "A#6/Bb6";
        else if (pitch >= 1760) return "A6";
        else if (pitch >= 1661) return "G#6/Ab6";
        else if (pitch >= 1567) return "G6";
        else if (pitch >= 1479) return "F#6/Gb6";
        else if (pitch >= 1396) return "F6";
        else if (pitch >= 1318) return "E6";
        else if (pitch >= 1244) return "D#6/Eb6";
        else if (pitch >= 1174) return "D6";
        else if (pitch >= 1108) return "C#6/Db6";
        else if (pitch >= 1046) return "C6";
        else if (pitch >= 987) return "B5";
        else if (pitch >= 932) return "A#5/Bb5";
        else if (pitch >= 880) return "A5";
        else if (pitch >= 830) return "G#5/Ab5";
        else if (pitch >= 783) return "G5";
        else if (pitch >= 739) return "F#5/Gb5";
        else if (pitch >= 698) return "F5";
        else if (pitch >= 659) return "E5";
        else if (pitch >= 622) return "D#5/Eb5";
        else if (pitch >= 587) return "D5";
        else if (pitch >= 554) return "C#5/Db5";
        else if (pitch >= 523) return "C5";
        else if (pitch >= 493) return "B4";
        else if (pitch >= 466) return "A#4/Bb4";
        else if (pitch >= 440) return "A4";
        else if (pitch >= 415) return "G#4/Ab4";
        else if (pitch >= 392) return "G4";
        else if (pitch >= 369) return "F#4/Gb4";
        else if (pitch >= 349) return "F4";
        else if (pitch >= 329) return "E4";
        else if (pitch >= 311) return "D#4/Eb4";
        else if (pitch >= 293) return "D4";
        else if (pitch >= 277) return "C#4/Db4";
        else if (pitch >= 261) return "C4";
        else if (pitch >= 246) return "B3";
        else if (pitch >= 233) return "A#3/Bb3";
        else if (pitch >= 220) return "A3";
        else if (pitch >= 207) return "G#3/Ab3";
        else if (pitch >= 196) return "G3";
        else if (pitch >= 185) return "F#3/Gb3";
        else if (pitch >= 174) return "F3";
        else if (pitch >= 164) return "E3";
        else if (pitch >= 155) return "D#3/Eb3";
        else if (pitch >= 146) return "D3";
        else if (pitch >= 138) return "C#3/Db3";
        else if (pitch >= 130) return "C3";
        else if (pitch >= 123) return "B2";
        else if (pitch >= 116) return "A#2/Bb2";
        else if (pitch >= 110) return "A2";
        else if (pitch >= 103) return "G#2/Ab2";
        else if (pitch >= 98) return "G2";
        else if (pitch >= 92) return "F#2/Gb2";
        else if (pitch >= 87) return "F2";
        else if (pitch >= 82) return "E2";
        else if (pitch >= 77) return "D#2/Eb2";
        else if (pitch >= 73) return "D2";
        else if (pitch >= 69) return "C#2/Db2";
        else if (pitch >= 65) return "C2";
        else if (pitch >= 61) return "B1";
        else if (pitch >= 58) return "A#1/Bb1";
        else if (pitch >= 55) return "A1";
        else return "<A1";
    }

    #endregion

    #region " Private Methods "

    /// <summary>
    /// Returns the average volume from the current input source
    /// </summary>
    public float GetAverageVolume(int sampleCount = 256)
   {
       // Get samples
       float[] sampleData = new float[sampleCount];
       float absoluteSum = 0;

       audioSource.GetOutputData(sampleData, 0);

       // Add absolute values
       foreach (float s in sampleData)
       {
           absoluteSum += Mathf.Abs(s);
       }

        // Calculate average
        absoluteSum = (absoluteSum / sampleCount) * 100;

        // Update statistics
        if (absoluteSum > s_loudestVolume) s_loudestVolume = absoluteSum;

       return absoluteSum;
   }

    public float GetPitch(int sampleCount = 256)
   {
       float[] spectrum = new float[sampleCount];
        float maxV = 0;
        int maxN = 0;

        // Get sound spectrum
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        for (int i = 0; i < sampleCount; ++i)
        { // find max 
            if (spectrum[i] > maxV && spectrum[i] > PITCH_TRESHHOLD)
            {
                maxV = spectrum[i];
                maxN = i; // maxN is the index of max
            }
        }

        // pass the index to a float variable
        float freqN = maxN;

        if (maxN > 0 && maxN < sampleCount - 1)
        { 
            // interpolate index using neighbours
            float dL = spectrum[maxN-1]/spectrum[maxN];
            float dR = spectrum[maxN+1]/spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }

        // convert index to frequency
        freqN = freqN * (AudioSettings.outputSampleRate / 2) / sampleCount;

        // Update statistics
        if (freqN > s_highestPitch) s_highestPitch = freqN;

        return freqN;
   }

	#endregion
    
}

