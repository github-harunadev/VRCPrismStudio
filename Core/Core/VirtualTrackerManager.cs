/*
//using PrismStudio.OSC;
using OscCore;
using PrismStudio.OSC;
using System;
using UnityEngine;
using static PrismStudio.I18n;
using static PrismStudio.StudioSystem;

namespace PrismStudio
{
    /// <summary>
    /// PrismStudio Core: VirtualTrackerManager
    /// (c) harunadev
    /// </summary>
    [ExecuteInEditMode, RequireComponent(typeof(OscReceiver))]
    public class VirtualTrackerManager : Module
    {
        /// <summary>
        /// Internal
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public override string description(SupportedLanguage lang)
        {
            switch (lang)
            {
                case SupportedLanguage.Korean:
                    {
                        return "PrismStudio의 기본 기능; VirtualTracker를 관리합니다";
                    }
                case SupportedLanguage.Japanese:
                    {
                        return "PrismStudioの基本機能; VirtualTrackerを管理します";
                    }
            }
            return "Core function of PrismStudio; Manages VirtualTrackers";
        }

        OscReceiver oscReceiver;

        /// <summary>
        /// Tracker count
        /// | 트래커 갯수
        /// </summary>
        public int TrackerCount
        {
            get
            {
                return _trackerCount;
            }
            set
            {
                if (value < 0 || value > byte.MaxValue) return;
                if (_trackerCount != value)
                {
                    VerifyVirtualTracker(value);
                }
                _trackerCount = value;
            }
        }
        [SerializeField] private int _trackerCount = 0;

        /// <summary>
        /// Registered VirtualTrackerProperty
        /// | 등록된 VirtualTrackerProperty
        /// </summary>
        public VirtualTrackerProperty[] Trackers
        {
            get
            {
                return _trackers;
            }
            private set
            {
                _trackers = value;
            }
        }
        private VirtualTrackerProperty[] _trackers = new VirtualTrackerProperty[0];

        OscMessage[] tracker_toggle;
        OscMessage[] tracker_reset;
        OscMessage[] tracker_lookat;
        OscMessage[] tracker_camerarootp;
        OscMessage[] tracker_camerarootr;
        VirtualTrackerProperty[] tracker_property;

        bool trigger_tracker_refresh = false;
        int tracker_count;
        int prev_tracker_count = -1;

        void Awake()
        {
            oscReceiver = GetComponent<OscReceiver>();
            studioSystem = GetComponentInParent<StudioSystem>();
        }
        
        void Update()
        {
            VerifyVirtualTracker(TrackerCount);
        }
        void VerifyVirtualTracker(int newCount)
        {
            if (_trackers.Length != newCount)
            {
                _trackers = new VirtualTrackerProperty[newCount];
            }

            foreach (Transform t in transform)
            {
                VirtualTrackerProperty p = t.GetComponent<VirtualTrackerProperty>();
                if (p != null)
                {
                    if (p.virtualTrackerIndex < newCount)
                    {
                        p.gameObject.SetActive(true);
                        _trackers[p.virtualTrackerIndex] = p;
                    } else
                    {
                        p.gameObject.SetActive(false);
                    }
                }
            }


            for (int i = 0; i < newCount; i++)
            {
                if (_trackers[i] == null)
                {
                    GameObject newTracker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    newTracker.name = i.ToString();
                    newTracker.transform.parent = transform;
                    newTracker.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    DestroyImmediate(newTracker.GetComponent<Collider>());
                    VirtualTrackerProperty prop = newTracker.AddComponent<VirtualTrackerProperty>();
                    prop.SetManager(this);
                    prop.virtualTrackerIndex = i;

                    _trackers[i] = prop;
                }
            }

            initializeMessages();
        }

        void ReceiveRawTrackingData_0(OscMessage message) { ReceiveRawTrackingData(message, 0); }
        void ReceiveRawTrackingData_1(OscMessage message) { ReceiveRawTrackingData(message, 1); }
        void ReceiveRawTrackingData_2(OscMessage message) { ReceiveRawTrackingData(message, 2); }
        void ReceiveRawTrackingData_3(OscMessage message) { ReceiveRawTrackingData(message, 3); }
        void ReceiveRawTrackingData_4(OscMessage message) { ReceiveRawTrackingData(message, 4); }
        void ReceiveRawTrackingData_5(OscMessage message) { ReceiveRawTrackingData(message, 5); }
        void ReceiveRawTrackingData(OscMessage message, byte index)
        {
            int i = message.Address.IndexOf("_t_") + 3;
            int tracker_index = Convert.ToInt32(message.Address.Substring(i, message.Address.LastIndexOf('_') - i));

            tracker_property[tracker_index].SetRawTrackingData(index, (float)message[0]);
        }

        /// <summary>
        /// Internal
        /// </summary>
        /// <param name="studioSystem"></param>
        public override void OnStudioSystemInitialized(StudioSystem studioSystem)
        {
            initializeMessages();
            initializeReceiveActions();
        }

        /// <summary>
        /// Internal
        /// </summary>
        /// <param name="newCameraType"></param>
        public override void OnCameraTypeChanged(ClientCameraType newCameraType)
        {
            initializeMessages();
            initializeReceiveActions();
        }

        void initializeMessages()
        {
            tracker_count = transform.childCount;

            if (prev_tracker_count != tracker_count)
            {
                tracker_toggle = new OscMessage[tracker_count];
                tracker_reset = new OscMessage[tracker_count];
                tracker_lookat = new OscMessage[tracker_count];
                tracker_camerarootp = new OscMessage[tracker_count];
                tracker_camerarootr = new OscMessage[tracker_count];

                trigger_tracker_refresh = false;
                if (tracker_property == null || tracker_property.Length != tracker_count)
                {
                    trigger_tracker_refresh = true;
                }
                else
                {
                    for (int i = 0; i < tracker_count; i++)
                    {
                        if (tracker_property[i] == null)
                        {
                            trigger_tracker_refresh = true;
                            break;
                        }
                    }
                }
                if (trigger_tracker_refresh)
                {
                    tracker_property = new VirtualTrackerProperty[tracker_count];
                    for (int i = 0; i < tracker_count; i++)
                    {
                        tracker_property[i] = transform.GetChild(i).GetComponent<VirtualTrackerProperty>();
                    }
                }

                for (int i = 0; i < tracker_count; i++)
                {
                    tracker_toggle[i] = new OscMessage("/avatar/parameters/ps_t_" + i + "_o", 0f);
                    tracker_reset[i] = new OscMessage("/avatar/parameters/ps_t_" + i + "_r", 0f);
                    tracker_lookat[i] = new OscMessage("/avatar/parameters/ps_t_" + i + "_l", 0f);
                    tracker_camerarootp[i] = new OscMessage("/avatar/parameters/ps_t_" + i + "_rp", 0f);
                    tracker_camerarootr[i] = new OscMessage("/avatar/parameters/ps_t_" + i + "_rr", 0f);
                }
            }

            initializeReceiveActions();

            prev_tracker_count = tracker_count;
        }

        void initializeReceiveActions()
        {
            if (prev_tracker_count != tracker_count)
            {
                for (int i = 0; i < tracker_count; i++)
                {
                    oscReceiver.RemoveEndpoint("/avatar/parameters/ps_t_" + i + "_0", ReceiveRawTrackingData_0);
                    oscReceiver.RemoveEndpoint("/avatar/parameters/ps_t_" + i + "_1", ReceiveRawTrackingData_1);
                    oscReceiver.RemoveEndpoint("/avatar/parameters/ps_t_" + i + "_2", ReceiveRawTrackingData_2);
                    oscReceiver.RemoveEndpoint("/avatar/parameters/ps_t_" + i + "_3", ReceiveRawTrackingData_3);
                    oscReceiver.RemoveEndpoint("/avatar/parameters/ps_t_" + i + "_4", ReceiveRawTrackingData_4);
                    oscReceiver.RemoveEndpoint("/avatar/parameters/ps_t_" + i + "_5", ReceiveRawTrackingData_5);
                }

                for (int i = 0; i < tracker_count; i++)
                {
                    oscReceiver.AddEndpoint("/avatar/parameters/ps_t_" + i + "_0", ReceiveRawTrackingData_0);
                    oscReceiver.AddEndpoint("/avatar/parameters/ps_t_" + i + "_1", ReceiveRawTrackingData_1);
                    oscReceiver.AddEndpoint("/avatar/parameters/ps_t_" + i + "_2", ReceiveRawTrackingData_2);
                    oscReceiver.AddEndpoint("/avatar/parameters/ps_t_" + i + "_3", ReceiveRawTrackingData_3);
                    oscReceiver.AddEndpoint("/avatar/parameters/ps_t_" + i + "_4", ReceiveRawTrackingData_4);
                    oscReceiver.AddEndpoint("/avatar/parameters/ps_t_" + i + "_5", ReceiveRawTrackingData_5);
                }
            }
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
                initializeMessages();

                for (int i = 0; i < tracker_lookat.Length; i++)
                {
                    tracker_toggle[i].Set(0, tracker_property[i].gameObject.activeSelf);
                    tracker_reset[i].Set(0, tracker_property[i].resetFlag);
                    tracker_lookat[i].Set(0, tracker_property[i].weight);
                    tracker_camerarootp[i].Set(0, tracker_property[i].cameraRootP);
                    tracker_camerarootr[i].Set(0, tracker_property[i].cameraRootR);

                    system.Send(tracker_toggle[i]);
                    system.Send(tracker_reset[i]);
                    system.Send(tracker_lookat[i]);
                    system.Send(tracker_camerarootp[i]);
                    system.Send(tracker_camerarootr[i]);
                }

                return true;
            } catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }
}
*/