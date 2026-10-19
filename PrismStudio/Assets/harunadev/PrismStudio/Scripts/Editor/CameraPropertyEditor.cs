#if UNITY_EDITOR

using PrismStudio;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PrismStudio.CameraProperty;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

[CustomEditor(typeof(CameraProperty))]
public class CameraPropertyEditor : Editor
{
    int currentLanguage = 0;
    List<Dictionary<string, string>> languageData = new List<Dictionary<string, string>>()
    {
        {
            new Dictionary<string, string>()
            {
                { "notice", "Below properties can be controlled via animation." },
                { "settings_general", "General" },
                { "zoom", "Zoom" },
                { "exposure", "Exposure" },
                { "aperture", "Aperture" },
                { "settings_focus", "Focus" },
                { "focus_info", "Focus = 0 -> Automatic, Focus > 0 -> Manual (distance)" },
                { "focus", "Focus" },
                { "focus_point_x", "Focus Point X" },
                { "focus_point_y", "Focus Point Y" },
                { "settings_lookat", "LookAt" },
                { "lookat", "Local Avatar Lookat" },
                { "settings_lookatdirection", "LookAt Direction" },
                { "lookat_avatar_root", "Avatar root" },
                { "lookat_avatar_head", "Avatar head" },
                { "lookat_avatar_chest", "Avatar chest" },
                { "lookat_avatar_hips", "Avatar hips" },
                { "lookat_avatar_lfoot", "Avatar left foot" },
                { "lookat_avatar_rfoot", "Avatar right foot" },
                { "lookat_avatar_lhand", "Avatar left hand" },
                { "lookat_avatar_rhand", "Avatar right hand" },
                { "lookat_offset_vector", "LookAt Offset" }, 
                { "settings_cameraroot", "Camera Root Position/Rotation" },
                { "camerarootp_active", "Root Position origin" },
                { "camerarootp_avatar_root",  "Root Position avatar root" },
                { "camerarootp_avatar_head",  "Root Position avatar head" },
                { "camerarootp_avatar_chest", "Root Position avatar chest" },
                { "camerarootp_avatar_hips",  "Root Position avatar hips" },
                { "camerarootp_avatar_lfoot", "Root Position avatar left foot" },
                { "camerarootp_avatar_rfoot", "Root Position avatar right foot" },
                { "camerarootp_avatar_lhand", "Root Position avatar left hand" },
                { "camerarootp_avatar_rhand", "Root Position avatar right hand" },
                { "camerarootr_active", "Root Rotation origin" },
                { "camerarootr_avatar_root",  "Root Rotation avatar root" },
                { "camerarootr_avatar_head",  "Root Rotation avatar head" },
                { "camerarootr_avatar_chest", "Root Rotation avatar chest" },
                { "camerarootr_avatar_hips",  "Root Rotation avatar hips" },
                { "camerarootr_avatar_lfoot", "Root Rotation avatar left foot" },
                { "camerarootr_avatar_rfoot", "Root Rotation avatar right foot" },
                { "camerarootr_avatar_lhand", "Root Rotation avatar left hand" },
                { "camerarootr_avatar_rhand", "Root Rotation avatar right hand" },
                { "settings_etc", "Misc." },
                { "model_hide", "Hide model" },
                { "vrclens", "VRCLens" },
                { "vrclens_whitebalance", "White balance" },
                { "virtuallens2", "VirtualLens2" },
                { "virtuallens2_layermask", "Layer Mask" },
                { "virtuallens2_layermask_local", "Local player" },
                { "virtuallens2_layermask_remote", "Remote player" },
                { "virtuallens2_layermask_ui", "Game UI" },
                { "virtuallens2_autofocus", "Auto Focus Modes" },
                { "virtuallens2_autofocus_point", "Point AF (Default)" },
                { "virtuallens2_autofocus_face", "Face Detection" },
                { "virtuallens2_autofocus_selfie", "Selfie AF (Local)" },
                { "virtuallens2_autofocusspeed", "AF Speed" },
                { "virtuallens2_autofocusspeed_info", "VRChat's FPS highly influences the AF speed! Ensure you're having a stable constant FPS." },
                { "virtuallens2_autofocusspeed_immediate", "Immediate" },
                { "virtuallens2_autofocusspeed_fast", "Fast" },
                { "virtuallens2_autofocusspeed_medium", "Medium" },
                { "virtuallens2_autofocusspeed_slow", "Slow" },
                { "virtuallens2_pauseautofocus", "Pause AF" },
                { "virtuallens2_cullingdistance", "Culling Distance" },
                { "virtuallens2_cullingdistance_default", "Default" },
                { "virtuallens2_cullingdistance_x10", "x10" },
                { "virtuallens2_cullingdistance_x100", "x100" },
                { "integral", "Integral" },
                { "integral_shoot_none", "None (Do nothing)" },
                { "integral_shoot_clear", "Clear" },
                { "integral_shoot_open", "Open" },
                { "integral_shoot_bulb", "Bulb" },
                { "integral_shoot_clearnopen", "Clear & Open" },
                { "integral_shoot_single", "Single Frame" },
                { "integral_shoot_ss", "Open with Shutter Speed" },
                { "integral_shutterspeed", "Shutter Speed" },
                { "integral_showgrid", "Show grid" },
                { "integral_focusmode", "Focus mode" },
                { "integral_focusmode_manual", "Manual" },
                { "integral_focusmode_af", "Auto Focus" },
                { "integral_focusmode_motionaf", "Motion AF" },
                { "integral_pauseaf", "Pause Auto Focus" },
                { "integral_vignette", "Vignette" },
                { "integral_axialchromaticaberration", "Axial Chromatic Aberration" },
                { "integral_bokehshape", "Bokeh shape" },
                { "integral_bokehshape_circle", "Circle" },
                { "integral_bokehshape_hexagon", "Hexagon" },
                { "integral_bokehshape_star", "Star" },
                { "integral_bokehshape_heart", "Heart" },
                { "integral_bokehshape_bubble", "Bubble" },
                { "integral_bokehshape_rotation", "Bokeh shape rotation" },
                { "integral_far", "Far" },
                { "integral_curve", "Curve" },
                { "integral_curve_linear", "Linear" },
                { "integral_curve_log", "Log" },
                { "integral_aspectratio", "Aspect ratio" },
                { "integral_aspectratio_16_9", "16:9" },
                { "integral_aspectratio_4_3", "4:3" },
                { "integral_aspectratio_3_2", "3:2" },
                { "integral_aspectratio_1_1", "1:1" },
                { "integral_aspectratio_2_35_1", "2.35:1" },
                { "integral_aspectratio_7_6", "7:6" },
                { "integral_aspectratio_5_4", "5:4" },
                { "integral_aspectratio_7_5", "7:5" },
                { "integral_aspectratio_2_1", "2:1" },
                { "integral_aspectratio_custom", "Custom" },
                { "integral_compositionmode", "Composition Mode" },
                { "integral_compositionmode_normal", "Normal" },
                { "integral_compositionmode_lighten", "Lighten (Single)" },
                { "integral_compositionmode_darken", "Darken (Single)" },
                { "integral_compositionmode_depth", "Depth" },
                { "integral_rollingshuttermode", "Rolling Shutter Mode" },
                { "integral_rollingshuttermode_off", "Off" },
                { "integral_rollingshuttermode_ltr", "Left → Right" },
                { "integral_rollingshuttermode_ttb", "Top → Bottom" },
                { "integral_rollingshuttermode_rtl", "Right → Left" },
                { "integral_rollingshuttermode_btt", "Bottom → Top" },
                { "vrcnative", "VRChat Native Camera" },
                { "vrcnative_Mode", "Camera Mode" },
                { "vrcnative_Capture", "Capture" },
                { "vrcnative_CaptureDelayed", "Capture (Delayed)" },
                { "vrcnative_LayerMask", "Layer Mask" },
                { "vrcnative_layermask_environment", "World" },
                { "vrcnative_GreenScreen", "Greenscreen" },
                { "vrcnative_Lock", "Pickup Lock" },
                { "vrcnative_SmoothMovement", "Built-in Smooth Movement" },
                { "vrcnative_AutoLevelRoll", "Auto Level Roll" },
                { "vrcnative_AutoLevelPitch", "Auto Level Pitch" },
                { "vrcnative_AudioFromCamera", "Audio From Camera" },
                { "vrcnative_ShowFocus", "Show Focus" },
                { "vrcnative_Streaming", "Spout2 Streaming" },
                { "vrcnative_OrientationIsLandscape", "Landscape Orientation" },
                { "vrcnative_hue", "Hue" },
                { "vrcnative_saturation", "Saturation" },
                { "vrcnative_lightness", "Lightness" },
                { "vrcnative_smoothingstrength", "Smoothing Strength" },
            }
        },
        {
            new Dictionary<string, string>()
            {
                { "notice", "아래 항목들을 애니메이션을 통해 조절할 수 있습니다." },
                { "settings_general", "일반 설정" },
                { "zoom", "확대 (Zoom)" },
                { "exposure", "노출 (Exposure)" },
                { "aperture", "조리개 (Aperture)" },
                { "settings_focus", "초점 설정" },
                { "focus_info", "초점 = 0 -> 자동, 초점 > 0 -> 수동 (거리)" },
                { "focus", "초점" },
                { "focus_point_x", "초점 X" },
                { "focus_point_y", "초점 Y" },
                { "settings_lookat", "LookAt 설정" },
                { "lookat", "로컬 아바타 바라보기" },
                { "settings_lookatdirection", "바라보는 방향 설정" },
                { "lookat_avatar_root",  "아바타 Root" },
                { "lookat_avatar_head",  "아바타 머리" },
                { "lookat_avatar_chest", "아바타 가슴" },
                { "lookat_avatar_hips",  "아바타 골반" },
                { "lookat_avatar_lfoot", "아바타 왼발" },
                { "lookat_avatar_rfoot", "아바타 오른발" },
                { "lookat_avatar_lhand", "아바타 왼손" },
                { "lookat_avatar_rhand", "아바타 오른손" },
                { "lookat_offset_vector", "바라보기 오프셋" },
                { "settings_cameraroot", "카메라 루트 위치/각도 설정" },
                { "camerarootp_active", "PrismStudio 위치" },
                { "camerarootp_avatar_root",  "아바타 Root 위치" },
                { "camerarootp_avatar_head",  "아바타 머리 위치" },
                { "camerarootp_avatar_chest", "아바타 가슴 위치" },
                { "camerarootp_avatar_hips",  "아바타 골반 위치" },
                { "camerarootp_avatar_lfoot", "아바타 왼발 위치" },
                { "camerarootp_avatar_rfoot", "아바타 오른발 위치" },
                { "camerarootp_avatar_lhand", "아바타 왼손 위치" },
                { "camerarootp_avatar_rhand", "아바타 오른손 위치" },
                { "camerarootr_active", "PrismStudio 각도" },
                { "camerarootr_avatar_root",  "아바타 Root 각도" },
                { "camerarootr_avatar_head",  "아바타 머리 각도" },
                { "camerarootr_avatar_chest", "아바타 가슴 각도" },
                { "camerarootr_avatar_hips",  "아바타 골반 각도" },
                { "camerarootr_avatar_lfoot", "아바타 왼발 각도" },
                { "camerarootr_avatar_rfoot", "아바타 오른발 각도" },
                { "camerarootr_avatar_lhand", "아바타 왼손 각도" },
                { "camerarootr_avatar_rhand", "아바타 오른손 각도" },
                { "settings_etc", "기타" },
                { "model_hide", "카메라 모델 숨기기" },
                { "vrclens", "VRCLens" },
                { "vrclens_whitebalance", "화이트 밸런스" },
                { "virtuallens2", "VirtualLens2" },
                { "virtuallens2_layermask", "레이어 마스크" },
                { "virtuallens2_layermask_local", "로컬 플레이어" },
                { "virtuallens2_layermask_remote", "다른 플레이어" },
                { "virtuallens2_layermask_ui", "게임 UI" },
                { "virtuallens2_autofocus", "AF 모드" },
                { "virtuallens2_autofocus_point", "포인트 AF (기본)" },
                { "virtuallens2_autofocus_face", "얼굴 인식 AF" },
                { "virtuallens2_autofocus_selfie", "셀카 AF (로컬 전용)" },
                { "virtuallens2_autofocusspeed", "AF 속도" },
                { "virtuallens2_autofocusspeed_info", "VRChat의 FPS는 이 속도에 큰 영향을 끼칩니다! 안정적인 성능이 나오도록 게임 FPS를 제한해주세요." },
                { "virtuallens2_autofocusspeed_immediate", "즉시" },
                { "virtuallens2_autofocusspeed_fast", "빠르게" },
                { "virtuallens2_autofocusspeed_medium", "보통" },
                { "virtuallens2_autofocusspeed_slow", "느리게" },
                { "virtuallens2_pauseautofocus", "AF 일시정지" },
                { "virtuallens2_cullingdistance", "최대 렌더 거리" },
                { "virtuallens2_cullingdistance_default", "기본" },
                { "virtuallens2_cullingdistance_x10", "x10" },
                { "virtuallens2_cullingdistance_x100", "x100" },
                { "integral", "Integral" },
                { "integral_shoot_none", "아무것도 하지 않음" },
                { "integral_shoot_clear", "초기화" },
                { "integral_shoot_open", "촬영" },
                { "integral_shoot_bulb", "벌브 촬영" },
                { "integral_shoot_clearnopen", "초기화 후 촬영" },
                { "integral_shoot_single", "1프레임만" },
                { "integral_shoot_ss", "셔터 속도만큼 촬영" },
                { "integral_shutterspeed", "셔터 속도" },
                { "integral_showgrid", "그리드 표시" },
                { "integral_focusmode", "초점 모드" },
                { "integral_focusmode_manual", "수동" },
                { "integral_focusmode_af", "자동" },
                { "integral_focusmode_motionaf", "모션 자동" },
                { "integral_pauseaf", "자동 초점 일시정지" },
                { "integral_vignette", "비네트" },
                { "integral_axialchromaticaberration", "축색수차 (Axial Chromatic Aberration)" },
                { "integral_bokehshape", "보케 모양" },
                { "integral_bokehshape_circle", "원형" },
                { "integral_bokehshape_hexagon", "육각형" },
                { "integral_bokehshape_star", "별" },
                { "integral_bokehshape_heart", "하트" },
                { "integral_bokehshape_bubble", "버블" },
                { "integral_bokehshape_rotation", "보케 모양 회전" },
                { "integral_far", "최대 렌더링 거리" },
                { "integral_curve", "커브" },
                { "integral_curve_linear", "선형 (Linear)" },
                { "integral_curve_log", "로그 (Log)" },
                { "integral_aspectratio", "종횡비" },
                { "integral_aspectratio_16_9", "16:9" },
                { "integral_aspectratio_4_3", "4:3" },
                { "integral_aspectratio_3_2", "3:2" },
                { "integral_aspectratio_1_1", "1:1" },
                { "integral_aspectratio_2_35_1", "2.35:1" },
                { "integral_aspectratio_7_6", "7:6" },
                { "integral_aspectratio_5_4", "5:4" },
                { "integral_aspectratio_7_5", "7:5" },
                { "integral_aspectratio_2_1", "2:1" },
                { "integral_aspectratio_custom", "커스텀" },
                { "integral_compositionmode", "컴포지션 모드" },
                { "integral_compositionmode_normal", "일반" },
                { "integral_compositionmode_lighten", "밝게 (1프레임)" },
                { "integral_compositionmode_darken", "어둡게 (1프레임)" },
                { "integral_compositionmode_depth", "깊이 (Depth)" },
                { "integral_rollingshuttermode", "롤링 셔터 모드" },
                { "integral_rollingshuttermode_off", "비활성" },
                { "integral_rollingshuttermode_ltr", "왼쪽→오른쪽" },
                { "integral_rollingshuttermode_ttb", "위→아래" },
                { "integral_rollingshuttermode_rtl", "오른쪽→왼쪽" },
                { "integral_rollingshuttermode_btt", "아래→위" },
                { "vrcnative", "VRChat 네이티브 카메라" },
                { "vrcnative_Mode", "카메라 모드" },
                { "vrcnative_Capture", "사진 찍기" },
                { "vrcnative_CaptureDelayed", "사진 찍기 (딜레이)" },
                { "vrcnative_LayerMask", "레이어" },
                { "vrcnative_layermask_environment", "월드" },
                { "vrcnative_GreenScreen", "그린스크린" },
                { "vrcnative_Lock", "픽업 잠금" },
                { "vrcnative_SmoothMovement", "자체 카메라 스무스" },
                { "vrcnative_AutoLevelRoll", "자동 레벨 롤" },
                { "vrcnative_AutoLevelPitch", "자동 레벨 피치" },
                { "vrcnative_AudioFromCamera", "카메라에서 오디오" },
                { "vrcnative_ShowFocus", "초점 표시" },
                { "vrcnative_Streaming", "Spout2로 출력" },
                { "vrcnative_OrientationIsLandscape", "가로 모드" },
                { "vrcnative_hue", "색조" },
                { "vrcnative_saturation", "채도" },
                { "vrcnative_lightness", "명도" },
                { "vrcnative_smoothingstrength", "부드럽게 강도" },
            }
        },
        {
            new Dictionary<string, string>()
            {
                { "notice", "以下の項目をアニメーションで調整することができます。" },
                { "settings_general", "一般設定" },
                { "zoom", "拡大 (Zoom)" },
                { "exposure", "露出 (Exposure)" },
                { "aperture", "絞り (Aperture)" },
                { "settings_focus", "焦点設定" },
                { "focus_info", "焦点 = 0 -> 自動, 焦点 > 0 -> 手動  (距離)" },
                { "focus", "焦点" },
                { "focus_point_x", "焦点 X" },
                { "focus_point_y", "焦点 Y" },
                { "settings_lookat", "LookAt（注目） 設定" },
                { "lookat", "ローカルアバターを注目" },
                { "settings_lookatdirection", "注目する位置の設定" },
                { "lookat_avatar_root",  "アバター Root" },
                { "lookat_avatar_head",  "アバター 頭" },
                { "lookat_avatar_chest", "アバター 胸" },
                { "lookat_avatar_hips",  "アバター 骨盤" },
                { "lookat_avatar_lfoot", "アバター 左足" },
                { "lookat_avatar_rfoot", "アバター 右足" },
                { "lookat_avatar_lhand", "アバター 左手" },
                { "lookat_avatar_rhand", "アバター 右手" },
                { "lookat_offset_vector", "オフセット座標" },
                { "settings_cameraroot", "Camera Root 位置/角度設定" },
                { "camerarootp_active", "PrismStudio 位置" },
                { "camerarootp_avatar_root",  "アバター Root 位置" },
                { "camerarootp_avatar_head",  "アバター 頭 位置"  },
                { "camerarootp_avatar_chest", "アバター 胸 位置"  },
                { "camerarootp_avatar_hips",  "アバター 骨盤 位置" },
                { "camerarootp_avatar_lfoot", "アバター 左足 位置" },
                { "camerarootp_avatar_rfoot", "アバター 右足 位置" },
                { "camerarootp_avatar_lhand", "アバター 左手 位置" },
                { "camerarootp_avatar_rhand", "アバター 右手 位置" },
                { "camerarootr_active", "PrismStudio 角度" },
                { "camerarootr_avatar_root",  "アバター Root 角度"},
                { "camerarootr_avatar_head",  "アバター 頭 角度"  },
                { "camerarootr_avatar_chest", "アバター 胸 角度"  },
                { "camerarootr_avatar_hips",  "アバター 骨盤 角度"},
                { "camerarootr_avatar_lfoot", "アバター 左足 角度"},
                { "camerarootr_avatar_rfoot", "アバター 右足 角度" },
                { "camerarootr_avatar_lhand", "アバター 左手 角度"},
                { "camerarootr_avatar_rhand", "アバター 右手 角度" },
                { "settings_etc", "その他" },
                { "model_hide", "カメラモデルを非表示" },
                { "vrclens", "VRCLens" },
                { "vrclens_whitebalance", "ホワイトバランス" },
                { "virtuallens2", "VirtualLens2" },
                { "virtuallens2_layermask", "レイヤー マスク" },
                { "virtuallens2_layermask_local", "ローカル プレーヤー" },
                { "virtuallens2_layermask_remote", "他のプレーヤー" },
                { "virtuallens2_layermask_ui", "ゲームUI" },
                { "virtuallens2_autofocus", "AFモード" },
                { "virtuallens2_autofocus_point", "ポイントAF (基本)" },
                { "virtuallens2_autofocus_face", "顔認識AF" },
                { "virtuallens2_autofocus_selfie", "自撮りAF (ローカル専用)" },
                { "virtuallens2_autofocusspeed", "AF速度" },
                { "virtuallens2_autofocusspeed_info", "VRChatのFPSはこの速度に大きな影響を及ぼします！ 安定的な性能が出るようにゲームFPSを制限してください。" },
                { "virtuallens2_autofocusspeed_immediate", "即時" },
                { "virtuallens2_autofocusspeed_fast", "速い" },
                { "virtuallens2_autofocusspeed_medium", "普通" },
                { "virtuallens2_autofocusspeed_slow", "遅い" },
                { "virtuallens2_pauseautofocus", "AF一時停止" },
                { "virtuallens2_cullingdistance", "最大描画距離" },
                { "virtuallens2_cullingdistance_default", "標準" },
                { "virtuallens2_cullingdistance_x10", "x10" },
                { "virtuallens2_cullingdistance_x100", "x100" },
                { "integral", "Integral" },
                { "integral_shoot_none", "何もしない" },
                { "integral_shoot_clear", "初期化" },
                { "integral_shoot_open", "撮影" },
                { "integral_shoot_bulb", "バルブ撮影" },
                { "integral_shoot_clearnopen", "初期化 & 撮影" },
                { "integral_shoot_single", "1フレーム撮影" },
                { "integral_shoot_ss", "シャッタースピード撮影" },
                { "integral_shutterspeed", "シャッタースピード" },
                { "integral_showgrid", "グリッド表示" },
                { "integral_focusmode", "焦点モード" },
                { "integral_focusmode_manual", "手動 (MF)" },
                { "integral_focusmode_af", "自動 (AF)" },
                { "integral_focusmode_motionaf", "モーションAF" },
                { "integral_pauseaf", "自動焦点一時停止" },
                { "integral_vignette", "ビネット" },
                { "integral_axialchromaticaberration", "軸方向色収差 (Axial Chromatic Aberration)" },
                { "integral_bokehshape", "ボケ形" },
                { "integral_bokehshape_circle", "円形" },
                { "integral_bokehshape_hexagon", "六角形" },
                { "integral_bokehshape_star", "星" },
                { "integral_bokehshape_heart", "ハート" },
                { "integral_bokehshape_bubble", "気泡形" },
                { "integral_bokehshape_rotation", "ボケ形回転" },
                { "integral_far", "最大レンダリング距離" },
                { "integral_curve", "曲線" },
                { "integral_curve_linear", "線形 (Linear)" },
                { "integral_curve_log", "対数 (Log)" },
                { "integral_aspectratio", "写真サイズ比" },
                { "integral_aspectratio_16_9", "16:9" },
                { "integral_aspectratio_4_3", "4:3" },
                { "integral_aspectratio_3_2", "3:2" },
                { "integral_aspectratio_1_1", "1:1" },
                { "integral_aspectratio_2_35_1", "2.35:1" },
                { "integral_aspectratio_7_6", "7:6" },
                { "integral_aspectratio_5_4", "5:4" },
                { "integral_aspectratio_7_5", "7:5" },
                { "integral_aspectratio_2_1", "2:1" },
                { "integral_aspectratio_custom", "カスタム" },
                { "integral_compositionmode", "コンポジションモード" },
                { "integral_compositionmode_normal", "一般" },
                { "integral_compositionmode_lighten", "明るく(1フレーム)" },
                { "integral_compositionmode_darken", "暗く(1フレーム)" },
                { "integral_compositionmode_depth", "深度" },
                { "integral_rollingshuttermode", "ローリングシャッターモード" },
                { "integral_rollingshuttermode_off", "無効" },
                { "integral_rollingshuttermode_ltr", "左→右" },
                { "integral_rollingshuttermode_ttb", "上→下" },
                { "integral_rollingshuttermode_rtl", "右→左" },
                { "integral_rollingshuttermode_btt", "下→上" },
                { "vrcnative", "VRChatネイティブカメラ" },
                { "vrcnative_Mode", "カメラモード" },
                { "vrcnative_Capture", "写真撮影" },
                { "vrcnative_CaptureDelayed", "写真撮影（タイマー）" },
                { "vrcnative_LayerMask", "レイヤー" },
                { "vrcnative_layermask_environment", "ワールド" },
                { "vrcnative_GreenScreen", "グリーンスクリーン" },
                { "vrcnative_Lock", "ピックアップロック" },
                { "vrcnative_SmoothMovement", "カメラスムーズ" },
                { "vrcnative_AutoLevelRoll", "オートレベルロール" },
                { "vrcnative_AutoLevelPitch", "オートレベルピッチ" },
                { "vrcnative_AudioFromCamera", "カメラからオーディオ" },
                { "vrcnative_ShowFocus", "フォーカス表示" },
                { "vrcnative_Streaming", "Spout2で出力" },
                { "vrcnative_OrientationIsLandscape", "横モード" },
                { "vrcnative_hue", "色相" },
                { "vrcnative_saturation", "彩度" },
                { "vrcnative_lightness", "明度" },
                { "vrcnative_smoothingstrength", "スムージング強度" },
            }
        },
    };

