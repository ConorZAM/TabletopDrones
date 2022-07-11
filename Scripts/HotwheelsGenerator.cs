using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotwheelsGenerator : MonoBehaviour
{
    LineRenderer lineRenderer;
    public float updatePeriod = 0.1f;
    float updateCounter = 0f;

    public GameObject trackPrefab;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (drawing)
        {
            updateCounter += Time.deltaTime;

            if (updateCounter >= updatePeriod)
            {
                DrawTrack();
            }
        }
    }

    Vector3 previousPosition;

    private void DrawTrack()
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose))
        {
            //Line Renderer
            lineRenderer.positionCount = lineRenderer.positionCount + 1;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, pose.Position);



            // Use Planes

            // Spawn prefab and align with direction and scale of previous position and pose.position

            updateCounter -= updatePeriod;
        }
    }

    bool drawing = false;

    public void StartDrawing(BaseInputEventData inputEventData)
    {
        drawing = true;

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose pose))
        {
            previousPosition = pose.Position;
        }
    }

    public void EndDrawing(BaseInputEventData inputEventData)
    {
        drawing = false;
    }

}
