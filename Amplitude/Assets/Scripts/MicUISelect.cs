using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MicUISelect : MonoBehaviour
{

    private TMP_Dropdown UIDropdownElement;
    private List<TMP_Dropdown.OptionData> micList;


    // Start is called before the first frame update
    void Start()
    {
        MicrophoneInput.DeviceListChanged.AddListener(OnDeviceListChanged);
        UIDropdownElement = GetComponent<TMP_Dropdown>();
        UIDropdownElement.onValueChanged.AddListener(OnValueChanged);
    }

    void OnDeviceListChanged()
    {
        UIDropdownElement.ClearOptions();

        // Add dropdown options
        micList = new List<TMP_Dropdown.OptionData>();
        foreach (string mic in MicrophoneInput.AvailableMics)
        {
            micList.Add(new TMP_Dropdown.OptionData(mic));
        }
        
        UIDropdownElement.AddOptions(micList);
    }

    void OnValueChanged(int option)
    {
        MicrophoneInput.ChangeMic(option);
    }
}
