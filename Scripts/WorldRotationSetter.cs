using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldRotationSetter : MonoBehaviour
{
    public Transform world;
    public TMP_InputField text;
    public Slider slider;

    public void SetWorldRotationFromSlider(float yaw)
    {
        world.eulerAngles = new Vector3(0, yaw, 0);
        text.text = yaw.ToString();
    }

    public void SetWorldRotationFromText(string text)
    {
        float yaw = float.Parse(text);
        world.eulerAngles = new Vector3(0, yaw, 0);
        slider.value = yaw;
    }
}
