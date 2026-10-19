
using OscCore;
using PrismStudio;
using PrismStudio.OSC;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(OscReceiver))]
public class ReceiverMonitor : MonoBehaviour
{
    OscReceiver oscReceiver;

    public Dictionary<string, string> monitoredPairs = new Dictionary<string, string>();

    private void Awake()
    {
        oscReceiver = GetComponent<OscReceiver>();
    }

    void OnDisable()
    {
        oscReceiver.monitor -= Monitor;
    }

    void OnEnable()
    {
        oscReceiver.monitor += Monitor;
    }

    void Monitor(OscMessage message)
    {
        string s = "";

        for (int i = 0; i < message.Count; i++)
        {
            s += $"{message[i].ToString()} ";
        }

        monitoredPairs[message.Address] = s;
    }
}

#if UNITY_EDITOR 

[CustomEditor(typeof(ReceiverMonitor))]
public class ReceiverMonitorEditor : Editor
{
    ReceiverMonitor monitor;


    private void OnEnable()
    {
        EditorApplication.update += UpdateCallback;
    }

    private void OnDisable()
    {
        EditorApplication.update -= UpdateCallback;
    }

    void UpdateCallback()
    {
        Repaint();
    }

    public override void OnInspectorGUI()
    {
        monitor = (ReceiverMonitor)target;

        GUILayout.Label("All the received OSC messages will appear here as a dictionary", new GUIStyle("label") { wordWrap = true });

        if (GUILayout.Button("Clear"))
        {
            monitor.monitoredPairs = new Dictionary<string, string>();
        }

        EditorGUILayout.BeginVertical(new GUIStyle("box"));

        Dictionary<string, string> copiedDict = new Dictionary<string, string>(monitor.monitoredPairs);

        foreach (string key in copiedDict.Keys)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight + 4));

            EditorGUILayout.BeginHorizontal(GUILayout.Width(320));

            GUILayout.Label(key, new GUIStyle("label") { wordWrap = true });

            GUILayout.Space(2);

            EditorGUILayout.EndHorizontal();

            GUILayout.Label(copiedDict[key], new GUIStyle("label") { wordWrap = true });

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }
}
#endif