using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static PrismStudio.CameraPoint;


public class EasyArkShot : PostProcessor
{
    [SerializeField] public float radius = 1;
    [SerializeField, Range(0, 1)] public float originalPositionAsRoot = 0;
    [SerializeField] public float progress = 0;
    [SerializeField, Range(0, 1)] public float weight = 1;
    [SerializeField] bool fixRootRotation = false;
    [SerializeField] Vector3 rootRotation = Vector3.zero;

    Transform root;
    Transform up;
    Transform target;

    void VerifyObject()
    {
        if (root == null)
        {
            GameObject r = new GameObject("center");
            r.transform.SetParent(this.transform);
            root = r.transform;
        } else
        {
            if (root.parent != this.transform)
            {
                root.transform.SetParent(this.transform);
            }
        }

        if (up == null)
        {
            GameObject u = new GameObject("up");
            u.transform.SetParent(root);
            up = u.transform;
        } else
        {
            if (up.parent != root)
            {
                up.transform.SetParent(root);
            }
        }
        up.SetLocalPositionAndRotation(Vector3.up, Quaternion.identity);

        if (target == null)
        {
            GameObject t = new GameObject("target");
            t.transform.SetParent(root);
            target = t.transform;
            target.transform.LookAt(root, (up.position - root.position).normalized);
        } else
        {
            if (target.parent != this.transform)
            {
                target.transform.SetParent(root);
            }
        }

        root.SetLocalPositionAndRotation(Vector3.Lerp(Vector3.forward * radius, Vector3.zero, originalPositionAsRoot), Quaternion.Euler(0, 360f * progress, 0));

        target.SetLocalPositionAndRotation(Vector3.back * radius, target.localRotation);

    }

    private void Update()
    {
        VerifyObject();

        DrawingCircle.DrawCircle(root.position, root.rotation, radius, 512, Color.green);
        DrawingCircle.DrawCircle(root.position, root.rotation, radius / 100, 128, Color.blue);
    }

    public override ProcessData Process(ProcessData data)
    {
        if (root != null && target != null)
        {
            VerifyObject();

            root.SetLocalPositionAndRotation(Vector3.Lerp(Vector3.forward * radius, Vector3.zero, originalPositionAsRoot), Quaternion.Euler(0, 360f * progress, 0));
            
            if (fixRootRotation)
            {
                target.SetLocalPositionAndRotation(Vector3.back * radius, data.rotation);

                transform.SetPositionAndRotation(data.position, Quaternion.Euler(rootRotation));
            } else
            {
                target.SetLocalPositionAndRotation(Vector3.back * radius, Quaternion.identity);

                transform.SetLocalPositionAndRotation(GetLocalPosition(data.originalPosition, data.originalRotation, data.position), GetLocalRotation(data.originalRotation, data.rotation));
            }

            data.position = Vector3.Lerp(data.position, target.position, weight);
            data.rotation = Quaternion.Lerp(data.rotation, target.rotation, weight);
        }

        return data;
    }

    Vector3 GetLocalPosition(Vector3 positionA, Quaternion rotationA, Vector3 positionB)
    {
        return Quaternion.Inverse(rotationA) * (positionB - positionA);
    }

    Quaternion GetLocalRotation(Quaternion rotationA, Quaternion rotationB)
    {
        return Quaternion.Inverse(rotationA) * rotationB;
    }

    protected static class DrawingCircle
    {
        public static void DrawCircle(Vector3 center, Quaternion rotation, float radius, int segments, Color color)
        {
            float angleStep = 360.0f / segments;

            for (int i = 0; i < segments; i++)
            {
                float startAngle = i * angleStep;
                float endAngle = (i + 1) * angleStep;

                Vector3 startPoint = GetPointOnCircle(center, rotation, radius, startAngle);
                Vector3 endPoint = GetPointOnCircle(center, rotation, radius, endAngle);

                Debug.DrawLine(startPoint, endPoint, color);
            }
        }

        public static Vector3 GetPointOnCircle(Vector3 center, Quaternion rotation, float radius, float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian) * radius;
            float z = Mathf.Sin(radian) * radius;
            Vector3 point = new Vector3(x, 0, z);
            return center + rotation * point;
        }
    }
}