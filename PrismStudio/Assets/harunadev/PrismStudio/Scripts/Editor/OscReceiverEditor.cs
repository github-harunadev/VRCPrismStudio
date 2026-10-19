#if UNITY_EDITOR && PACKAGE_INPUTSYSTEM

using PrismStudio;
using PrismStudio.OSC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using static PrismStudio.CameraPoint;

[CustomEditor(typeof(OscReceiver))]
[CanEditMultipleObjects]
public class OscReceiverEditor : Editor
{
    int currentLanguage = 0;

    public readonly static List<Dictionary<string, string>> languageData = new List<Dictionary<string, string>>()
    {
        {
            new Dictionary<string, string>()
            {
                { "settings_connection", "Connectivity" },
                { "receiverport", "Receiver Port" },
                { "receiver_autostart", "Autostart Receiver (Non-Editor)" },
                { "receiver_connected", "OSC Receiver Opened" },
                { "receiver_not_connected", "OSC Receiver Closed" },
                { "receiver_disconnect", "Close Receiver" },
                { "receiver_connect", "Open Receiver" },
            }
        },
        {
            new Dictionary<string, string>()
            {
                { "settings_connection", "연결 설정" },
                { "receiverport", "받을 포트" },
                { "receiver_autostart", "수신 세션 자동시작" },
                { "receiver_connected", "OSC 수신 세션 열림" },
                { "receiver_not_connected", "OSC 수신 세션 닫힘" },
                { "receiver_disconnect", "수신 세션 닫기" },
                { "receiver_connect", "수신 세션 열기" }
            }
        },
        {
            new Dictionary<string, string>()
            {
                { "settings_connection", "接続設定" },
                { "receiverport", "受信ポート" },
                { "receiver_autostart", "受信OSCセッション 自動開始" },
                { "receiver_connected", "受信OSCセッション 開始中" },
                { "receiver_not_connected", "受信OSCセッション 開始されていません" },
                { "receiver_disconnect", "受信セッション停止" },
                { "receiver_connect", "受信セッション開始" },
            }
        },
    };
    OscReceiver oscReceiver;

    Texture2D connectionYes;
    Texture2D connectionNo;

    public override void OnInspectorGUI()
    {
        oscReceiver = ((OscReceiver)target);

        SerializedProperty ReceiverPort = serializedObject.FindProperty("ReceiverPort");
        SerializedProperty AutoStartReceiver = serializedObject.FindProperty("AutoStartReceiver");

        GUILayout.BeginVertical(new GUIStyle("box"));

        GUILayout.BeginHorizontal(new GUIStyle("box"));

        if (connectionYes == null) connectionYes = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/check-custom.png");
        if (connectionNo == null) connectionNo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/close-custom.png");

        GUILayout.Label(oscReceiver.receiverOpened ? connectionYes : connectionNo);

        GUILayout.Label(oscReceiver.receiverOpened ? languageData[currentLanguage]["receiver_connected"] : languageData[currentLanguage]["receiver_not_connected"], EditorStyles.subheader());

        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(ReceiverPort, new GUIContent(languageData[currentLanguage]["receiverport"]));

        EditorGUILayout.PropertyField(AutoStartReceiver, new GUIContent(languageData[currentLanguage]["receiver_autostart"]));

        if (GUILayout.Button(oscReceiver.receiverOpened ? languageData[currentLanguage]["receiver_disconnect"] : languageData[currentLanguage]["receiver_connect"]))
        {
            if (oscReceiver.receiverOpened)
            {
                oscReceiver.CloseReceiver();
            }
            else
            {
                oscReceiver.OpenReceiver();
            }
        }

        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}


#endif