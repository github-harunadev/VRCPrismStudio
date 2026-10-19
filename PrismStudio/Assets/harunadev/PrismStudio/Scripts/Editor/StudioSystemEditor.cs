#if UNITY_EDITOR

using PrismStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StudioSystem))]
public class StudioSystemEditor : Editor
{
    I18n.SupportedLanguage _currentLanguage = I18n.SupportedLanguage.English;
    int currentLanguage
    {
        get
        {
            return (int)_currentLanguage;
        }
        set
        {
            _currentLanguage = (I18n.SupportedLanguage)value;
        }
    }
    public readonly static List<Dictionary<string, string>> languageData = new List<Dictionary<string, string>>()
    {
        {
            new Dictionary<string, string>()
            {
                { "version", "PrismStudio version" },
                { "latest_version", "Latest Version" },
                { "check_update", "Check Update" },
                { "settings_general", "General" },
                { "camera_type", "Camera type" },
                { "laggy_output", "Experiencing unstable video? (Guide)" },
                { "transmit_mode", "Transmit mode" },
                { "transmit_mode_value_changed", "Only send changed values" },
                { "transmit_mode_every_frame", "Send all values every frame" },
                { "transmit_mode_vcwes", "Only send changed values, but send all every second" },
                { "settings_connection", "Connectivity" },
                { "ip", "Sender IP" },
                { "senderport", "Sender Port" },
                { "receiverport", "Receiver Port" },
                { "sender_autostart", "Autostart Sender (Non-Editor)" },
                { "receiver_autostart", "Autostart Receiver (Non-Editor)" },
                { "sender_connected", "OSC Sender Connected" },
                { "sender_not_connected", "OSC Sender Not connected" },
                { "sender_disconnect", "Disconnect Sender" },
                { "sender_connect", "Connect Sender" },
                { "receiver_connected", "OSC Receiver Opened" },
                { "receiver_not_connected", "OSC Receiver Closed" },
                { "receiver_use_oscquery", "Use OSCQuery" },
                { "receiver_flexible_tcp_port", "Autochange TCP port" },
                { "receiver_flexible_udp_port", "Autochange UDP port" },
                { "receivertcpport", "Receiver TCP Port" },
                { "receiver_disconnect", "Close Receiver" },
                { "receiver_connect", "Open Receiver" },
                { "settings_modules", "Connected Modules" },
                { "add_module", "Add Module" },
                { "cancel", "Cancel" },
                { "settings_etc", "Misc." },
                { "show_prismstudio_info", "About PrismStudio" },
                { "check_update_title", "Checking update..." },
                { "check_update_content", "Retrieving information..." },
                { "vrcnative_transmittype_warning", "When using VRChat Native Camera, 'Value Changed' transmit mode is highly recommended." },
            }
        },
        {
            new Dictionary<string, string>()
            {
                { "version", "PrismStudio 버전" },
                { "latest_version", "최신 버전" },
                { "check_update", "업데이트 확인" },
                { "settings_general", "일반 설정" },
                { "camera_type", "카메라 종류" },
                { "laggy_output", "영상 프레임이 불안정한가요? (가이드)" },
                { "transmit_mode", "전송 방식" },
                { "transmit_mode_value_changed", "이전에 전송된 값에서 변경된 값만 전송합니다" },
                { "transmit_mode_every_frame", "모든 값을 프레임마다 전송합니다" },
                { "transmit_mode_vcwes", "변경된 값을 전송하지만, 모든 값을 초마다 전송합니다" },
                { "settings_connection", "연결 설정" },
                { "ip", "보낼 IP 주소" },
                { "senderport", "보낼 포트" },
                { "receiverport", "받을 포트" },
                { "sender_autostart", "전송 세션 자동시작" },
                { "receiver_autostart", "수신 세션 자동시작" },
                { "sender_connected", "OSC 전송 세션 연결됨" },
                { "sender_not_connected", "OSC 전송 세션 연결되어있지 않음" },
                { "sender_disconnect", "전송 세션 연결 끊기" },
                { "sender_connect", "전송 세션 연결하기" },
                { "receiver_connected", "OSC 수신 세션 열림" },
                { "receiver_not_connected", "OSC 수신 세션 닫힘" },
                { "receiver_disconnect", "수신 세션 닫기" },
                { "receiver_connect", "수신 세션 열기" },
                { "settings_modules", "연결된 모듈" },
                { "add_module", "모듈 추가" },
                { "cancel", "취소" },
                { "settings_etc", "기타" },
                { "show_prismstudio_info", "PrismStudio에 대해서" },
                { "check_update_title", "업데이트 확인" },
                { "check_update_content", "데이터를 불러오는 중..." },
                { "vrcnative_transmittype_warning", "VRChat 네이티브 카메라 사용시, '변경된 값만 전송하기' 전송 방식을 사용하는걸 강력히 추천합니다." },
            }
        },
        {
            new Dictionary<string, string>()
            {
                { "version", "PrismStudio バージョン" },
                { "latest_version", "最新バージョン" },
                { "check_update", "アップデート確認" },
                { "settings_general", "一般設定" },
                { "camera_type", "カメラ種類" },
                { "laggy_output", "映像フレームが不安定ですか？（ガイド）" },
                { "transmit_mode", "伝送方式" },
                { "transmit_mode_value_changed", "変更された数値のみを送信します" },
                { "transmit_mode_every_frame", "すべての数値をフレームごとに送信します" },
                { "transmit_mode_vcwes", "毎秒すべての数値を送信しますが、それ以外に送信されるのは変更された数値のみです" },
                { "settings_connection", "接続設定" },
                { "ip", "送信IPアドレス" },
                { "senderport", "送信ポート" },
                { "receiverport", "受信ポート" },
                { "sender_autostart", "送信OSCセッション 自動開始" },
                { "receiver_autostart", "受信OSCセッション 自動開始" },
                { "sender_connected", "送信OSCセッション 接続中" },
                { "sender_not_connected", "送信OSCセッション 接続されていません" },
                { "sender_disconnect", "送信セッション切断" },
                { "sender_connect", "送信セッション接続" },
                { "receiver_connected", "受信OSCセッション 開始中" },
                { "receiver_not_connected", "受信OSCセッション 開始されていません" },
                { "receiver_disconnect", "受信セッション停止" },
                { "receiver_connect", "受信セッション開始" },
                { "settings_modules", "モジュール" },
                { "add_module", "モジュール追加" },
                { "cancel", "キャンセル" },
                { "settings_etc", "その他" },
                { "show_prismstudio_info", "PrismStudioについて" },
                { "check_update_title", "アップデート確認" },
                { "check_update_content", "データを読み込み中···" },
                { "vrcnative_transmittype_warning", "VRChatネイティブカメラを使用する際は、「変更された値だけを送信する」方式を強くおすすめします。" },
            }
        },
    };

