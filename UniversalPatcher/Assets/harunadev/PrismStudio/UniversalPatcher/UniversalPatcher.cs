#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using UnityEditor;
using nadena.dev.modular_avatar.core;
using UnityEngine.Animations;
using VRC.SDKBase;
using System;
using VRC.SDK3.Dynamics.Contact.Components;
using UnityEditor.Animations;
using VRC.Dynamics;
using VRC.SDK3.Dynamics.Constraint.Components;
using nadena.dev.ndmf;
using harunadev;

[assembly: ExportsPlugin(typeof(UniversalAutoPatcher))]
namespace harunadev
{
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class UniversalPatcher : MonoBehaviour, IEditorOnly
    {
        public readonly string version = "1.4";
    
        public readonly string targetvrclensversion = "1.10.0";
        public readonly string targetvirtuallens2version = "2.12.3";
        public readonly string targetintegralversion = "0.39.0";
        public readonly string targetflexfisheyeversion = "20260303";
    
        public readonly string targetprismversion = "1.17";
    
        string TemplateTrackerFolderPath = "Assets/harunadev/PrismStudio/UniversalPatcher/Animations/Generated/";
        string TemplateTrackerAnimatorPath = "Assets/harunadev/PrismStudio/UniversalPatcher/Animations/Generated/Template.controller";
    
        public enum SupportedCameraType
        {
            VirtualLens2,
            VRCLens,
            Integral,
            FlexFishEye,
            Custom
        }

        public bool autoPatch = true;

        public VRCAvatarDescriptor TargetAvatar;
    
        public SupportedCameraType TargetCameraType = SupportedCameraType.VirtualLens2;
        public GameObject TargetCamera;
    
        public bool matchAvatarWriteDefaults = false;
    
        SerializedObject TargetVirtualLens2;
    
        GameObject TargetVRCLens;
        AbstractConstraint.Constraint TargetVRCLens_CamBase;
        Transform TargetVRCLens_DynVR_Parent;
        Vector3 TargetVRCLens_DynVR_PrevPosition;
        Quaternion TargetVRCLens_DynVR_PrevRotation;
        Transform TargetVRCLens_DynVR;
        AbstractConstraint.Constraint TargetVRCLens_DynVR_ParentConstraint;
        Transform TargetVRCLens_DynDesktop_Parent;
        Vector3 TargetVRCLens_DynDesktop_PrevPosition;
        Quaternion TargetVRCLens_DynDesktop_PrevRotation;
        Transform TargetVRCLens_DynDesktop;
        AbstractConstraint.Constraint TargetVRCLens_DynDesktop_ParentConstraint;
        Transform TargetVRCLens_DuplicateDynVR;
        Transform TargetVRCLens_DuplicateDynDesktop;
    
        GameObject TargetIntegral;
    
        GameObject TargetFishEyeCam;
        Transform TargetFishEyeCam_Camera;
        AbstractConstraint.Constraint TargetFishEyeCam_Camera_ParentConstraint;

        AbstractConstraint.Constraint TargetCustomCamera_ParentConstraint;

        private void Update()
        {
            if (TargetAvatar != null && autoPatch)
            {
                this.transform.parent = TargetAvatar.transform;
            }
        }

        public bool Patch()
        {
            if (Patch_().Length > 0)
            {
                return false;
            }
            return true;
        }

        public string Patch_()
        {
            if (Patch__() != null) { return Patch__().ToString(); }
            return "";
        }

        public Exception Patch__()
        {
            try
            {
                if (!FindCameraProperty())
                {
                    return new CameraNotFoundException();
                }

                Transform t = TargetAvatar.transform.Find("PrismStudio");

                if (t != null)
                {
                    RevertUniversalPatcher r = t.GetComponent<RevertUniversalPatcher>();

                    if (r != null)
                    {
                        r.RevertPatch();
                    }
                }

                this.transform.parent = TargetAvatar.transform;
                this.transform.SetAsLastSibling();
                this.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                this.name = "PrismStudio";

                if (!autoPatch) PrefabUtility.UnpackPrefabInstance(this.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

                SetCameraSpecific(TargetCameraType);

                VRCParentConstraint rootFix = this.gameObject.AddComponent<VRCParentConstraint>();
                rootFix.FreezeToWorld = true;
                rootFix.IsActive = true;

                AbstractConstraint.Constraint prismstudio_worldorigin_worlddrop = AbstractConstraint.GetConstraint(this.transform.Find("worldorigin/worlddrop"));
                AbstractConstraint.Constraint prismstudio_world = AbstractConstraint.GetConstraint(this.transform.Find("world"));

                prismstudio_worldorigin_worlddrop.SetSource(0, new ConstraintSource()
                {
                    sourceTransform = TargetAvatar.transform,
                    weight = 1
                });
                prismstudio_world.SetSource(0, new ConstraintSource()
                {
                    sourceTransform = TargetAvatar.transform,
                    weight = 1
                });

                switch (TargetCameraType)
                {
                    case SupportedCameraType.VirtualLens2:
                        {
                            if (!autoPatch)
                            {
                                TargetVirtualLens2.FindProperty("externalPoseSource").objectReferenceValue = this.transform.Find("main");
                                TargetVirtualLens2.FindProperty("enableManualFocusing").boolValue = true;

                                TargetVirtualLens2.ApplyModifiedProperties();
                            }
                        }
                        break;
                    case SupportedCameraType.VRCLens:
                        {
                            TargetVRCLens_CamBase = AbstractConstraint.GetConstraint(TargetVRCLens.transform.Find("WorldC/CamPickup/CamBase"));

                            for (int i = 0; i < TargetVRCLens_CamBase.sourceCount; i++)
                            {
                                if (TargetVRCLens_CamBase.GetSource(i).sourceTransform.name == "DynVR")
                                {
                                    TargetVRCLens_DynVR = TargetVRCLens_CamBase.GetSource(i).sourceTransform;
                                }
                                if (TargetVRCLens_CamBase.GetSource(i).sourceTransform.name == "DynDesktop")
                                {
                                    TargetVRCLens_DynDesktop = TargetVRCLens_CamBase.GetSource(i).sourceTransform;
                                }
                            }
                            TargetVRCLens_DynVR_PrevPosition = TargetVRCLens_DynVR.localPosition;
                            TargetVRCLens_DynVR_PrevRotation = TargetVRCLens_DynVR.localRotation;
                            TargetVRCLens_DynVR_Parent = TargetVRCLens_DynVR.parent;
                            TargetVRCLens_DynDesktop_PrevPosition = TargetVRCLens_DynDesktop.localPosition;
                            TargetVRCLens_DynDesktop_PrevRotation = TargetVRCLens_DynDesktop.localRotation;
                            TargetVRCLens_DynDesktop_Parent = TargetVRCLens_DynDesktop.parent;

                            TargetVRCLens_DuplicateDynVR = new GameObject("DynVR").transform;
                            TargetVRCLens_DuplicateDynDesktop = new GameObject("DynDesktop").transform;

                            TargetVRCLens_DynVR.SetParent(TargetVRCLens.transform);
                            TargetVRCLens_DynVR.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                            TargetVRCLens_DynDesktop.SetParent(TargetVRCLens.transform);
                            TargetVRCLens_DynDesktop.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

                            TargetVRCLens_DynVR_ParentConstraint = AbstractConstraint.GetAbstractConstraint(TargetVRCLens_DynVR.gameObject, TargetVRCLens_DynVR.gameObject.AddComponent<ParentConstraint>());
                            TargetVRCLens_DynVR_ParentConstraint.AddSource(new ConstraintSource()
                            {
                                sourceTransform = TargetVRCLens_DuplicateDynVR,
                                weight = 1
                            });
                            TargetVRCLens_DynVR_ParentConstraint.AddSource(new ConstraintSource()
                            {
                                sourceTransform = this.transform.Find("main"),
                                weight = 0
                            });
                            TargetVRCLens_DynVR_ParentConstraint.SetTranslationOffset(0, Vector3.zero);
                            TargetVRCLens_DynVR_ParentConstraint.SetTranslationOffset(1, Vector3.zero);
                            TargetVRCLens_DynVR_ParentConstraint.SetRotationOffset(0, Vector3.zero);
                            TargetVRCLens_DynVR_ParentConstraint.SetRotationOffset(1, Vector3.zero);
                            TargetVRCLens_DynVR_ParentConstraint.locked = true;
                            TargetVRCLens_DynVR_ParentConstraint.constraintActive = true;

                            TargetVRCLens_DynDesktop_ParentConstraint = AbstractConstraint.GetAbstractConstraint(TargetVRCLens_DynDesktop.gameObject, TargetVRCLens_DynDesktop.gameObject.AddComponent<ParentConstraint>());
                            TargetVRCLens_DynDesktop_ParentConstraint.AddSource(new ConstraintSource()
                            {
                                sourceTransform = TargetVRCLens_DuplicateDynDesktop,
                                weight = 1
                            });
                            TargetVRCLens_DynDesktop_ParentConstraint.AddSource(new ConstraintSource()
                            {
                                sourceTransform = this.transform.Find("main"),
                                weight = 0
                            });
                            TargetVRCLens_DynDesktop_ParentConstraint.SetTranslationOffset(0, Vector3.zero);
                            TargetVRCLens_DynDesktop_ParentConstraint.SetTranslationOffset(1, Vector3.zero);
                            TargetVRCLens_DynDesktop_ParentConstraint.SetRotationOffset(0, Vector3.zero);
                            TargetVRCLens_DynDesktop_ParentConstraint.SetRotationOffset(1, Vector3.zero);
                            TargetVRCLens_DynDesktop_ParentConstraint.locked = true;
                            TargetVRCLens_DynDesktop_ParentConstraint.constraintActive = true;

                            TargetVRCLens_DuplicateDynVR.SetParent(TargetVRCLens_DynVR_Parent);
                            TargetVRCLens_DuplicateDynVR.SetLocalPositionAndRotation(TargetVRCLens_DynVR_PrevPosition, TargetVRCLens_DynVR_PrevRotation);

                            TargetVRCLens_DuplicateDynDesktop.SetParent(TargetVRCLens_DynDesktop_Parent);
                            TargetVRCLens_DuplicateDynDesktop.SetLocalPositionAndRotation(TargetVRCLens_DynDesktop_PrevPosition, TargetVRCLens_DynDesktop_PrevRotation);
                            
                        }
                        break;
                    case SupportedCameraType.Integral: 
                        {
                            ModularAvatarBoneProxy hand_boneProxy = TargetIntegral.transform.Find("Integral/Tracker/Hand").GetComponent<ModularAvatarBoneProxy>();
                            AbstractConstraint.Constraint hand_constraint = AbstractConstraint.GetAbstractConstraint(hand_boneProxy.gameObject, hand_boneProxy.gameObject.AddComponent<ParentConstraint>());
                            hand_constraint.AddSource(new ConstraintSource()
                            {
                                sourceTransform = hand_boneProxy.target,
                                weight = 1
                            });
                            hand_constraint.SetTranslationOffset(0, Vector3.zero);
                            hand_constraint.SetRotationOffset(0, hand_boneProxy.target.localEulerAngles - hand_constraint.transform.localEulerAngles);
                            hand_constraint.constraintActive = true;
                            DestroyImmediate(hand_boneProxy);

                            ModularAvatarBoneProxy touchpoint_boneProxy = TargetIntegral.transform.Find("Integral/Tracker/Finger").GetComponent<ModularAvatarBoneProxy>();
                            if (touchpoint_boneProxy.target == null)
                            {
                                if (touchpoint_boneProxy.boneReference == HumanBodyBones.LeftIndexDistal)
                                {
                                    touchpoint_boneProxy.boneReference = HumanBodyBones.LeftIndexIntermediate;
                                }
                                else
                                {
                                    touchpoint_boneProxy.boneReference = HumanBodyBones.RightIndexIntermediate;
                                }
                            }
                            ParentConstraint touchpoint_constraint = touchpoint_boneProxy.gameObject.AddComponent<ParentConstraint>();
                            touchpoint_constraint.AddSource(new ConstraintSource()
                            {
                                sourceTransform = touchpoint_boneProxy.target,
                                weight = 1
                            });
                            touchpoint_constraint.SetTranslationOffset(0, Vector3.zero);
                            touchpoint_constraint.SetRotationOffset(0, touchpoint_boneProxy.target.localEulerAngles - touchpoint_constraint.transform.localEulerAngles);
                            touchpoint_constraint.constraintActive = true;
                            DestroyImmediate(touchpoint_boneProxy);


                            ParentConstraint integral_cameratracker = TargetIntegral.transform.Find("Integral/Tracker/Hand/CameraTracker").gameObject.AddComponent<ParentConstraint>();
                            integral_cameratracker.AddSource(new ConstraintSource()
                            {
                                sourceTransform = TargetIntegral.transform.Find("Integral/Tracker/Hand"),
                                weight = 1
                            });
                            integral_cameratracker.AddSource(new ConstraintSource()
                            {
                                sourceTransform = this.transform.Find("main"),
                                weight = 0
                            });
                            integral_cameratracker.constraintActive = true;


                            ParentConstraint touchtracker_constraint = TargetIntegral.transform.Find("Integral/Tracker/Finger/TouchTracker").gameObject.AddComponent<ParentConstraint>();
                            touchtracker_constraint.AddSource(new ConstraintSource()
                            {
                                sourceTransform = TargetIntegral.transform.Find("Integral/Tracker/Finger"),
                                weight = 1
                            });
                            touchtracker_constraint.AddSource(new ConstraintSource()
                            {
                                sourceTransform = this.transform.Find("CameraSpecific/Integral/touch/p"),
                                weight = 0
                            });
                            touchtracker_constraint.constraintActive = true;

                            this.transform.Find("CameraSpecific/Integral/DisplayHead").SetPositionAndRotation(
                                 TargetAvatar.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).position,
                                 TargetAvatar.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).rotation);

                            Transform displayRotationOffset = this.transform.Find("CameraSpecific/Integral/DisplayHead/ViewPosition");

                            float tforward = Vector3.Angle(displayRotationOffset.forward, TargetAvatar.transform.forward);
                            float tup = Vector3.Angle(displayRotationOffset.up, TargetAvatar.transform.forward);
                            float tright = Vector3.Angle(displayRotationOffset.right, TargetAvatar.transform.forward);

                            if (tforward > 30)
                            {
                                if (tup < 30)
                                {
                                    displayRotationOffset.forward = displayRotationOffset.up;
                                }
                                else if (tright < 30)
                                {
                                    displayRotationOffset.forward = displayRotationOffset.right;
                                }
                            }

                            ParentConstraint displayhead_parent = this.transform.Find("CameraSpecific/Integral/DisplayHead").GetComponent<ParentConstraint>();
                            displayhead_parent.SetSource(0, new ConstraintSource()
                            {
                                sourceTransform = TargetAvatar.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head),
                                weight = 1
                            });
                            displayhead_parent.SetTranslationOffset(0, Vector3.zero);
                            displayhead_parent.SetRotationOffset(0, Vector3.zero);
                            displayhead_parent.translationAtRest = Vector3.zero;
                            displayhead_parent.rotationAtRest = Vector3.zero;
                            displayhead_parent.locked = true;
                            displayhead_parent.constraintActive = true;

                            ParentConstraint touch_parent = this.transform.Find("CameraSpecific/Integral/touch").GetComponent<ParentConstraint>();
                            touch_parent.SetSource(0, new ConstraintSource()
                            {
                                sourceTransform = TargetIntegral.transform.Find("Integral/Avatar/Track/Offset/Screen/Quad"),
                                weight = 1
                            });
                            touch_parent.constraintActive = true;

                            ScaleConstraint touch_scale = this.transform.Find("CameraSpecific/Integral/touch").GetComponent<ScaleConstraint>();
                            touch_scale.SetSource(0, new ConstraintSource()
                            {
                                sourceTransform = TargetIntegral.transform.Find("Integral/Avatar/Track/Offset/Screen/Quad"),
                                weight = 1
                            });
                            touch_scale.constraintActive = true;
                        }
                        break;
                    case SupportedCameraType.FlexFishEye: 
                        {
                            TargetFishEyeCam_Camera = TargetFishEyeCam.transform.Find("WorldFixed");

                            TargetFishEyeCam_Camera_ParentConstraint = AbstractConstraint.GetAbstractConstraint(TargetFishEyeCam_Camera.gameObject, TargetFishEyeCam_Camera.gameObject.GetComponent<ParentConstraint>());

                            TargetFishEyeCam_Camera_ParentConstraint.AddSource(new ConstraintSource()
                            {
                                sourceTransform = this.transform.Find("main"),
                                weight = 0
                            });
                            TargetFishEyeCam_Camera_ParentConstraint.SetTranslationOffset(1, Vector3.zero);
                            TargetFishEyeCam_Camera_ParentConstraint.SetRotationOffset(1, Vector3.zero);
                        }
                        break;
                    default: 
                        {
                            TargetCustomCamera_ParentConstraint = AbstractConstraint.GetAbstractConstraint(TargetCamera.gameObject, TargetCamera.gameObject.AddComponent<ParentConstraint>());

                            TargetCustomCamera_ParentConstraint.AddSource(new ConstraintSource()
                            {
                                sourceTransform = this.transform.Find("main"),
                                weight = 0
                            });
                            TargetCustomCamera_ParentConstraint.SetTranslationOffset(0, Vector3.zero);
                            TargetCustomCamera_ParentConstraint.SetRotationOffset(0, Vector3.zero);
                            TargetCustomCamera_ParentConstraint.locked = true;
                            TargetCustomCamera_ParentConstraint.constraintActive = true;
                        }
                        break;
                }   

                EditorUtility.ClearProgressBar();

                RevertUniversalPatcher revertPatcher = this.gameObject.AddComponent<RevertUniversalPatcher>();
                revertPatcher.SetCameraType(TargetCameraType);
                revertPatcher.SetMetadata(this.version, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                switch (TargetCameraType)
                {
                    case SupportedCameraType.VirtualLens2:
                        {

                        }
                        break;
                    case SupportedCameraType.VRCLens:
                        {
                            revertPatcher.SetData(
                                TargetVRCLens_DynVR.gameObject,
                                TargetVRCLens_DynDesktop.gameObject,
                                TargetVRCLens_DuplicateDynVR.gameObject,
                                TargetVRCLens_DuplicateDynDesktop.gameObject);
                        }
                        break;
                    case SupportedCameraType.Integral:
                        {
                            revertPatcher.SetData(TargetIntegral);
                        }
                        break;
                    case SupportedCameraType.FlexFishEye:
                        {
                            revertPatcher.SetData(TargetFishEyeCam_Camera.gameObject);
                        }
                        break;
                }

                DestroyImmediate(this);
            } catch (Exception e)
            {
                Debug.LogException(e);
                return e;
            }
            return null;
        }
    
