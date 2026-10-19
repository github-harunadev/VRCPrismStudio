#if UNITY_EDITOR && PACKAGE_INPUTSYSTEM

using PrismStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.InputSystem;
using static PostProcessorEditor;
using static PrismStudio.CameraPoint;

[CustomEditor(typeof(CameraPoint))]
[CanEditMultipleObjects]
public class PointEditor : Editor
{
    public int currentLanguage = 0;

    public static List<Dictionary<string, string>> languageData
    {
        get
        {
            return _languageData;
        }
    }
    static List<Dictionary<string, string>> _languageData = new List<Dictionary<string, string>>()
    {
        {
            new Dictionary<string, string>()
            {
                { "settings_general", "General" },
                { "angle_precision", "Angle precision level" },
                { "angle_precision_low", "Angle precision level is too low!" },
                { "angle_precision_high", "Angle precision level is too high!" },
                { "angle_precision_info", "Angle precision level makes the angle precise by sacrificing max movement area size. Too high value can result in camera softlock, and too low value can result in poor precision.\nRecommended value is 10~50." },
                { "use_cinemachine", "Use Cinemachine" },
                { "target_cinemachine", "Target Cinemachine Brain" },
                { "utilities", "Utilities" },
                { "move_cam_scene_title", "Scene View" },
                { "move_cam_scene", "Move camera to scene viewpoint" },
                { "shake_cam_title", "Camera Shake" },
                { "shake_cam_info", "This camera shake function will instantly add random position/rotation shake to the camera. This function is only used to give a constant camera shaking motion to the camera animation. In normal cases, it's recommend to use the CameraShake feature (in PostProcessor) rather than this function." },
                { "shake_cam_position", "Add random camera position shake" },
                { "shake_cam_position_value", "Position shake strength" },
                { "shake_cam_rotation", "Add random camera rotation shake" },
                { "shake_cam_rotation_value", "Rotation shake strength" },
                { "move_fpp_title", "Fly Camera" },
                { "move_fpp", "Fly camera (Press Right Click and drag)" },
                { "move_fpp_warning", "Fly camera is NOT for actual camera moving!!! This is only a tool to help you easily move the camera." },
                { "move_fpp_activated", "Activated" },
                { "move_fpp_lookspeed", "Fly camera sensitivity" },
                { "move_fpp_movespeed", "Fly camera move speed" },
                { "move_fpp_info", "While right click pressed: \n\tWASDQE : Move\n\tRFV : Tilt (use with caution)\n\tSpace : Force Stop Fly camera" },
                { "settings_postprocessors", "PostProcessors" },
                { "add_postprocessor", "Add PostProcessor" },
            }
        },
        {
            new Dictionary<string, string>()
            {
                { "settings_general", "일반 설정" },
                { "angle_precision", "회전 정확도" },
                { "angle_precision_low", "회전 정확도가 너무 낮습니다!" },
                { "angle_precision_high", "회전 정확도가 너무 높습니다!" },
                { "angle_precision_info", "회전 정확도는 카메라 최대 이동 영역을 제한하면서, 카메라 회전 정확도를 향상시킵니다. 너무 높은 수치로 설정하면 오작동을 일으킬 수 있으며, 너무 낮은 수치로 설정하면 카메라 회전이 불안정해집니다.\n추천하는 값은 10~50 사이입니다." },
                { "use_cinemachine", "Use Cinemachine" },
                { "target_cinemachine", "Target Cinemachine Brain" },
                { "utilities", "유틸리티" },
                { "move_cam_scene_title", "Scene창" },
                { "move_cam_scene", "카메라를 Scene창의 시점으로 이동" },
                { "shake_cam_title", "가상 손떨림" },
                { "shake_cam_info", "이 기능은 카메라에 랜덤한 위치/각도 손떨림을 즉시 적용합니다. 이 기능은 정해진 카메라 애니메이션에 정해진 만큼의 손떨림을 줄때만 사용됩니다. 보통의 경우에는 이 기능이 아닌 PostProcessor의 CameraShake 기능을 사용하는 것을 추천드립니다." },
                { "shake_cam_position", "위치 손떨림 추가" },
                { "shake_cam_position_value", "위치 손떨림 강도" },
                { "shake_cam_rotation", "각도 손떨림 추가" },
                { "shake_cam_rotation_value", "각도 손떨림 강도" },
                { "move_fpp_title", "FPP 카메라" },
                { "move_fpp", "FPP 카메라 (우클릭&드래그)" },
                { "move_fpp_warning", "FPP 카메라는 촬영용이 아닙니다!!! FPP 카메라는 그저 카메라를 쉽게 이동시켜줄 수 있게끔 해주는 도구일 뿐 입니다." },
                { "move_fpp_activated", "작동중" },
                { "move_fpp_lookspeed", "FPP 카메라 마우스 감도" },
                { "move_fpp_movespeed", "FPP 카메라 이동 속도" },
                { "move_fpp_info", "왼클릭 드래그하는 중에:\n\tWASDQE키 : 카메라 이동\n\tRFV키 : 틸트 (사용에 주의)\n\t스페이스키 : FPP 카메라 중지" },
                { "settings_postprocessors", "후처리 (PostProcessor)" },
                { "add_postprocessor", "후처리 추가" },
            }
        },
        {
            new Dictionary<string, string>()
            {
                { "settings_general", "一般設定" },
                { "angle_precision", "回転精度" },
                { "angle_precision_low", "回転精度が低すぎます!" },
                { "angle_precision_high", "回転精度が高すぎます!" },
                { "angle_precision_info", "回転精度はカメラの最大移動領域を制限しながら、カメラの回転精度を向上させます。 数値が高すぎると誤作動を起こす可能性があり、低すぎるとカメラの回転が不安定になります。\n推奨値は10~50の間です。" },
                { "use_cinemachine", "Use Cinemachine" },
                { "target_cinemachine", "Target Cinemachine Brain" },
                { "utilities", "ユーティリティ" },
                { "move_cam_scene_title", "Sceneウィンドウ" },
                { "move_cam_scene", "カメラをSceneウィンドウの視点に移動" },
                { "shake_cam_title", "仮想手ブレ" },
                { "shake_cam_info", "この機能はカメラにランダムな手ブレを適用します。 この機能は指定されたカメラアニメーションに指定された手ブレを追加する場合にのみ使用されます。 通常の場合はこの機能ではなく、PostProcessorのCameraShake機能を使用することをおすすめします。" },
                { "shake_cam_position", "位置手ブレ追加" },
                { "shake_cam_position_value", "位置手ブレ強度" },
                { "shake_cam_rotation", "角度手ブレ追加" },
                { "shake_cam_rotation_value", "角度手ブレ強度" },
                { "move_fpp_title", "FPPカメラ" },
                { "move_fpp", "FPPカメラ（右クリック&ドラッグ）" },
                { "move_fpp_warning", "FPPカメラは撮影用ではありません！！！ FPPカメラはただカメラを簡単に移動させてくれる道具に過ぎません。" },
                { "move_fpp_activated", "作動中" },
                { "move_fpp_lookspeed", "FPPカメラのマウス感度" },
                { "move_fpp_movespeed", "FPPカメラの移動速度" },
                { "move_fpp_info", "右クリックドラッグ中に:\n\tWASDQEキー : カメラ移動\n\tRFVキー : チルト（使用に注意）\n\tスペースキー : FPPカメラ中止" },
                { "settings_postprocessors", "ポストプロセッサー" },
                { "add_postprocessor", "ポストプロセッサー追加" },
            }
        },
    };

