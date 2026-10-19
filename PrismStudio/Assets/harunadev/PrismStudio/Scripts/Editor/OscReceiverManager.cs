using PrismStudio.OSC;
using System;
using UnityEditor;
using UnityEngine;

public class OscReceiverManager : EditorWindow
{
    [MenuItem("PrismStudio/OscReceiver Manager")]
    static void ShowWindow()
    {
        OscReceiverManager window = (OscReceiverManager)GetWindow(typeof(OscReceiverManager));
        window.Show();
    }

    OscReceiver[] receivers = null;
    SerializedObject[] serializedReceivers = null;

    Vector2 scroll;
    Texture2D connectionYes;
    Texture2D connectionNo;

    bool triggerRefresh = false;

    

    void OnGUI()
    {
        if (connectionYes == null) connectionYes = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/check-custom.png");
        if (connectionNo == null) connectionNo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/close-custom.png");

        if (GUILayout.Button("Refresh") || receivers == null || triggerRefresh)
        {
            receivers = GameObject.FindObjectsOfType<OscReceiver>();
            serializedReceivers = new SerializedObject[receivers.Length];

            for (int i = 0; i < receivers.Length; i++)
            {
                if (receivers[i] != null) serializedReceivers[i] = new SerializedObject(receivers[i]);
            }
        }

        GUILayout.BeginVertical(new GUIStyle("box"));

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Toggle All"))
        {
            for (int i = 0; i < receivers.Length; i++)
            {
                if (receivers[i] != null)
                {
                    if (receivers[i].receiverOpened)
                    {
                        receivers[i].CloseReceiver();
                    }
                    else
                    {
                        receivers[i].OpenReceiver();
                    }
                }
            }
        }
        if (GUILayout.Button("Open All"))
        {
            for (int i = 0; i < receivers.Length; i++)
            {
                if (receivers[i] != null) receivers[i].OpenReceiver();
            }
        }
        if (GUILayout.Button("Close All"))
        {
            for (int i = 0; i < receivers.Length; i++)
            {
                if (receivers[i] != null) receivers[i].CloseReceiver();
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Rearrange Port"))
        {
            int nextPort = 9003;

            for (int i = 0; i < receivers.Length; i++)
            {
                if (receivers[i] != null)
                {
                    receivers[i].ReceiverPort = nextPort;

                    nextPort++;
                }
            }
        }

        if (GUILayout.Button("Copy Config (for OSCRelay)"))
        {
            string built = "";

            for (int i = 0; i < receivers.Length; i++)
            {
                if (receivers[i] != null)
                {
                    built += $"127.0.0.1:{receivers[i].ReceiverPort}\n";
                }
            }

            EditorGUIUtility.systemCopyBuffer = built;
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.Space(16);

        scroll = EditorGUILayout.BeginScrollView(scroll);

        for (int i = 0; i < receivers.Length; i++)
        {
            if (receivers[i] != null)
            {
                GUILayout.BeginHorizontal(new GUIStyle("box"));

                try
                {
                    SerializedProperty ReceiverPort = serializedReceivers[i].FindProperty("ReceiverPort");
                    SerializedProperty AutoStartReceiver = serializedReceivers[i].FindProperty("AutoStartReceiver");

                    if (GUILayout.Button(receivers[i].receiverOpened ? connectionYes : connectionNo, new GUIStyle("label")))
                    {
                        EditorGUIUtility.PingObject(receivers[i].gameObject);
                    }

                    GUILayout.Label(receivers[i].gameObject.name);

                    ReceiverPort.intValue = EditorGUILayout.IntField(ReceiverPort.intValue, GUILayout.Width(60));

                    GUILayout.Space(8);

                    AutoStartReceiver.boolValue = EditorGUILayout.ToggleLeft("Auto-start", AutoStartReceiver.boolValue, GUILayout.Width(88));

                    if (GUILayout.Button("Toggle", GUILayout.Width(60)))
                    {
                        if (receivers[i].receiverOpened)
                        {
                            receivers[i].CloseReceiver();
                        }
                        else
                        {
                            receivers[i].OpenReceiver();
                        }
                    }

                    if (GUILayout.Button(connectionYes, GUILayout.Width(36)))
                    {
                        receivers[i].OpenReceiver();
                    }

                    if (GUILayout.Button(connectionNo, GUILayout.Width(36)))
                    {
                        receivers[i].CloseReceiver();
                    }

                    serializedReceivers[i].ApplyModifiedProperties();
                }
                catch (Exception)
                {
                    triggerRefresh = true;
                }

                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal(new GUIStyle("box"));

                GUILayout.Label("Not found");

                GUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();
    }
}
