using Cinemachine;
using OscCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PrismStudio.CameraPoint.PostProcessor;
using static PrismStudio.I18n;
using static PrismStudio.StudioSystem;

namespace PrismStudio
{
    /// <summary>
    /// PrismStudio Core: CameraPoint
    /// (c) harunadev
    /// </summary>
    public class CameraPoint : Module
    {
        /// <summary>
        /// Description of CameraPoint
        /// CameraPoint 설명
        /// </summary>
        /// <param name="lang">language</param>
        /// <returns></returns>
        public override string description(SupportedLanguage lang)
        {
            switch (lang)
            {
                case SupportedLanguage.Korean:
                    {
                        return "PrismStudio의 기본 기능; 오브젝트의 위치가 카메라의 위치가 됩니다";
                    }
                case SupportedLanguage.Japanese:
                    {
                        return "PrismStudioの基本機能; オブジェクトの位置がカメラの位置になります";
                    }
            }
            return "Core function of PrismStudio; The position of the object becomes the position of the camera";
        }

        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_general;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_camerashake;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_fppcam;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_postprocessors;

        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] public float fpp_lookSpeed;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] public float fpp_moveSpeed;

        /// <summary>
        /// Point object's forward vector calculation distance
        /// Point 오브젝트의 앞 벡터 계산 거리
        /// </summary>
        [SerializeField] public float PointForwardDistance = 50f;
        
        /// <summary>
        /// Use Cinemachine
        /// | Cinemachine을 사용
        /// </summary>
        [SerializeField] public bool useCinemachine = false;
        /// <summary>
        /// Target Cinemachine Brain
        /// | 추적할 CinemachineBrain 컴포넌트
        /// </summary>
        [SerializeField] public CinemachineBrain targetCinemachine;

        /// <summary>
        /// Direct access to PostProcessor collection object (public get only)
        /// PostProcessor 모음 오브젝트에 접근
        /// </summary>
        public Transform PostProcessorObject
        {
            get
            {
                if (_PostProcessorObject == null)
                {
                    _PostProcessorObject = this.transform.Find("PostProcessors");
                }
                return _PostProcessorObject;
            }
            private set
            {
                _PostProcessorObject = value;
            }
        }
        Transform _PostProcessorObject = null;
        
        /// <summary>
        /// Direct access to FinalizedTransform Object (the finallized position &amp; rotation)
        /// FinalizedTransform 오브젝트에 접근 (최종 위치 &amp; 각도)
        /// </summary>
        public Transform FinalizedTransformObject
        {
            get
            {
                if (_FinalizedTransformObject == null)
                {
                    _FinalizedTransformObject = this.transform.Find("FinalizedTransform");
                }
                return _FinalizedTransformObject;
            }
            private set
            {
                _FinalizedTransformObject = value;
            }
        }
        Transform _FinalizedTransformObject = null;
        Vector3 FinalizedTransformObjectScale = new Vector3(0.97f, 0.97f, 0.97f);

        Vector3 PrevPosition = Vector3.zero;
        Vector3 PrevRotation = Vector3.zero;
        Vector3 PrevRotationUp = Vector3.zero;
        //float PrevRotationW = 0;

        Vector3 PointForward;
        Vector3 PointUp;
        OscMessage px = new OscMessage("/avatar/parameters/ps_x", 0f);
        OscMessage py = new OscMessage("/avatar/parameters/ps_y", 0f);
        OscMessage pz = new OscMessage("/avatar/parameters/ps_z", 0f);
        OscMessage sx = new OscMessage("/avatar/parameters/ps_rx", 0f);
        OscMessage sy = new OscMessage("/avatar/parameters/ps_ry", 0f);
        OscMessage sz = new OscMessage("/avatar/parameters/ps_rz", 0f);
        OscMessage ux = new OscMessage("/avatar/parameters/ps_ux", 0f);
        OscMessage uy = new OscMessage("/avatar/parameters/ps_uy", 0f);
        OscMessage uz = new OscMessage("/avatar/parameters/ps_uz", 0f);
        OscMessage vrcnative_pose = new OscMessage("/usercamera/Pose", 0f, 0f, 0f, 0f, 0f, 0f);
        //OscMessage sr;

        private void Update()
        {
            if (useCinemachine && targetCinemachine != null)
            {
                transform.SetPositionAndRotation(targetCinemachine.transform.position, targetCinemachine.transform.rotation);
            }

            if (_PostProcessorObject == null)
            {
                _PostProcessorObject = this.transform.Find("PostProcessors");
            }
            _PostProcessorObject.localPosition = Vector3.zero;
            _PostProcessorObject.localRotation = Quaternion.identity;
            _PostProcessorObject.localScale = Vector3.one;

            if (_FinalizedTransformObject == null)
            {
                _FinalizedTransformObject = this.transform.Find("FinalizedTransform");
            }
            _FinalizedTransformObject.transform.localScale = FinalizedTransformObjectScale;

            UpdateFinalTransform();
        }

        Vector3 ForwardPoint(Quaternion rotation, float distance)
        {
            return (rotation * (Vector3.forward * distance));
        }

        Vector3 UpPoint(Quaternion rotation)
        {
            return (rotation * (Vector3.up));
        }

        float GetRawRotation(float rot)
        {
            if (rot < 0)
            {
                return 360f - Mathf.Abs(rot - (360f * ((int)rot / 360)));
            }
            return rot - (360f * ((int)rot / 360));
        }
        /*
        void initializeMessages(ClientCameraType cameraType)
        {
            switch (cameraType)
            {
                case ClientCameraType.VRCLens:
                    {
                        px = new OscMessage("/avatar/parameters/VRCL_PrismStudio_Point_px");
                        py = new OscMessage("/avatar/parameters/VRCL_PrismStudio_Point_py");
                        pz = new OscMessage("/avatar/parameters/VRCL_PrismStudio_Point_pz");
                        sx = new OscMessage("/avatar/parameters/VRCL_PrismStudio_Point_sx");
                        sy = new OscMessage("/avatar/parameters/VRCL_PrismStudio_Point_sy");
                        sz = new OscMessage("/avatar/parameters/VRCL_PrismStudio_Point_sz");
                        sr = new OscMessage("/avatar/parameters/VRCL_PrismStudio_Point_w");
                    }
                    break;
                case ClientCameraType.VirtualLens2:
                    {
                        px = new OscMessage("/avatar/parameters/PrismStudio_px");
                        py = new OscMessage("/avatar/parameters/PrismStudio_py");
                        pz = new OscMessage("/avatar/parameters/PrismStudio_pz");
                        sx = new OscMessage("/avatar/parameters/PrismStudio_sx");
                        sy = new OscMessage("/avatar/parameters/PrismStudio_sy");
                        sz = new OscMessage("/avatar/parameters/PrismStudio_sz");
                        sr = new OscMessage("/avatar/parameters/PrismStudio_w");
                    }
                    break;
                case ClientCameraType.Integral:
                    {
                        px = new OscMessage("/avatar/parameters/PrismStudio_px");
                        py = new OscMessage("/avatar/parameters/PrismStudio_py");
                        pz = new OscMessage("/avatar/parameters/PrismStudio_pz");
                        sx = new OscMessage("/avatar/parameters/PrismStudio_sx");
                        sy = new OscMessage("/avatar/parameters/PrismStudio_sy");
                        sz = new OscMessage("/avatar/parameters/PrismStudio_sz");
                        sr = new OscMessage("/avatar/parameters/PrismStudio_w");
                    }
                    break;
            }
        }
        */
        void initializeMessages(ClientCameraType cameraType)
        {
            /*
            switch (cameraType)
            {
                case ClientCameraType.VRChatNative:
                    {
                        px.Address = "/usercamera/Pose";
                    }
                    break;
                default:
                    {
                        px.Address = "/avatar/parameters/ps_x";
                        py.Address = "/avatar/parameters/ps_y";
                        pz.Address = "/avatar/parameters/ps_z";
                        sx.Address = "/avatar/parameters/ps_rx";
                        sy.Address = "/avatar/parameters/ps_ry";
                        sz.Address = "/avatar/parameters/ps_rz";
                        ux.Address = "/avatar/parameters/ps_ux";
                        uy.Address = "/avatar/parameters/ps_uy";
                        uz.Address = "/avatar/parameters/ps_uz";
                    }
                    break;
            }*/
            //sr = new OscMessage("/avatar/parameters/ps_w");
            //switch (cameraType)
            //{
            //    case ClientCameraType.VRCLens:
            //        {
            //            px = new OscMessage("/avatar/parameters/ps_x");
            //            py = new OscMessage("/avatar/parameters/ps_y");
            //            pz = new OscMessage("/avatar/parameters/ps_z");
            //            sx = new OscMessage("/avatar/parameters/ps_rx");
            //            sy = new OscMessage("/avatar/parameters/ps_ry");
            //            sz = new OscMessage("/avatar/parameters/ps_rz");
            //            sr = new OscMessage("/avatar/parameters/ps_w");
            //        }
            //        break;
            //    case ClientCameraType.VirtualLens2:
            //        {
            //            px = new OscMessage("/avatar/parameters/PrismStudio_px");
            //            py = new OscMessage("/avatar/parameters/PrismStudio_py");
            //            pz = new OscMessage("/avatar/parameters/PrismStudio_pz");
            //            sx = new OscMessage("/avatar/parameters/PrismStudio_sx");
            //            sy = new OscMessage("/avatar/parameters/PrismStudio_sy");
            //            sz = new OscMessage("/avatar/parameters/PrismStudio_sz");
            //            sr = new OscMessage("/avatar/parameters/PrismStudio_w");
            //        }
            //        break;
            //    case ClientCameraType.Integral:
            //        {
            //            px = new OscMessage("/avatar/parameters/PrismStudio_px");
            //            py = new OscMessage("/avatar/parameters/PrismStudio_py");
            //            pz = new OscMessage("/avatar/parameters/PrismStudio_pz");
            //            sx = new OscMessage("/avatar/parameters/PrismStudio_sx");
            //            sy = new OscMessage("/avatar/parameters/PrismStudio_sy");
            //            sz = new OscMessage("/avatar/parameters/PrismStudio_sz");
            //            sr = new OscMessage("/avatar/parameters/PrismStudio_w");
            //        }
            //        break;
            //}
        }

        void UpdateFinalTransform()
        {
            ProcessData data = new ProcessData()
            {
                position = transform.position,
                rotation = transform.rotation,
                originalPosition = transform.position,
                originalRotation = transform.rotation,
            };

            for (int i = 0; i < PostProcessors.Length; i++)
            {
                data.originalPosition = transform.position;
                data.originalRotation = transform.rotation;

                data = PostProcessors[i].Process(data);
            }

            FinalizedTransformObject.SetPositionAndRotation(data.position, data.rotation);
        }


        /// <summary>
        /// Called from StudioSystem
        /// StudioSystem에서 호출하는 함수
        /// </summary>
        /// <param name="system">StudioSystem</param>
        /// <param name="immediate">send OSC now?</param>
        /// <returns>is success?</returns>
        /// <returns>is success?</returns>
        public override bool OnSend(StudioSystem system, bool immediate)
        {
            // return base.Send(system, immediate);

            try
            {
                Vector3 position = FinalizedTransformObject.position;
                Quaternion rotation = FinalizedTransformObject.rotation;

                PointForward = ForwardPoint(rotation, PointForwardDistance);
                PointUp = UpPoint(rotation);

                if (immediate)
                {
                    initializeMessages(system.cameraType);

                    if (system.cameraType == ClientCameraType.VRChatNative)
                    {
                        Vector3 eulerAngle = rotation.eulerAngles;

                        vrcnative_pose.Set(0, (position.x));
                        vrcnative_pose.Set(1, (position.y));
                        vrcnative_pose.Set(2, (position.z));
                        vrcnative_pose.Set(3, (eulerAngle.x));
                        vrcnative_pose.Set(4, (eulerAngle.y));
                        vrcnative_pose.Set(5, (eulerAngle.z));

                        system.Send(vrcnative_pose);
                    } else
                    {
                        px.Set(0, (position.x));
                        py.Set(0, (position.y));
                        pz.Set(0, (position.z));
                        sz.Set(0, (PointForward.z));
                        sx.Set(0, (PointForward.x));
                        sy.Set(0, (PointForward.y));
                        uz.Set(0, (PointUp.z));
                        ux.Set(0, (PointUp.x));
                        uy.Set(0, (PointUp.y));
                        //sr.Add((GetRawRotation(rotation.eulerAngles.z) / 360f));

                        system.Send(px);
                        system.Send(py);
                        system.Send(pz);
                        system.Send(sx);
                        system.Send(sy);
                        system.Send(sz);
                        system.Send(ux);
                        system.Send(uy);
                        system.Send(uz);
                        //system.Send(sr);

                    }

                    PrevPosition = new Vector3(position.x, position.y, position.z);
                    PrevRotation = new Vector3(PointForward.x, PointForward.y, PointForward.z);
                    PrevRotationUp = new Vector3(PointUp.x, PointUp.y, PointUp.z);
                    //PrevRotationW = GetRawRotation(rotation.eulerAngles.z) / 360f;
                }
                else
                {
                    initializeMessages(system.cameraType);

                    if (system.cameraType == ClientCameraType.VRChatNative)
                    {
                        Vector3 eulerAngle = rotation.eulerAngles;

                        vrcnative_pose.Set(0, (position.x));
                        vrcnative_pose.Set(1, (position.y));
                        vrcnative_pose.Set(2, (position.z));
                        vrcnative_pose.Set(3, (eulerAngle.x));
                        vrcnative_pose.Set(4, (eulerAngle.y));
                        vrcnative_pose.Set(5, (eulerAngle.z));

                        system.Send(vrcnative_pose);

                        PrevPosition = new Vector3(position.x, position.y, position.z);
                        PrevRotation = new Vector3(PointForward.x, PointForward.y, PointForward.z);
                        PrevRotationUp = new Vector3(PointUp.x, PointUp.y, PointUp.z);
                    }
                    else
                    {
                        if (PrevPosition != transform.localPosition)
                        {
                            px.Set(0, (position.x));
                            py.Set(0, (position.y));
                            pz.Set(0, (position.z));

                            system.Send(px);
                            system.Send(py);
                            system.Send(pz);

                            PrevPosition = new Vector3(position.x, position.y, position.z);
                        }

                        //if (PrevRotation != PointForward || PrevRotationW != GetRawRotation(rotation.eulerAngles.z) / 360f)
                        if (PrevRotation != PointForward || PrevRotationUp != PointUp)
                        {
                            sz.Set(0, (PointForward.z));
                            sx.Set(0, (PointForward.x));
                            sy.Set(0, (PointForward.y));
                            uz.Set(0, (PointUp.z));
                            ux.Set(0, (PointUp.x));
                            uy.Set(0, (PointUp.y));
                            //sr.Add((GetRawRotation(rotation.eulerAngles.z) / 360f));

                            system.Send(sz);
                            system.Send(sx);
                            system.Send(sy);
                            system.Send(ux);
                            system.Send(uy);
                            system.Send(uz);
                            //system.Send(sr);

                            PrevRotation = new Vector3(PointForward.x, PointForward.y, PointForward.z);
                            PrevRotationUp = new Vector3(PointUp.x, PointUp.y, PointUp.z);
                            //PrevRotationW = GetRawRotation(rotation.eulerAngles.z) / 360f;
                        }
                    }
                }
                return true;
            } catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        /// <summary>
        /// Registered PostProcessors
        /// 등록된 PostProcessor 배열
        /// </summary>
        public PostProcessor[] PostProcessors
        {
            get
            {
                ValidatePostProcessorsList();

                return _PostProcessors;
            }
        }
        [SerializeField] PostProcessor[] _PostProcessors = new PostProcessor[0];

        /// <summary>
        /// Validate PostProcessor array
        /// PostProcessor 배열을 검사
        /// </summary>
        public void ValidatePostProcessorsList()
        {
            if (PostProcessorObject.childCount != _PostProcessors.Length)
            {
                UpdatePostProcessorsList();
            }
            _PostProcessors = _PostProcessors.Where(m => m != null).ToArray();
        }

        /// <summary>
        /// Repopulate PostProcessor Array
        /// PostProcessor 배열 재구성
        /// </summary>
        public void UpdatePostProcessorsList()
        {
            List<PostProcessor> l = new List<PostProcessor>();

            foreach (Transform t in PostProcessorObject)
            {
                if (t.TryGetComponent<PostProcessor>(out var p))
                {
                    l.Add(p);
                }
            }

            _PostProcessors = l.Where(m => m != null).ToArray();
        }

        #region PostProcessor define
        /// <summary>
        /// PrismStudio Core: PostProcessor
        /// (c) harunadev
        /// </summary>
        [Serializable, ExecuteInEditMode]
        public abstract class PostProcessor : MonoBehaviour
        {
            /// <summary>
            /// PostProcessor Initializer
            /// </summary>
            public PostProcessor()
            {

            }

            /// <summary>
            /// Process the given Vector3 &amp; Quaternion
            /// 주어진 Vector3와 Quaternion을 처리합니다
            /// </summary>
            /// <param name="data">data from caller</param>
            /// <returns>pair of Vector3 and Quaternion as a form of ProcessData</returns>
            public virtual ProcessData Process(ProcessData data)
            {
                return data;
            }

            /// <summary>
            /// Pair of Vector3 and Quaternion
            /// Vector3와 Quaternion의 집합
            /// </summary>
            public struct ProcessData
            {
                /// <summary>
                /// Position (Vector3)
                /// </summary>
                public Vector3 position;
                /// <summary>
                /// Rotation (Quaternion)
                /// </summary>
                public Quaternion rotation;
                /// <summary>
                /// Original Position (Vector3)
                /// </summary>
                public Vector3 originalPosition;
                /// <summary>
                /// Original Rotation (Quaternion)
                /// </summary>
                public Quaternion originalRotation;
            }
        }
        #endregion
    }

    /// <summary>
    /// PrismStudio Core: PostProcessorCollection
    /// (c) harunadev
    /// </summary>
    public class PostProcessorCollection : MonoBehaviour 
    {
        
    }
}
