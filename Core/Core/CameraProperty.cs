using OscCore;
using System;
using UnityEngine;
using static PrismStudio.I18n;
using static PrismStudio.StudioSystem;


namespace PrismStudio
{
    /// <summary>
    /// PrismStudio Core: CameraProperty
    /// (c) harunadev
    /// VRCLens is a property owned by Hirabiki.
    /// VirtualLens2 is a property owned by logilabo (ろじらぼ).
    /// </summary>
    public class CameraProperty : Module
    {
        /// <summary>
        /// Description of CameraProperty
        /// | CameraProperty 설명
        /// </summary>
        /// <param name="lang">language</param>
        /// <returns></returns>
        public override string description(SupportedLanguage lang)
        {
            switch (lang)
            {
                case SupportedLanguage.Korean:
                    {
                        return "PrismStudio의 기본 기능; 카메라 속성을 관리합니다";
                    }
                case SupportedLanguage.Japanese:
                    {
                        return "PrismStudioの基本機能; カメラのプロパティを管理します";
                    }
            }
            return "Core function of PrismStudio; Manages the camera properties";
        }

        /// <summary>
        /// Zoom value 
        /// | 확대값
        /// </summary>
        [SerializeField]
        public float zoom = .38f;

        /// <summary>
        /// Exposure value
        /// | 노출값
        /// </summary>
        [SerializeField]
        public float exposure = .5f;

        /// <summary>
        /// Aperture value
        /// | 조리개값
        /// </summary>
        [SerializeField]
        public float aperture = .38f;

        /// <summary>
        /// Focus value, 0  ->  Auto, n > 0  ->  Manual 
        /// | 초점값, 0  =>  자동, n > 0  => 수동
        /// </summary>
        [SerializeField]
        public float focus = 0;

        /// <summary>
        /// X axis of focus point at 2D plane, only works for manual focus 
        /// | 초점 포인트의 X좌표값, 수동초첨중 사용가능
        /// </summary>
        [SerializeField]
        public float focusX = 0;

        /// <summary>
        /// Y axis of focus point at 2D plane, only works for manual focus 
        /// | 초점 포인트의 Y좌표값, 수동초점중 사용가능
        /// </summary>
        [SerializeField]
        public float focusY = 0;

        /// <summary>
        /// Look at local avatar, ignores PrismStudio initial angle 
        /// | 로컬 아바타를 바라보기, PrismStudio의 각도값을 무시함
        /// </summary>
        [SerializeField]
        public float avatarLookAt = 0;

        /// <summary>
        /// Offset Vector3 for Look at local avatar 
        /// | 로컬 아바타 바라보기에 적용되는 오프셋 좌표
        /// </summary>
        [SerializeField]
        public Vector3 ap = Vector3.zero;

        /// <summary>
        /// Look at local avatar's root 
        /// | 로컬 아바타의 Root 바라보기
        /// </summary>
        [SerializeField]
        public float avatarLookAt_root = 0;

        /// <summary>
        /// Look at local avatar's head 
        /// | 로컬 아바타의 머리 바라보기
        /// </summary>
        [SerializeField]
        public float avatarLookAt_head = 0;

        /// <summary>
        /// Look at local avatar's chest 
        /// | 로컬 아바타의 가슴 바라보기
        /// </summary>
        [SerializeField]
        public float avatarLookAt_chest = 0;

        /// <summary>
        /// Look at local avatar's hips 
        /// | 로컬 아바타의 허리 바라보기
        /// </summary>
        [SerializeField]
        public float avatarLookAt_hips = 0;

        /// <summary>
        /// Look at local avatar's left foot 
        /// | 로컬 아바타의 왼발 바라보기
        /// </summary>
        [SerializeField]
        public float avatarLookAt_lfoot = 0;

        /// <summary>
        /// Look at local avatar's right foot 
        /// | 로컬 아바타의 오른발 바라보기
        /// </summary>
        [SerializeField]
        public float avatarLookAt_rfoot = 0;

        /// <summary>
        /// Look at local avatar's left hand 
        /// | 로컬 아바타의 왼손 바라보기
        /// </summary>
        [SerializeField]
        public float avatarLookAt_lhand = 0;

        /// <summary>
        /// Look at local avatar's right hand 
        /// | 로컬 아바타의 오른손 바라보기
        /// </summary>
        [SerializeField]
        public float avatarLookAt_rhand = 0;

        /// <summary>
        /// 1 = Use provided position by PrismStudio
        /// | 1 = PrismStudio에서 제공하는 위치를 사용
        /// </summary>
        [SerializeField] public float cameraRootP_active = 1;
        /// <summary>
        /// Use avatar root as PrismStudio root position
        /// | 아바타 root 위치를 PrismStudio 기준 위치로 사용
        /// </summary>
        [SerializeField] public float cameraRootP_avatar_root = 0;
        /// <summary>
        /// Use avatar head as PrismStudio root position
        /// | 아바타 머리 위치를 PrismStudio 기준 위치로 사용
        /// </summary>
        [SerializeField] public float cameraRootP_avatar_head = 0;
        /// <summary>
        /// Use avatar chest as PrismStudio root position
        /// | 아바타 가슴 위치를 PrismStudio 기준 위치로 사용
        /// </summary>
        [SerializeField] public float cameraRootP_avatar_chest = 0;
        /// <summary>
        /// Use avatar hips as PrismStudio root position
        /// | 아바타 허리 위치를 PrismStudio 기준 위치로 사용
        /// </summary>
        [SerializeField] public float cameraRootP_avatar_hips = 0;
        /// <summary>
        /// Use avatar left foot as PrismStudio root position
        /// | 아바타 왼쪽 발 위치를 PrismStudio 기준 위치로 사용
        /// </summary>
        [SerializeField] public float cameraRootP_avatar_lfoot = 0;
        /// <summary>
        /// Use avatar right foot as PrismStudio root position
        /// | 아바타 오른쪽 발 위치를 PrismStudio 기준 위치로 사용
        /// </summary>
        [SerializeField] public float cameraRootP_avatar_rfoot = 0;
        /// <summary>
        /// Use avatar left hand as PrismStudio root position
        /// | 아바타 왼손 위치를 PrismStudio 기준 위치로 사용
        /// </summary>
        [SerializeField] public float cameraRootP_avatar_lhand = 0;
        /// <summary>
        /// Use avatar right hand as PrismStudio root position
        /// | 아바타 오른손 위치를 PrismStudio 기준 위치로 사용
        /// </summary>
        [SerializeField] public float cameraRootP_avatar_rhand = 0;
        /// <summary>
        /// 1 = Use provided rotation by PrismStudio
        /// | 1 = PrismStudio에서 제공하는 각도를 사용
        /// </summary>
        [SerializeField] public float cameraRootR_active = 1;
        /// <summary>
        /// Use avatar root as PrismStudio root rotation
        /// | 아바타 root 위치를 PrismStudio 기준 각도로 사용
        /// </summary>
        [SerializeField] public float cameraRootR_avatar_root = 0;
        /// <summary>
        /// Use avatar head as PrismStudio root rotation
        /// | 아바타 머리 위치를 PrismStudio 기준 각도로 사용
        /// </summary>
        [SerializeField] public float cameraRootR_avatar_head = 0;
        /// <summary>
        /// Use avatar chest as PrismStudio root rotation
        /// | 아바타 가슴 위치를 PrismStudio 기준 각도로 사용
        /// </summary>
        [SerializeField] public float cameraRootR_avatar_chest = 0;
        /// <summary>
        /// Use avatar hips as PrismStudio root rotation
        /// | 아바타 허리 위치를 PrismStudio 기준 각도로 사용
        /// </summary>
        [SerializeField] public float cameraRootR_avatar_hips = 0;
        /// <summary>
        /// Use avatar left foot as PrismStudio root rotation
        /// | 아바타 왼쪽 발 위치를 PrismStudio 기준 각도로 사용
        /// </summary>
        [SerializeField] public float cameraRootR_avatar_lfoot = 0;
        /// <summary>
        /// Use avatar right foot as PrismStudio root rotation
        /// | 아바타 오른쪽 발 위치를 PrismStudio 기준 각도로 사용
        /// </summary>
        [SerializeField] public float cameraRootR_avatar_rfoot = 0;
        /// <summary>
        /// Use avatar left hand as PrismStudio root rotation
        /// | 아바타 왼손 위치를 PrismStudio 기준 각도로 사용
        /// </summary>
        [SerializeField] public float cameraRootR_avatar_lhand = 0;
        /// <summary>
        /// Use avatar right hand as PrismStudio root rotation
        /// | 아바타 오른손 위치를 PrismStudio 기준 각도로 사용
        /// </summary>
        [SerializeField] public float cameraRootR_avatar_rhand = 0;

        /// <summary>
        /// Hide VRCLens Camera 3D model (functions not affected)
        /// VRCLens 카메라 3D 모델 숨기기 (기능에 영향 없음)
        /// </summary>
        [SerializeField]
        public bool hidemodel = false;

        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_general;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_focus;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_lookat;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_cameraroot;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_misc;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_vrclens;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_virtuallens2;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_integral;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_vrcnative;

        OscMessage __zoom = new OscMessage("/avatar/parameters/VRCLZoomRadial", 0f);
        OscMessage __exposure = new OscMessage("/avatar/parameters/VRCLExposureRadial", 0f);
        OscMessage __aperture = new OscMessage("/avatar/parameters/VRCLApertureRadial", 0f);
        OscMessage __focus = new OscMessage("/avatar/parameters/VRCLFocusRadial", 0f);
        OscMessage __focusX = new OscMessage("/avatar/parameters/ps_fx", 0f);
        OscMessage __focusY = new OscMessage("/avatar/parameters/ps_fy", 0f);
        OscMessage __avatarLookAt = new OscMessage("/avatar/parameters/ps_l", 0f);
        OscMessage __ax = new OscMessage("/avatar/parameters/ps_lx", 0f);
        OscMessage __ay = new OscMessage("/avatar/parameters/ps_ly", 0f);
        OscMessage __az = new OscMessage("/avatar/parameters/ps_lz", 0f);
        OscMessage __avatarLookAt_root = new OscMessage("/avatar/parameters/ps_l_root", 0f);
        OscMessage __avatarLookAt_head = new OscMessage("/avatar/parameters/ps_l_head", 0f);
        OscMessage __avatarLookAt_chest = new OscMessage("/avatar/parameters/ps_l_chest", 0f);
        OscMessage __avatarLookAt_hips = new OscMessage("/avatar/parameters/ps_l_hips", 0f);
        OscMessage __avatarLookAt_lfoot = new OscMessage("/avatar/parameters/ps_l_lfoot", 0f);
        OscMessage __avatarLookAt_rfoot = new OscMessage("/avatar/parameters/ps_l_rfoot", 0f);
        OscMessage __avatarLookAt_lhand = new OscMessage("/avatar/parameters/ps_l_lhand", 0f);
        OscMessage __avatarLookAt_rhand = new OscMessage("/avatar/parameters/ps_l_rhand", 0f);
        OscMessage __cameraRootP = new OscMessage("/avatar/parameters/ps_rp", 0f);
        OscMessage __cameraRootP_root = new OscMessage("/avatar/parameters/ps_rp_root", 0f);
        OscMessage __cameraRootP_head = new OscMessage("/avatar/parameters/ps_rp_head", 0f);
        OscMessage __cameraRootP_chest = new OscMessage("/avatar/parameters/ps_rp_chest", 0f);
        OscMessage __cameraRootP_hips = new OscMessage("/avatar/parameters/ps_rp_hips", 0f);
        OscMessage __cameraRootP_lfoot = new OscMessage("/avatar/parameters/ps_rp_lfoot", 0f);
        OscMessage __cameraRootP_rfoot = new OscMessage("/avatar/parameters/ps_rp_rfoot", 0f);
        OscMessage __cameraRootP_lhand = new OscMessage("/avatar/parameters/ps_rp_lhand", 0f);
        OscMessage __cameraRootP_rhand = new OscMessage("/avatar/parameters/ps_rp_rhand", 0f);
        OscMessage __cameraRootR = new OscMessage("/avatar/parameters/ps_rr", 0f);
        OscMessage __cameraRootR_root = new OscMessage("/avatar/parameters/ps_rr_root", 0f);
        OscMessage __cameraRootR_head = new OscMessage("/avatar/parameters/ps_rr_head", 0f);
        OscMessage __cameraRootR_chest = new OscMessage("/avatar/parameters/ps_rr_chest", 0f);
        OscMessage __cameraRootR_hips = new OscMessage("/avatar/parameters/ps_rr_hips", 0f);
        OscMessage __cameraRootR_lfoot = new OscMessage("/avatar/parameters/ps_rr_lfoot", 0f);
        OscMessage __cameraRootR_rfoot = new OscMessage("/avatar/parameters/ps_rr_rfoot", 0f);
        OscMessage __cameraRootR_lhand = new OscMessage("/avatar/parameters/ps_rr_lhand", 0f);
        OscMessage __cameraRootR_rhand = new OscMessage("/avatar/parameters/ps_rr_rhand", 0f);
        OscMessage __hidemodel = new OscMessage("/avatar/parameters/ps_incognito", false);