    StudioSystem studioSystem;

    public CameraPoint cameraPoint;
    private Transform transform;
    private SerializedObject serializedTransform;

    private SerializedProperty positionProperty;
    private SerializedProperty rotationProperty;

    private Texture2D rightClick;
    #region FPP Controls
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    private float tilt = 0.0f;

    private float smoothingFactor = 10f;
    private float inputA = 0.0f;
    private float inputD = 0.0f;
    private float inputW = 0.0f;
    private float inputS = 0.0f;
    private float inputQ = 0.0f;
    private float inputE = 0.0f;
    private float inputR = 0.0f;
    private float inputF = 0.0f;
    private bool boolInputA = false;
    private bool boolInputD = false;
    private bool boolInputW = false;
    private bool boolInputS = false;
    private bool boolInputQ = false;
    private bool boolInputE = false;
    private bool boolInputR = false;
    private bool boolInputF = false;

    bool FPPActive = false;
    Vector2 PreviousMousePosition = Vector2.zero;
    Vector3 OverrideLocalPosition;
    Quaternion OverrideLocalRotation;
    Vector3 OverrideLocalEulerAngles;

    #endregion

    private static float randPositionStrength = 0.1f;
    private static float randRotationStrength = 5f;

    private static double lastUpdateTime;
    private static double currentTime;
    private static float deltaTime;

