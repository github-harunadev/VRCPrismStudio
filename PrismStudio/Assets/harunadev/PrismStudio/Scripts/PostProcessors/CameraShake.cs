using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static PrismStudio.CameraPoint;

public class CameraShake : PostProcessor
{
    [SerializeField] public float maxPositionVibration = 0.025f;
    [SerializeField] public float maxRotationVibration = 2.6f;
    [SerializeField] public float maxTime = 0.4f;
    [SerializeField] public float minTime = 0.1f;
    [SerializeField] public float smooth = 0.8f;

    Vector3 previousPositionOffset = Vector3.zero;
    Vector3 nextPositionOffset = Vector3.zero;

    Vector3 previousRotationOffset = Vector3.zero;
    Vector3 nextRotationOffset = Vector3.zero;

    Vector3 finalPositionOffset;
    Vector3 finalRotationOffset;
    Vector3 previousFinalPositionOffset = Vector3.zero;
    Vector3 previousFinalRotationOffset = Vector3.zero;
    Vector3 outputFinalPositionOffset;
    Vector3 outputFinalRotationOffset;

    float targetTime = 0;
    float currentTime = 0;
    float percentage = 2;

    public override ProcessData Process(ProcessData data)
    {
        if (percentage >= 1)
        {
            percentage = 0;
            currentTime = 0;
            targetTime = random(minTime, maxTime);

            previousPositionOffset = nextPositionOffset;
            previousRotationOffset = nextRotationOffset;

            finalPositionOffset = previousPositionOffset;
            finalRotationOffset = previousRotationOffset;

            nextPositionOffset = new Vector3(random(-maxPositionVibration, maxPositionVibration), random(-maxPositionVibration, maxPositionVibration), random(-maxPositionVibration, maxPositionVibration));
            nextRotationOffset = new Vector3(random(-maxRotationVibration, maxRotationVibration), random(-maxRotationVibration, maxRotationVibration), random(-maxRotationVibration, maxRotationVibration));
        
            if (previousFinalPositionOffset == Vector3.zero &&
                previousFinalRotationOffset == Vector3.zero)
            {
                previousFinalPositionOffset = nextPositionOffset;
                previousFinalRotationOffset = nextRotationOffset;
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            percentage = currentTime / targetTime;

            finalPositionOffset = Vector3.Lerp(previousPositionOffset, nextPositionOffset, curve(percentage));
            finalRotationOffset = Vector3.Lerp(previousRotationOffset, nextRotationOffset, curve(percentage));
        }

        outputFinalPositionOffset = Vector3.Slerp(previousFinalPositionOffset, finalPositionOffset, smooth * Time.deltaTime);
        outputFinalRotationOffset = Vector3.Slerp(previousFinalRotationOffset, finalRotationOffset, smooth * Time.deltaTime);

        data.position = data.position + outputFinalPositionOffset;
        data.rotation = Quaternion.Euler(data.rotation.eulerAngles + outputFinalRotationOffset);

        previousFinalPositionOffset = outputFinalPositionOffset;
        previousFinalRotationOffset = outputFinalRotationOffset;

        return data;
    }

    float random(float min, float max)
    {
        return Random.Range(min, max);
    }

    float curve(float x)
    {
        return Mathf.SmoothStep(0, 1, x);
    }
}