        float PrevZoom = -1;
        float PrevExposure = -1;
        float PrevAperture = -1;
        float PrevFocus = -1;
        float PrevFocusX = -1;
        float PrevFocusY = -1;
        float PrevAvatarLookAt = -1;
        Vector3 PrevAp = Vector3.one;
        float PrevAvatarLookAt_root = -1;
        float PrevAvatarLookAt_head = -1;
        float PrevAvatarLookAt_chest = -1;
        float PrevAvatarLookAt_hips = -1;
        float PrevAvatarLookAt_lfoot = -1;
        float PrevAvatarLookAt_rfoot = -1;
        float PrevAvatarLookAt_lhand = -1;
        float PrevAvatarLookAt_rhand = -1;
        float PrevCameraRootP = -1;
        float PrevCameraRootP_root = -1;
        float PrevCameraRootP_head = -1;
        float PrevCameraRootP_chest = -1;
        float PrevCameraRootP_hips = -1;
        float PrevCameraRootP_lfoot = -1;
        float PrevCameraRootP_rfoot = -1;
        float PrevCameraRootP_lhand = -1;
        float PrevCameraRootP_rhand = -1;
        float PrevCameraRootR = -1;
        float PrevCameraRootR_root = -1;
        float PrevCameraRootR_head = -1;
        float PrevCameraRootR_chest = -1;
        float PrevCameraRootR_hips = -1;
        float PrevCameraRootR_lfoot = -1;
        float PrevCameraRootR_rfoot = -1;
        float PrevCameraRootR_lhand = -1;
        float PrevCameraRootR_rhand = -1;
        bool PrevHidemodel = false;

        #region VRCLens
        /// <summary>
        /// Camera specific value (VRCLens)
        /// </summary>
        [SerializeField] public float vrclens_WhiteBalance = 0.5f;

        OscMessage VRCLens_WhiteBalance = new OscMessage("/avatar/parameters/VRCLInt_WhiteBal", 0f);

        float prev_VRCLens_WhiteBalance;
        #endregion

        #region VirtualLens2
        /// <summary>
        /// Layer Mask Configuration object for VirtualLens2
        /// </summary>
        [Serializable]
        public struct VirtualLens2_LayerMaskConfig : IEquatable<VirtualLens2_LayerMaskConfig>
        {
            /// <summary>
            /// Is Local Player visible?
            /// </summary>
            public bool LocalPlayer;
            /// <summary>
            /// Is Remote Player visible?
            /// </summary>
            public bool RemotePlayer;
            /// <summary>
            /// is Game/World UI visible?
            /// </summary>
            public bool UI;

            /// <summary>
            /// Compare LayerMaskConfig
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public static bool operator == (VirtualLens2_LayerMaskConfig left, VirtualLens2_LayerMaskConfig right)
            {
                return left.Equals(right);
            }
            /// <summary>
            /// Compare LayerMaskConfig and flip
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public static bool operator != (VirtualLens2_LayerMaskConfig left, VirtualLens2_LayerMaskConfig right)
            {
                return !left.Equals(right);
            }