    string[] virtuallens2_layermask = new string[3];
    string[] virtuallens2_afmode = new string[3];
    string[] virtuallens2_afspeed = new string[4];
    string[] virtuallens2_cullingdistance = new string[3];
    string[] integral_shootmode = new string[7];
    string[] integral_bokehshape = new string[5];
    string[] integral_focusmode = new string[3];
    string[] integral_curve = new string[2];
    string[] integral_compositionmode = new string[4];
    string[] integral_rollingshuttermode = new string[5];
    string[] integral_aspectratio = new string[10];
    string[] vrcnative_layermask = new string[4];

    StudioSystem studioSystem;
    [SerializeField]
    CameraProperty cameraProperty;

    Texture2D focus_point;

    SerializedProperty foldout_general;
    SerializedProperty foldout_focus;
    SerializedProperty foldout_lookat;
    SerializedProperty foldout_cameraroot;
    SerializedProperty foldout_misc;
    SerializedProperty foldout_vrclens;
    SerializedProperty foldout_virtuallens2;
    SerializedProperty foldout_integral;
    SerializedProperty foldout_vrcnative;

    SerializedProperty zoom;
    SerializedProperty exposure;
    SerializedProperty aperture;
    SerializedProperty focus;
    SerializedProperty focusX;
    SerializedProperty focusY;
    SerializedProperty avatarLookAt;
    SerializedProperty avatarLookAt_root;
    SerializedProperty avatarLookAt_head;
    SerializedProperty avatarLookAt_chest;
    SerializedProperty avatarLookAt_hips;
    SerializedProperty avatarLookAt_lfoot;
    SerializedProperty avatarLookAt_rfoot;
    SerializedProperty avatarLookAt_lhand;
    SerializedProperty avatarLookAt_rhand;
    SerializedProperty cameraRootP_active;
    SerializedProperty cameraRootP_avatar_root;
    SerializedProperty cameraRootP_avatar_head;
    SerializedProperty cameraRootP_avatar_chest;
    SerializedProperty cameraRootP_avatar_hips;
    SerializedProperty cameraRootP_avatar_lfoot;
    SerializedProperty cameraRootP_avatar_rfoot;
    SerializedProperty cameraRootP_avatar_lhand;
    SerializedProperty cameraRootP_avatar_rhand;
    SerializedProperty cameraRootR_active;
    SerializedProperty cameraRootR_avatar_root;
    SerializedProperty cameraRootR_avatar_head;
    SerializedProperty cameraRootR_avatar_chest;
    SerializedProperty cameraRootR_avatar_hips;
    SerializedProperty cameraRootR_avatar_lfoot;
    SerializedProperty cameraRootR_avatar_rfoot;
    SerializedProperty cameraRootR_avatar_lhand;
    SerializedProperty cameraRootR_avatar_rhand;
    SerializedProperty ap;
    SerializedProperty hidemodel;


