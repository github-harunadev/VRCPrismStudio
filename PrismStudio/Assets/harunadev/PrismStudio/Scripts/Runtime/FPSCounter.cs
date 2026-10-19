using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class FPSCounter : MonoBehaviour
{
    StringBuilder output = new StringBuilder();

    Text debugText;

    float fps;
    float avgfps;
    float highest;

    int index;
    public float updateInterval = 0.5f; // Interval to update FPS
    public int percentile = 1; // Percentile for low FPS

    private float accum = 0f; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Time left for current interval
    private List<float> fpsValues = new List<float>(); // List to store FPS values


    // Start is called before the first frame update
    private void Awake()
    {
        debugText = GetComponent<Text>();
        timeleft = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;


        if (timeleft <= 0.0)
        {
            output.Clear();

            output.Append("Frametime: " + (Time.unscaledDeltaTime * 1000).ToString("0.00") + "ms");

            fps = (int)(1f / Time.unscaledDeltaTime);
            output.Append("\nFPS: " + fps.ToString("0.00"));
            
            avgfps = accum / frames;
            output.Append("\nAverage FPS: " + avgfps.ToString("0.00"));

            if (highest < fps) highest = fps;
            output.Append("\nHighest FPS: " + highest);

            fpsValues.Add(avgfps);
            
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;

            fpsValues.Sort();

            index = Mathf.Max(0, (int)(fpsValues.Count * (1 - percentile / 100f)));
            output.Append("\n" + percentile + "% Low FPS: " + fpsValues[index].ToString("0.00"));

            debugText.text = output.ToString();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(FPSCounter))]
public class FPSCounterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
    }
}
#endif