    StudioSystem studioSystem;
    Texture2D prismstudioLogo;
    Texture2D openIcon;
    Texture2D connectionYes;
    Texture2D connectionNo;

    string latestVersion = "";

    bool readyToAddModule;
    Module moduleToAdd;

    public override void OnInspectorGUI()
    {
        studioSystem = target as StudioSystem;
        studioSystem.ForceEnable();

        GUILayout.BeginVertical(new GUIStyle("box"));
        if (prismstudioLogo == null) prismstudioLogo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/prismstudio_logo.png");
        Rect r = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, EditorGUIUtility.currentViewWidth * 0.15f);
        GUI.Box(r, prismstudioLogo);
        GUILayout.Label(studioSystem.version, new GUIStyle("label") { fontSize = 16 });
        GUILayout.Label(languageData[currentLanguage]["version"]);
        GUILayout.EndVertical();

        if (latestVersion != "") EditorGUILayout.LabelField(languageData[currentLanguage]["latest_version"], latestVersion);
        if (GUILayout.Button(languageData[currentLanguage]["check_update"]))
        {
            checkUpdate();
        }


        currentLanguage = (int)studioSystem.currentLanguage;
        currentLanguage = GUILayout.Toolbar(currentLanguage, I18n.SupportedLanguageNames);
        //EditorGUILayout.PropertyField(savedLanguage, new GUIContent("Language / 언어 / 言語"));