            /// <summary>
            /// Compare LayerMaskConfig
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(VirtualLens2_LayerMaskConfig other)
            {
                if (this.LocalPlayer != other.LocalPlayer) return false;
                if (this.RemotePlayer != other.RemotePlayer) return false;
                if (this.UI != other.UI) return false;
                return true;
            }
            /// <summary>
            /// Compare LayerMaskConfig
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return this.Equals((VirtualLens2_LayerMaskConfig)obj);
            }
            /// <summary>
            /// Get mask flag Int 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                int temp_flag = 0;
                if (LocalPlayer)
                {
                    temp_flag |= (1 << 0);
                }
                else
                {
                    temp_flag &= ~(1 << 0);
                }
                if (RemotePlayer)
                {
                    temp_flag |= (1 << 1);
                }
                else
                {
                    temp_flag &= ~(1 << 1);
                }
                if (UI)
                {
                    temp_flag |= (1 << 2);
                }
                else
                {
                    temp_flag &= ~(1 << 2);
                }
                return temp_flag;
            }
        }
        /// <summary>
        /// Camera specific value (VirtualLens2)
        /// </summary>
        [SerializeField] public VirtualLens2_LayerMaskConfig virtuallens2_LayerMask = new VirtualLens2_LayerMaskConfig()
        {
            LocalPlayer = true,
            RemotePlayer = true,
            UI = false,
        };
        /// <summary>
        /// Auto Focus Modes for VirtualLen2
        /// </summary>
        public enum VirtualLens2_AutoFocusModes
        {
            /// <summary>
            /// Point Auto Focus
            /// </summary>
            PointAF = 0,
            /// <summary>
            /// Face detection Auto Focus
            /// </summary>
            FaceAF,
            /// <summary>
            /// Selfie detection Auto Focus
            /// </summary>
            SelfieAF
        }
        /// <summary>
        /// Camera specific value (VirtualLens2)
        /// </summary>
        [SerializeField] public VirtualLens2_AutoFocusModes virtuallens2_AFMode = VirtualLens2_AutoFocusModes.PointAF;
        /// <summary>
        /// Auto Focus Speed control for VirtualLens2
        /// </summary>
        public enum VirtualLens2_AutoFocusSpeeds
        {
            /// <summary>
            /// Immediate speed
            /// </summary>
            Immediate = 0,
            /// <summary>
            /// Fast interpolate
            /// </summary>
            Fast,
            /// <summary>
            /// Medium interpolate
            /// </summary>
            Medium,
            /// <summary>
            /// Slow interpolate
            /// </summary>
            Slow
        }
        /// <summary>
        /// Camera specific value (VirtualLens2)
        /// </summary>
        [SerializeField] public VirtualLens2_AutoFocusSpeeds virtuallens2_AFSpeed = VirtualLens2_AutoFocusSpeeds.Immediate;
        /// <summary>
        /// Camera specific value (VirtualLens2)
        /// </summary>
        [SerializeField] public bool virtuallens2_PauseAF;
        /// <summary>
        /// Culling distances control for VirtualLens2
        /// </summary>
        public enum VirtualLens2_CullingDistances
        {
            /// <summary>
            /// Default (Use initial)
            /// </summary>
            Default = 0,
            /// <summary>
            /// Initial * 10
            /// </summary>
            x10,
            /// <summary>
            /// Initial * 100
            /// </summary>
            x100
        }
        /// <summary>
        /// Camera specific value (VirtualLens2)
        /// </summary>
        [SerializeField] public VirtualLens2_CullingDistances virtuallens2_CullingDistance = VirtualLens2_CullingDistances.Default;

        OscMessage VirtualLens2_LayerMask_LocalPlayer = new OscMessage("/avatar/parameters/VirtualLens2_LocalPlayerMask", 0f);
        OscMessage VirtualLens2_LayerMask_RemotePlayer = new OscMessage("/avatar/parameters/VirtualLens2_RemotePlayerMask", 0f);
        OscMessage VirtualLens2_LayerMask_UI = new OscMessage("/avatar/parameters/VirtualLens2_UIMask", 0f);
        OscMessage VirtualLens2_AFMode = new OscMessage("/avatar/parameters/VirtualLens2_AFMode", 0f);
        OscMessage VirtualLens2_AFSpeed = new OscMessage("/avatar/parameters/VirtualLens2_AFSpeed", 0f);
        OscMessage VirtualLens2_PauseAF = new OscMessage("/avatar/parameters/VirtualLens2_FocusLock", 0f);
        OscMessage VirtualLens2_CullingDistance = new OscMessage("/avatar/parameters/VirtualLens2_FarPlane", 0f);

        VirtualLens2_LayerMaskConfig prev_VirtualLens2_LayerMask;
        VirtualLens2_AutoFocusModes prev_VirtualLens2_AFMode;
        VirtualLens2_AutoFocusSpeeds prev_VirtualLens2_AFSpeed;
        bool prev_VirtualLens2_PauseAF;
        VirtualLens2_CullingDistances prev_VirtualLens2_CullingDistance;
        #endregion

        #region Integral
        /// <summary>
        /// Shoot modes available for Integral
        /// </summary>
        public enum Integral_ShootModes
        {
            /// <summary>
            /// Do nothing
            /// </summary>
            None = 0,
            /// <summary>
            /// Clear frame
            /// </summary>
            Clear,
            /// <summary>
            /// Open
            /// </summary>
            Open,
            /// <summary>
            /// Bulb
            /// </summary>
            Bulb,
            /// <summary>
            /// Clear frame and open
            /// </summary>
            ClearAndOpen,
            /// <summary>
            /// Open for single frame
            /// </summary>
            Single,
            /// <summary>
            /// Open with Shutter speed applied
            /// </summary>
            SS
        }
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public Integral_ShootModes integral_ShootMode = Integral_ShootModes.None;
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public float integral_ShutterSpeed;
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public bool integral_GridMode;
        /// <summary>
        /// Focus mode config for Integral
        /// </summary>
        public enum Integral_FocusModes
        {
            /// <summary>
            /// Manual focus
            /// </summary>
            Manual = 0,
            /// <summary>
            /// Point auto focus
            /// </summary>
            AF,
            /// <summary>
            /// Motion auto focus
            /// </summary>
            MotionAF
        }
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public Integral_FocusModes integral_FocusMode = Integral_FocusModes.AF;
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public bool integral_PauseAF;
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public float integral_Vignetting;
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public float integral_AxialChromaticAberration;
        /// <summary>
        /// Bokeh shapes for Integral
        /// </summary>
        public enum Integral_BokehShapes
        {
            /// <summary>
            /// Circle (Default)
            /// </summary>
            Circle = 0,
            /// <summary>
            /// Hexagon
            /// </summary>
            Hexagon,
            /// <summary>
            /// 5 corner star
            /// </summary>
            Star,
            /// <summary>
            /// Heart
            /// </summary>
            Heart,
            /// <summary>
            /// Bubble
            /// </summary>
            Bubble
        }
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public Integral_BokehShapes integral_BokehShape = Integral_BokehShapes.Circle;
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public float integral_BokehShapeRotation;
        /// <summary>
        /// Composition modes for Integral
        /// </summary>
        public enum Integral_CompositionModes
        {
            /// <summary>
            /// Normal (Default)
            /// </summary>
            Normal = 0,
            /// <summary>
            /// Lighten (for single frame)
            /// </summary>
            Lighten,
            /// <summary>
            /// Darken (for single frame)
            /// </summary>
            Darken,
            /// <summary>
            /// Depth capture
            /// </summary>
            Depth
        }
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public Integral_CompositionModes integral_CompositionMode = Integral_CompositionModes.Normal;
        /// <summary>
        /// Rolling shutter modes for Integral
        /// </summary>
        public enum Integral_RollingShutterModes
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Left to Right
            /// </summary>
            LeftToRight,
            /// <summary>
            /// Top to Bottom
            /// </summary>
            TopToBottom,
            /// <summary>
            /// Right to Left
            /// </summary>
            RightToLeft,
            /// <summary>
            /// Bottom to Top
            /// </summary>
            BottomToTop,
        }
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public Integral_RollingShutterModes integral_RollingShutterMode = Integral_RollingShutterModes.None;
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public float integral_Far = 0.75f;
        /// <summary>
        /// Color curves for Integral
        /// </summary>
        public enum Integral_Curves
        {
            /// <summary>
            /// Linear curve
            /// </summary>
            Linear = 0,
            /// <summary>
            /// Log curve
            /// </summary>
            Log
        }
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public Integral_Curves integral_Curve = Integral_Curves.Linear;
        /// <summary>
        /// Aspect ratio presets for Integral
        /// </summary>
        public enum Integral_AspectRatioPreset
        {
            /// <summary>
            /// 16:9
            /// </summary>
            [InspectorName("16:9")]
            _16_9,
            /// <summary>
            /// 4:3
            /// </summary>
            [InspectorName("4:3")]
            _4_3,
            /// <summary>
            /// 3:2
            /// </summary>
            [InspectorName("3:2")]
            _3_2,
            /// <summary>
            /// 1:1
            /// </summary>
            [InspectorName("1:1")]
            _1_1,
            /// <summary>
            /// 2.35:1
            /// </summary>
            [InspectorName("2.35:1")]
            _2_35_1,
            /// <summary>
            /// 7:6
            /// </summary>
            [InspectorName("7:6")]
            _7_6,
            /// <summary>
            /// 5:4
            /// </summary>
            [InspectorName("5:4")]
            _5_4,
            /// <summary>
            /// 7:5
            /// </summary>
            [InspectorName("7:5")]
            _7_5,
            /// <summary>
            /// 2:1
            /// </summary>
            [InspectorName("2:1")]
            _2_1,
            /// <summary>
            /// Use custom value (float integral_AspectRatioCustomValue)
            /// </summary>
            Custom
        }
        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public Integral_AspectRatioPreset integral_AspectRatio
        {
            get
            {
                switch (integral_AspectRatioCustomValue)
                {
                    case 0f: return Integral_AspectRatioPreset._16_9;
                    case 0.75f: return Integral_AspectRatioPreset._4_3;
                    case 0.6666667f: return Integral_AspectRatioPreset._3_2;
                    case 1f: return Integral_AspectRatioPreset._1_1;
                    case 0.4255319f: return Integral_AspectRatioPreset._2_35_1;
                    case 0.8571429f: return Integral_AspectRatioPreset._7_6;
                    case 0.8f: return Integral_AspectRatioPreset._5_4;
                    case 0.7142857f: return Integral_AspectRatioPreset._7_5;
                    case 0.5f: return Integral_AspectRatioPreset._2_1;
                    default: return Integral_AspectRatioPreset.Custom;
                }
            }
            set
            {
                switch (value)
                {
                    case Integral_AspectRatioPreset._16_9:
                        {
                            integral_AspectRatioCustomValue = 0;
                        }
                        break;
                    case Integral_AspectRatioPreset._4_3:
                        {
                            integral_AspectRatioCustomValue = 0.75f;
                        }
                        break;
                    case Integral_AspectRatioPreset._3_2:
                        {
                            integral_AspectRatioCustomValue = 0.6666667f;
                        }
                        break;
                    case Integral_AspectRatioPreset._1_1:
                        {
                            integral_AspectRatioCustomValue = 1f;
                        }
                        break;
                    case Integral_AspectRatioPreset._2_35_1:
                        {
                            integral_AspectRatioCustomValue = 0.4255319f;
                        }
                        break;
                    case Integral_AspectRatioPreset._7_6:
                        {
                            integral_AspectRatioCustomValue = 0.8571429f;
                        }
                        break;
                    case Integral_AspectRatioPreset._5_4:
                        {
                            integral_AspectRatioCustomValue = 0.8f;
                        }
                        break;
                    case Integral_AspectRatioPreset._7_5:
                        {
                            integral_AspectRatioCustomValue = 0.7142857f;
                        }
                        break;
                    case Integral_AspectRatioPreset._2_1:
                        {
                            integral_AspectRatioCustomValue = 0.5f;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Camera specific value (Integral)
        /// </summary>
        [SerializeField] public float integral_AspectRatioCustomValue;

        OscMessage Integral_ShootMode = new OscMessage("/avatar/parameters/Integral_Mode", 0);
        OscMessage Integral_ShutterSpeed = new OscMessage("/avatar/parameters/Integral_ShutterSpeed", 0f);
        OscMessage Integral_GridMode = new OscMessage("/avatar/parameters/Integral_GridMode", 0);
        OscMessage Integral_FocusMode = new OscMessage("/avatar/parameters/Integral_FocusMode", 0);
        OscMessage Integral_PauseAF = new OscMessage("/avatar/parameters/Integral_PauseAF", 0);
        OscMessage Integral_Vignetting = new OscMessage("/avatar/parameters/Integral_Vignetting", 0f);
        OscMessage Integral_AxialChromaticAberration = new OscMessage("/avatar/parameters/Integral_AxialChromaticAberration", 0f);
        OscMessage Integral_BokehShape = new OscMessage("/avatar/parameters/Integral_BokehShape", 0);
        OscMessage Integral_BokehShapeRotation = new OscMessage("/avatar/parameters/Integral_BokehShapeRotation", 0f);
        OscMessage Integral_CompositionMode = new OscMessage("/avatar/parameters/Integral_CompositionMode", 0);
        OscMessage Integral_RollingShutterMode = new OscMessage("/avatar/parameters/Integral_RollingShutterMode", 0);
        OscMessage Integral_Far = new OscMessage("/avatar/parameters/Integral_Far", 0f);
        OscMessage Integral_Curve = new OscMessage("/avatar/parameters/Integral_Curve", 0);
        OscMessage Integral_AspectRatio = new OscMessage("/avatar/parameters/Integral_AspectRatio", 0f);

        Integral_ShootModes prev_Integral_ShootMode;
        float prev_Integral_ShutterSpeed;
        bool prev_Integral_GridMode;
        Integral_FocusModes prev_Integral_FocusMode;
        bool prev_Integral_PauseAF;
        float prev_Integral_Vignetting;
        float prev_Integral_AxialChromaticAberration;
        Integral_BokehShapes prev_Integral_BokehShape;
        float prev_Integral_BokehShapeRotation;
        Integral_CompositionModes prev_Integral_CompositionMode;
        Integral_RollingShutterModes prev_Integral_RollingShutterMode;
        float prev_Integral_Far;
        Integral_Curves prev_Integral_Curve;
        float prev_Integral_AspectRatioCustomValue;
        #endregion

        #region Flex Fisheye

        public enum FlexFishEye_AspectRatioPreset
        {
            /// <summary>
            /// 16:9
            /// </summary>
            [InspectorName("16:9")]
            _16_9 = 0,
            /// <summary>
            /// 1:1
            /// </summary>
            [InspectorName("1:1")]
            _1_1,
            /// <summary>
            /// 3:2
            /// </summary>
            [InspectorName("3:2")]
            _3_2,
            /// <summary>
            /// 4:3
            /// </summary>
            [InspectorName("4:3")]
            _4_3,
            /// <summary>
            /// 1:1
            /// </summary>
            [InspectorName("Cinematic")]
            Cinematic,
        }

        /// <summary>
        /// Camera specific value (Flex Fisheye)
        /// </summary>
        [SerializeField] public float flexfisheye_FOV = 0.5f;
        [SerializeField] public float flexfisheye_ScreenSize = 0.5f;
        [SerializeField] public Vector2 flexfisheye_Distortion = Vector2.zero;
        [SerializeField] public Vector2 flexfisheye_DistortionCenter = Vector2.zero;
        [SerializeField] public bool flexfisheye_DistortionMultiple = false;
        [SerializeField] public bool flexfisheye_ReverseFishEye = false;
        [SerializeField] public bool flexfisheye_WhiteBack = false;
        [SerializeField] public FlexFishEye_AspectRatioPreset flexfisheye_PhotoAspectRatio = FlexFishEye_AspectRatioPreset._16_9;
        [SerializeField] public float flexfisheye_ColorAdjust = 0.5f;

        //OscMessage VRCLens_WhiteBalance;

        //float prev_VRCLens_WhiteBalance;
        #endregion

        #region VRChat Native

        /// <summary>
        /// Enum for VRChat Native Camera's operation mode
        /// </summary>
        public enum VRCNative_Modes
        {
            /// <summary>
            /// Disable camera
            /// </summary>
            Disabled = 0,
            /// <summary>
            /// Photo mode
            /// </summary>
            Photo = 1,
            /// <summary>
            /// Stream camera (Spout2 enabled separately)
            /// </summary>
            Stream = 2,
            /// <summary>
            /// Emoji capture
            /// </summary>
            Emoji = 3,
            /// <summary>
            /// Multi-Layer mode
            /// </summary>
            MultiLayer = 4,
            /// <summary>
            /// Print Camera mode
            /// </summary>
            Print = 5,
            /// <summary>
            /// Drone mode
            /// </summary>
            Drone = 6
        }

        /// <summary>
        /// Layer Mask configuration object for VRChat Native Camera
        /// </summary>
        [Serializable]
        public struct VRCNative_LayerMaskConfig : IEquatable<VRCNative_LayerMaskConfig>
        {
            /// <summary>
            /// Is Local Player visible?
            /// </summary>
            public bool LocalPlayer;
            /// <summary>
            /// Is Remote Player visible?
            /// </summary>
            public bool RemotePlayer;
            /// <summary>
            /// is Internal UI visible?
            /// </summary>
            public bool UI;
            /// <summary>
            /// is Environment (World) visible?
            /// </summary>
            public bool Environment;

            /// <summary>
            /// Compare LayerMaskConfig
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public static bool operator ==(VRCNative_LayerMaskConfig left, VRCNative_LayerMaskConfig right)
            {
                return left.Equals(right);
            }
            /// <summary>
            /// Compare LayerMaskConfig and flip
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public static bool operator !=(VRCNative_LayerMaskConfig left, VRCNative_LayerMaskConfig right)
            {
                return !left.Equals(right);
            }

            /// <summary>
            /// Compare LayerMaskConfig
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(VRCNative_LayerMaskConfig other)
            {
                if (this.LocalPlayer != other.LocalPlayer) return false;
                if (this.RemotePlayer != other.RemotePlayer) return false;
                if (this.UI != other.UI) return false;
                if (this.Environment != other.Environment) return false;
                return true;
            }
            /// <summary>
            /// Compare LayerMaskConfig
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return this.Equals((VRCNative_LayerMaskConfig)obj);
            }
            /// <summary>
            /// Get mask flag Int 
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                int temp_flag = 0;
                if (LocalPlayer)
                {
                    temp_flag |= (1 << 0);
                }
                else
                {
                    temp_flag &= ~(1 << 0);
                }
                if (RemotePlayer)
                {
                    temp_flag |= (1 << 1);
                }
                else
                {
                    temp_flag &= ~(1 << 1);
                }
                if (UI)
                {
                    temp_flag |= (1 << 2);
                }
                else
                {
                    temp_flag &= ~(1 << 2);
                }
                if (Environment)
                {
                    temp_flag |= (1 << 3);
                }
                else
                {
                    temp_flag &= ~(1 << 3);
                }
                return temp_flag;
            }
        }

        /// <summary>
        /// VRChat Native Camera's operation mode
        /// </summary>
        [SerializeField] public VRCNative_Modes vrcnative_Mode;
        /// <summary>
        /// VRChat Native Camera's Layer Mask configuration
        /// </summary>
        [SerializeField] public VRCNative_LayerMaskConfig vrcnative_LayerMask = new VRCNative_LayerMaskConfig()
        {
            LocalPlayer = true,
            RemotePlayer = true,
            UI = false,
            Environment = true
        };
        /// <summary>
        /// VRChat Native Camera's Greenscreen toggle
        /// </summary>
        [SerializeField] public bool  vrcnative_GreenScreen;
        /// <summary>
        /// VRChat Native Camera's Pickup lock toggle
        /// </summary>
        [SerializeField] public bool  vrcnative_Lock;
        /// <summary>
        /// VRChat Native Camera's movement smoothing postprocessing
        /// </summary>
        [SerializeField] public bool  vrcnative_SmoothMovement;
        /// <summary>
        /// VRChat Native Camera's Auto-Level Roll toggle
        /// </summary>
        [SerializeField] public bool  vrcnative_AutoLevelRoll;
        /// <summary>
        /// VRChat Native Camera's Auto-Level Pitch toggle
        /// </summary>
        [SerializeField] public bool  vrcnative_AutoLevelPitch;
        /// <summary>
        /// VRChat Native Camera's Audio Listener on Camera
        /// </summary>
        [SerializeField] public bool  vrcnative_AudioFromCamera;
        /// <summary>
        /// VRChat Native Camera's focus gizmo visibility toggle
        /// </summary>
        [SerializeField] public bool  vrcnative_ShowFocus;
        /// <summary>
        /// VRChat Native Camera's Spout2 streaming mode toggle
        /// </summary>
        [SerializeField] public bool  vrcnative_Streaming;
        /// <summary>
        /// VRChat Native Camera's orientation toggle (true = Landscape 16:9)
        /// </summary>
        [SerializeField] public bool  vrcnative_OrientationIsLandscape = true;

        /// <summary>
        /// VRChat Native Camera's Greenscreen Hue
        /// </summary>
        [SerializeField] public float vrcnative_Hue = 120f;
        /// <summary>
        /// VRChat Native Camera's Greenscreen Saturation
        /// </summary>
        [SerializeField] public float vrcnative_Saturation = 100f;
        /// <summary>
        /// VRChat Native Camera's Greenscreen Lightness
        /// </summary>
        [SerializeField] public float vrcnative_Lightness = 60f;
        /// <summary>
        /// VRChat Native Camera's Movement smoothing strength
        /// </summary>
        [SerializeField] public float vrcnative_SmoothingStrength = 5f;



        OscMessage VRCNative_Mode = new OscMessage("/usercamera/Mode", 0);
        OscMessage VRCNative_Capture = new OscMessage("/usercamera/Capture", false);
        OscMessage VRCNative_CaptureDelayed = new OscMessage("/usercamera/CaptureDelayed", false);

        OscMessage VRCNative_ShowUIInCamera = new OscMessage("/usercamera/ShowUIInCamera", false);
        OscMessage VRCNative_LocalPlayer = new OscMessage("/usercamera/LocalPlayer", false);
        OscMessage VRCNative_RemotePlayer = new OscMessage("/usercamera/RemotePlayer", false);
        OscMessage VRCNative_Environment = new OscMessage("/usercamera/Environment", false);
        OscMessage VRCNative_GreenScreen = new OscMessage("/usercamera/GreenScreen", false);
        OscMessage VRCNative_Lock = new OscMessage("/usercamera/Lock", false);
        OscMessage VRCNative_SmoothMovement = new OscMessage("/usercamera/SmoothMovement", false);
        OscMessage VRCNative_AutoLevelRoll = new OscMessage("/usercamera/AutoLevelRoll", false);
        OscMessage VRCNative_AutoLevelPitch = new OscMessage("/usercamera/AutoLevelPitch", false);
        OscMessage VRCNative_AudioFromCamera = new OscMessage("/usercamera/AudioFromCamera", false);
        OscMessage VRCNative_ShowFocus = new OscMessage("/usercamera/ShowFocus", false);
        OscMessage VRCNative_Streaming = new OscMessage("/usercamera/Streaming", false);
        OscMessage VRCNative_OrientationIsLandscape = new OscMessage("/usercamera/OrientationIsLandscape", false);

        OscMessage VRCNative_Hue = new OscMessage("/usercamera/Hue", 0f);
        OscMessage VRCNative_Saturation = new OscMessage("/usercamera/Saturation", 0f);
        OscMessage VRCNative_Lightness = new OscMessage("/usercamera/Lightness", 0f);
        OscMessage VRCNative_SmoothingStrength = new OscMessage("/usercamera/SmoothingStrength", 0f);


        VRCNative_Modes prev_VRCNative_Mode;

        VRCNative_LayerMaskConfig prev_VRCNative_LayerMask;
        bool prev_VRCNative_GreenScreen;
        bool prev_VRCNative_Lock;
        bool prev_VRCNative_SmoothMovement;
        bool prev_VRCNative_AutoLevelRoll;
        bool prev_VRCNative_AutoLevelPitch;
        bool prev_VRCNative_AudioFromCamera;
        bool prev_VRCNative_ShowFocus;
        bool prev_VRCNative_Streaming;
        bool prev_VRCNative_OrientationIsLandscape;

        float prev_VRCNative_Hue;
        float prev_VRCNative_Saturation;
        float prev_VRCNative_Lightness;
        float prev_VRCNative_SmoothingStrength;
        #endregion

        void Update()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        void initializeMessages(ClientCameraType cameraType)
        {
            switch (cameraType)
            {
                case ClientCameraType.VRCLens:
                    {
                        __zoom.Address = ("/avatar/parameters/VRCLZoomRadial");
                        __exposure.Address = ("/avatar/parameters/VRCLExposureRadial");
                        __aperture.Address = ("/avatar/parameters/VRCLApertureRadial");
                        __focus.Address = ("/avatar/parameters/VRCLFocusRadial");
                        __focusX.Address = ("/avatar/parameters/ps_fx");
                        __focusY.Address = ("/avatar/parameters/ps_fy");
                    }
                    break;
                case ClientCameraType.VirtualLens2:
                    {
                        __zoom.Address = ("/avatar/parameters/VirtualLens2_Zoom");
                        __exposure.Address = ("/avatar/parameters/VirtualLens2_Exposure");
                        __aperture.Address = ("/avatar/parameters/VirtualLens2_Aperture");
                        __focus.Address = ("/avatar/parameters/VirtualLens2_Distance");
                        __focusX.Address = ("/avatar/parameters/VirtualLens2_TouchOverrideX");
                        __focusY.Address = ("/avatar/parameters/VirtualLens2_TouchOverrideY");
                    }
                    break;
                case ClientCameraType.Integral:
                    {
                        __zoom.Address = ("/avatar/parameters/Integral_Zoom");
                        __exposure.Address = ("/avatar/parameters/Integral_Exposure");
                        __aperture.Address = ("/avatar/parameters/Integral_Aperture");
                        __focus.Address = ("/avatar/parameters/Integral_Focus");
                        __focusX.Address = ("/avatar/parameters/ps_fx");
                        __focusY.Address = ("/avatar/parameters/ps_fy");
                    }
                    break;
                case ClientCameraType.FlexFishEye:
                    {
                        __zoom.Address = ("/avatar/parameters/FishEye_SensorSize");
                        __exposure.Address = ("/none");
                        __aperture.Address = ("/avatar/parameters/FishEye_Aperture");
                        __focus.Address = ("/avatar/parameters/FishEye_FocusLength");
                        __focusX.Address = ("/none");
                        __focusY.Address = ("/none");
                    }
                    break;
                case ClientCameraType.VRChatNative:
                    {
                        __zoom.Address = ("/usercamera/Zoom");
                        __exposure.Address = ("/usercamera/Exposure");
                        __aperture.Address = ("/usercamera/Aperture");
                        __focus.Address = ("/usercamera/FocalDistance");
                        __focusX.Address = ("/none");
                        __focusY.Address = ("/none");
                    }
                    break;
            }

            switch (cameraType)
            {
                case ClientCameraType.VRChatNative:
                    {
                        __avatarLookAt.Address = ("/usercamera/LookAtMe");
                        __ax.Address = ("/usercamera/LookAtMeXOffset");
                        __ay.Address = ("/usercamera/LookAtMeYOffset");
                    }
                    break;
                default:
                    {
                        __avatarLookAt.Address = ("/avatar/parameters/ps_l");
                        __ax.Address = ("/avatar/parameters/ps_lx");
                        __ay.Address = ("/avatar/parameters/ps_ly");
                    }
                    break;
            }
            //Debug.Log("OscMessage Initialized");
        }

        /// <summary>
        /// Called from StudioSystem
        /// StudioSystem에서 호출하는 함수
        /// </summary>
        /// <param name="system">StudioSystem</param>
        /// <param name="immediate">send OSC now?</param>
        /// <returns>is success?</returns>
        public override bool OnSend(StudioSystem system, bool immediate)
        {
            // return base.Send(system, immediate);

            try
            {
                if (immediate)
                {
                    initializeMessages(system.cameraType);

                    switch (system.cameraType)
                    {
                        case ClientCameraType.VRChatNative:
                            {
                                __zoom.Set(0, Mathf.Lerp(20, 150, zoom));
                                __exposure.Set(0, Mathf.Lerp(-10, 4, exposure));
                                __aperture.Set(0, Mathf.Lerp(1.4f, 32, aperture));
                                __focus.Set(0, Mathf.Lerp(0, 10, focus));
                                __avatarLookAt.Set(0, (avatarLookAt) > 0.5f);

                                system.Send(__zoom);
                                system.Send(__exposure);
                                system.Send(__aperture);
                                system.Send(__focus);
                                system.Send(__avatarLookAt);

                                PrevZoom = zoom;
                                PrevExposure = exposure;
                                PrevAperture = aperture;
                                PrevFocus = focus;
                                PrevFocusX = focusX;
                                PrevFocusY = focusY;
                                PrevAvatarLookAt = avatarLookAt;
                                PrevAp = ap;
                                PrevAvatarLookAt_root = avatarLookAt_root;
                                PrevAvatarLookAt_head = avatarLookAt_head;
                                PrevAvatarLookAt_chest = avatarLookAt_chest;
                                PrevAvatarLookAt_hips = avatarLookAt_hips;
                                PrevAvatarLookAt_lfoot = avatarLookAt_lfoot;
                                PrevAvatarLookAt_rfoot = avatarLookAt_rfoot;
                                PrevAvatarLookAt_lhand = avatarLookAt_lhand;
                                PrevAvatarLookAt_rhand = avatarLookAt_rhand;
                                PrevCameraRootP = cameraRootP_active;
                                PrevCameraRootP_root = cameraRootP_avatar_root;
                                PrevCameraRootP_head = cameraRootP_avatar_head;
                                PrevCameraRootP_chest = cameraRootP_avatar_chest;
                                PrevCameraRootP_hips = cameraRootP_avatar_hips;
                                PrevCameraRootP_lfoot = cameraRootP_avatar_lfoot;
                                PrevCameraRootP_rfoot = cameraRootP_avatar_rfoot;
                                PrevCameraRootP_lhand = cameraRootP_avatar_lhand;
                                PrevCameraRootP_rhand = cameraRootP_avatar_rhand;
                                PrevCameraRootR = cameraRootR_active;
                                PrevCameraRootR_root = cameraRootR_avatar_root;
                                PrevCameraRootR_head = cameraRootR_avatar_head;
                                PrevCameraRootR_chest = cameraRootR_avatar_chest;
                                PrevCameraRootR_hips = cameraRootR_avatar_hips;
                                PrevCameraRootR_lfoot = cameraRootR_avatar_lfoot;
                                PrevCameraRootR_rfoot = cameraRootR_avatar_rfoot;
                                PrevCameraRootR_lhand = cameraRootR_avatar_lhand;
                                PrevCameraRootR_rhand = cameraRootR_avatar_rhand;
                                PrevHidemodel = hidemodel;
                            }
                            break;
                        default:
                            {
                                __zoom.Set(0, (zoom));
                                __exposure.Set(0, (exposure));
                                __aperture.Set(0, (aperture));
                                __focus.Set(0, (focus));
                                __focusX.Set(0, (focusX));
                                __focusY.Set(0, (focusY));
                                __avatarLookAt.Set(0, (avatarLookAt));
                                __ax.Set(0, (ap.x));
                                __ay.Set(0, (ap.y));
                                __az.Set(0, (ap.z));
                                __avatarLookAt_root.Set(0, (avatarLookAt_root));
                                __avatarLookAt_head.Set(0, (avatarLookAt_head));
                                __avatarLookAt_chest.Set(0, (avatarLookAt_chest));
                                __avatarLookAt_hips.Set(0, (avatarLookAt_hips));
                                __avatarLookAt_lfoot.Set(0, (avatarLookAt_lfoot));
                                __avatarLookAt_rfoot.Set(0, (avatarLookAt_rfoot));
                                __avatarLookAt_lhand.Set(0, (avatarLookAt_lhand));
                                __avatarLookAt_rhand.Set(0, (avatarLookAt_rhand));
                                __cameraRootP.Set(0, (cameraRootP_active));
                                __cameraRootP_root.Set(0, (cameraRootP_avatar_root));
                                __cameraRootP_head.Set(0, (cameraRootP_avatar_head));
                                __cameraRootP_chest.Set(0, (cameraRootP_avatar_chest));
                                __cameraRootP_hips.Set(0, (cameraRootP_avatar_hips));
                                __cameraRootP_lfoot.Set(0, (cameraRootP_avatar_lfoot));
                                __cameraRootP_rfoot.Set(0, (cameraRootP_avatar_rfoot));
                                __cameraRootP_lhand.Set(0, (cameraRootP_avatar_lhand));
                                __cameraRootP_rhand.Set(0, (cameraRootP_avatar_rhand));
                                __cameraRootR.Set(0, (cameraRootR_active));
                                __cameraRootR_root.Set(0, (cameraRootR_avatar_root));
                                __cameraRootR_head.Set(0, (cameraRootR_avatar_head));
                                __cameraRootR_chest.Set(0, (cameraRootR_avatar_chest));
                                __cameraRootR_hips.Set(0, (cameraRootR_avatar_hips));
                                __cameraRootR_lfoot.Set(0, (cameraRootR_avatar_lfoot));
                                __cameraRootR_rfoot.Set(0, (cameraRootR_avatar_rfoot));
                                __cameraRootR_lhand.Set(0, (cameraRootR_avatar_lhand));
                                __cameraRootR_rhand.Set(0, (cameraRootR_avatar_rhand));

                                __hidemodel.Set(0, (hidemodel));

                                system.Send(__zoom);
                                system.Send(__exposure);
                                system.Send(__aperture);
                                system.Send(__focus);
                                system.Send(__focusX);
                                system.Send(__focusY);
                                system.Send(__avatarLookAt);
                                system.Send(__ax);
                                system.Send(__ay);
                                system.Send(__az);
                                system.Send(__avatarLookAt_root);
                                system.Send(__avatarLookAt_head);
                                system.Send(__avatarLookAt_chest);
                                system.Send(__avatarLookAt_hips);
                                system.Send(__avatarLookAt_lfoot);
                                system.Send(__avatarLookAt_rfoot);
                                system.Send(__avatarLookAt_lhand);
                                system.Send(__avatarLookAt_rhand);
                                system.Send(__cameraRootP);
                                system.Send(__cameraRootP_root);
                                system.Send(__cameraRootP_head);
                                system.Send(__cameraRootP_chest);
                                system.Send(__cameraRootP_hips);
                                system.Send(__cameraRootP_lfoot);
                                system.Send(__cameraRootP_rfoot);
                                system.Send(__cameraRootP_lhand);
                                system.Send(__cameraRootP_rhand);
                                system.Send(__cameraRootR);
                                system.Send(__cameraRootR_root);
                                system.Send(__cameraRootR_head);
                                system.Send(__cameraRootR_chest);
                                system.Send(__cameraRootR_hips);
                                system.Send(__cameraRootR_lfoot);
                                system.Send(__cameraRootR_rfoot);
                                system.Send(__cameraRootR_lhand);
                                system.Send(__cameraRootR_rhand);
                                system.Send(__hidemodel);

                                PrevZoom = zoom;
                                PrevExposure = exposure;
                                PrevAperture = aperture;
                                PrevFocus = focus;
                                PrevFocusX = focusX;
                                PrevFocusY = focusY;
                                PrevAvatarLookAt = avatarLookAt;
                                PrevAp = ap;
                                PrevAvatarLookAt_root = avatarLookAt_root;
                                PrevAvatarLookAt_head = avatarLookAt_head;
                                PrevAvatarLookAt_chest = avatarLookAt_chest;
                                PrevAvatarLookAt_hips = avatarLookAt_hips;
                                PrevAvatarLookAt_lfoot = avatarLookAt_lfoot;
                                PrevAvatarLookAt_rfoot = avatarLookAt_rfoot;
                                PrevAvatarLookAt_lhand = avatarLookAt_lhand;
                                PrevAvatarLookAt_rhand = avatarLookAt_rhand;
                                PrevCameraRootP = cameraRootP_active;
                                PrevCameraRootP_root = cameraRootP_avatar_root;
                                PrevCameraRootP_head = cameraRootP_avatar_head;
                                PrevCameraRootP_chest = cameraRootP_avatar_chest;
                                PrevCameraRootP_hips = cameraRootP_avatar_hips;
                                PrevCameraRootP_lfoot = cameraRootP_avatar_lfoot;
                                PrevCameraRootP_rfoot = cameraRootP_avatar_rfoot;
                                PrevCameraRootP_lhand = cameraRootP_avatar_lhand;
                                PrevCameraRootP_rhand = cameraRootP_avatar_rhand;
                                PrevCameraRootR = cameraRootR_active;
                                PrevCameraRootR_root = cameraRootR_avatar_root;
                                PrevCameraRootR_head = cameraRootR_avatar_head;
                                PrevCameraRootR_chest = cameraRootR_avatar_chest;
                                PrevCameraRootR_hips = cameraRootR_avatar_hips;
                                PrevCameraRootR_lfoot = cameraRootR_avatar_lfoot;
                                PrevCameraRootR_rfoot = cameraRootR_avatar_rfoot;
                                PrevCameraRootR_lhand = cameraRootR_avatar_lhand;
                                PrevCameraRootR_rhand = cameraRootR_avatar_rhand;
                                PrevHidemodel = hidemodel;
                            }
                            break;
                    }

                    SendCameraDependent_Immediate(system);
                }
                else
                {
                    initializeMessages(system.cameraType);

                    switch (system.cameraType)
                    {
                        case ClientCameraType.VRChatNative:
                            {
                                if (PrevZoom != zoom)
                                {
                                __zoom.Set(0, Mathf.Lerp(20, 150, zoom));
                                    system.Send(__zoom);
                                    PrevZoom = zoom;
                                }

                                if (PrevExposure != exposure)
                                {
                                __exposure.Set(0, Mathf.Lerp(-10, 4, exposure));
                                    system.Send(__exposure);
                                    PrevExposure = exposure;
                                }

                                if (PrevAperture != aperture)
                                {
                                __aperture.Set(0, Mathf.Lerp(1.4f, 32, aperture));
                                    system.Send(__aperture);
                                    PrevAperture = aperture;
                                }

                                if (PrevFocus != focus)
                                {
                                __focus.Set(0, Mathf.Lerp(0, 10, focus));
                                    system.Send(__focus);
                                    PrevFocus = focus;
                                }

                                if (PrevAvatarLookAt != avatarLookAt)
                                {
                                __avatarLookAt.Set(0, (avatarLookAt) > 0.5f);
                                    system.Send(__avatarLookAt);
                                    PrevAvatarLookAt = avatarLookAt;
                                }
                            }
                            break;
                        default:
                            {
                                if (PrevZoom != zoom)
                                {
                                    __zoom.Set(0, (zoom));
                                    system.Send(__zoom);
                                    PrevZoom = zoom;
                                }

                                if (PrevExposure != exposure)
                                {
                                    __exposure.Set(0, (exposure));
                                    system.Send(__exposure);
                                    PrevExposure = exposure;
                                }

                                if (PrevAperture != aperture)
                                {
                                    __aperture.Set(0, (aperture));
                                    system.Send(__aperture);
                                    PrevAperture = aperture;
                                }

                                if (PrevFocus != focus)
                                {
                                    __focus.Set(0, (focus));
                                    system.Send(__focus);
                                    PrevFocus = focus;
                                }

                                if (PrevFocusX != focusX)
                                {
                                    __focusX.Set(0, (focusX));
                                    system.Send(__focusX);
                                    PrevFocusX = focusX;
                                }

                                if (PrevFocusY != focusY)
                                {
                                    __focusY.Set(0, (focusY));
                                    system.Send(__focusY);
                                    PrevFocusY = focusY;
                                }

                                if (PrevAvatarLookAt != avatarLookAt)
                                {
                                    __avatarLookAt.Set(0, (avatarLookAt));
                                    system.Send(__avatarLookAt);
                                    PrevAvatarLookAt = avatarLookAt;
                                }

                                if (PrevAp != ap)
                                {
                                    __ax.Set(0, (ap.x));
                                    __ay.Set(0, (ap.y));
                                    __az.Set(0, (ap.z));
                                    system.Send(__ax);
                                    system.Send(__ay);
                                    system.Send(__az);
                                    PrevAp = ap;
                                }

                                if (PrevAvatarLookAt_root != avatarLookAt_root)
                                {
                                    __avatarLookAt_root.Set(0, (avatarLookAt_root));
                                    system.Send(__avatarLookAt_root);
                                    PrevAvatarLookAt_root = avatarLookAt_root;
                                }

                                if (PrevAvatarLookAt_head != avatarLookAt_head)
                                {
                                    __avatarLookAt_head.Set(0, (avatarLookAt_head));
                                    system.Send(__avatarLookAt_head);
                                    PrevAvatarLookAt_head = avatarLookAt_head;
                                }

                                if (PrevAvatarLookAt_chest != avatarLookAt_chest)
                                {
                                    __avatarLookAt_chest.Set(0, (avatarLookAt_chest));
                                    system.Send(__avatarLookAt_chest);
                                    PrevAvatarLookAt_chest = avatarLookAt_chest;
                                }

                                if (PrevAvatarLookAt_hips != avatarLookAt_hips)
                                {
                                    __avatarLookAt_hips.Set(0, (avatarLookAt_hips));
                                    system.Send(__avatarLookAt_hips);
                                    PrevAvatarLookAt_hips = avatarLookAt_hips;
                                }

                                if (PrevAvatarLookAt_lfoot != avatarLookAt_lfoot)
                                {
                                    __avatarLookAt_lfoot.Set(0, (avatarLookAt_lfoot));
                                    system.Send(__avatarLookAt_lfoot);
                                    PrevAvatarLookAt_lfoot = avatarLookAt_lfoot;
                                }

                                if (PrevAvatarLookAt_rfoot != avatarLookAt_rfoot)
                                {
                                    __avatarLookAt_rfoot.Set(0, (avatarLookAt_rfoot));
                                    system.Send(__avatarLookAt_rfoot);
                                    PrevAvatarLookAt_rfoot = avatarLookAt_rfoot;
                                }

                                if (PrevAvatarLookAt_lhand != avatarLookAt_lhand)
                                {
                                    __avatarLookAt_lhand.Set(0, (avatarLookAt_lhand));
                                    system.Send(__avatarLookAt_lhand);
                                    PrevAvatarLookAt_lhand = avatarLookAt_lhand;
                                }

                                if (PrevAvatarLookAt_rhand != avatarLookAt_rhand)
                                {
                                    __avatarLookAt_rhand.Set(0, (avatarLookAt_rhand));
                                    system.Send(__avatarLookAt_rhand);
                                    PrevAvatarLookAt_rhand = avatarLookAt_rhand;
                                }



                                if (PrevCameraRootP != cameraRootP_active)
                                {
                                    __cameraRootP.Set(0, (cameraRootP_active));
                                    system.Send(__cameraRootP);
                                    PrevCameraRootP = avatarLookAt;
                                }

                                if (PrevCameraRootP_root != cameraRootP_avatar_root)
                                {
                                    __cameraRootP_root.Set(0, (cameraRootP_avatar_root));
                                    system.Send(__cameraRootP_root);
                                    PrevCameraRootP_root = cameraRootP_avatar_root;
                                }

                                if (PrevCameraRootP_head != cameraRootP_avatar_head)
                                {
                                    __cameraRootP_head.Set(0, (cameraRootP_avatar_head));
                                    system.Send(__cameraRootP_head);
                                    PrevCameraRootP_head = cameraRootP_avatar_head;
                                }

                                if (PrevCameraRootP_chest != cameraRootP_avatar_chest)
                                {
                                    __cameraRootP_chest.Set(0, (cameraRootP_avatar_chest));
                                    system.Send(__cameraRootP_chest);
                                    PrevCameraRootP_chest = cameraRootP_avatar_chest;
                                }

                                if (PrevCameraRootP_hips != cameraRootP_avatar_hips)
                                {
                                    __cameraRootP_hips.Set(0, (cameraRootP_avatar_hips));
                                    system.Send(__cameraRootP_hips);
                                    PrevCameraRootP_hips = cameraRootP_avatar_hips;
                                }

                                if (PrevCameraRootP_lfoot != cameraRootP_avatar_lfoot)
                                {
                                    __cameraRootP_lfoot.Set(0, (cameraRootP_avatar_lfoot));
                                    system.Send(__cameraRootP_lfoot);
                                    PrevCameraRootP_lfoot = cameraRootP_avatar_lfoot;
                                }

                                if (PrevCameraRootP_rfoot != cameraRootP_avatar_rfoot)
                                {
                                    __cameraRootP_rfoot.Set(0, (cameraRootP_avatar_rfoot));
                                    system.Send(__cameraRootP_rfoot);
                                    PrevCameraRootP_rfoot = cameraRootP_avatar_rfoot;
                                }

                                if (PrevCameraRootP_lhand != cameraRootP_avatar_lhand)
                                {
                                    __cameraRootP_lhand.Set(0, (cameraRootP_avatar_lhand));
                                    system.Send(__cameraRootP_lhand);
                                    PrevCameraRootP_lhand = cameraRootP_avatar_lhand;
                                }

                                if (PrevCameraRootP_rhand != cameraRootP_avatar_rhand)
                                {
                                    __cameraRootP_rhand.Set(0, (cameraRootP_avatar_rhand));
                                    system.Send(__cameraRootP_rhand);
                                    PrevCameraRootP_rhand = cameraRootP_avatar_rhand;
                                }

                                if (PrevCameraRootR != cameraRootR_active)
                                {
                                    __cameraRootR.Set(0, (cameraRootR_active));
                                    system.Send(__cameraRootR);
                                    PrevCameraRootR = avatarLookAt;
                                }

                                if (PrevCameraRootR_root != cameraRootR_avatar_root)
                                {
                                    __cameraRootR_root.Set(0, (cameraRootR_avatar_root));
                                    system.Send(__cameraRootR_root);
                                    PrevCameraRootR_root = cameraRootR_avatar_root;
                                }

                                if (PrevCameraRootR_head != cameraRootR_avatar_head)
                                {
                                    __cameraRootR_head.Set(0, (cameraRootR_avatar_head));
                                    system.Send(__cameraRootR_head);
                                    PrevCameraRootR_head = cameraRootR_avatar_head;
                                }

                                if (PrevCameraRootR_chest != cameraRootR_avatar_chest)
                                {
                                    __cameraRootR_chest.Set(0, (cameraRootR_avatar_chest));
                                    system.Send(__cameraRootR_chest);
                                    PrevCameraRootR_chest = cameraRootR_avatar_chest;
                                }

                                if (PrevCameraRootR_hips != cameraRootR_avatar_hips)
                                {
                                    __cameraRootR_hips.Set(0, (cameraRootR_avatar_hips));
                                    system.Send(__cameraRootR_hips);
                                    PrevCameraRootR_hips = cameraRootR_avatar_hips;
                                }

                                if (PrevCameraRootR_lfoot != cameraRootR_avatar_lfoot)
                                {
                                    __cameraRootR_lfoot.Set(0, (cameraRootR_avatar_lfoot));
                                    system.Send(__cameraRootR_lfoot);
                                    PrevCameraRootR_lfoot = cameraRootR_avatar_lfoot;
                                }

                                if (PrevCameraRootR_rfoot != cameraRootR_avatar_rfoot)
                                {
                                    __cameraRootR_rfoot.Set(0, (cameraRootR_avatar_rfoot));
                                    system.Send(__cameraRootR_rfoot);
                                    PrevCameraRootR_rfoot = cameraRootR_avatar_rfoot;
                                }

                                if (PrevCameraRootR_lhand != cameraRootR_avatar_lhand)
                                {
                                    __cameraRootR_lhand.Set(0, (cameraRootR_avatar_lhand));
                                    system.Send(__cameraRootR_lhand);
                                    PrevCameraRootR_lhand = cameraRootR_avatar_lhand;
                                }

                                if (PrevCameraRootR_rhand != cameraRootR_avatar_rhand)
                                {
                                    __cameraRootR_rhand.Set(0, (cameraRootR_avatar_rhand));
                                    system.Send(__cameraRootR_rhand);
                                    PrevCameraRootR_rhand = cameraRootR_avatar_rhand;
                                }

                                if (PrevHidemodel != hidemodel)
                                {
                                    __hidemodel.Set(0, (hidemodel));
                                    system.Send(__hidemodel);
                                    PrevHidemodel = hidemodel;
                                }
                            }
                            break;
                    }

                    SendCameraDependent(system);
                }
                return true;
            } catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        void SendCameraDependent_Immediate(StudioSystem system)
        {
            switch (system.cameraType)
            {
                case ClientCameraType.VRCLens:
                    {
                        VRCLens_WhiteBalance.Set(0, vrclens_WhiteBalance);

                        system.Send(VRCLens_WhiteBalance);

                        prev_VRCLens_WhiteBalance = vrclens_WhiteBalance;
                    }
                    break;
                case ClientCameraType.VirtualLens2:
                    {
                        VirtualLens2_LayerMask_LocalPlayer.Set(0, virtuallens2_LayerMask.LocalPlayer ? 1 : 0);
                        VirtualLens2_LayerMask_RemotePlayer.Set(0, virtuallens2_LayerMask.RemotePlayer ? 1 : 0);
                        VirtualLens2_LayerMask_UI.Set(0, virtuallens2_LayerMask.UI ? 1 : 0);

                        VirtualLens2_AFMode.Set(0, ((int)virtuallens2_AFMode));
                        VirtualLens2_AFSpeed.Set(0, ((int)virtuallens2_AFSpeed));
                        VirtualLens2_PauseAF.Set(0, virtuallens2_PauseAF ? 1 : 0);
                        VirtualLens2_CullingDistance.Set(0, ((int)virtuallens2_CullingDistance));

                        system.Send(VirtualLens2_LayerMask_LocalPlayer);
                        system.Send(VirtualLens2_LayerMask_RemotePlayer);
                        system.Send(VirtualLens2_LayerMask_UI);
                        system.Send(VirtualLens2_AFMode);
                        system.Send(VirtualLens2_AFSpeed);
                        system.Send(VirtualLens2_PauseAF);
                        system.Send(VirtualLens2_CullingDistance);

                        prev_VirtualLens2_LayerMask = virtuallens2_LayerMask;
                        prev_VirtualLens2_AFMode = virtuallens2_AFMode;
                        prev_VirtualLens2_AFSpeed = virtuallens2_AFSpeed;
                        prev_VirtualLens2_PauseAF = virtuallens2_PauseAF;
                        prev_VirtualLens2_CullingDistance = virtuallens2_CullingDistance;
                    }
                    break;
                case ClientCameraType.Integral:
                    {
                        switch (integral_ShootMode)
                        {
                            case Integral_ShootModes.None:
                                {
                                    Integral_ShootMode.Set(0, 0);
                                }
                                break;
                            case Integral_ShootModes.Clear:
                                {
                                    Integral_ShootMode.Set(0, 5);
                                }
                                break;
                            case Integral_ShootModes.Open:
                                {
                                    Integral_ShootMode.Set(0, 3);
                                }
                                break;
                            case Integral_ShootModes.Bulb:
                                {
                                    Integral_ShootMode.Set(0, 2);
                                }
                                break;
                            case Integral_ShootModes.ClearAndOpen:
                                {
                                    Integral_ShootMode.Set(0, 6);
                                }
                                break;
                            case Integral_ShootModes.Single:
                                {
                                    Integral_ShootMode.Set(0, 4);
                                }
                                break;
                            case Integral_ShootModes.SS:
                                {
                                    Integral_ShootMode.Set(0, 7);
                                }
                                break;
                        }
                        Integral_ShutterSpeed.Set(0, integral_ShutterSpeed);
                        Integral_GridMode.Set(0, integral_GridMode ? 1 : 0);

                        if (focus == 0)
                        {
                            if (integral_FocusMode == 0)
                            {
                                integral_FocusMode = Integral_FocusModes.AF;
                            }
                        }
                        else
                        {
                            if (integral_FocusMode > 0)
                            {
                                integral_FocusMode = Integral_FocusModes.Manual;
                            }
                        }
                        Integral_FocusMode.Set(0, ((int)integral_FocusMode));
                        Integral_PauseAF.Set(0, integral_PauseAF ? 1 : 0);
                        Integral_Vignetting.Set(0, integral_Vignetting);
                        Integral_AxialChromaticAberration.Set(0, integral_AxialChromaticAberration);
                        Integral_BokehShape.Set(0, ((int)integral_BokehShape));
                        Integral_BokehShapeRotation.Set(0, integral_BokehShapeRotation);
                        switch (integral_CompositionMode)
                        {
                            case Integral_CompositionModes.Normal:
                                {
                                    Integral_CompositionMode.Set(0, 0);
                                }
                                break;
                            case Integral_CompositionModes.Lighten:
                                {
                                    Integral_CompositionMode.Set(0, 2);
                                }
                                break;
                            case Integral_CompositionModes.Darken:
                                {
                                    Integral_CompositionMode.Set(0, 4);
                                }
                                break;
                            case Integral_CompositionModes.Depth:
                                {
                                    Integral_CompositionMode.Set(0, 5);
                                }
                                break;
                        }
                        Integral_RollingShutterMode.Set(0, ((int)integral_RollingShutterMode));
                        Integral_Far.Set(0, integral_Far);
                        Integral_Curve.Set(0, ((int)integral_Curve));
                        Integral_AspectRatio.Set(0, integral_AspectRatioCustomValue);

                        system.Send(Integral_ShootMode);
                        system.Send(Integral_ShutterSpeed);
                        system.Send(Integral_GridMode);
                        system.Send(Integral_FocusMode);
                        system.Send(Integral_PauseAF);
                        system.Send(Integral_Vignetting);
                        system.Send(Integral_AxialChromaticAberration);
                        system.Send(Integral_BokehShape);
                        system.Send(Integral_BokehShapeRotation);
                        system.Send(Integral_CompositionMode);
                        system.Send(Integral_RollingShutterMode);
                        system.Send(Integral_Far);
                        system.Send(Integral_Curve);
                        system.Send(Integral_AspectRatio);

                        prev_Integral_ShootMode = integral_ShootMode;
                        prev_Integral_ShutterSpeed = integral_ShutterSpeed;
                        prev_Integral_GridMode = integral_GridMode;
                        prev_Integral_FocusMode = integral_FocusMode;
                        prev_Integral_PauseAF = integral_PauseAF;
                        prev_Integral_Vignetting = integral_Vignetting;
                        prev_Integral_AxialChromaticAberration = integral_AxialChromaticAberration;
                        prev_Integral_BokehShape = integral_BokehShape;
                        prev_Integral_BokehShapeRotation = integral_BokehShapeRotation;
                        prev_Integral_CompositionMode = integral_CompositionMode;
                        prev_Integral_RollingShutterMode = integral_RollingShutterMode;
                        prev_Integral_Far = integral_Far;
                        prev_Integral_Curve = integral_Curve;
                        prev_Integral_AspectRatioCustomValue = integral_AspectRatioCustomValue;

                    }
                    break;
                case ClientCameraType.VRChatNative:
                    {
                        VRCNative_Mode.Set(0, (int)vrcnative_Mode);

                        VRCNative_ShowUIInCamera.Set(0, vrcnative_LayerMask.UI);
                        VRCNative_LocalPlayer.Set(0, vrcnative_LayerMask.LocalPlayer);
                        VRCNative_RemotePlayer.Set(0, vrcnative_LayerMask.RemotePlayer);
                        VRCNative_Environment.Set(0, vrcnative_LayerMask.Environment);
                        VRCNative_GreenScreen.Set(0, vrcnative_GreenScreen);
                        VRCNative_Lock.Set(0, vrcnative_Lock);
                        VRCNative_SmoothMovement.Set(0, vrcnative_SmoothMovement);
                        VRCNative_AutoLevelRoll.Set(0, vrcnative_AutoLevelRoll);
                        VRCNative_AutoLevelPitch.Set(0, vrcnative_AutoLevelPitch);
                        VRCNative_AudioFromCamera.Set(0, vrcnative_AudioFromCamera);
                        VRCNative_ShowFocus.Set(0, vrcnative_ShowFocus);
                        VRCNative_Streaming.Set(0, vrcnative_Streaming);
                        VRCNative_OrientationIsLandscape.Set(0, vrcnative_OrientationIsLandscape);

                        VRCNative_Hue.Set(0, vrcnative_Hue);
                        VRCNative_Saturation.Set(0, vrcnative_Saturation);
                        VRCNative_Lightness.Set(0, vrcnative_Lightness);
                        VRCNative_SmoothingStrength.Set(0, vrcnative_SmoothingStrength);
                        __ax.Set(0, (ap.x));
                        __ay.Set(0, (ap.y));

                        system.Send(VRCNative_Mode);

                        system.Send(VRCNative_ShowUIInCamera);
                        system.Send(VRCNative_LocalPlayer);
                        system.Send(VRCNative_RemotePlayer);
                        system.Send(VRCNative_Environment);
                        system.Send(VRCNative_GreenScreen);
                        system.Send(VRCNative_Lock);
                        system.Send(VRCNative_SmoothMovement);
                        system.Send(VRCNative_AutoLevelRoll);
                        system.Send(VRCNative_AutoLevelPitch);
                        system.Send(VRCNative_AudioFromCamera);
                        system.Send(VRCNative_ShowFocus);
                        system.Send(VRCNative_Streaming);
                        system.Send(VRCNative_OrientationIsLandscape);

                        system.Send(VRCNative_Hue);
                        system.Send(VRCNative_Saturation);
                        system.Send(VRCNative_Lightness);
                        system.Send(VRCNative_SmoothingStrength);
                        system.Send(__ax);
                        system.Send(__ay);


                        prev_VRCNative_Mode                   = vrcnative_Mode;                   

                        prev_VRCNative_LayerMask              = vrcnative_LayerMask;              
                        prev_VRCNative_GreenScreen            = vrcnative_GreenScreen;            
                        prev_VRCNative_Lock                   = vrcnative_Lock;                   
                        prev_VRCNative_SmoothMovement         = vrcnative_SmoothMovement;         
                        prev_VRCNative_AutoLevelRoll          = vrcnative_AutoLevelRoll;          
                        prev_VRCNative_AutoLevelPitch         = vrcnative_AutoLevelPitch;         
                        prev_VRCNative_AudioFromCamera        = vrcnative_AudioFromCamera;        
                        prev_VRCNative_ShowFocus              = vrcnative_ShowFocus;              
                        prev_VRCNative_Streaming              = vrcnative_Streaming;              
                        prev_VRCNative_OrientationIsLandscape = vrcnative_OrientationIsLandscape; 

                        prev_VRCNative_Hue                    = vrcnative_Hue;                    
                        prev_VRCNative_Saturation             = vrcnative_Saturation;             
                        prev_VRCNative_Lightness              = vrcnative_Lightness;              
                        prev_VRCNative_SmoothingStrength      = vrcnative_SmoothingStrength;
                        PrevAp = ap;
                    }
                    break;
            }
        }

        void SendCameraDependent(StudioSystem system)
        {
            switch (system.cameraType)
            {
                case ClientCameraType.VRCLens:
                    {
                        if (prev_VRCLens_WhiteBalance != vrclens_WhiteBalance)
                        {
                            VRCLens_WhiteBalance.Set(0, vrclens_WhiteBalance);
                            system.Send(VRCLens_WhiteBalance);
                            prev_VRCLens_WhiteBalance = vrclens_WhiteBalance;
                        }
                    }
                    break;
                case ClientCameraType.VirtualLens2:
                    {
                        if (prev_VirtualLens2_LayerMask != virtuallens2_LayerMask)
                        {
                            VirtualLens2_LayerMask_LocalPlayer.Set(0, virtuallens2_LayerMask.LocalPlayer ? 1 : 0);
                            VirtualLens2_LayerMask_RemotePlayer.Set(0, virtuallens2_LayerMask.RemotePlayer ? 1 : 0);
                            VirtualLens2_LayerMask_UI.Set(0, virtuallens2_LayerMask.UI ? 1 : 0);
                            system.Send(VirtualLens2_LayerMask_LocalPlayer);
                            system.Send(VirtualLens2_LayerMask_RemotePlayer);
                            system.Send(VirtualLens2_LayerMask_UI);
                            prev_VirtualLens2_LayerMask = virtuallens2_LayerMask;
                        }

                        if (prev_VirtualLens2_AFMode != virtuallens2_AFMode)
                        {
                            VirtualLens2_AFMode.Set(0, ((int)virtuallens2_AFMode));
                            system.Send(VirtualLens2_AFMode);
                            prev_VirtualLens2_AFMode = virtuallens2_AFMode;
                        }

                        if (prev_VirtualLens2_AFSpeed != virtuallens2_AFSpeed)
                        {
                            VirtualLens2_AFSpeed.Set(0, ((int)virtuallens2_AFSpeed));
                            system.Send(VirtualLens2_AFSpeed);
                            prev_VirtualLens2_AFSpeed = virtuallens2_AFSpeed;
                        }

                        if (prev_VirtualLens2_PauseAF != virtuallens2_PauseAF)
                        {
                            VirtualLens2_PauseAF.Set(0, virtuallens2_PauseAF ? 1 : 0);
                            system.Send(VirtualLens2_PauseAF);
                            prev_VirtualLens2_PauseAF = virtuallens2_PauseAF;
                        }

                        if (prev_VirtualLens2_CullingDistance != virtuallens2_CullingDistance)
                        {
                            VirtualLens2_CullingDistance.Set(0, ((int)virtuallens2_CullingDistance));
                            system.Send(VirtualLens2_CullingDistance);
                            prev_VirtualLens2_CullingDistance = virtuallens2_CullingDistance;
                        }
                    }
                    break;
                case ClientCameraType.Integral:
                    {
                        if (prev_Integral_ShootMode != integral_ShootMode)
                        {
                            switch (integral_ShootMode)
                            {
                                case Integral_ShootModes.None:
                                    {
                                        Integral_ShootMode.Set(0, 0);
                                    }
                                    break;
                                case Integral_ShootModes.Clear:
                                    {
                                        Integral_ShootMode.Set(0, 5);
                                    }
                                    break;
                                case Integral_ShootModes.Open:
                                    {
                                        Integral_ShootMode.Set(0, 3);
                                    }
                                    break;
                                case Integral_ShootModes.Bulb:
                                    {
                                        Integral_ShootMode.Set(0, 2);
                                    }
                                    break;
                                case Integral_ShootModes.ClearAndOpen:
                                    {
                                        Integral_ShootMode.Set(0, 6);
                                    }
                                    break;
                                case Integral_ShootModes.Single:
                                    {
                                        Integral_ShootMode.Set(0, 4);
                                    }
                                    break;
                                case Integral_ShootModes.SS:
                                    {
                                        Integral_ShootMode.Set(0, 7);
                                    }
                                    break;
                            }
                            system.Send(Integral_ShootMode);
                            prev_Integral_ShootMode = integral_ShootMode;
                        }

                        if (prev_Integral_ShutterSpeed != integral_ShutterSpeed)
                        {
                            Integral_ShutterSpeed.Set(0, integral_ShutterSpeed);
                            system.Send(Integral_ShutterSpeed);
                            prev_Integral_ShutterSpeed = integral_ShutterSpeed;
                        }

                        if (prev_Integral_GridMode != integral_GridMode)
                        {
                            Integral_GridMode.Set(0, integral_GridMode ? 1 : 0);
                            system.Send(Integral_GridMode);
                            prev_Integral_GridMode = integral_GridMode;
                        }
                        
                        {
                            if (focus == 0)
                            {
                                if (integral_FocusMode == 0)
                                {
                                    integral_FocusMode = Integral_FocusModes.AF;
                                }
                            }
                            else
                            {
                                if (integral_FocusMode > 0)
                                {
                                    integral_FocusMode = Integral_FocusModes.Manual;
                                }
                            }
                            if (prev_Integral_FocusMode != integral_FocusMode)
                            {
                                Integral_FocusMode.Set(0, ((int)integral_FocusMode));
                                system.Send(Integral_FocusMode);
                                prev_Integral_FocusMode = integral_FocusMode;
                            }
                        }
                        
                        if (prev_Integral_PauseAF != integral_PauseAF)
                        {
                            Integral_PauseAF.Set(0, integral_PauseAF ? 1 : 0);
                            system.Send(Integral_PauseAF);
                            prev_Integral_PauseAF = integral_PauseAF;
                        }

                        if (prev_Integral_Vignetting != integral_Vignetting)
                        {
                            Integral_Vignetting.Set(0, integral_Vignetting);
                            system.Send(Integral_Vignetting);
                            prev_Integral_Vignetting = integral_Vignetting;
                        }

                        if (prev_Integral_AxialChromaticAberration != integral_AxialChromaticAberration)
                        {
                            Integral_AxialChromaticAberration.Set(0, integral_AxialChromaticAberration);
                            system.Send(Integral_AxialChromaticAberration);
                            prev_Integral_AxialChromaticAberration = integral_AxialChromaticAberration;
                        }

                        if (prev_Integral_BokehShape != integral_BokehShape)
                        {
                            Integral_BokehShape.Set(0, ((int)integral_BokehShape));
                            system.Send(Integral_BokehShape);
                            prev_Integral_BokehShape = integral_BokehShape;
                        }

                        if (prev_Integral_BokehShapeRotation != integral_BokehShapeRotation)
                        {
                            Integral_BokehShapeRotation.Set(0, integral_BokehShapeRotation);
                            system.Send(Integral_BokehShapeRotation);
                            prev_Integral_BokehShapeRotation = integral_BokehShapeRotation;
                        }

                        if (prev_Integral_CompositionMode != integral_CompositionMode)
                        {
                            Integral_CompositionMode.Set(0, ((int)integral_CompositionMode));
                            system.Send(Integral_CompositionMode);
                            prev_Integral_CompositionMode = integral_CompositionMode;
                        }

                        if (prev_Integral_RollingShutterMode != integral_RollingShutterMode)
                        {
                            Integral_RollingShutterMode.Set(0, ((int)integral_RollingShutterMode));
                            system.Send(Integral_RollingShutterMode);
                            prev_Integral_RollingShutterMode = integral_RollingShutterMode;
                        }

                        if (prev_Integral_Far != integral_Far)
                        {
                            Integral_Far.Set(0, integral_Far);
                            system.Send(Integral_Far);
                            prev_Integral_Far = integral_Far;
                        }

                        if (prev_Integral_Curve != integral_Curve)
                        {
                            Integral_Curve.Set(0, ((int)integral_Curve));
                            system.Send(Integral_Curve);
                            prev_Integral_Curve = integral_Curve;
                        }

                        if (prev_Integral_AspectRatioCustomValue != integral_AspectRatioCustomValue)
                        {
                            Integral_AspectRatio.Set(0, integral_AspectRatioCustomValue);
                            system.Send(Integral_AspectRatio);
                            prev_Integral_AspectRatioCustomValue = integral_AspectRatioCustomValue;
                        }
                    }
                    break;
                case ClientCameraType.VRChatNative:
                    {
                        if (prev_VRCNative_Mode != vrcnative_Mode)
                        {
                            VRCNative_Mode.Set(0, (int)vrcnative_Mode);
                            system.Send(VRCNative_Mode);
                            prev_VRCNative_Mode = vrcnative_Mode;
                        }

                        if (prev_VRCNative_LayerMask != vrcnative_LayerMask)
                        {
                            VRCNative_ShowUIInCamera.Set(0, vrcnative_LayerMask.UI);
                            VRCNative_LocalPlayer.Set(0, vrcnative_LayerMask.LocalPlayer);
                            VRCNative_RemotePlayer.Set(0, vrcnative_LayerMask.RemotePlayer);
                            VRCNative_Environment.Set(0, vrcnative_LayerMask.Environment);
                            system.Send(VRCNative_ShowUIInCamera);
                            system.Send(VRCNative_LocalPlayer);
                            system.Send(VRCNative_RemotePlayer);
                            system.Send(VRCNative_Environment);
                            prev_VRCNative_LayerMask = vrcnative_LayerMask;
                        }

                        if (prev_VRCNative_GreenScreen != vrcnative_GreenScreen)
                        {
                            VRCNative_GreenScreen.Set(0, vrcnative_GreenScreen);
                            system.Send(VRCNative_GreenScreen);
                            prev_VRCNative_GreenScreen = vrcnative_GreenScreen;
                        }

                        if (prev_VRCNative_Lock != vrcnative_Lock)
                        {
                            VRCNative_Lock.Set(0, vrcnative_Lock);
                            system.Send(VRCNative_Lock);
                            prev_VRCNative_Lock = vrcnative_Lock;
                        }

                        if (prev_VRCNative_SmoothMovement != vrcnative_SmoothMovement)
                        {
                            VRCNative_SmoothMovement.Set(0, vrcnative_SmoothMovement);
                            system.Send(VRCNative_SmoothMovement);
                            prev_VRCNative_SmoothMovement = vrcnative_SmoothMovement;
                        }

                        if (prev_VRCNative_AutoLevelRoll != vrcnative_AutoLevelRoll)
                        {
                            VRCNative_AutoLevelRoll.Set(0, vrcnative_AutoLevelRoll);
                            system.Send(VRCNative_AutoLevelRoll);
                            prev_VRCNative_AutoLevelRoll = vrcnative_AutoLevelRoll;
                        }

                        if (prev_VRCNative_AutoLevelPitch != vrcnative_AutoLevelPitch)
                        {
                            VRCNative_AutoLevelPitch.Set(0, vrcnative_AutoLevelPitch);
                            system.Send(VRCNative_AutoLevelPitch);
                            prev_VRCNative_AutoLevelPitch = vrcnative_AutoLevelPitch;
                        }

                        if (prev_VRCNative_AudioFromCamera != vrcnative_AudioFromCamera)
                        {
                            VRCNative_AudioFromCamera.Set(0, vrcnative_AudioFromCamera);
                            system.Send(VRCNative_AudioFromCamera);
                            prev_VRCNative_AudioFromCamera = vrcnative_AudioFromCamera;
                        }

                        if (prev_VRCNative_ShowFocus != vrcnative_ShowFocus)
                        {
                            VRCNative_ShowFocus.Set(0, vrcnative_ShowFocus);
                            system.Send(VRCNative_ShowFocus);
                            prev_VRCNative_ShowFocus = vrcnative_ShowFocus;
                        }

                        if (prev_VRCNative_Streaming != vrcnative_Streaming)
                        {
                            VRCNative_Streaming.Set(0, vrcnative_Streaming);
                            system.Send(VRCNative_Streaming);
                            prev_VRCNative_Streaming = vrcnative_Streaming;
                        }

                        if (prev_VRCNative_OrientationIsLandscape != vrcnative_OrientationIsLandscape)
                        {
                            VRCNative_OrientationIsLandscape.Set(0, vrcnative_OrientationIsLandscape);
                            system.Send(VRCNative_OrientationIsLandscape);
                            prev_VRCNative_OrientationIsLandscape = vrcnative_OrientationIsLandscape;
                        }

                        if (prev_VRCNative_Hue != vrcnative_Hue)
                        {
                            VRCNative_Hue.Set(0, vrcnative_Hue);
                            system.Send(VRCNative_Hue);
                            prev_VRCNative_Hue = vrcnative_Hue;
                        }

                        if (prev_VRCNative_Saturation != vrcnative_Saturation)
                        {
                            VRCNative_Saturation.Set(0, vrcnative_Saturation);
                            system.Send(VRCNative_Saturation);
                            prev_VRCNative_Saturation = vrcnative_Saturation;
                        }

                        if (prev_VRCNative_Lightness != vrcnative_Lightness)
                        {
                            VRCNative_Lightness.Set(0, vrcnative_Lightness);
                            system.Send(VRCNative_Lightness);
                            prev_VRCNative_Lightness = vrcnative_Lightness;
                        }

                        if (prev_VRCNative_SmoothingStrength != vrcnative_SmoothingStrength)
                        {
                            VRCNative_SmoothingStrength.Set(0, vrcnative_SmoothingStrength);
                            system.Send(VRCNative_SmoothingStrength);
                            prev_VRCNative_SmoothingStrength = vrcnative_SmoothingStrength;
                        }

                        if (PrevAp != ap)
                        {
                            __ax.Set(0, (ap.x));
                            __ay.Set(0, (ap.y));
                            system.Send(__ax);
                            system.Send(__ay);
                            PrevAp = ap;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Take Picture (using VRChat Native Camera)
        /// </summary>
        /// <param name="system"></param>
        public void VRCNative_TakePicture(StudioSystem system)
        {
            VRCNative_Capture.Set(0, true);
            system.Send(VRCNative_Capture);

            VRCNative_Capture.Set(0, false);
            system.Send(VRCNative_Capture);
        }

        /// <summary>
        /// Take Picture with delay (using VRChat Native Camera)
        /// </summary>
        /// <param name="system"></param>
        public void VRCNative_TakeTimerPicture(StudioSystem system)
        {
            VRCNative_CaptureDelayed.Set(0, true);
            system.Send(VRCNative_CaptureDelayed);

            VRCNative_CaptureDelayed.Set(0, false);
            system.Send(VRCNative_CaptureDelayed);
        }
    }
}
