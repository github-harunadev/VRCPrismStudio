using UnityEngine;
using static PrismStudio.CameraPoint;

public class Stablize : PostProcessor
{
    Vector3 previousPosition = Vector3.zero;
    Quaternion previousRotation = Quaternion.identity;
    /// <summary>
    /// Toggle positional processing
    /// 위치 처리 토글
    /// </summary>
    [SerializeField] public bool smoothenPosition = true;
    /// <summary>
    /// Weight of positional processing, higher value -> more immediate movement
    /// 위치 처리에 대한 비중, 높은 값 -> 더 즉각적인 움직임
    /// </summary>
    [SerializeField] public float positionWeight = 15f;
    /// <summary>
    /// Toggle rotational processing
    /// 각도 처리 토글
    /// </summary>
    [SerializeField] public bool smoothenRotation = true;
    /// <summary>
    /// Weight of positional processing, higher value -> more immediate movement
    /// 각도 처리에 대한 비중, 높은 값 -> 더 즉각적인 움직임
    /// </summary>
    [SerializeField] public float rotationWeight = 50f;

    /// <summary>
    /// Process the given Vector3 &amp; Quaternion
    /// 주어진 Vector3와 Quaternion을 처리합니다
    /// </summary>
    /// <param name="data">data from caller</param>
    /// <returns>pair of Vector3 and Quaternion as a form of ProcessData</returns>
    public override ProcessData Process(ProcessData data)
    {
        if (previousPosition == Vector3.zero)
        {
            previousPosition = data.position;
        }

        if (previousRotation == Quaternion.identity)
        {
            previousRotation = data.rotation;
        }

        if (smoothenPosition) data.position = Vector3.Lerp(previousPosition, data.position, positionWeight * Time.deltaTime);
        if (smoothenRotation) data.rotation = Quaternion.Lerp(previousRotation, data.rotation, rotationWeight * Time.deltaTime);

        previousPosition = data.position;
        previousRotation = data.rotation;

        return data;
    }
}