        if (currentLanguage != (int)studioSystem.currentLanguage)
        {
            studioSystem.SetLanguage((I18n.SupportedLanguage)currentLanguage);
            EditorUtility.SetDirty(studioSystem);
        }

        SerializedProperty cameraType = serializedObject.FindProperty("cameraType");
        EditorGUILayout.PropertyField(cameraType, new GUIContent(languageData[currentLanguage]["camera_type"]));

        SerializedProperty foldout_connectivity = serializedObject.FindProperty("foldout_connectivity");
        if (!foldout_connectivity.boolValue)
        {
            if (openIcon == null) openIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/arrow-right-custom.png");

            GUILayout.BeginHorizontal(new GUIStyle("box"));

            if (connectionYes == null) connectionYes = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/check-custom.png");
            if (connectionNo == null) connectionNo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/close-custom.png");

            GUILayout.Label(studioSystem.senderConnected ? connectionYes : connectionNo);

            GUILayout.Label(studioSystem.senderConnected ? languageData[currentLanguage]["sender_connected"] : languageData[currentLanguage]["sender_not_connected"], EditorStyles.subheader());

            if (GUILayout.Button(openIcon, new GUIStyle("box"), GUILayout.Width(22), GUILayout.Height(22)))
            {
                foldout_connectivity.boolValue = true;
            }

            GUILayout.EndHorizontal();
        }

        //EditorGUILayout.LabelField(languageData[currentLanguage]["version"], studioSystem.version);

        GUILayout.Space(12);

        SerializedProperty foldout_generals = serializedObject.FindProperty("foldout_generals");
        foldout_generals.boolValue = EditorGUILayout.Foldout(foldout_generals.boolValue, languageData[currentLanguage]["settings_general"], EditorStyles.foldoutHeader());

        if (foldout_generals.boolValue)
        {
            SerializedProperty FPS = serializedObject.FindProperty("FPS");
            SerializedProperty VSync = serializedObject.FindProperty("VSync");
            SerializedProperty transmitMode = serializedObject.FindProperty("transmitMode");

            EditorGUILayout.PropertyField(FPS, new GUIContent("FPS"));

            if (GUILayout.Button(languageData[currentLanguage]["laggy_output"]))
            {
                switch (_currentLanguage)
                {
                    case I18n.SupportedLanguage.English:
                        Application.OpenURL("https://docs.haruna.dev/en/vrcprismstudio/laggy/");
                        break;
                    case I18n.SupportedLanguage.Korean:
                        Application.OpenURL("https://docs.haruna.dev/vrcprismstudio/laggy");
                        break;
                    case I18n.SupportedLanguage.Japanese:
                        Application.OpenURL("https://docs.haruna.dev/ja/vrcprismstudio/laggy/");
                        break;
                }
            }

            EditorGUILayout.PropertyField(VSync, new GUIContent("VSync"));

            EditorGUILayout.PropertyField(transmitMode, new GUIContent(languageData[currentLanguage]["transmit_mode"]));
            switch (studioSystem.transmitMode)
            {
                case StudioSystem.TransmitMode.ValueChanged:
                    GUILayout.Label("Value Changed :\n\t" + languageData[currentLanguage]["transmit_mode_value_changed"], EditorStyles.normal());
                    break;
                case StudioSystem.TransmitMode.EveryFrame:
                    GUILayout.Label("Every Frame :\n\t" + languageData[currentLanguage]["transmit_mode_every_frame"], EditorStyles.normal());
                    break;
                case StudioSystem.TransmitMode.ValueChangedWithEverySecond:
                    GUILayout.Label("Value Changed With Every Second :\n\t" + languageData[currentLanguage]["transmit_mode_vcwes"], EditorStyles.normal());
                    break;
            }

            if (studioSystem.cameraType == StudioSystem.ClientCameraType.VRChatNative && studioSystem.transmitMode != StudioSystem.TransmitMode.ValueChanged)
            {
                EditorGUILayout.HelpBox(languageData[currentLanguage]["vrcnative_transmittype_warning"], MessageType.Warning);
            }
        }