    SerializedProperty vrclens_WhiteBalance;

    SerializedProperty virtuallens2_LayerMask;
    SerializedProperty virtuallens2_AFMode;
    SerializedProperty virtuallens2_AFSpeed;
    SerializedProperty virtuallens2_PauseAF;
    SerializedProperty virtuallens2_CullingDistance;

    SerializedProperty integral_ShootMode;
    SerializedProperty integral_ShutterSpeed;
    SerializedProperty integral_GridMode;
    SerializedProperty integral_FocusMode;
    SerializedProperty integral_PauseAF;
    SerializedProperty integral_Vignetting;
    SerializedProperty integral_AxialChromaticAberration;
    SerializedProperty integral_BokehShape;
    SerializedProperty integral_BokehShapeRotation;
    SerializedProperty integral_CompositionMode;
    SerializedProperty integral_RollingShutterMode;
    SerializedProperty integral_Far;
    SerializedProperty integral_Curve;
    SerializedProperty integral_AspectRatioCustomValue;


    SerializedProperty vrcnative_Mode;
    SerializedProperty vrcnative_LayerMask;
    SerializedProperty vrcnative_GreenScreen;
    SerializedProperty vrcnative_Lock;
    SerializedProperty vrcnative_SmoothMovement;
    SerializedProperty vrcnative_AutoLevelRoll;
    SerializedProperty vrcnative_AutoLevelPitch;
    SerializedProperty vrcnative_AudioFromCamera;
    SerializedProperty vrcnative_ShowFocus;
    SerializedProperty vrcnative_Streaming;
    SerializedProperty vrcnative_OrientationIsLandscape;
    SerializedProperty vrcnative_Hue;
    SerializedProperty vrcnative_Saturation;
    SerializedProperty vrcnative_Lightness;
    SerializedProperty vrcnative_SmoothingStrength;


