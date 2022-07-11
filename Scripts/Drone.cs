using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Drone : MonoBehaviour
{
    public int systemId;
    public DroneDatabase.DroneInfo droneInfo;

    public TMP_Text latText;
    public TMP_Text lonText;
    public TMP_Text altText;
    public TMP_Text batText;

    // Update is called once per frame
    void Update()
    {
        UpdateInfoPanel();
    }

    private void UpdateInfoPanel()
    {
        // Only bothering to check the first text object, assuming the rest exist
        if(latText != null)
        {
            latText.text = droneInfo.lat.ToString();
            lonText.text = droneInfo.lon.ToString();
            altText.text = droneInfo.alt.ToString();
            batText.text = droneInfo.battery.ToString();
        }
    }
}
