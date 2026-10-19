using PrismStudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class CustomOSCSender : MonoBehaviour
{
    [SerializeField] public StudioSystem system;

    [SerializeField] public string address;
    [SerializeField] public bool sendWhenAwake = true;

    public enum SendType
    {
        Bool,
        Int,
        Float
    }

    [SerializeField] public SendType currentSendType = SendType.Bool;
    [SerializeField] public bool valueBool = false;
    [SerializeField] public int valueInt = 0;
    [SerializeField] public float valueFloat = 0;

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if (sendWhenAwake) Send();
    }

    public void TrySend()
    {
        Send();
    }

    void Initialize()
    {
        if (system == null)
        {
            var studioSystem = GameObject.Find("PrismStudio");
            if (studioSystem != null)
            {
                system = studioSystem.GetComponent<StudioSystem>();
            }
        }
    }

    void Send()
    {
        if (system == null) Initialize();

        if (system != null)
        {
            switch (currentSendType)
            {
                case SendType.Bool:
                    system.Send(address, valueBool);
                    break;
                case SendType.Int:
                    system.Send(address, valueInt);
                    break;
                case SendType.Float:
                    system.Send(address, valueFloat);
                    break;
            }
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(CustomOSCSender))]
public class CustomOSCSenderEditor : Editor
{
    CustomOSCSender sender;

    int currentLanguage = 0;
    List<Dictionary<string, string>> languageData = new List<Dictionary<string, string>>()
    {
        {
            new Dictionary<string, string>()
            {
                { "send_when_awake", "Send when enabled" },
                { "address", "Address" },
                { "send_type", "Send data type" },
                { "value_bool", "Value (Bool)" },
                { "value_int", "Value (Int)" },
                { "value_float", "Value (Float)" },
                { "send_now", "Send Now" },
                { "studio_notconnected", "PrismStudio is not connected! OSC signals cannot be sent." },
                { "studio_invalid", "Cannot find PrismStudio! OSC signals cannot be sent." },
            }
        },
        {
            new Dictionary<string, string>()
            {
                { "send_when_awake", "오브젝트가 켜지면 바로 전송" },
                { "address", "주소" },
                { "send_type", "전송 값 타입" },
                { "value_bool", "값 (Bool)" },
                { "value_int", "값 (Int)" },
                { "value_float", "값 (Float)" },
                { "send_now", "지금 전송" },
                { "studio_notconnected", "PrismStudio의 OSC가 활성화 되어있지 않습니다!" },
                { "studio_invalid", "PrismStudio를 찾을 수 없습니다!" },
            }
        },
        {
            new Dictionary<string, string>()
            {
                { "send_when_awake", "オブジェクトがオンになったらすぐ転送" },
                { "address", "住所" },
                { "send_type", "転送値タイプ" },
                { "value_bool", "値 (Bool)" },
                { "value_int", "値 (Int)" },
                { "value_float", "値 (Float)" },
                { "send_now", "今転送" },
                { "studio_notconnected", "PrismStudioのOSCが有効になっていません！" },
                { "studio_invalid", "PrismStudioが見つかりません！" },
            }
        },
    };

    SerializedProperty studiosystem;
    SerializedProperty address;
    SerializedProperty sendWhenAwake;
    SerializedProperty currentSendType;

    SerializedProperty valueBool;
    SerializedProperty valueInt;
    SerializedProperty valueFloat;

    public override void OnInspectorGUI()
    {
        sender = (CustomOSCSender)target;

        studiosystem = serializedObject.FindProperty("system");

        address = serializedObject.FindProperty("address");
        sendWhenAwake = serializedObject.FindProperty("sendWhenAwake");
        currentSendType = serializedObject.FindProperty("currentSendType");

        valueBool = serializedObject.FindProperty("valueBool");
        valueInt = serializedObject.FindProperty("valueInt");
        valueFloat = serializedObject.FindProperty("valueFloat");

        EditorGUILayout.PropertyField(studiosystem, new GUIContent("PrismStudio"));

        EditorGUILayout.PropertyField(sendWhenAwake, new GUIContent(languageData[currentLanguage]["send_when_awake"]));

        GUILayout.Space(12);

        EditorGUILayout.PropertyField(address, new GUIContent(languageData[currentLanguage]["address"]));
        EditorGUILayout.PropertyField(currentSendType, new GUIContent(languageData[currentLanguage]["send_type"]));

        switch (currentSendType.enumValueIndex)
        {
            case 0:
                EditorGUILayout.PropertyField(valueBool, new GUIContent(languageData[currentLanguage]["value_bool"]));
                break;
            case 1:
                EditorGUILayout.PropertyField(valueInt, new GUIContent(languageData[currentLanguage]["value_int"]));
                break;
            case 2:
                EditorGUILayout.PropertyField(valueFloat, new GUIContent(languageData[currentLanguage]["value_float"]));
                break;
        }

        serializedObject.ApplyModifiedProperties();

        GUILayout.Space(12);

        if (sender.system == null)
        {
            EditorGUILayout.HelpBox(languageData[currentLanguage]["studio_invalid"], MessageType.Error);
        } else if (sender.system.senderConnected == false)
        {
            EditorGUILayout.HelpBox(languageData[currentLanguage]["studio_notconnected"], MessageType.Error);
        }

        if (GUILayout.Button(languageData[currentLanguage]["send_now"]))
        {
            sender.TrySend();
        }
    }
}
#endif