        GUILayout.Space(12);

        foldout_connectivity.boolValue = EditorGUILayout.Foldout(foldout_connectivity.boolValue, languageData[currentLanguage]["settings_connection"], EditorStyles.foldoutHeader());

        if (foldout_connectivity.boolValue)
        {
            SerializedProperty SenderIP = serializedObject.FindProperty("SenderIP");
            SerializedProperty SenderPort = serializedObject.FindProperty("SenderPort");
            SerializedProperty AutoStartSender = serializedObject.FindProperty("AutoStartSender");

            GUILayout.BeginVertical(new GUIStyle("box"));

            GUILayout.BeginHorizontal(new GUIStyle("box"));

            if (connectionYes == null) connectionYes = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/check-custom.png");
            if (connectionNo == null) connectionNo = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/close-custom.png");

            GUILayout.Label(studioSystem.senderConnected ? connectionYes : connectionNo);

            GUILayout.Label(studioSystem.senderConnected ? languageData[currentLanguage]["sender_connected"] : languageData[currentLanguage]["sender_not_connected"], EditorStyles.subheader());

            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(SenderIP, new GUIContent(languageData[currentLanguage]["ip"]));
            EditorGUILayout.PropertyField(SenderPort, new GUIContent(languageData[currentLanguage]["senderport"]));

            EditorGUILayout.PropertyField(AutoStartSender, new GUIContent(languageData[currentLanguage]["sender_autostart"]));

            if (GUILayout.Button(studioSystem.senderConnected ? languageData[currentLanguage]["sender_disconnect"] : languageData[currentLanguage]["sender_connect"]))
            {
                if (studioSystem.senderConnected)
                {
                    studioSystem.DisconnectSender();
                }
                else
                {
                    studioSystem.ConnectSender();
                }
            }

            GUILayout.EndVertical();
        }

        GUILayout.Space(12);

        SerializedProperty foldout_modules = serializedObject.FindProperty("foldout_modules");
        foldout_modules.boolValue = EditorGUILayout.Foldout(foldout_modules.boolValue, languageData[currentLanguage]["settings_modules"], EditorStyles.foldoutHeader());

        if (foldout_modules.boolValue)
        {
            GUILayout.BeginVertical(new GUIStyle("box"));

            int queueRemove = -1;
            for (int i = -2; i < studioSystem.modules.Length; i++)
            {
                Module item;
                bool isSystem = i < 0;
                if (i == -2)
                {
                    item = studioSystem.Point;
                } else if (i == -1)
                {
                    item = studioSystem.CameraProperty;
                } else
                {
                    item = studioSystem.modules[i];
                }
                GUIContent itemicon = null;

                if (item != null)
                {
                    itemicon = (new GUIContent(EditorGUIUtility.GetIconForObject(item)));
                } else
                {
                    itemicon = EditorGUIUtility.IconContent("d_console.warnicon.inactive.sml@2x");
                }

                GUILayout.BeginHorizontal(new GUIStyle("box"), GUILayout.Height(96));

                if (GUILayout.Button(itemicon, new GUIStyle("label"), GUILayout.Width(32), GUILayout.Height(32)))
                {
                    EditorGUIUtility.PingObject(item.gameObject);
                }

                GUILayout.BeginVertical();

                GUILayout.Label(item.GetType().FullName, EditorStyles.itemName());
                GUILayout.Label(item.description(_currentLanguage), EditorStyles.itemDescription());

                GUILayout.EndVertical();

                if (!isSystem)
                {
                    if (GUILayout.Button(EditorGUIUtility.IconContent("d_winbtn_win_close_a"), new GUIStyle("box"), GUILayout.Width(24)))
                    {
                        queueRemove = i;
                    }
                }

                GUILayout.EndHorizontal();
            }

            if (queueRemove != -1)
            {
                Undo.RecordObject(studioSystem, "Remove Module");

                studioSystem.RemoveModule(queueRemove);
                EditorUtility.SetDirty(studioSystem);
            }

            if (GUILayout.Button(languageData[currentLanguage][readyToAddModule ? "cancel" : "add_module"], GUILayout.Height(EditorGUIUtility.singleLineHeight * 2))) {
                readyToAddModule = !readyToAddModule;
                if (readyToAddModule)
                {
                    moduleToAdd = null;
                }
            }

            if (readyToAddModule)
            {
                moduleToAdd = (Module)EditorGUILayout.ObjectField(moduleToAdd, typeof(Module), true);
                if (moduleToAdd != null)
                {
                    bool found = false;
                    foreach (Module m in studioSystem.modules)
                    {
                        if (moduleToAdd == m)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        Undo.RecordObject(studioSystem, "Add Module");

                        studioSystem.AddModule(moduleToAdd);
                        EditorUtility.SetDirty(studioSystem);
                    }

                    readyToAddModule = false;
                    moduleToAdd = null;
                }
            }
            
            GUILayout.EndVertical();
        }