    private void OnEnable()
    {
        cameraProperty = target as CameraProperty;

        if (!studioSystem) studioSystem = cameraProperty.transform.parent.GetComponent<StudioSystem>();

        foldout_general = serializedObject.FindProperty("foldout_general");
        foldout_focus = serializedObject.FindProperty("foldout_focus");
        foldout_lookat = serializedObject.FindProperty("foldout_lookat");
        foldout_cameraroot = serializedObject.FindProperty("foldout_cameraroot");
        foldout_misc = serializedObject.FindProperty("foldout_misc");
        foldout_vrclens = serializedObject.FindProperty("foldout_vrclens");
        foldout_virtuallens2 = serializedObject.FindProperty("foldout_virtuallens2");
        foldout_integral = serializedObject.FindProperty("foldout_integral");
        foldout_vrcnative = serializedObject.FindProperty("foldout_vrcnative");

        zoom = serializedObject.FindProperty("zoom");
        exposure = serializedObject.FindProperty("exposure");
        aperture = serializedObject.FindProperty("aperture");
        focus = serializedObject.FindProperty("focus");
        focusX = serializedObject.FindProperty("focusX");
        focusY = serializedObject.FindProperty("focusY");
        avatarLookAt = serializedObject.FindProperty("avatarLookAt");
        avatarLookAt_root = serializedObject.FindProperty("avatarLookAt_root");
        avatarLookAt_head = serializedObject.FindProperty("avatarLookAt_head");
        avatarLookAt_chest = serializedObject.FindProperty("avatarLookAt_chest");
        avatarLookAt_hips = serializedObject.FindProperty("avatarLookAt_hips");
        avatarLookAt_lfoot = serializedObject.FindProperty("avatarLookAt_lfoot");
        avatarLookAt_rfoot = serializedObject.FindProperty("avatarLookAt_rfoot");
        avatarLookAt_lhand = serializedObject.FindProperty("avatarLookAt_lhand");
        avatarLookAt_rhand = serializedObject.FindProperty("avatarLookAt_rhand");
        cameraRootP_active = serializedObject.FindProperty("cameraRootP_active");
        cameraRootP_avatar_root = serializedObject.FindProperty("cameraRootP_avatar_root");
        cameraRootP_avatar_head = serializedObject.FindProperty("cameraRootP_avatar_head");
        cameraRootP_avatar_chest = serializedObject.FindProperty("cameraRootP_avatar_chest");
        cameraRootP_avatar_hips = serializedObject.FindProperty("cameraRootP_avatar_hips");
        cameraRootP_avatar_lfoot = serializedObject.FindProperty("cameraRootP_avatar_lfoot");
        cameraRootP_avatar_rfoot = serializedObject.FindProperty("cameraRootP_avatar_rfoot");
        cameraRootP_avatar_lhand = serializedObject.FindProperty("cameraRootP_avatar_lhand");
        cameraRootP_avatar_rhand = serializedObject.FindProperty("cameraRootP_avatar_rhand");
        cameraRootR_active = serializedObject.FindProperty("cameraRootR_active");
        cameraRootR_avatar_root = serializedObject.FindProperty("cameraRootR_avatar_root");
        cameraRootR_avatar_head = serializedObject.FindProperty("cameraRootR_avatar_head");
        cameraRootR_avatar_chest = serializedObject.FindProperty("cameraRootR_avatar_chest");
        cameraRootR_avatar_hips = serializedObject.FindProperty("cameraRootR_avatar_hips");
        cameraRootR_avatar_lfoot = serializedObject.FindProperty("cameraRootR_avatar_lfoot");
        cameraRootR_avatar_rfoot = serializedObject.FindProperty("cameraRootR_avatar_rfoot");
        cameraRootR_avatar_lhand = serializedObject.FindProperty("cameraRootR_avatar_lhand");
        cameraRootR_avatar_rhand = serializedObject.FindProperty("cameraRootR_avatar_rhand");
        ap = serializedObject.FindProperty("ap");
        hidemodel = serializedObject.FindProperty("hidemodel");


        vrclens_WhiteBalance = serializedObject.FindProperty("vrclens_WhiteBalance");

        virtuallens2_LayerMask = serializedObject.FindProperty("virtuallens2_LayerMask");
        virtuallens2_AFMode = serializedObject.FindProperty("virtuallens2_AFMode");
        virtuallens2_AFSpeed = serializedObject.FindProperty("virtuallens2_AFSpeed");
        virtuallens2_PauseAF = serializedObject.FindProperty("virtuallens2_PauseAF");
        virtuallens2_CullingDistance = serializedObject.FindProperty("virtuallens2_CullingDistance");

        integral_ShootMode = serializedObject.FindProperty("integral_ShootMode");
        integral_ShutterSpeed = serializedObject.FindProperty("integral_ShutterSpeed");
        integral_GridMode = serializedObject.FindProperty("integral_GridMode");
        integral_FocusMode = serializedObject.FindProperty("integral_FocusMode");
        integral_PauseAF = serializedObject.FindProperty("integral_PauseAF");
        integral_Vignetting = serializedObject.FindProperty("integral_Vignetting");
        integral_AxialChromaticAberration = serializedObject.FindProperty("integral_AxialChromaticAberration");
        integral_BokehShape = serializedObject.FindProperty("integral_BokehShape");
        integral_BokehShapeRotation = serializedObject.FindProperty("integral_BokehShapeRotation");
        integral_CompositionMode = serializedObject.FindProperty("integral_CompositionMode");
        integral_RollingShutterMode = serializedObject.FindProperty("integral_RollingShutterMode");
        integral_Far = serializedObject.FindProperty("integral_Far");
        integral_Curve = serializedObject.FindProperty("integral_Curve");
        integral_AspectRatioCustomValue = serializedObject.FindProperty("integral_AspectRatioCustomValue");

        vrcnative_Mode = serializedObject.FindProperty("vrcnative_Mode");
        vrcnative_LayerMask = serializedObject.FindProperty("vrcnative_LayerMask");
        vrcnative_GreenScreen = serializedObject.FindProperty("vrcnative_GreenScreen");
        vrcnative_Lock = serializedObject.FindProperty("vrcnative_Lock");
        vrcnative_SmoothMovement = serializedObject.FindProperty("vrcnative_SmoothMovement");
        vrcnative_AutoLevelRoll = serializedObject.FindProperty("vrcnative_AutoLevelRoll");
        vrcnative_AutoLevelPitch = serializedObject.FindProperty("vrcnative_AutoLevelPitch");
        vrcnative_AudioFromCamera = serializedObject.FindProperty("vrcnative_AudioFromCamera");
        vrcnative_ShowFocus = serializedObject.FindProperty("vrcnative_ShowFocus");
        vrcnative_Streaming = serializedObject.FindProperty("vrcnative_Streaming");
        vrcnative_OrientationIsLandscape = serializedObject.FindProperty("vrcnative_OrientationIsLandscape");
        vrcnative_Hue = serializedObject.FindProperty("vrcnative_Hue");
        vrcnative_Saturation = serializedObject.FindProperty("vrcnative_Saturation");
        vrcnative_Lightness = serializedObject.FindProperty("vrcnative_Lightness");
        vrcnative_SmoothingStrength = serializedObject.FindProperty("vrcnative_SmoothingStrength");
    }