        void SetCameraSpecific(SupportedCameraType cameraType)
        {
            ModularAvatarMergeAnimator[] mergeAnimators = this.gameObject.GetComponentsInChildren<ModularAvatarMergeAnimator>();

            foreach (ModularAvatarMergeAnimator m in mergeAnimators)
            {
                m.matchAvatarWriteDefaults = this.matchAvatarWriteDefaults;
            }

            switch (cameraType)
            {
                case SupportedCameraType.VirtualLens2:
                    {
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/VRCLens").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/Integral").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/FlexFishEye").gameObject);
                        if (this.transform.Find("ModularAvatar/CameraSpecific/VirtualLens2").GetComponent<ModularAvatarMergeAnimator>().animator == null)
                            DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/VirtualLens2").gameObject);


                        DestroyImmediate(this.transform.Find("CameraSpecific/VRCLens").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/Integral").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/FlexFishEye").gameObject);
                        if (this.transform.Find("CameraSpecific/VirtualLens2").childCount == 0)
                            DestroyImmediate(this.transform.Find("CameraSpecific/VirtualLens2").gameObject);
                    }
                    break;
                case SupportedCameraType.VRCLens:
                    {
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/VirtualLens2").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/Integral").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/FlexFishEye").gameObject);

                        DestroyImmediate(this.transform.Find("CameraSpecific/VirtualLens2").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/Integral").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/FlexFishEye").gameObject);
                        if (this.transform.Find("CameraSpecific/VRCLens").childCount == 0)
                            DestroyImmediate(this.transform.Find("CameraSpecific/VRCLens").gameObject);
                    }
                    break;
                case SupportedCameraType.Integral:
                    {
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/VirtualLens2").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/VRCLens").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/FlexFishEye").gameObject);
                        if (this.transform.Find("ModularAvatar/CameraSpecific/Integral").GetComponent<ModularAvatarMergeAnimator>().animator == null)
                            DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/Integral").gameObject);

                        DestroyImmediate(this.transform.Find("CameraSpecific/VirtualLens2").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/VRCLens").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/FlexFishEye").gameObject);
                        if (this.transform.Find("CameraSpecific/Integral").childCount == 0)
                            DestroyImmediate(this.transform.Find("CameraSpecific/Integral").gameObject);
                    }
                    break;
                case SupportedCameraType.FlexFishEye:
                    {
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/VRCLens").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/VirtualLens2").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/Integral").gameObject);
                        if (this.transform.Find("ModularAvatar/CameraSpecific/FlexFishEye").GetComponent<ModularAvatarMergeAnimator>().animator == null)
                            DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/FlexFishEye").gameObject);

                        DestroyImmediate(this.transform.Find("CameraSpecific/VirtualLens2").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/VRCLens").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/Integral").gameObject);
                        if (this.transform.Find("CameraSpecific/FlexFishEye").childCount == 0)
                            DestroyImmediate(this.transform.Find("CameraSpecific/FlexFishEye").gameObject);
                    }
                    break;
                default:
                    {
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/VirtualLens2").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/VRCLens").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/Integral").gameObject);
                        DestroyImmediate(this.transform.Find("ModularAvatar/CameraSpecific/FlexFishEye").gameObject);

                        DestroyImmediate(this.transform.Find("CameraSpecific/VirtualLens2").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/VRCLens").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/Integral").gameObject);
                        DestroyImmediate(this.transform.Find("CameraSpecific/FlexFishEye").gameObject);
                    }
                    break;
            }
        }
    
        bool FindCameraProperty()
        {
            try
            {
                switch (TargetCameraType)
                {
                    case SupportedCameraType.VirtualLens2:
                        {
                            if (autoPatch)
                            {
                                TargetVirtualLens2 = new SerializedObject(TargetCamera.GetComponent("Transform"));

                            } else
                            {
                                TargetVirtualLens2 = new SerializedObject(TargetCamera.GetComponent("VirtualLensSettings"));
                            }
                            if (TargetVirtualLens2 != null)
                            {
                                return true;
                            }
                        }
                        break;
                    case SupportedCameraType.VRCLens:
                        {
                            if (TargetCamera.name == "VRCLens" && TargetCamera.transform.parent == TargetAvatar.transform)
                            {
                                TargetVRCLens = TargetCamera;
                                return true;
                            }
                        }
                        break;
                    case SupportedCameraType.Integral:
                        {
                            if (TargetCamera.name == "Integral" && TargetCamera.transform.parent == TargetAvatar.transform)
                            {
                                TargetIntegral = TargetCamera;
                                return true;
                            } else if (TargetCamera.name == "Integral_LeftHand" && TargetCamera.transform.parent == TargetAvatar.transform)
                            {
                                TargetIntegral = TargetCamera;
                                return true;
                            }
                        }
                        break;
                    case SupportedCameraType.FlexFishEye:
                        {
                            if (TargetCamera.name == "FishEyeCam" && TargetCamera.transform.parent == TargetAvatar.transform)
                            {
                                TargetFishEyeCam = TargetCamera;
                                return true;
                            }
                        }
                        break;
                    default:
                        {
                            return true;
                        }
                }
            } catch (Exception e) 
            {
                Debug.LogException(e);
                return false;
            }
            return false;
        }

        public void SanitizeAutoPatch(SupportedCameraType cameraType)
        {
            switch (cameraType)
            {
                case SupportedCameraType.VirtualLens2:
                    {
                        TargetVirtualLens2 = new SerializedObject(TargetCamera.GetComponent("VirtualLensSettings"));

                        TargetVirtualLens2.FindProperty("externalPoseSource").objectReferenceValue = this.transform.Find("main");
                        TargetVirtualLens2.FindProperty("enableManualFocusing").boolValue = true;

                        TargetVirtualLens2.ApplyModifiedProperties();
                    }
                    break;
                case SupportedCameraType.VRCLens:
                    {
                        
                    }
                    break;
                case SupportedCameraType.Integral:
                    {
                        
                    }
                    break;
                case SupportedCameraType.FlexFishEye:
                    {
                        
                    }
                    break;
                default:
                    {
                        
                    }
                    break;
            }

        }

        public class CameraNotFoundException : Exception
        {

        }
    }

    
    [DisallowMultipleComponent]
    [CustomEditor(typeof(UniversalPatcher))]
    public class UniversalPatcherEditor : Editor
    {
        readonly static string PrefabPath = "Assets/harunadev/PrismStudio/UniversalPatcher/UniversalPatcher.prefab";
    
        [MenuItem("GameObject/PrismStudio Auto Setup/Auto Detect (자동 감지)")] static void c0(MenuCommand menuCommand) { AutoDetectSetup(menuCommand); }
        [MenuItem("GameObject/PrismStudio Auto Setup/VirtualLens2")] static void c1(MenuCommand menuCommand) { AutoSetup(UniversalPatcher.SupportedCameraType.VirtualLens2, menuCommand.context as GameObject); }
        [MenuItem("GameObject/PrismStudio Auto Setup/VRCLens")] static void c2(MenuCommand menuCommand) { AutoSetup(UniversalPatcher.SupportedCameraType.VRCLens, menuCommand.context as GameObject); }
        [MenuItem("GameObject/PrismStudio Auto Setup/Integral")] static void c3(MenuCommand menuCommand) { AutoSetup(UniversalPatcher.SupportedCameraType.Integral, menuCommand.context as GameObject); }
        [MenuItem("GameObject/PrismStudio Auto Setup/FlexFishEye")] static void c4(MenuCommand menuCommand) { AutoSetup(UniversalPatcher.SupportedCameraType.FlexFishEye, menuCommand.context as GameObject); }
    
        static void AutoDetectSetup(MenuCommand menuCommand)
        {
            if (menuCommand.context == null)
            {
                GameObject iprefab = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath));
                Selection.activeGameObject = iprefab;
                return;
            }

            GameObject target = menuCommand.context as GameObject;
    
            if (target.GetComponent<VRCAvatarDescriptor>() == null)
            {
                VRCAvatarDescriptor atparent = target.GetComponentInParent<VRCAvatarDescriptor>();
                if (atparent != null)
                {
                    target = atparent.gameObject;
                } else
                {
                    Debug.LogError("Cannot auto-detect camera type!");
                    return;
                }
            }
    
            if (target.transform.Find("VirtualLens2"))
            {
                AutoSetup(UniversalPatcher.SupportedCameraType.VirtualLens2, target);
            } else if (target.transform.Find("VRCLens"))
            {
                AutoSetup(UniversalPatcher.SupportedCameraType.VRCLens, target);
            } else if (target.transform.Find("Integral") || target.transform.Find("Integral_LeftHand"))
            {
                AutoSetup(UniversalPatcher.SupportedCameraType.Integral, target);
            } else if (target.transform.Find("FishEyeCam") || target.transform.Find("FishEyeCam_Left"))
            {
                AutoSetup(UniversalPatcher.SupportedCameraType.FlexFishEye, target);
            } else 
            {
                Debug.LogError("Cannot auto-detect camera type!");
            }
        }
    
        static void AutoSetup(UniversalPatcher.SupportedCameraType cameraType, GameObject selectedObject)
        {
            GameObject iprefab = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath));
            VRCAvatarDescriptor targetavatar = selectedObject.GetComponent<VRCAvatarDescriptor>();
            UniversalPatcher ipatcher = iprefab.GetComponent<UniversalPatcher>();
    
            ipatcher.TargetAvatar = targetavatar;
            ipatcher.TargetCameraType = cameraType;
    
            switch (cameraType)
            {
                case UniversalPatcher.SupportedCameraType.VirtualLens2:
                    {
                        ipatcher.TargetCamera = targetavatar.transform.Find("VirtualLens2").gameObject;
                    }
                    break;
                case UniversalPatcher.SupportedCameraType.VRCLens:
                    {
                        ipatcher.TargetCamera = targetavatar.transform.Find("VRCLens").gameObject;
                    }
                    break;
                case UniversalPatcher.SupportedCameraType.Integral:
                    {
                        if (targetavatar.transform.Find("Integral") != null)
                        {
                            ipatcher.TargetCamera = targetavatar.transform.Find("Integral").gameObject;
                        } else
                        {
                            ipatcher.TargetCamera = targetavatar.transform.Find("Integral_LeftHand").gameObject;
                        }
                    }
                    break;
                case UniversalPatcher.SupportedCameraType.FlexFishEye:
                    {
                        if (targetavatar.transform.Find("FishEyeCam") != null)
                        {
                            ipatcher.TargetCamera = targetavatar.transform.Find("FishEyeCam").gameObject;
                        }
                        else
                        {
                            ipatcher.TargetCamera = targetavatar.transform.Find("FishEyeCam_Left").gameObject;
                        }
                    }
                    break;
            }
    
            Selection.activeGameObject = iprefab;
        }
    
    
        UniversalPatcher UniversalPatcher;
    
        int currentLanguage = 0;
    
        bool AlreadyPatched = false;
        bool IsPatchPreviousVersion = false;
        bool UnityVersionMismatch = false;
    
        List<Dictionary<string, string>> languageDatabase = new List<Dictionary<string, string>>()
        {
            new Dictionary<string, string>()
            {
                { "patcher_version", "Patcher version" },
                { "unity_version", "Unity version" },
                { "target_camera_version", "Target %1 version" },
                { "target_camera_version_readme", "While UniversalPatcher aims to be compatible with all the versions of your favorite camera, it's always the best to use the recommended (target) version." },
                { "target_prismstudio_version", "Target VRCPrismStudio version" },
                { "autopatch", "Auto-patch" },
                { "autopatch_desc", "This will use NDMF framework to automatically install PrismStudio to your avatar on upload." },
                { "avatar", "Avatar" },
                { "camera", "Target camera type" },
                { "match_avatar_writedefaults", "Match Avatar's Write Defaults" },
                { "match_avatar_writedefaults_info", "Some base avatars require unified 'Write Defaults' configurations. If you encounter issues that make your camera or PrismStudio unusable, try toggling this option." },
                { "tracker_count", "VirtualTracker Count" },
                { "tracker_count_info", "VRCPrismStudio can track a virtual tracker, and use LookAt to look at the tracker. This can be used to track other players, if you wish to make VRCPrismStudio look at other players. Do note that Virtual Trackers need to be placed inside their avatar. Refer to the usage manual for more information." },
                { "tracker_count_too_many", "Too many trackers! Consider use lower than 8. Recommended is 4." },
                { "desc", "Make sure you're using a supported version. After patching, upload your avatar." },
                { "patch_start_button", "Start patching" },
                { "error_unity_mismatch", "Unity 2022 is required!" },
                { "warning_previous_patch_found", "An old previous patch is found. Patcher will repatch the target with the latest patch." },
                { "warning_duplicate_patch_found", "Target is already patched with current patcher. Patcher will repatch the target." },
                { "dialog_confirm_title", "Proceed patching?" },
                { "dialog_confirm_message", "Check everything is configured. If you think it's good to go, press OK." },
                { "dialog_confirm_ok", "OK" },
                { "dialog_confirm_cancel", "Cancel" },
                { "dialog_patch_success_title", "Success" },
                { "dialog_patch_success_content", "Camera is successfully patched! You can now upload your avatar." },
                { "dialog_patch_fail_title", "Failed" },
                { "dialog_patch_fail_content", "Patcher has failed to patch. Check any errors on Console window, and report any issues to harunadev." },
                { "dialog_patch_button_confirm", "Close" },
            },
            new Dictionary<string, string>()
            {
                { "patcher_version", "패쳐 버전" },
                { "unity_version", "유니티 버전" },
                { "target_camera_version", "%1 요구 버전" },
                { "target_camera_version_readme", "UniversalPatcher는 어떤 버전이든 호환되는 것을 목표로 하고 있지만, Patcher가 추천 (요구) 하는 버전을 사용하는 것이 제일 이상적입니다." },
                { "target_prismstudio_version", "호환되는 VRCPrismStudio 버전" },
                { "autopatch", "자동 패치" },
                { "autopatch_desc", "이 기능을 활성화하면 NDMF 프레임워크를 사용하여 PrismStudio를 아바타에 자동으로 설치합니다." },
                { "avatar", "아바타" },
                { "camera", "카메라 타입" },
                { "match_avatar_writedefaults", "아바타 Write Defaults 설정을 사용" },
                { "match_avatar_writedefaults_info", "특정 아바타는 Write Defaults 설정이 통일되어야 정상작동하는 경우가 있습니다. 만약 카메라/PrismStudio가 사용불가능한 상태가 된다면, 이 옵션을 이전과 반대로 설정하세요." },
                { "tracker_count", "버추얼 트래커 갯수" },
                { "tracker_count_info", "VRCPrismStudio는 버추얼 트래커를 추적하고, LookAt을 사용해서 트래커를 바라보도록 할 수 있습니다. 이는 본인 외의 다른 플레이어를 추적하는데 사용될 수 있습니다. 다른 플레이어의 아바타에 버추얼 트래커가 배치되어야 합니다.  매뉴얼을 확인하여 상세한 사용법을 확인하세요." },
                { "tracker_count_too_many", "트래커 갯수가 너무 많습니다! 8개 이하, 또는 4개 이상을 사용하는 것을 추천합니다." },
                { "desc", "카메라의 버전이 패쳐가 지원하는 버전인지 확인하세요. 패치가 완료되면, 아바타를 업로드하세요." },
                { "patch_start_button", "패치 시작" },
                { "error_unity_mismatch", "Unity 2022.3 가 필요합니다!" },
                { "warning_previous_patch_found", "이전 버전의 패치가 발견되었습니다. 아래 버튼을 누르면 새로운 버전의 패치를 다시 적용합니다." },
                { "warning_duplicate_patch_found", "동일한 버전의 패치가 발견되었습니다. 아래 버튼을 누르면 다시 패치합니다." },
                { "dialog_confirm_title", "패치하시겠습니까?" },
                { "dialog_confirm_message", "모든게 정확히 설정되어있는지 확인해주세요. 모든게 괜찮다 생각되면 '진행하기' 버튼을 누르세요." },
                { "dialog_confirm_ok", "진행하기" },
                { "dialog_confirm_cancel", "취소" },
                { "dialog_patch_success_title", "성공" },
                { "dialog_patch_success_content", "카메라가 성공적으로 패치되었습니다! 이제 아바타를 업로하실 수 있습니다." },
                { "dialog_patch_fail_title", "실패" },
                { "dialog_patch_fail_content", "패치하는 중 오류가 발생했습니다. Console 창에 표시되는 에러를 확인하고, 필요시 harunadev에게 에러 제보를 해주세요." },
                { "dialog_patch_button_confirm", "확인" },
            },
            new Dictionary<string, string>()
            {
                { "patcher_version", "パッチャー バージョン" },
                { "unity_version", "Unity バージョン" },
                { "target_camera_version", "必要%1バージョン" },
                { "target_camera_version_readme", "UniversalPatcherはどのバージョンでも互換性のあるものを目指しますが、Patcherが推奨（要求）するバージョンを使用することが最も理想的です" },
                { "target_prismstudio_version", "互換性のあるVRCPrismStudioバージョン" },
                { "autopatch", "自動パッチ" },
                { "autopatch_desc", "この機能を有効にすると、NDMFフレームワークを使用してPrismStudioをアバターに自動的にインストールします。" },
                { "avatar", "アバター" },
                { "camera", "カメラ" },
                { "match_avatar_writedefaults", "アバター Write Defaults 設定を使用" },
                { "match_avatar_writedefaults_info", "特定のアバターは、Write Defaults設定が統一されてから正常に動作する場合があります。 もしカメラ/PrismStudioが使用不可能な状態になった場合、このオプションを以前と反対に設定してください。" },
                { "tracker_count", "バーチャルトラッカーの本数" },
                { "tracker_count_info", "VRCPrismStudioはバーチャルトラッカーを追跡し、LookAtを使ってトラッカーを眺めることができます。 これは、自分以外の他のプレーヤーを追跡するために使用される場合があります。 他のプレイヤーのアバターにバーチャル トラッカーを配置する必要があります。  マニュアルを確認して詳細な使い方を確認してください。" },
                { "tracker_count_too_many", "トラッカーの数が多すぎます！ 8 つ以下、または 4 つ以上を使用することをお勧めします。" },
                { "desc", "カメラのバージョンが対応するバージョンであることを確認してください。パッチが完了したら、アバターをアップロードしてください。" },
                { "patch_start_button", "パッチ開始" },
                { "error_unity_mismatch", "Unity 2022.3が必要です！" },
                { "warning_previous_patch_found", "以前のバージョンのパッチが見つかりました。 下のボタンを押すと新しいバージョンのパッチを再適用します。" },
                { "warning_duplicate_patch_found", "同じバージョンのパッチが見つかりました。 下のボタンを押すと再パッチします。" },
                { "dialog_confirm_title", "パッチしますか？" },
                { "dialog_confirm_message", "すべてが正確に設定されているか確認してください。 すべてが大丈夫だと思ったら「進行する」ボタンを押してください。" },
                { "dialog_confirm_ok", "進行する" },
                { "dialog_confirm_cancel", "キャンセル" },
                { "dialog_patch_success_title", "成功" },
                { "dialog_patch_success_content", "正常にパッチされました！ これでアバターをアップロードできます。" },
                { "dialog_patch_fail_title", "失敗" },
                { "dialog_patch_fail_content", "パッチ中にエラーが発生しました。 Consoleウィンドウに表示されるエラーを確認し、必要に応じてharunadevにエラー情報を提供してください。" },
                { "dialog_patch_button_confirm", "確認" },
            },
        };

        SerializedProperty autoPatch;
        SerializedProperty TargetCameraType;
        SerializedProperty TargetAvatar;
        SerializedProperty TargetCamera;
        SerializedProperty matchAvatarWriteDefaults;
        SerializedProperty CameraTrackerCount;

        public override void OnInspectorGUI()
        {
            UniversalPatcher = (UniversalPatcher)target;
    
            serializedObject.Update();
    
            currentLanguage = GUILayout.Toolbar(currentLanguage, new string[] { "English", "한국어", "日本語" });

            autoPatch = serializedObject.FindProperty("autoPatch");
            TargetCameraType = serializedObject.FindProperty("TargetCameraType");
            TargetAvatar = serializedObject.FindProperty("TargetAvatar");
            TargetCamera = serializedObject.FindProperty("TargetCamera");
            matchAvatarWriteDefaults = serializedObject.FindProperty("matchAvatarWriteDefaults");
            CameraTrackerCount = serializedObject.FindProperty("CameraTrackerCount");

            GUILayout.Space(12);
    
            GUILayout.BeginHorizontal();
    
            GUILayout.BeginVertical(new GUIStyle("box"));
            GUILayout.Label(UniversalPatcher.version, new GUIStyle("label") { fontSize = 16, fontStyle = FontStyle.Bold });
            GUILayout.Label(languageDatabase[currentLanguage]["patcher_version"]);
            GUILayout.EndVertical();
    
            GUILayout.BeginVertical(new GUIStyle("box"));
            GUILayout.Label(UniversalPatcher.targetprismversion, new GUIStyle("label") { fontSize = 16, fontStyle = FontStyle.Bold });
            GUILayout.Label(languageDatabase[currentLanguage]["target_prismstudio_version"]);
            GUILayout.EndVertical();
    
            GUILayout.EndHorizontal();
    
#if !UNITY_2022_3_OR_NEWER
    
            GUILayout.BeginVertical(new GUIStyle("box"));
            GUILayout.Label(languageDatabase[currentLanguage]["error_unity_mismatch"]);
            GUILayout.EndVertical();
    
            UnityVersionMismatch = true;
    
#endif
    
            GUILayout.Space(12);

            EditorGUILayout.PropertyField(autoPatch, new GUIContent(languageDatabase[currentLanguage]["autopatch"]));
            if (autoPatch.boolValue)
            {
                EditorGUILayout.HelpBox(languageDatabase[currentLanguage]["autopatch_desc"], MessageType.Info);
            }

            GUILayout.Space(12);

            TargetCameraType.enumValueIndex = EditorGUILayout.Popup(new GUIContent(languageDatabase[currentLanguage]["camera"]), TargetCameraType.enumValueIndex, TargetCameraType.enumNames);

            GUILayout.BeginHorizontal();
    
            GUILayout.BeginVertical(new GUIStyle("box"));
            switch (TargetCameraType.enumValueIndex)
            {
                case 0:
                    {
                        GUILayout.Label(UniversalPatcher.targetvirtuallens2version, new GUIStyle("label") { fontSize = 16, fontStyle = FontStyle.Bold });
                    }
                    break;
                case 1:
                    {
                        GUILayout.Label(UniversalPatcher.targetvrclensversion, new GUIStyle("label") { fontSize = 16, fontStyle = FontStyle.Bold });
                    }
                    break;
                case 2:
                    {
                        GUILayout.Label(UniversalPatcher.targetintegralversion, new GUIStyle("label") { fontSize = 16, fontStyle = FontStyle.Bold });
                    }
                    break;
                case 3:
                    {
                        GUILayout.Label(UniversalPatcher.targetflexfisheyeversion, new GUIStyle("label") { fontSize = 16, fontStyle = FontStyle.Bold });
                    }
                    break;
                default:
                    {
                        GUILayout.Label(UniversalPatcher.SupportedCameraType.Custom.ToString(), new GUIStyle("label") { fontSize = 16, fontStyle = FontStyle.Bold });
                    }
                    break;
            }
            GUILayout.Label(languageDatabase[currentLanguage]["target_camera_version"].Replace("%1", TargetCameraType.enumNames[TargetCameraType.enumValueIndex]));
            GUILayout.EndVertical();
    
            GUILayout.EndHorizontal();
    
            EditorGUILayout.HelpBox(languageDatabase[currentLanguage]["target_camera_version_readme"], MessageType.None);
    
            GUILayout.Space(12);
    
            GUILayout.Label(languageDatabase[currentLanguage]["desc"], EditorStyles.normal);
    
            GUILayout.Space(12);
    
            GUILayout.FlexibleSpace();
    
            EditorGUILayout.PropertyField(TargetAvatar, new GUIContent(languageDatabase[currentLanguage]["avatar"]));
    
            EditorGUILayout.PropertyField(TargetCamera, new GUIContent(TargetCameraType.enumNames[TargetCameraType.enumValueIndex]));
    
            if (TargetCamera.objectReferenceValue != null)
            {
                switch (TargetCameraType.enumValueIndex)
                {
                    case 0:
                        {
                            if (((GameObject)TargetCamera.objectReferenceValue).GetComponent("VirtualLensSettings") == null)
                            {
                                TargetCamera.objectReferenceValue = null;
                                Debug.LogWarning($"Selected camera object is not {TargetCameraType.enumNames[TargetCameraType.enumValueIndex]} object");
                            }
                        }
                        break;
                    case 1:
                        {
                            if (((GameObject)TargetCamera.objectReferenceValue).transform.Find("CamScreen") == null)
                            {
                                TargetCamera.objectReferenceValue = null;
                                Debug.LogWarning($"Selected camera object is not {TargetCameraType.enumNames[TargetCameraType.enumValueIndex]} object");
                            }
                        }
                        break;
                    case 2:
                        {
                            if (((GameObject)TargetCamera.objectReferenceValue).transform.Find("Integral") == null)
                            {
                                TargetCamera.objectReferenceValue = null;
                                Debug.LogWarning($"Selected camera object is not {TargetCameraType.enumNames[TargetCameraType.enumValueIndex]} object");
                            }
                        }
                        break;
                    case 3:
                        {
                            if (((GameObject)TargetCamera.objectReferenceValue).transform.Find("FishEyeMenu") == null)
                            {
                                TargetCamera.objectReferenceValue = null;
                                Debug.LogWarning($"Selected camera object is not {TargetCameraType.enumNames[TargetCameraType.enumValueIndex]} object");
                            }
                        }
                        break;
                }
            }

            GUILayout.Space(12);

            EditorGUILayout.PropertyField(matchAvatarWriteDefaults, new GUIContent(languageDatabase[currentLanguage]["match_avatar_writedefaults"]));
            EditorGUILayout.HelpBox(languageDatabase[currentLanguage]["match_avatar_writedefaults_info"], MessageType.Info);

            GUILayout.Space(12);
    
            serializedObject.ApplyModifiedProperties();
    
            if (UniversalPatcher.TargetAvatar != null)
            {
                Transform t = UniversalPatcher.TargetAvatar.transform.Find("PrismStudio");
    
                if (t != null)
                {
                    RevertUniversalPatcher r = t.GetComponent<RevertUniversalPatcher>();
                    
                    if (r != null)
                    {
                        AlreadyPatched = true;
    
                        IsPatchPreviousVersion = r.version != UniversalPatcher.version;
                    }
                } else
                {
                    AlreadyPatched = false;
                }
            }
    
            if (AlreadyPatched)
            {
                if (IsPatchPreviousVersion)
                {
                    EditorGUILayout.HelpBox(languageDatabase[currentLanguage]["warning_previous_patch_found"], MessageType.Warning);
                } else
                {
                    EditorGUILayout.HelpBox(languageDatabase[currentLanguage]["warning_duplicate_patch_found"], MessageType.Warning);
                }
            }

            if (!autoPatch.boolValue) 
            {
                EditorGUI.BeginDisabledGroup(!(UniversalPatcher.TargetAvatar && UniversalPatcher.TargetCamera));

                if (GUILayout.Button(languageDatabase[currentLanguage]["patch_start_button"], GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
                {
                    if (EditorUtility.DisplayDialog(languageDatabase[currentLanguage]["dialog_confirm_title"], languageDatabase[currentLanguage]["dialog_confirm_message"], languageDatabase[currentLanguage]["dialog_confirm_ok"], languageDatabase[currentLanguage]["dialog_confirm_cancel"]))
                    {
                        if (UniversalPatcher.Patch())
                        {
                            EditorUtility.DisplayDialog(languageDatabase[currentLanguage]["dialog_patch_success_title"], languageDatabase[currentLanguage]["dialog_patch_success_content"], languageDatabase[currentLanguage]["dialog_patch_button_confirm"]);
                        }
                        else
                        {
                            EditorUtility.DisplayDialog(languageDatabase[currentLanguage]["dialog_patch_fail_title"], languageDatabase[currentLanguage]["dialog_patch_fail_content"], languageDatabase[currentLanguage]["dialog_patch_button_confirm"]);
                        }
                        EditorUtility.ClearProgressBar();
                        Selection.activeObject = null;
                    }
                }

                EditorGUI.EndDisabledGroup();
            }
        }
    }

    public class UniversalAutoPatcher : Plugin<UniversalAutoPatcher>
    {
        public override string QualifiedName => "harunadev.UniversalAutoPatcher";
        public override string DisplayName => "UniversalPatcher (AutoMode)";

        protected override void Configure()
        {
            InPhase(BuildPhase.Generating).BeforePlugin("nadena.dev.modular-avatar").AfterPlugin("dev.logilabo.virtuallens2.apply-non-destructive").Run("Run UniversalPatcher (AutoMode)", ctx =>
            {
                VRCAvatarDescriptor avatar = ctx.AvatarRootObject.GetComponent<VRCAvatarDescriptor>();
                UniversalPatcher patcher = avatar.GetComponentInChildren<UniversalPatcher>(true);

                if (patcher != null)
                {
                    patcher.TargetAvatar = avatar;
                    Exception result = patcher.Patch__();

                    if (result != null)
                    {
                        UnityEngine.Object.DestroyImmediate(patcher.gameObject);
                        ErrorReport.ReportException(result);
                    }
                }
            });

            InPhase(BuildPhase.Generating).BeforePlugin("dev.logilabo.virtuallens2.apply-non-destructive").Run("Run Before VirtualLens2 (AutoMode)", ctx =>
            {
                VRCAvatarDescriptor avatar = ctx.AvatarRootObject.GetComponent<VRCAvatarDescriptor>();
                UniversalPatcher patcher = avatar.GetComponentInChildren<UniversalPatcher>(true);

                if (patcher != null)
                {
                    if (patcher.TargetCameraType == UniversalPatcher.SupportedCameraType.VirtualLens2)
                        patcher.SanitizeAutoPatch(UniversalPatcher.SupportedCameraType.VirtualLens2);
                }
            });
        }
    }


    public class RevertUniversalPatcher : MonoBehaviour, IEditorOnly
    {
        [SerializeField] public UniversalPatcher.SupportedCameraType cameraType;
        [SerializeField] public GameObject[] objectData;
    
        [SerializeField] public string version;
        [SerializeField] public string date;
    
    
        public void SetCameraType(UniversalPatcher.SupportedCameraType camera)
        {
            this.cameraType = camera;
        }
        
    
        public void SetData( params GameObject[] objects)
        {
            this.objectData = objects;
        }
    
        public void SetMetadata(string version, string date)
        {
            this.version = version;
            this.date = date;
        }
    
        public void RevertPatch()
        {
            switch (cameraType)
            {
                case UniversalPatcher.SupportedCameraType.VirtualLens2:
                    {
    
                    }
                    break;
                case UniversalPatcher.SupportedCameraType.VRCLens:
                    {
                        Vector3    DynVR_Pos = objectData[2].transform.position;
                        Quaternion DynVR_Rot = objectData[2].transform.rotation;
                        Vector3    DynDesktop_Pos = objectData[3].transform.position;
                        Quaternion DynDesktop_Rot = objectData[3].transform.rotation;
    
                        DestroyImmediate(objectData[0].GetComponent<ParentConstraint>());
                        DestroyImmediate(objectData[1].GetComponent<ParentConstraint>());
    
                        objectData[0].transform.SetPositionAndRotation(DynVR_Pos, DynVR_Rot);
                        objectData[1].transform.SetPositionAndRotation(DynDesktop_Pos, DynDesktop_Rot);
    
                        objectData[0].transform.SetParent(objectData[2].transform.parent);
                        objectData[1].transform.SetParent(objectData[3].transform.parent);
    
                        DestroyImmediate(objectData[2]);
                        DestroyImmediate(objectData[3]);
                    }
                    break;
                case UniversalPatcher.SupportedCameraType.Integral:
                    {
                        PrefabUtility.RevertPrefabInstance(objectData[0], InteractionMode.AutomatedAction);
                    }
                    break;
                case UniversalPatcher.SupportedCameraType.FlexFishEye:
                    {
                        AbstractConstraint.Constraint p = AbstractConstraint.GetConstraint(objectData[0]);
                        List<int> removeindex = new List<int>();

                        for (int i = 0; i < p.sourceCount; i++)
                        {
                            AbstractConstraint.ConstraintSource s = p.GetSource(i);
                            if (s.sourceTransform == this.transform.Find("main"))
                            {
                                removeindex.Add(i);
                            }
                        }

                        removeindex.Sort();
                        removeindex.Reverse();

                        foreach (int i in removeindex)
                        {
                            p.RemoveSource(i);
                        }
                    }
                    break;
            }
    
            DestroyImmediate(this.gameObject);
        }
    }
    
    [CustomEditor(typeof(RevertUniversalPatcher))]
    public class RevertUniversalPatcherEditor : Editor
    {
        RevertUniversalPatcher RevertUniversalPatcher;
    
        int currentLanguage = 0;
    
        List<Dictionary<string, string>> languageDatabase = new List<Dictionary<string, string>>()
        {
            new Dictionary<string, string>()
            {
                {"version", "Patched version" },
                {"date", "Patched date" },
                {"camera_info", "Target camera"},
                {"revert_info", "Pressing the button below will revert the patch."},
                {"revert_start_button", "Revert patch"},
                {"dialog_confirm_title", "Confirm revert?"},
                {"dialog_confirm_message", "This task will revert the camera to it's normal form, effectively removing PrismStudio from your avatar. This action is irreversible and you will need to patch the camera again if you wish to use PrismStudio again. Proceed?"},
                {"dialog_confirm_ok", "Confirm"},
                {"dialog_confirm_cancel", "Cancel"},
            },
            new Dictionary<string, string>()
            {
                {"version", "패치 버전" },
                {"date", "패치 날짜" },
                {"camera_info", "카메라"},
                {"revert_info", "아래 버튼을 누르면 카메라 패치가 원상복구됩니다."},
                {"revert_start_button", "패치 원상복구하기"},
                {"dialog_confirm_title", "원상복구하시겠습니까?"},
                {"dialog_confirm_message", "이 작업은 카메라를 원상복구함으로써 PrismStudio 기능을 아바타로부터 제거합니다. 이 작업은 되돌릴 수 없으며, 다시 PrismStudio를 사용하기 원하는 경우 카메라를 다시 패치해야합니다. 진행하시겠습니까?"},
                {"dialog_confirm_ok", "진행"},
                {"dialog_confirm_cancel", "취소"},
            },
            new Dictionary<string, string>()
            {
                {"version", "パッチバージョン" },
                {"date", "パッチ日付" },
                {"camera_info", "カメラ"},
                {"revert_info", "下のボタンを押すと、パッチが元に戻ります。"},
                {"revert_start_button", "パッチ元に戻す"},
                {"dialog_confirm_title", "元に戻しますか？"},
                {"dialog_confirm_message", "この作業はカメラを原状復旧することによってPrismStudio機能をアバターから取り除きます。 この作業は元に戻すことができず、再びPrismStudioを使用したい場合は、カメラを再びパッチする必要があります。 進めますか？"},
                {"dialog_confirm_ok", "進行"},
                {"dialog_confirm_cancel", "キャンセル"},
            },
        };
    
        public override void OnInspectorGUI()
        {
            RevertUniversalPatcher = (RevertUniversalPatcher)target;
    
            serializedObject.Update();
    
            currentLanguage = GUILayout.Toolbar(currentLanguage, new string[] { "English", "한국어", "日本語" });
    
            GUILayout.BeginVertical(new GUIStyle("box"));
            GUILayout.Label(RevertUniversalPatcher.date, new GUIStyle("label") { fontSize = 16, fontStyle = FontStyle.Bold }); 
            GUILayout.Label(languageDatabase[currentLanguage]["date"]);
            GUILayout.EndVertical();
    
            GUILayout.BeginVertical(new GUIStyle("box"));
            GUILayout.Label(RevertUniversalPatcher.version, new GUIStyle("label") { fontSize = 16, fontStyle = FontStyle.Bold }); 
            GUILayout.Label(languageDatabase[currentLanguage]["version"]);
            GUILayout.EndVertical();
    
            GUILayout.BeginVertical(new GUIStyle("box"));
            GUILayout.Label(RevertUniversalPatcher.cameraType.ToString(), new GUIStyle("label") { fontSize = 16, fontStyle = FontStyle.Bold });
            GUILayout.Label(languageDatabase[currentLanguage]["camera_info"]);
            GUILayout.EndVertical();
    
    
            serializedObject.ApplyModifiedProperties();
    
            if (GUILayout.Button(languageDatabase[currentLanguage]["revert_start_button"], GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
            {
                if (EditorUtility.DisplayDialog(languageDatabase[currentLanguage]["dialog_confirm_title"], languageDatabase[currentLanguage]["dialog_confirm_message"], languageDatabase[currentLanguage]["dialog_confirm_ok"], languageDatabase[currentLanguage]["dialog_confirm_cancel"]))
                {
                    RevertUniversalPatcher.RevertPatch();
                }
            }
        }
    }
    
    public static class EditorStyles
    {
        public static GUIStyle header
        {
            get
            {
                GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
                style.fontSize = 18;
                return style;
            }
        }
        public static GUIStyle subheader
        {
            get
            {
                GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
                style.fontSize = 14;
                return style;
            }
        }
        public static GUIStyle normalrich
        {
            get
            {
                GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
                style.wordWrap = true;
                style.richText = true;
                style.fontSize = 12;
                return style;
            }
        }
        public static GUIStyle normal
        {
            get
            {
                GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
                style.wordWrap = true;
                style.fontSize = 12;
                return style;
            }
        }
    
        public static GUIStyle foldoutHeader
        {
            get
            {
                GUIStyle style = new GUIStyle(GUI.skin.GetStyle("foldout"));
                style.fontSize = 14;
                return style;
            }
        }
    }

    public static class AbstractConstraint
    {
        public struct Constraint
        {
            public UnityEngine.Animations.IConstraint unityConstraint;
            public ParentConstraint unityParentConstraint;
            public PositionConstraint unityPositionConstraint;
            public RotationConstraint unityRotationConstraint;
            public ScaleConstraint unityScaleConstraint;
            public LookAtConstraint unityLookAtConstraint;
            public AimConstraint unityAimConstraint;
            public VRCConstraintBase vrcConstraint;
            public bool isUnityConstraint;
            public int sourceCount
            {
                get
                {
                    if (isUnityConstraint)
                    {
                        return unityConstraint.sourceCount;
                    }
                    else
                    {
                        return vrcConstraint.Sources.Count;
                    }
                }
            }
            public bool locked
            {
                get
                {
                    if (isUnityConstraint)
                    {
                        return unityConstraint.locked;
                    }
                    else
                    {
                        return vrcConstraint.Locked;
                    }
                }
                set
                {
                    if (isUnityConstraint)
                    {
                        unityConstraint.locked = value;
                    }
                    else
                    {
                        vrcConstraint.Locked = value;
                    }
                }
            }
            public bool constraintActive
            {
                get
                {
                    if (isUnityConstraint)
                    {
                        return unityConstraint.constraintActive;
                    }
                    else
                    {
                        return vrcConstraint.IsActive;
                    }
                }
                set
                {
                    if (isUnityConstraint)
                    {
                        unityConstraint.constraintActive = value;
                    }
                    else
                    {
                        vrcConstraint.IsActive = value;
                    }
                }
            }

            public readonly GameObject gameObject
            {
                get
                {
                    if (isUnityConstraint)
                    {
                        if      (unityParentConstraint    ) return unityParentConstraint  .gameObject;
                        else if (unityPositionConstraint  ) return unityPositionConstraint.gameObject;
                        else if (unityRotationConstraint  ) return unityRotationConstraint.gameObject;
                        else if (unityScaleConstraint     ) return unityScaleConstraint   .gameObject;
                        else if (unityLookAtConstraint    ) return unityLookAtConstraint  .gameObject;
                        else if (unityAimConstraint       ) return unityAimConstraint     .gameObject;
                        else return null;
                    }
                    else
                    {
                        return vrcConstraint.gameObject;
                    }
                }
            }
            public readonly Transform transform
            {
                get
                {
                    if (isUnityConstraint)
                    {
                        if      (unityParentConstraint    ) return unityParentConstraint  .transform;
                        else if (unityPositionConstraint  ) return unityPositionConstraint.transform;
                        else if (unityRotationConstraint  ) return unityRotationConstraint.transform;
                        else if (unityScaleConstraint     ) return unityScaleConstraint   .transform;
                        else if (unityLookAtConstraint    ) return unityLookAtConstraint  .transform;
                        else if (unityAimConstraint       ) return unityAimConstraint     .transform;
                        else return null;
                    }
                    else
                    {
                        return vrcConstraint.transform;
                    }
                }
            }

            public Axis translationAxis 
            {
                get
                {
                    if (isUnityConstraint)
                    {
                        if      (unityParentConstraint    ) return unityParentConstraint.translationAxis;
                        else return unityPositionConstraint.translationAxis;
                    }
                    return new Axis();
                }
                set
                {
                    if (isUnityConstraint)
                    {
                        if (unityParentConstraint) unityParentConstraint.translationAxis = value;
                        else unityPositionConstraint.translationAxis = value;
                    }
                }
            }

            public ConstraintSource GetSource(int index)
            {
                return AbstractConstraint.GetSource(this, index);
            }

            public void AddSource(UnityEngine.Animations.ConstraintSource source)
            {
                if (isUnityConstraint)
                    AbstractConstraint.AddSource(this, source);
                else 
                    AbstractConstraint.AddSource(this, new VRCConstraintSource()
                    {
                        SourceTransform = source.sourceTransform,
                        Weight = source.weight
                    });
            }
            public void AddSource(VRCConstraintSource source)
            {
                if (isUnityConstraint)
                    AbstractConstraint.AddSource(this, new UnityEngine.Animations.ConstraintSource()
                    {
                        sourceTransform = source.SourceTransform,
                        weight = source.Weight
                    });
                else 
                    AbstractConstraint.AddSource(this, source);
            }
            public void SetSource(int index, UnityEngine.Animations.ConstraintSource source)
            {
                if (isUnityConstraint)
                    AbstractConstraint.SetSource(this, index, source);
                else
                    AbstractConstraint.SetSource(this, index, new VRCConstraintSource()
                    {
                        SourceTransform = source.sourceTransform,
                        Weight = source.weight
                    });
            }
            public void SetSource(int index, VRCConstraintSource source)
            {
                if (isUnityConstraint)
                    AbstractConstraint.SetSource(this, index, new UnityEngine.Animations.ConstraintSource()
                    {
                        sourceTransform = source.SourceTransform,
                        weight = source.Weight
                    });
                else
                    AbstractConstraint.SetSource(this, index, source);
            }
            public void RemoveSource(int index)
            {
                AbstractConstraint.RemoveSource(this, index);
            }
            public void SetTranslationOffset(int index, Vector3 offset)
            {
                if (isUnityConstraint)
                {
                    unityParentConstraint.SetTranslationOffset(index, offset);
                }
                else
                {
                    VRCConstraintSource s = vrcConstraint.Sources[index];
                    s.ParentPositionOffset = offset;
                    vrcConstraint.Sources.RemoveAt(index);
                    vrcConstraint.Sources.Insert(index, s);
                }
            }
            public void SetRotationOffset(int index, Vector3 offset)
            {
                if (isUnityConstraint)
                {
                    unityParentConstraint.SetRotationOffset(index, offset);
                }
                else
                {
                    VRCConstraintSource s = vrcConstraint.Sources[index];
                    s.ParentRotationOffset = offset;
                    vrcConstraint.Sources.RemoveAt(index);
                    vrcConstraint.Sources.Insert(index, s);
                }
            }
        }
        public struct ConstraintSource
        {
            public UnityEngine.Animations.ConstraintSource unityConstraintSource;
            public VRCConstraintSource vrcConstraintSource;
            public bool isUnityConstraint;
            public Transform sourceTransform
            {
                get
                {
                    if (isUnityConstraint)
                    {
                        return unityConstraintSource.sourceTransform;
                    }
                    else
                    {
                        return vrcConstraintSource.SourceTransform;
                    }
                }
            }
        }

        public static Constraint GetConstraint(GameObject target)
        {
            UnityEngine.Animations.IConstraint uc = target.GetComponent<UnityEngine.Animations.IConstraint>();
            if (uc != null)
            {
                return new Constraint()
                {
                    unityConstraint = uc,
                    unityParentConstraint = (ParentConstraint)target.GetComponent<ParentConstraint>(),
                    unityPositionConstraint = (PositionConstraint)target.GetComponent<PositionConstraint>(),
                    unityRotationConstraint = (RotationConstraint)target.GetComponent<RotationConstraint>(),
                    unityScaleConstraint = (ScaleConstraint)target.GetComponent<ScaleConstraint>(),
                    unityLookAtConstraint = (LookAtConstraint)target.GetComponent<LookAtConstraint>(),
                    unityAimConstraint = (AimConstraint)target.GetComponent<AimConstraint>(),
                    vrcConstraint = null,
                    isUnityConstraint = true
                };
            } else
            {
                return new Constraint()
                {
                    unityConstraint = null,
                    vrcConstraint = target.GetComponent<VRCConstraintBase>(),
                    isUnityConstraint = false
                };
            }
        }
        public static Constraint GetConstraint(Transform target)
        {
            return GetConstraint(target.gameObject);
        }
        public static Constraint GetAbstractConstraint(GameObject target, UnityEngine.Animations.IConstraint constraint)
        {
            return new Constraint()
            {
                unityConstraint = constraint,
                unityParentConstraint = constraint as ParentConstraint,
                unityPositionConstraint = constraint as PositionConstraint,
                unityRotationConstraint = constraint as RotationConstraint,
                unityScaleConstraint = constraint as ScaleConstraint,
                unityLookAtConstraint = constraint as LookAtConstraint,
                unityAimConstraint = constraint as AimConstraint,
                isUnityConstraint = true
            };
        }
        public static Constraint GetAbstractConstraint(VRCConstraintBase constraint)
        {
            return new Constraint()
            {
                vrcConstraint = constraint,
                isUnityConstraint = false
            };
        }
        public static void Destroy(Constraint constraint)
        {
            if (constraint.isUnityConstraint)
            {
                if      (constraint.unityParentConstraint    ) UnityEngine.Object.Destroy(constraint.unityParentConstraint   );
                else if (constraint.unityPositionConstraint  ) UnityEngine.Object.Destroy(constraint.unityPositionConstraint );
                else if (constraint.unityRotationConstraint  ) UnityEngine.Object.Destroy(constraint.unityRotationConstraint );
                else if (constraint.unityScaleConstraint     ) UnityEngine.Object.Destroy(constraint.unityScaleConstraint    );
                else if (constraint.unityLookAtConstraint    ) UnityEngine.Object.Destroy(constraint.unityLookAtConstraint   );
                else if (constraint.unityAimConstraint       ) UnityEngine.Object.Destroy(constraint.unityAimConstraint      );
            }
            else
            {
                UnityEngine.Object.Destroy(constraint.vrcConstraint);
            }
        }
        public static void DestroyImmediate(Constraint constraint)
        {
            if (constraint.isUnityConstraint)
            {
                if      (constraint.unityParentConstraint    ) UnityEngine.Object.DestroyImmediate(constraint.unityParentConstraint   );
                else if (constraint.unityPositionConstraint  ) UnityEngine.Object.DestroyImmediate(constraint.unityPositionConstraint );
                else if (constraint.unityRotationConstraint  ) UnityEngine.Object.DestroyImmediate(constraint.unityRotationConstraint );
                else if (constraint.unityScaleConstraint     ) UnityEngine.Object.DestroyImmediate(constraint.unityScaleConstraint    );
                else if (constraint.unityLookAtConstraint    ) UnityEngine.Object.DestroyImmediate(constraint.unityLookAtConstraint   );
                else if (constraint.unityAimConstraint       ) UnityEngine.Object.DestroyImmediate(constraint.unityAimConstraint      );
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(constraint.vrcConstraint);
            }
        }
        public static ConstraintSource GetSource(Constraint target, int index)
        {
            if (target.isUnityConstraint)
            {
                return new ConstraintSource()
                {
                    unityConstraintSource = target.unityConstraint.GetSource(index),
                    isUnityConstraint = true
                };
            } else
            {
                return new ConstraintSource()
                {
                    vrcConstraintSource = target.vrcConstraint.Sources[index],
                    isUnityConstraint = false
                };
            }
        }
        public static void AddSource(Constraint target, UnityEngine.Animations.ConstraintSource source)
        {
            AddSource(target, new ConstraintSource()
            {
                unityConstraintSource = source,
                isUnityConstraint = true
            });
        }
        public static void AddSource(Constraint target, VRCConstraintSource source)
        {
            AddSource(target, new ConstraintSource()
            {
                vrcConstraintSource = source,
                isUnityConstraint = false
            });
        }
        public static void AddSource(Constraint target, ConstraintSource source)
        {
            if (target.isUnityConstraint)
            {
                target.unityConstraint.AddSource(source.unityConstraintSource);
            }
            else
            {
                target.vrcConstraint.Sources.Add(source.vrcConstraintSource);
            }
        }
        public static void SetSource(Constraint target, int index, UnityEngine.Animations.ConstraintSource source)
        {
            SetSource(target, index, new ConstraintSource()
            {
                unityConstraintSource = source,
                isUnityConstraint = true
            });
        }
        public static void SetSource(Constraint target, int index, VRCConstraintSource source)
        {
            SetSource(target, index, new ConstraintSource()
            {
                vrcConstraintSource = source,
                isUnityConstraint = false
            });
        }
        public static void SetSource(Constraint target, int index, ConstraintSource source)
        {
            if (target.isUnityConstraint)
            {
                target.unityConstraint.SetSource(index, source.unityConstraintSource);
            }
            else
            {
                target.vrcConstraint.Sources.RemoveAt(index);
                target.vrcConstraint.Sources.Insert(index, source.vrcConstraintSource);
            }
        }
        public static void RemoveSource(Constraint target, int index)
        {
            if (target.isUnityConstraint)
            {
                target.unityConstraint.RemoveSource(index);
            }
            else
            {
                target.vrcConstraint.Sources.RemoveAt(index);
            }
        }
    }
}

#endif