    private Rect AddPostProcessorButtonRect;

    private void OnEnable()
    {
        cameraPoint = ((CameraPoint)target);
        transform = cameraPoint.transform;
        serializedTransform = new SerializedObject(transform);

        if (!studioSystem) studioSystem = transform.parent.parent.GetComponent<StudioSystem>();

        positionProperty = serializedTransform.FindProperty("m_LocalPosition");
        rotationProperty = serializedTransform.FindProperty("m_LocalRotation");

        foldout_general = serializedObject.FindProperty("foldout_general");
        foldout_camerashake = serializedObject.FindProperty("foldout_camerashake");
        foldout_fppcam = serializedObject.FindProperty("foldout_fppcam");
        foldout_postprocessors = serializedObject.FindProperty("foldout_postprocessors");

        PointForwardDistance = serializedObject.FindProperty("PointForwardDistance");
        useCinemachine = serializedObject.FindProperty("useCinemachine");
        targetCinemachine = serializedObject.FindProperty("targetCinemachine");

        lastUpdateTime = EditorApplication.timeSinceStartup;
        EditorApplication.update += UpdateCallback;
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        EditorApplication.update -= UpdateCallback;
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void UpdateCallback()
    {
        currentTime = EditorApplication.timeSinceStartup;
        deltaTime = (float)(currentTime - lastUpdateTime);

        lastUpdateTime = currentTime;

        Repaint();
    }


    SerializedProperty foldout_general;
    SerializedProperty foldout_camerashake;
    SerializedProperty foldout_fppcam;
    SerializedProperty foldout_postprocessors;

    SerializedProperty PointForwardDistance;
    SerializedProperty useCinemachine;
    SerializedProperty targetCinemachine;

    public override void OnInspectorGUI()
    {
        currentLanguage = ((int)studioSystem.currentLanguage);

        foldout_general.boolValue = EditorGUILayout.Foldout(foldout_general.boolValue, languageData[currentLanguage]["settings_general"], EditorStyles.foldoutHeader());
        if (foldout_general.boolValue)
        {
            EditorGUILayout.PropertyField(PointForwardDistance, new GUIContent(languageData[currentLanguage]["angle_precision"]));
            EditorGUILayout.HelpBox(languageData[currentLanguage]["angle_precision_" + (PointForwardDistance.floatValue > 100 ? "high" : (PointForwardDistance.floatValue < 5 ? "low" : "info"))], (PointForwardDistance.floatValue > 100 || PointForwardDistance.floatValue < 5 ? MessageType.Warning : MessageType.Info));

            EditorGUILayout.PropertyField(useCinemachine, new GUIContent(languageData[currentLanguage]["use_cinemachine"]));

            if (useCinemachine.boolValue)
            {
                EditorGUILayout.PropertyField(targetCinemachine, new GUIContent(languageData[currentLanguage]["target_cinemachine"]));
            }
        }

        /*
        GUILayout.Space(12);
        GUILayout.Label(languageData[currentLanguage]["move_cam_scene_title"], EditorStyles.header());

        if (GUILayout.Button(languageData[currentLanguage]["move_cam_scene"]))
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            if (sceneView != null)
            {
                positionProperty.vector3Value = sceneView.camera.transform.position;
                rotationProperty.quaternionValue = sceneView.camera.transform.rotation;
            }
        }*/

        GUILayout.Space(12);

        foldout_camerashake.boolValue = EditorGUILayout.Foldout(foldout_camerashake.boolValue, languageData[currentLanguage]["shake_cam_title"], EditorStyles.foldoutHeader());
        if (foldout_camerashake.boolValue)
        {
            EditorGUILayout.HelpBox(languageData[currentLanguage]["shake_cam_info"], MessageType.Warning);

            randPositionStrength = EditorGUILayout.Slider(new GUIContent(languageData[currentLanguage]["shake_cam_position_value"]), randPositionStrength, 0, 2);
            randRotationStrength = EditorGUILayout.Slider(new GUIContent(languageData[currentLanguage]["shake_cam_rotation_value"]), randRotationStrength, 0, 10);

            if (GUILayout.Button(languageData[currentLanguage]["shake_cam_position"]))
            {
                positionProperty.vector3Value += new Vector3(RandomNP() * randPositionStrength, RandomNP() * randPositionStrength, RandomNP() * randPositionStrength);
            }

            if (GUILayout.Button(languageData[currentLanguage]["shake_cam_rotation"]))
            {
                rotationProperty.quaternionValue = Quaternion.Euler(rotationProperty.quaternionValue.eulerAngles + new Vector3(RandomNP() * randRotationStrength, RandomNP() * randRotationStrength, RandomNP() * randRotationStrength));
            }
        }

        GUILayout.Space(12);

        foldout_fppcam.boolValue = EditorGUILayout.Foldout(foldout_fppcam.boolValue, languageData[currentLanguage]["move_fpp_title"], EditorStyles.foldoutHeader());
        if (foldout_fppcam.boolValue)
        {
            EditorGUILayout.HelpBox(languageData[currentLanguage]["move_fpp_warning"], MessageType.Warning);
            
            if (rightClick == null) rightClick = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/mouse-right-click-outline-custom.png");

            GUILayout.Label(new GUIContent(languageData[currentLanguage]["move_fpp"] + (FPPActive ? "\n" + languageData[currentLanguage]["move_fpp_activated"] : ""), rightClick), new GUIStyle("button"), GUILayout.Height(256));

            Rect labelRect = GUILayoutUtility.GetLastRect();

            EditorGUILayout.HelpBox(languageData[currentLanguage]["move_fpp_info"], MessageType.Info);

            cameraPoint.fpp_lookSpeed = EditorGUILayout.Slider(new GUIContent(languageData[currentLanguage]["move_fpp_lookspeed"]), cameraPoint.fpp_lookSpeed, 0, 3);
            cameraPoint.fpp_moveSpeed = EditorGUILayout.Slider(new GUIContent(languageData[currentLanguage]["move_fpp_movespeed"]), cameraPoint.fpp_moveSpeed, 0, 9);

            if (labelRect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                PreviousMousePosition = Event.current.mousePosition;
                FPPActive = true;
                Event.current.Use();
            }
            if ((Event.current.type == EventType.MouseUp || Event.current.type == EventType.ContextClick) && FPPActive)
            {
                FPPActive = false;
                Event.current.Use();
            }

            if (Event.current.type != EventType.Layout)
            {
                if (Event.current.keyCode == KeyCode.Space)
                {
                    FPPActive = false;
                }
                if (FPPActive)
                {
                    if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseMove)
                    {
                        Event.current.Use();
                    }
                    else if (Event.current.keyCode == KeyCode.A)
                    {
                        boolInputA = Event.current.type == EventType.KeyDown ? true : (Event.current.type == EventType.KeyUp ? false : boolInputA);
                        Event.current.Use();
                    }
                    else if (Event.current.keyCode == KeyCode.D)
                    {
                        boolInputD = Event.current.type == EventType.KeyDown ? true : (Event.current.type == EventType.KeyUp ? false : boolInputD);
                        Event.current.Use();
                    }
                    else if (Event.current.keyCode == KeyCode.W)
                    {
                        boolInputW = Event.current.type == EventType.KeyDown ? true : (Event.current.type == EventType.KeyUp ? false : boolInputW);
                        Event.current.Use();
                    }
                    else if (Event.current.keyCode == KeyCode.S)
                    {
                        boolInputS = Event.current.type == EventType.KeyDown ? true : (Event.current.type == EventType.KeyUp ? false : boolInputS);
                        Event.current.Use();
                    }
                    else if (Event.current.keyCode == KeyCode.Q)
                    {
                        boolInputQ = Event.current.type == EventType.KeyDown ? true : (Event.current.type == EventType.KeyUp ? false : boolInputQ);
                        Event.current.Use();
                    }
                    else if (Event.current.keyCode == KeyCode.E)
                    {
                        boolInputE = Event.current.type == EventType.KeyDown ? true : (Event.current.type == EventType.KeyUp ? false : boolInputE);
                        Event.current.Use();
                    }
                    else if (Event.current.keyCode == KeyCode.R)
                    {
                        boolInputR = Event.current.type == EventType.KeyDown ? true : (Event.current.type == EventType.KeyUp ? false : boolInputR);
                        Event.current.Use();
                    }
                    else if (Event.current.keyCode == KeyCode.F)
                    {
                        boolInputF = Event.current.type == EventType.KeyDown ? true : (Event.current.type == EventType.KeyUp ? false : boolInputF);
                        Event.current.Use();
                    }

                    inputA = Mathf.Lerp(inputA, boolInputA ? 1f : 0f, deltaTime * smoothingFactor);
                    inputD = Mathf.Lerp(inputD, boolInputD ? 1f : 0f, deltaTime * smoothingFactor);
                    inputW = Mathf.Lerp(inputW, boolInputW ? 1f : 0f, deltaTime * smoothingFactor);
                    inputS = Mathf.Lerp(inputS, boolInputS ? 1f : 0f, deltaTime * smoothingFactor);
                    inputQ = Mathf.Lerp(inputQ, boolInputQ ? 1f : 0f, deltaTime * smoothingFactor);
                    inputE = Mathf.Lerp(inputE, boolInputE ? 1f : 0f, deltaTime * smoothingFactor);
                    inputR = Mathf.Lerp(inputR, boolInputR ? 1f : 0f, deltaTime * smoothingFactor);
                    inputF = Mathf.Lerp(inputF, boolInputF ? 1f : 0f, deltaTime * smoothingFactor);

                    PreviousMousePosition -= Event.current.mousePosition;

                    rotationX -= (PreviousMousePosition.x * cameraPoint.fpp_lookSpeed);
                    rotationY -= (PreviousMousePosition.y * cameraPoint.fpp_lookSpeed);
                    rotationY = Mathf.Clamp(rotationY, -89.8f, 89.8f); // Limit vertical rotation

                    tilt = ((-inputR + inputF) * cameraPoint.fpp_lookSpeed);
                    if (Event.current.keyCode == KeyCode.V && Event.current.type == EventType.KeyDown)
                    {
                        tilt = 0;
                        OverrideLocalEulerAngles = new Vector3(OverrideLocalEulerAngles.x, OverrideLocalEulerAngles.y);
                        Event.current.Use();
                    }

                    OverrideLocalRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
                    OverrideLocalRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

                    OverrideLocalEulerAngles = new Vector3(rotationY, rotationX, OverrideLocalEulerAngles.z + tilt);

                    Vector3 direction = new Vector3(-inputA + inputD, -inputQ + inputE, inputW + -inputS);
                    direction = transform.TransformDirection(direction);

                    OverrideLocalPosition += direction * (cameraPoint.fpp_moveSpeed * deltaTime);

                    positionProperty.vector3Value = OverrideLocalPosition;
                    rotationProperty.quaternionValue = Quaternion.Euler(OverrideLocalEulerAngles);

                    PreviousMousePosition = Event.current.mousePosition;
                }
                else
                {
                    boolInputA = false;
                    boolInputD = false;
                    boolInputW = false;
                    boolInputS = false;
                    boolInputQ = false;
                    boolInputE = false;
                    boolInputR = false;
                    boolInputF = false;
                    inputA = 0;
                    inputD = 0;
                    inputW = 0;
                    inputS = 0;
                    inputQ = 0;
                    inputE = 0;
                    inputR = 0;
                    inputF = 0;

                    OverrideLocalPosition = transform.localPosition;
                    OverrideLocalRotation = transform.localRotation;
                    OverrideLocalEulerAngles = TransformUtils.GetInspectorRotation(transform);
                    rotationX = OverrideLocalEulerAngles.y;
                    rotationY = OverrideLocalEulerAngles.x;
                }
            }
        }

        GUILayout.Space(12);

        foldout_postprocessors.boolValue = EditorGUILayout.Foldout(foldout_postprocessors.boolValue, languageData[currentLanguage]["settings_postprocessors"], EditorStyles.foldoutHeader());
        if (foldout_postprocessors.boolValue)
        {
            GUILayout.BeginVertical(new GUIStyle("box"));

            PostProcessorEditorStatic.OnInspectorGUI(cameraPoint, ref AddPostProcessorButtonRect);

            GUILayout.EndVertical();
        }

        serializedTransform.ApplyModifiedProperties();
        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI(SceneView view)
    {
        if (Event.current.type == EventType.MouseUp || Event.current.type == EventType.ContextClick)
        {
            FPPActive = false;
        }
    }

    float RandomNP()
    {
        return ((UnityEngine.Random.value - 0.5f) * 2);
    }

    

}


#endif