    public override void OnInspectorGUI()
    {
        currentLanguage = ((int)studioSystem.currentLanguage);

        GUILayout.Label(languageData[currentLanguage]["notice"], EditorStyles.normal());

        GUILayout.Space(12);

        foldout_general.boolValue = EditorGUILayout.Foldout(foldout_general.boolValue, languageData[currentLanguage]["settings_general"], EditorStyles.foldoutHeader());
        if (foldout_general.boolValue)
        {
            zoom.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["zoom"], zoom.floatValue, 0, 1);
            exposure.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["exposure"], exposure.floatValue, 0, 1);
            aperture.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["aperture"], aperture.floatValue, 0, 1);
        }

        GUILayout.Space(12);

        foldout_focus.boolValue = EditorGUILayout.Foldout(foldout_focus.boolValue, languageData[currentLanguage]["settings_focus"], EditorStyles.foldoutHeader());
        if (foldout_focus.boolValue)
        {
            if (studioSystem.cameraType != StudioSystem.ClientCameraType.VRChatNative)
            {
                GUILayout.Label(languageData[currentLanguage]["focus_info"], EditorStyles.normal());
                focus.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["focus"], focus.floatValue, 0, 1);

                focusX.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["focus_point_x"], focusX.floatValue, -1f, 1f);
                focusY.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["focus_point_y"], focusY.floatValue, -1f, 1f);

                Rect r = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, EditorGUIUtility.currentViewWidth * 0.5625f);
                GUI.Box(r, "");
                if (focus_point == null) focus_point = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/harunadev/PrismStudio/Assets/focus_point.png");

                GUI.Box(new Rect(-8 + r.x + (r.width / 2) + ((r.width / 2) * (focusX.floatValue)), -8 + r.y + (r.height / 2) + ((r.height / 2) * (focusY.floatValue * -1)), 16, 16), focus_point, new GUIStyle());

                if (r.Contains(Event.current.mousePosition) && Event.current.isMouse && (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown))
                {
                    focusX.floatValue = (Event.current.mousePosition.x - r.x - (r.width / 2)) * (1f / (r.width / 2));
                    focusY.floatValue = (Event.current.mousePosition.y - r.y - (r.height / 2)) * (-1f / (r.height / 2));
                }
            } else
            {
                focus.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["focus"], focus.floatValue, 0, 1);
            }
        }

        GUILayout.Space(12);
        foldout_lookat.boolValue = EditorGUILayout.Foldout(foldout_lookat.boolValue, languageData[currentLanguage]["settings_lookat"], EditorStyles.foldoutHeader());
        if (foldout_lookat.boolValue)
        {
            avatarLookAt.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["lookat"], avatarLookAt.floatValue, 0, 1);

            GUILayout.Space(12);
            GUILayout.Label(languageData[currentLanguage]["settings_lookatdirection"], EditorStyles.subheader());

            if (studioSystem.cameraType != StudioSystem.ClientCameraType.VRChatNative)
            {
                avatarLookAt_root.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["lookat_avatar_root"], avatarLookAt_root.floatValue, 0, 1);
                avatarLookAt_head.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["lookat_avatar_head"], avatarLookAt_head.floatValue, 0, 1);
                avatarLookAt_chest.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["lookat_avatar_chest"], avatarLookAt_chest.floatValue, 0, 1);
                avatarLookAt_hips.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["lookat_avatar_hips"], avatarLookAt_hips.floatValue, 0, 1);
                avatarLookAt_lfoot.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["lookat_avatar_lfoot"], avatarLookAt_lfoot.floatValue, 0, 1);
                avatarLookAt_rfoot.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["lookat_avatar_rfoot"], avatarLookAt_rfoot.floatValue, 0, 1);
                avatarLookAt_lhand.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["lookat_avatar_lhand"], avatarLookAt_lhand.floatValue, 0, 1);
                avatarLookAt_rhand.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["lookat_avatar_rhand"], avatarLookAt_rhand.floatValue, 0, 1);
            }

            EditorGUILayout.PropertyField(ap, new GUIContent(languageData[currentLanguage]["lookat_offset_vector"]));
        }

        if (studioSystem.cameraType != StudioSystem.ClientCameraType.VRChatNative)
        {
            GUILayout.Space(12);
            foldout_cameraroot.boolValue = EditorGUILayout.Foldout(foldout_cameraroot.boolValue, languageData[currentLanguage]["settings_cameraroot"], EditorStyles.foldoutHeader());
            if (foldout_cameraroot.boolValue)
            {
                cameraRootP_active.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootp_active"], cameraRootP_active.floatValue, 0, 1);

                cameraRootP_avatar_root.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootp_avatar_root"], cameraRootP_avatar_root.floatValue, 0, 1);
                cameraRootP_avatar_head.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootp_avatar_head"], cameraRootP_avatar_head.floatValue, 0, 1);
                cameraRootP_avatar_chest.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootp_avatar_chest"], cameraRootP_avatar_chest.floatValue, 0, 1);
                cameraRootP_avatar_hips.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootp_avatar_hips"], cameraRootP_avatar_hips.floatValue, 0, 1);
                cameraRootP_avatar_lfoot.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootp_avatar_lfoot"], cameraRootP_avatar_lfoot.floatValue, 0, 1);
                cameraRootP_avatar_rfoot.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootp_avatar_rfoot"], cameraRootP_avatar_rfoot.floatValue, 0, 1);
                cameraRootP_avatar_lhand.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootp_avatar_lhand"], cameraRootP_avatar_lhand.floatValue, 0, 1);
                cameraRootP_avatar_rhand.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootp_avatar_rhand"], cameraRootP_avatar_rhand.floatValue, 0, 1);

                GUILayout.Space(12);

                cameraRootR_active.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootr_active"], cameraRootR_active.floatValue, 0, 1);

                cameraRootR_avatar_root.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootr_avatar_root"], cameraRootR_avatar_root.floatValue, 0, 1);
                cameraRootR_avatar_head.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootr_avatar_head"], cameraRootR_avatar_head.floatValue, 0, 1);
                cameraRootR_avatar_chest.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootr_avatar_chest"], cameraRootR_avatar_chest.floatValue, 0, 1);
                cameraRootR_avatar_hips.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootr_avatar_hips"], cameraRootR_avatar_hips.floatValue, 0, 1);
                cameraRootR_avatar_lfoot.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootr_avatar_lfoot"], cameraRootR_avatar_lfoot.floatValue, 0, 1);
                cameraRootR_avatar_rfoot.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootr_avatar_rfoot"], cameraRootR_avatar_rfoot.floatValue, 0, 1);
                cameraRootR_avatar_lhand.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootr_avatar_lhand"], cameraRootR_avatar_lhand.floatValue, 0, 1);
                cameraRootR_avatar_rhand.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["camerarootr_avatar_rhand"], cameraRootR_avatar_rhand.floatValue, 0, 1);

            }
        }

        GUILayout.Space(12);
        foldout_misc.boolValue = EditorGUILayout.Foldout(foldout_misc.boolValue, languageData[currentLanguage]["settings_etc"], EditorStyles.foldoutHeader());
        if (foldout_misc.boolValue)
        {
            hidemodel.boolValue = EditorGUILayout.Toggle(languageData[currentLanguage]["model_hide"], hidemodel.boolValue);
        }
        
        GUILayout.Space(12);
        switch (studioSystem.cameraType)
        {
            case StudioSystem.ClientCameraType.VRCLens:
                {
                    foldout_vrclens.boolValue   = EditorGUILayout.Foldout(foldout_vrclens.boolValue, languageData[currentLanguage]["vrclens"], EditorStyles.foldoutHeader());
                    if (foldout_vrclens.boolValue)
                    {
                        vrclens_WhiteBalance.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["vrclens_whitebalance"], vrclens_WhiteBalance.floatValue, 0, 1);
                    }

                    GUILayout.Space(12);
                    foldout_vrcnative.boolValue = EditorGUILayout.Foldout(foldout_vrcnative.boolValue, languageData[currentLanguage]["vrcnative"], EditorStyles.foldoutHeader());
                    if (foldout_vrcnative.boolValue)
                    {
                        if (GUILayout.Button(languageData[currentLanguage]["vrcnative_Capture"]))
                        {
                            cameraProperty.VRCNative_TakePicture(studioSystem);
                        }

                        if (GUILayout.Button(languageData[currentLanguage]["vrcnative_CaptureDelayed"]))
                        {
                            cameraProperty.VRCNative_TakeTimerPicture(studioSystem);
                        }
                    }
                }
                break;
            case StudioSystem.ClientCameraType.VirtualLens2:
                {
                    foldout_virtuallens2.boolValue = EditorGUILayout.Foldout(foldout_virtuallens2.boolValue , languageData[currentLanguage]["virtuallens2"], EditorStyles.foldoutHeader());
                    if (foldout_virtuallens2.boolValue)
                    {
                        virtuallens2_layermask[0] = languageData[currentLanguage]["virtuallens2_layermask_local"];
                        virtuallens2_layermask[1] = languageData[currentLanguage]["virtuallens2_layermask_remote"];
                        virtuallens2_layermask[2] = languageData[currentLanguage]["virtuallens2_layermask_ui"];

                        virtuallens2_afmode[0] = languageData[currentLanguage]["virtuallens2_autofocus_point"];
                        virtuallens2_afmode[1] = languageData[currentLanguage]["virtuallens2_autofocus_face"];
                        virtuallens2_afmode[2] = languageData[currentLanguage]["virtuallens2_autofocus_selfie"];

                        virtuallens2_afspeed[0] = languageData[currentLanguage]["virtuallens2_autofocusspeed_immediate"];
                        virtuallens2_afspeed[1] = languageData[currentLanguage]["virtuallens2_autofocusspeed_fast"];
                        virtuallens2_afspeed[2] = languageData[currentLanguage]["virtuallens2_autofocusspeed_medium"];
                        virtuallens2_afspeed[3] = languageData[currentLanguage]["virtuallens2_autofocusspeed_slow"];

                        virtuallens2_cullingdistance[0] = languageData[currentLanguage]["virtuallens2_cullingdistance_default"];
                        virtuallens2_cullingdistance[1] = languageData[currentLanguage]["virtuallens2_cullingdistance_x10"];
                        virtuallens2_cullingdistance[2] = languageData[currentLanguage]["virtuallens2_cullingdistance_x100"];

                        {
                            int temp_flag = 0;
                            int temp_initialflag = 0;
                            if (cameraProperty.virtuallens2_LayerMask.LocalPlayer)
                            {
                                temp_flag |= (1 << 0);
                            } else
                            {
                                temp_flag &= ~(1 << 0);
                            }
                            if (cameraProperty.virtuallens2_LayerMask.RemotePlayer)
                            {
                                temp_flag |= (1 << 1);
                            } else
                            {
                                temp_flag &= ~(1 << 1);
                            }
                            if (cameraProperty.virtuallens2_LayerMask.UI)
                            {
                                temp_flag |= (1 << 2);
                            } else
                            {
                                temp_flag &= ~(1 << 2);
                            }
                            temp_initialflag = temp_flag;
                            temp_flag = EditorGUILayout.MaskField(new GUIContent(languageData[currentLanguage]["virtuallens2_layermask"]), temp_flag, virtuallens2_layermask);
                            if (temp_initialflag != temp_flag)
                            {
                                Undo.RecordObject(cameraProperty, "Modify VirtualLens2 LayerMask");
                                VirtualLens2_LayerMaskConfig temp_config = new VirtualLens2_LayerMaskConfig();
                                for (int i = 0; i < virtuallens2_layermask.Length; i++)
                                {
                                    bool result = (temp_flag & (1 << i)) == (1 << i);

                                    switch (i)
                                    {
                                        case 0:
                                            temp_config.LocalPlayer = result;
                                            break;
                                        case 1:
                                            temp_config.RemotePlayer = result;
                                            break;
                                        case 2:
                                            temp_config.UI = result;
                                            break;
                                    }
                                }
                                cameraProperty.virtuallens2_LayerMask = temp_config;
                                EditorUtility.SetDirty(cameraProperty);
                            }
                        }

                        virtuallens2_AFMode.enumValueIndex = EditorGUILayout.Popup(new GUIContent(languageData[currentLanguage]["virtuallens2_autofocus"]), virtuallens2_AFMode.enumValueIndex, virtuallens2_afmode);

                        virtuallens2_AFSpeed.enumValueIndex = EditorGUILayout.Popup(new GUIContent(languageData[currentLanguage]["virtuallens2_autofocusspeed"]), virtuallens2_AFSpeed.enumValueIndex, virtuallens2_afspeed);
                        if (virtuallens2_AFSpeed.enumValueIndex > 0)
                        {
                            EditorGUILayout.HelpBox(languageData[currentLanguage]["virtuallens2_autofocusspeed_info"], MessageType.Info);
                            GUILayout.Space(6);
                        }
                        virtuallens2_PauseAF.boolValue = EditorGUILayout.Toggle(new GUIContent(languageData[currentLanguage]["virtuallens2_pauseautofocus"]), virtuallens2_PauseAF.boolValue);

                        virtuallens2_CullingDistance.enumValueIndex = EditorGUILayout.Popup(new GUIContent(languageData[currentLanguage]["virtuallens2_cullingdistance"]), virtuallens2_CullingDistance.enumValueIndex, virtuallens2_cullingdistance);
                    }

                    GUILayout.Space(12);
                    foldout_vrcnative.boolValue = EditorGUILayout.Foldout(foldout_vrcnative.boolValue, languageData[currentLanguage]["vrcnative"], EditorStyles.foldoutHeader());
                    if (foldout_vrcnative.boolValue)
                    {
                        if (GUILayout.Button(languageData[currentLanguage]["vrcnative_Capture"]))
                        {
                            cameraProperty.VRCNative_TakePicture(studioSystem);
                        }

                        if (GUILayout.Button(languageData[currentLanguage]["vrcnative_CaptureDelayed"]))
                        {
                            cameraProperty.VRCNative_TakeTimerPicture(studioSystem);
                        }
                    }
                }
                break;
            case StudioSystem.ClientCameraType.Integral:
                {
                    foldout_integral.boolValue = EditorGUILayout.Foldout(foldout_integral.boolValue, languageData[currentLanguage]["integral"], EditorStyles.foldoutHeader());
                    if (foldout_integral.boolValue)
                    {
                        integral_shootmode[0] = languageData[currentLanguage]["integral_shoot_none"];
                        integral_shootmode[1] = languageData[currentLanguage]["integral_shoot_clear"];
                        integral_shootmode[2] = languageData[currentLanguage]["integral_shoot_open"];
                        integral_shootmode[3] = languageData[currentLanguage]["integral_shoot_bulb"];
                        integral_shootmode[4] = languageData[currentLanguage]["integral_shoot_clearnopen"];
                        integral_shootmode[5] = languageData[currentLanguage]["integral_shoot_single"];
                        integral_shootmode[6] = languageData[currentLanguage]["integral_shoot_ss"];

                        integral_bokehshape[0] = languageData[currentLanguage]["integral_bokehshape_circle"];
                        integral_bokehshape[1] = languageData[currentLanguage]["integral_bokehshape_hexagon"];
                        integral_bokehshape[2] = languageData[currentLanguage]["integral_bokehshape_star"];
                        integral_bokehshape[3] = languageData[currentLanguage]["integral_bokehshape_heart"];
                        integral_bokehshape[4] = languageData[currentLanguage]["integral_bokehshape_bubble"];
                        
                        integral_curve[0] = languageData[currentLanguage]["integral_curve_linear"];
                        integral_curve[1] = languageData[currentLanguage]["integral_curve_log"];
                        
                        integral_focusmode[0] = languageData[currentLanguage]["integral_focusmode_manual"];
                        integral_focusmode[1] = languageData[currentLanguage]["integral_focusmode_af"];
                        integral_focusmode[2] = languageData[currentLanguage]["integral_focusmode_motionaf"];

                        integral_compositionmode[0] = languageData[currentLanguage]["integral_compositionmode_normal"];
                        integral_compositionmode[1] = languageData[currentLanguage]["integral_compositionmode_lighten"];
                        integral_compositionmode[2] = languageData[currentLanguage]["integral_compositionmode_darken"];
                        integral_compositionmode[3] = languageData[currentLanguage]["integral_compositionmode_depth"];

                        integral_rollingshuttermode[0] = languageData[currentLanguage]["integral_rollingshuttermode_off"];
                        integral_rollingshuttermode[1] = languageData[currentLanguage]["integral_rollingshuttermode_ltr"];
                        integral_rollingshuttermode[2] = languageData[currentLanguage]["integral_rollingshuttermode_ttb"];
                        integral_rollingshuttermode[3] = languageData[currentLanguage]["integral_rollingshuttermode_rtl"];
                        integral_rollingshuttermode[4] = languageData[currentLanguage]["integral_rollingshuttermode_btt"];

                        integral_aspectratio[0] = languageData[currentLanguage]["integral_aspectratio_16_9"];
                        integral_aspectratio[1] = languageData[currentLanguage]["integral_aspectratio_4_3"];
                        integral_aspectratio[2] = languageData[currentLanguage]["integral_aspectratio_3_2"];
                        integral_aspectratio[3] = languageData[currentLanguage]["integral_aspectratio_1_1"];
                        integral_aspectratio[4] = languageData[currentLanguage]["integral_aspectratio_2_35_1"];
                        integral_aspectratio[5] = languageData[currentLanguage]["integral_aspectratio_7_6"];
                        integral_aspectratio[6] = languageData[currentLanguage]["integral_aspectratio_5_4"];
                        integral_aspectratio[7] = languageData[currentLanguage]["integral_aspectratio_7_5"];
                        integral_aspectratio[8] = languageData[currentLanguage]["integral_aspectratio_2_1"];
                        integral_aspectratio[9] = languageData[currentLanguage]["integral_aspectratio_custom"];

                        {
                            bool doNothing = false;

                            integral_ShootMode.enumValueIndex = EditorGUILayout.Popup(integral_ShootMode.enumValueIndex, integral_shootmode);

                            if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_clear"], button()))
                            {
                                integral_ShootMode.enumValueIndex = 1;
                                doNothing = true;
                            } else
                            {
                                if (integral_ShootMode.enumValueIndex == 1)
                                {
                                    integral_ShootMode.enumValueIndex = 0;
                                    doNothing = true;
                                }
                            }

                            if (integral_ShootMode.enumValueIndex == 2)
                            {
                                if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_open"], button_activated()))
                                {
                                    if (!doNothing)
                                    {
                                        integral_ShootMode.enumValueIndex = 0;
                                        doNothing = true;
                                    }
                                }
                            }
                            else
                            {
                                if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_open"]))
                                {
                                    if (!doNothing)
                                    {
                                        integral_ShootMode.enumValueIndex = 2;
                                        doNothing = true;
                                    }
                                }
                            }

                            if (integral_ShootMode.enumValueIndex == 3)
                            {
                                if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_bulb"], button_activated()))
                                {
                                    if (!doNothing)
                                    {
                                        integral_ShootMode.enumValueIndex = 0;
                                        doNothing = true;
                                    }
                                }
                            }
                            else
                            {
                                if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_bulb"]))
                                {
                                    if (!doNothing)
                                    {
                                        integral_ShootMode.enumValueIndex = 3;
                                        doNothing = true;
                                    }
                                }
                            }

                            if (integral_ShootMode.enumValueIndex == 4)
                            {
                                if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_clearnopen"], button_activated()))
                                {
                                    if (!doNothing)
                                    {
                                        integral_ShootMode.enumValueIndex = 0;
                                        doNothing = true;
                                    }
                                }
                            }
                            else
                            {
                                if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_clearnopen"]))
                                {
                                    if (!doNothing)
                                    {
                                        integral_ShootMode.enumValueIndex = 4;
                                        doNothing = true;
                                    }
                                }
                            }

                            if (integral_ShootMode.enumValueIndex == 5)
                            {
                                if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_single"], button_activated()))
                                {
                                    if (!doNothing)
                                    {
                                        integral_ShootMode.enumValueIndex = 0;
                                        doNothing = true;
                                    }
                                }
                            }
                            else
                            {
                                if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_single"]))
                                {
                                    if (!doNothing)
                                    {
                                        integral_ShootMode.enumValueIndex = 5;
                                        doNothing = true;
                                    }
                                }
                            }

                            if (integral_ShootMode.enumValueIndex == 6)
                            {
                                if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_ss"], button_activated()))
                                {
                                    if (!doNothing)
                                    {
                                        integral_ShootMode.enumValueIndex = 0;
                                        doNothing = true;
                                    }
                                }
                            }
                            else
                            {
                                if (GUILayout.Button(languageData[currentLanguage]["integral_shoot_ss"]))
                                {
                                    if (!doNothing)
                                    {
                                        integral_ShootMode.enumValueIndex = 6;
                                        doNothing = true;
                                    }
                                }
                            }
                        }

                        integral_ShutterSpeed.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["integral_shutterspeed"], integral_ShutterSpeed.floatValue, 0, 1);

                        integral_GridMode.boolValue = EditorGUILayout.Toggle(languageData[currentLanguage]["integral_showgrid"], integral_GridMode.boolValue);

                        {
                            integral_FocusMode.enumValueIndex = EditorGUILayout.Popup(new GUIContent(languageData[currentLanguage]["integral_focusmode"]), integral_FocusMode.enumValueIndex, integral_focusmode);
                            integral_PauseAF.boolValue = EditorGUILayout.Toggle(languageData[currentLanguage]["integral_pauseaf"], integral_PauseAF.boolValue);
                        }

                        integral_Vignetting.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["integral_vignette"], integral_Vignetting.floatValue, 0, 1);
                        
                        integral_AxialChromaticAberration.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["integral_axialchromaticaberration"], integral_AxialChromaticAberration.floatValue, 0, 1);

                        {
                            integral_BokehShape.enumValueIndex = EditorGUILayout.Popup(new GUIContent(languageData[currentLanguage]["integral_bokehshape"]), integral_BokehShape.enumValueIndex, integral_bokehshape);
                            integral_BokehShapeRotation.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["integral_bokehshape_rotation"], integral_BokehShapeRotation.floatValue, 0, 1);
                        }

                        {
                            integral_CompositionMode.enumValueIndex = EditorGUILayout.Popup(new GUIContent(languageData[currentLanguage]["integral_compositionmode"]), integral_CompositionMode.enumValueIndex, integral_compositionmode);
                        }

                        {
                            integral_RollingShutterMode.enumValueIndex = EditorGUILayout.Popup(new GUIContent(languageData[currentLanguage]["integral_rollingshuttermode"]), integral_RollingShutterMode.enumValueIndex, integral_rollingshuttermode);
                        }

                        integral_Far.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["integral_far"], integral_Far.floatValue, 0, 1);

                        {
                            integral_Curve.enumValueIndex = EditorGUILayout.Popup(new GUIContent(languageData[currentLanguage]["integral_curve"]), integral_Curve.enumValueIndex, integral_curve);
                        }

                        {
                            integral_AspectRatioCustomValue.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["integral_aspectratio"], integral_AspectRatioCustomValue.floatValue, 0, 1);
                            cameraProperty.integral_AspectRatio = (CameraProperty.Integral_AspectRatioPreset)EditorGUILayout.Popup(((int)cameraProperty.integral_AspectRatio), integral_aspectratio);
                        }
                    }

                    GUILayout.Space(12);
                    foldout_vrcnative.boolValue = EditorGUILayout.Foldout(foldout_vrcnative.boolValue, languageData[currentLanguage]["vrcnative"], EditorStyles.foldoutHeader());
                    if (foldout_vrcnative.boolValue)
                    {
                        if (GUILayout.Button(languageData[currentLanguage]["vrcnative_Capture"]))
                        {
                            cameraProperty.VRCNative_TakePicture(studioSystem);
                        }

                        if (GUILayout.Button(languageData[currentLanguage]["vrcnative_CaptureDelayed"]))
                        {
                            cameraProperty.VRCNative_TakeTimerPicture(studioSystem);
                        }
                    }
                }
                break;
            case StudioSystem.ClientCameraType.VRChatNative:
                {
                    foldout_vrcnative.boolValue = EditorGUILayout.Foldout(foldout_vrcnative.boolValue, languageData[currentLanguage]["vrcnative"], EditorStyles.foldoutHeader());
                    if (foldout_vrcnative.boolValue)
                    {
                        vrcnative_layermask[0] = languageData[currentLanguage]["virtuallens2_layermask_local"];
                        vrcnative_layermask[1] = languageData[currentLanguage]["virtuallens2_layermask_remote"];
                        vrcnative_layermask[2] = languageData[currentLanguage]["virtuallens2_layermask_ui"];
                        vrcnative_layermask[3] = languageData[currentLanguage]["vrcnative_layermask_environment"];

                        EditorGUILayout.PropertyField(vrcnative_Mode, new GUIContent(languageData[currentLanguage]["vrcnative_Mode"]));

                        if (GUILayout.Button(languageData[currentLanguage]["vrcnative_Capture"]))
                        {
                            cameraProperty.VRCNative_TakePicture(studioSystem);
                        }

                        if (GUILayout.Button(languageData[currentLanguage]["vrcnative_CaptureDelayed"]))
                        {
                            cameraProperty.VRCNative_TakeTimerPicture(studioSystem);
                        }

                        {
                            int temp_flag = 0;
                            int temp_initialflag = 0;
                            if (cameraProperty.vrcnative_LayerMask.LocalPlayer)
                            {
                                temp_flag |= (1 << 0);
                            }
                            else
                            {
                                temp_flag &= ~(1 << 0);
                            }
                            if (cameraProperty.vrcnative_LayerMask.RemotePlayer)
                            {
                                temp_flag |= (1 << 1);
                            }
                            else
                            {
                                temp_flag &= ~(1 << 1);
                            }
                            if (cameraProperty.vrcnative_LayerMask.UI)
                            {
                                temp_flag |= (1 << 2);
                            }
                            else
                            {
                                temp_flag &= ~(1 << 2);
                            }
                            if (cameraProperty.vrcnative_LayerMask.Environment)
                            {
                                temp_flag |= (1 << 3);
                            }
                            else
                            {
                                temp_flag &= ~(1 << 3);
                            }
                            temp_initialflag = temp_flag;
                            temp_flag = EditorGUILayout.MaskField(new GUIContent(languageData[currentLanguage]["vrcnative_LayerMask"]), temp_flag, vrcnative_layermask);
                            if (temp_initialflag != temp_flag)
                            {
                                Undo.RecordObject(cameraProperty, "Modify VRCNative Layer Mask");
                                VRCNative_LayerMaskConfig temp_config = new VRCNative_LayerMaskConfig();
                                for (int i = 0; i < vrcnative_layermask.Length; i++)
                                {
                                    bool result = (temp_flag & (1 << i)) == (1 << i);

                                    switch (i)
                                    {
                                        case 0:
                                            temp_config.LocalPlayer = result;
                                            break;
                                        case 1:
                                            temp_config.RemotePlayer = result;
                                            break;
                                        case 2:
                                            temp_config.UI = result;
                                            break;
                                        case 3:
                                            temp_config.Environment = result;
                                            break;
                                    }
                                }
                                cameraProperty.vrcnative_LayerMask = temp_config;
                                EditorUtility.SetDirty(cameraProperty);
                            }
                        }

                        EditorGUILayout.PropertyField(vrcnative_LayerMask             , new GUIContent(languageData[currentLanguage]["vrcnative_LayerMask"]));
                        EditorGUILayout.PropertyField(vrcnative_GreenScreen           , new GUIContent(languageData[currentLanguage]["vrcnative_GreenScreen"]));
                        EditorGUILayout.PropertyField(vrcnative_Lock                  , new GUIContent(languageData[currentLanguage]["vrcnative_Lock"]));
                        EditorGUILayout.PropertyField(vrcnative_SmoothMovement        , new GUIContent(languageData[currentLanguage]["vrcnative_SmoothMovement"]));
                        EditorGUILayout.PropertyField(vrcnative_AutoLevelRoll         , new GUIContent(languageData[currentLanguage]["vrcnative_AutoLevelRoll"]));
                        EditorGUILayout.PropertyField(vrcnative_AutoLevelPitch        , new GUIContent(languageData[currentLanguage]["vrcnative_AutoLevelPitch"]));
                        EditorGUILayout.PropertyField(vrcnative_AudioFromCamera       , new GUIContent(languageData[currentLanguage]["vrcnative_AudioFromCamera"]));
                        EditorGUILayout.PropertyField(vrcnative_ShowFocus             , new GUIContent(languageData[currentLanguage]["vrcnative_ShowFocus"]));
                        EditorGUILayout.PropertyField(vrcnative_Streaming             , new GUIContent(languageData[currentLanguage]["vrcnative_Streaming"]));
                        EditorGUILayout.PropertyField(vrcnative_OrientationIsLandscape, new GUIContent(languageData[currentLanguage]["vrcnative_OrientationIsLandscape"]));

                        vrcnative_Hue.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["vrcnative_hue"], vrcnative_Hue.floatValue, 0, 360);
                        vrcnative_Saturation.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["vrcnative_saturation"], vrcnative_Saturation.floatValue, 0, 100);
                        vrcnative_Lightness.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["vrcnative_lightness"], vrcnative_Lightness.floatValue, 0, 60);
                        vrcnative_SmoothingStrength.floatValue = EditorGUILayout.Slider(languageData[currentLanguage]["vrcnative_smoothingstrength"], vrcnative_SmoothingStrength.floatValue, 0.1f, 10);
                    }
                }
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    GUIStyle button() 
    {
        GUIStyle style = new GUIStyle("button");
        style.richText = true;
        return style;
    }
    GUIStyle button_activated()
    {
        GUIStyle style = new GUIStyle("button");
        style.normal.textColor = Color.green;
        style.hover.textColor = Color.green;
        style.focused.textColor = Color.green;
        style.active.textColor = Color.green;
        style.onNormal.textColor = Color.green;
        style.onHover.textColor = Color.green;
        style.onFocused.textColor = Color.green;
        style.onActive.textColor = Color.green;
        return style;
    }
}

#endif