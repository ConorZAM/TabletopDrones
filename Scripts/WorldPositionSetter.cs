using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldPositionSetter : MonoBehaviour
{
    public Transform world;

    public TMP_InputField xText;
    public TMP_InputField yText;
    public TMP_InputField zText;

    public void SetOffset(string _)
    {
        Vector3 offset = new Vector3(float.Parse(xText.text), float.Parse(yText.text), float.Parse(zText.text));
        world.position = offset;
    }
}
