using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneDatabase : MonoBehaviour
{
    // Add to this where necessary
    public char[] delimiters = new char[] { '\t', ',', ' ' };

    float currentTime = 0;

    // Data structure used to store the drone's state
    public struct DroneInfo
    {
        public int systemId;
        public double lat;
        public double lon;
        public double alt;
        public double battery;
        public double timeStamp;
    };

    public Dictionary<int, DroneInfo> Drones = new Dictionary<int, DroneInfo>();

    public void UpdateDroneInfo(DroneInfo newDroneInfo)
    {
        // If the drone already has information stored
        if (Drones.ContainsKey(newDroneInfo.systemId))
        {
            if (newDroneInfo.timeStamp > Drones[newDroneInfo.systemId].timeStamp)
            {
                // Update the drone info if the new info is more recent
                Drones[newDroneInfo.systemId] = newDroneInfo;
            }
        }
        else
        {
            // Otherwise, add the new drone to the dictionary
            Drones[newDroneInfo.systemId] = newDroneInfo;
        }
    }

    private void Update()
    {
        currentTime = Time.time;
    }
    public void ParseMessage(byte[] data)
    {
        // Make sure there is some data to parse
        if(data == null || data.Length == 0)
        {
            return;
        }

        // This will need changing depending on the type of encoding used
        string dataString = System.Text.Encoding.UTF8.GetString(data);

        // Again, need to split according to chosen delimiter
        string[] splitData = dataString.Split(delimiters);

        // Could probably handle this better, but it'll do for now!
        if(splitData.Length != 5)
        {
            Debug.LogError("Attempting to parse wrong sized message");
            return;
        }

        // Create the info struct
        DroneInfo parsedInfo = new DroneInfo
        {
            systemId = Convert.ToInt32(splitData[0]),
            lat = Convert.ToDouble(splitData[1]),
            lon = Convert.ToDouble(splitData[2]),
            alt = Convert.ToDouble(splitData[3]),
            battery = Convert.ToDouble(splitData[4]),
            timeStamp = currentTime
        };
        
        // Add the new info to the dict
        UpdateDroneInfo(parsedInfo);
    }
}