        GUILayout.Space(12);

        SerializedProperty foldout_misc = serializedObject.FindProperty("foldout_misc");
        foldout_misc.boolValue = EditorGUILayout.Foldout(foldout_misc.boolValue, languageData[currentLanguage]["settings_etc"], EditorStyles.foldoutHeader());

        if (foldout_misc.boolValue)
        {
            EditorGUILayout.BeginVertical("box");

            GUILayout.Space(12);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(12);
            GUILayout.Label(prismstudioLogo, GUILayout.Height(64));
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(12);

            GUILayout.Label("Made by harunadev");

            GUILayout.Space(12);

            GUILayout.Label("VRCLens is a property of Hirabiki.");
            GUILayout.Label("VirtualLens2 is a property of Logilabo (ろじらぼ).");
            GUILayout.Label("Integral is a property of suzufactory (スズ製作所).");
            GUILayout.Label("Flex FishEye Lens is a property of Goat-Cannery.");

            GUILayout.Space(12);

            GUILayout.Label("OscCore - https://github.com/tilde-love/osc-core");
            GUILayout.Label("Tiny Json - https://github.com/zanders3/json");

            if (GUILayout.Button("Special Thanks"))
            {
                SpecialThanksWindow window = EditorWindow.GetWindow<SpecialThanksWindow>(true, "Special Thanks", true);
                window.ShowUtility();
            }

            EditorGUILayout.EndVertical();
        }

        GUILayout.Space(12);

        SerializedProperty foldout_debug = serializedObject.FindProperty("foldout_debug");
        foldout_debug.boolValue = EditorGUILayout.Foldout(foldout_debug.boolValue, "Debug", EditorStyles.foldoutHeader());

        if (foldout_debug.boolValue)
        {
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }


    private void checkUpdate()
    {
        EditorUtility.DisplayProgressBar(languageData[currentLanguage]["check_update_title"], languageData[currentLanguage]["check_update_content"], 0);
        try
        {
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString("https://haruna.dev/prismstudio/version/");

                EditorUtility.ClearProgressBar();

                latestVersion = response;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            EditorUtility.ClearProgressBar();
        }
    }
}

public class SpecialThanksWindow : EditorWindow
{
    string data = "";
    private void OnGUI()
    {
        if (data == "")
        {
            download();
        }

        GUILayout.Label(data, EditorStyles.normalrich());

        if (data == "")
        {
            EditorUtility.DisplayDialog("Failed to fetch data", "Failed to fetch data from haruna.dev. Please check your network connection and try again", "Close");
            Close();
        }
    }


    private void download()
    {
        EditorUtility.DisplayProgressBar("Fetching", "Fetching informations...", 0); 
        try
        {
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString("https://haruna.dev/prismstudio/data/specialthanks/");

                EditorUtility.ClearProgressBar();

                data = response;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            EditorUtility.ClearProgressBar();
        }
    }
}

#endif