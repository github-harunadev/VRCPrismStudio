using System;
using UnityEngine;
using System.Net.Sockets;
using static PrismStudio.I18n;
using System.Collections.Generic;
using System.Linq;
using OscCore;

namespace PrismStudio
{
    /// <summary>
    /// PrismStudio Core: StudioSystem
    /// (c) harunadev
    /// </summary>
    [DefaultExecutionOrder(int.MaxValue), 
        ExecuteInEditMode]
    public class StudioSystem : MonoBehaviour
    {
        /// <summary>
        /// Version
        /// </summary>
        public string version
        {
            get
            {
                return Constants.version;
            }
        }

        /// <summary>
        /// Currently saved language
        /// </summary>
        [SerializeField] public SupportedLanguage currentLanguage = SupportedLanguage.English;

        /// <summary>
        /// Set PrismStudio global language to target language
        /// </summary>
        /// <param name="targetLanguage">target</param>
        public void SetLanguage(SupportedLanguage targetLanguage)
        {
            currentLanguage = targetLanguage;
        }

        /// <summary>
        /// FPS, or frequency of values sent per second 
        /// | FPS, 또는 1초에 전송되는 값의 빈도
        /// </summary>
        [SerializeField] public int FPS = 120;

        /// <summary>
        /// Vertical Sync, program will match FPS to the monitor's frequency
        /// | 수직동기화, 모니터와 FPS를 동일하게 설정합니다
        /// </summary>
        [SerializeField] public bool VSync = true;

        void ProcessPerformanceConfig()
        {
            Application.targetFrameRate = FPS;
            if (VSync)
            {
                QualitySettings.vSyncCount = 1;
            } else
            {
                QualitySettings.vSyncCount = 0;
            }
        }
        void UpdatePerformanceConfig()
        {
            FPS = Application.targetFrameRate;
            VSync = QualitySettings.vSyncCount > 0;
        }

        /// <summary>
        /// Initialize OSC Sender on start 
        /// | OSC 전송 세션을 자동 시작합니다
        /// </summary>
        [SerializeField] public bool AutoStartSender = true;

        /// <summary>
        /// Target IP 
        /// | IP 주소
        /// </summary>
        [SerializeField, Obsolete] public string IP
        {
            get
            {
                return SenderIP;
            }
            set
            {
                SenderIP = value;
            }
        }
        /// <summary>
        /// Target IP for OSC Sender 
        /// | OSC 데이터를 전송할 IP 주소
        /// </summary>
        [SerializeField] public string SenderIP = "127.0.0.1";

        /// <summary>
        /// Target port 
        /// | 포트
        /// </summary>
        [SerializeField, Obsolete] public int Port
        {
            get
            {
                return SenderPort;
            }
            set
            {
                SenderPort = value;
            }
        }
        /// <summary>
        /// Sender port 
        /// | 송신 포트
        /// </summary>
        [SerializeField] public int SenderPort = 9000;


        /// <summary>
        /// Camera point object, required for move and rotation 
        /// | 카메라 포인트 오브젝트, 위치와 각도 담당
        /// </summary>
        public CameraPoint Point
        {
            get;
            private set;
        }
        //CameraPoint _Point;

        /// <summary>
        /// Camera Property component, access to zoom, focus, and more 
        /// | 카메라 속성 컴포넌트, 확대 및 초점 등의 값 변경
        /// </summary>
        public CameraProperty CameraProperty
        {
            get;
            private set;
        }
        //CameraProperty _CameraProperty;

        // <summary>
        // LocalPlayerTracker, will track local avatar's body part position
        // | 로컬 플레이어 트래커는 로컬 아바타의 몸을 추적합니다.
        // </summary>
        // public LocalPlayerTracker LocalPlayerTracker
        // {
        //     get;
        //     private set;
        // }
        //LocalPlayerTracker _LocalPlayerTracker;

        /// <summary>
        /// OSC data transmit mode
        /// | OSC 전송 모드
        /// </summary>
        public enum TransmitMode
        {
            /// <summary>
            /// OSC data sent only when value changed
            /// | OSC 데이터는 변경되었을때만 전송됩니다
            /// </summary>
            ValueChanged,
            /// <summary>
            /// OSC data sent every frame
            /// | 매 프레임마다 OSC 데이터가 전송됩니다
            /// </summary>
            EveryFrame,
            /// <summary>
            /// OSC data sent when changed, but also sent every seconds for refresh
            /// | OSC 데이터는 변경되었을때, 그리고 1초마다 한번씩 전송됩니다.
            /// </summary>
            ValueChangedWithEverySecond
        }

        /// <summary>
        /// Configure how PrismStudio sends OSC values
        /// | 프리즘스튜디오가 OSC 데이터를 어떻게 보낼지 결정
        /// </summary>
        public TransmitMode transmitMode = TransmitMode.EveryFrame;

        /// <summary>
        /// Avatar Camera type
        /// | 아바타 카메라 종류
        /// </summary>
        public enum ClientCameraType
        {
            /// <summary>
            /// Parameter set for VRCLens
            /// | VRCLens용 파라미터를 사용
            /// </summary>
            [InspectorName("VRCLens by Hirabiki")]
            VRCLens,
            /// <summary>
            /// Parameter set for VirtualLens2
            /// | VirtualLens2용 파라미터를 사용
            /// </summary>
            [InspectorName("VirtualLens2 by logilabo (ろじらぼ)")]
            VirtualLens2,
            /// <summary>
            /// Parameter set for Integral
            /// | Integral용 파라미터를 사용
            /// </summary>
            [InspectorName("Integral by suzufactory (スズ製作所)")]
            Integral,
            /// <summary>
            /// Parameter set for FlexFishEye
            /// | FlexFishEye용 파라미터를 사용
            /// </summary>
            [InspectorName("FlexFishEye by Goat-Cannery BETA")]
            FlexFishEye,
            /// <summary>
            /// Parameter set for VRChat Native Camera
            /// | VRChat 네이티브 카메라용 파라미터를 사용
            /// </summary>
            [InspectorName("VRChat Native Camera (Open Beta)")]
            VRChatNative,
        }

        /// <summary>
        /// Current camera type
        /// | 현재 사용중인 카메라 종류
        /// </summary>
        public ClientCameraType cameraType = ClientCameraType.VRCLens;

        

        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_generals;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_connectivity;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_modules;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_misc;
        /// <summary>
        /// Internal (Editor)
        /// </summary>
        [SerializeField] private bool foldout_debug;


        private void OnEnable()
        {
            ForceEnable();

            /*
            bool isEditor = false;

            try
            {
                Type t = Assembly.Load("PrismStudio").GetType("IsEditor");
                FieldInfo f = t.GetField("isEditor", BindingFlags.Static | BindingFlags.Public);
                object o = (bool)f.GetValue(null);

                if (o != null)
                {
                    isEditor = (bool)o;
                }
            } catch (Exception)
            {
                // Debug.LogException(e);
            }

            if (!isEditor)
            {
                if (AutoStartSender) ConnectSender();
                if (AutoStartReceiver) OpenReceiver(); 
            }*/

            if (AutoStartSender) ConnectSender();

            CameraProperty.StudioSystemInitialized(this);
            Point.StudioSystemInitialized(this);
            //LocalPlayerTracker.StudioSystemInitialized(this);
            for (int i = 0; i < modules.Length; i++)
            {
                try
                {
                    modules[i].StudioSystemInitialized(this);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        /// <summary>
        /// Force initialize objects (for UnityEditor) 
        /// | 모든 오브젝트를 찾기 (UnityEditor 용)
        /// </summary>
        public void ForceEnable()
        {
            CameraProperty = transform.Find("Camera").GetComponent<CameraProperty>();
            Point = transform.Find("Camera/Point").GetComponent<CameraPoint>();
            //LocalPlayerTracker = transform.Find("LocalPlayer").GetComponent<LocalPlayerTracker>();
        }

        float time = 0;

        private void Update()
        {
            transform.localScale = Vector3.one;
        }

        private void LateUpdate()
        {
            ProcessPerformanceConfig();
            UpdatePerformanceConfig();

            transform.localScale = Vector3.one;

            if (senderConnected)
            {
                switch (transmitMode)
                {
                    case TransmitMode.ValueChanged:
                        send(false);
                        break;
                    case TransmitMode.EveryFrame:
                        send(true);
                        break;
                    case TransmitMode.ValueChangedWithEverySecond:
                        time += Time.deltaTime;
                        if (time >= 1)
                        {
                            send(true);
                            time = 0;
                        }
                        send(false);
                        break;
                }
            }
        }

        private void Start()
        {

        }

        private void OnDisable()
        {
            if (senderConnected) DisconnectSender();
            //if (receiverOpened) CloseReceiver();
        }

        private void OnDestroy()
        {
            if (senderConnected) DisconnectSender();
            //if (receiverOpened) CloseReceiver();
        }

        #region Modules

        /// <summary>
        /// Raw list of Modules (public get only, private set)
        /// | Module 리스트
        /// </summary>
        public Module[] modules
        {
            get { return _modules; }
            private set { 
                _modules = value;
                ValidateModule();
            }
        }
        [SerializeField] Module[] _modules = new Module[0];

        /// <summary>
        /// Validate all modules
        /// | 연결된 모듈 확인
        /// </summary>
        public void ValidateModule()
        {
            _modules = _modules.Where(m => m != null).ToArray();
        }

        /// <summary>
        /// Add module
        /// | 모듈 추가
        /// </summary>
        /// <param name="module">target</param>
        public void AddModule(Module module)
        {
            modules = modules.Append(module).ToArray();
        }
        /// <summary>
        /// Remove module at index
        /// | 모듈 제거 (리스트 위치 기반)
        /// </summary>
        /// <param name="index">index</param>
        public void RemoveModule(int index)
        {
            List<Module> l = modules.ToList();
            l.RemoveAt(index);
            modules = l.ToArray();
        }

        #endregion

        void send(bool immediate)
        {
            // System Modules
            if (Point.isActiveAndEnabled)
            {
                if (Point.OnSend(this, immediate) == false)
                {
                    Debug.LogWarning(I18n.Format(currentLanguage, "modulefail", "%0", "PrismStudio.CameraPoint"));
                }
            }
            if (CameraProperty.isActiveAndEnabled)
            {
                if (CameraProperty.OnSend(this, immediate) == false)
                {
                    Debug.LogWarning(I18n.Format(currentLanguage, "modulefail", "%0", "PrismStudio.CameraProperty"));
                }
            }
            //if (LocalPlayerTracker.OnSend(this, immediate) == false)
            //{
            //    Debug.LogWarning(I18n.Format(currentLanguage, "modulefail", "%0", "PrismStudio.LocalPlayerTracker"));
            //}

            // User Modules
            for (int i = 0; i < modules.Length; i++)
            {
                if (modules[i] && modules[i].isActiveAndEnabled)
                {
                    if (modules[i].OnSend(this, immediate) == false)
                    {
                        Debug.LogWarning(I18n.Format(currentLanguage, "modulefail", "%0", modules[i].GetType().FullName));
                    }
                }
            }
        }

        #region OSC

        UdpClient sender;

        /// <summary>
        /// Is OSC Client Connected
        /// | OSC 연결중인가
        /// </summary>
        [Obsolete]
        public bool connected
        {
            get
            {
                return senderConnected;
            }
        }
        /// <summary>
        /// Is OSC Sender Connected
        /// | OSC 송신 연결중인가
        /// </summary>
        public bool senderConnected
        {
            get
            {
                return sender != null;
            }
        }

        /// <summary>
        /// Start connect to target IP and Port 
        /// | 타겟 IP / 포트에 OSC 연결
        /// </summary>
        [Obsolete]
        public void Connect()
        {
            ConnectSender();
        }

        /// <summary>
        /// Disconnect / terminate current OSC connection 
        /// | 연결 해제
        /// </summary>
        [Obsolete]
        public void Disconnect()
        {
            DisconnectSender();
        }

        /// <summary>
        /// Start OSC sender connection to target IP and SenderPort 
        /// | 타겟 IP / 포트에 OSC 전송 세션 연결
        /// </summary>
        public void ConnectSender()
        {
            try
            {
                sender = new UdpClient();
                sender.Connect(SenderIP, SenderPort);

                //Debug.Log(I18n.Get(currentLanguage, "oscsenderstart"));
            }
            catch (Exception e)
            {
                Debug.LogException(new Exception(I18n.Format(currentLanguage, "oscstartfail", "%0", SenderIP, "%1", SenderPort.ToString()), e));
                sender = null;
            }
        }

        /// <summary>
        /// Disconnect / terminate OSC sender 
        /// | 전송 세션의 연결 해제
        /// </summary>
        public void DisconnectSender()
        {
            sender?.Close();
            sender?.Dispose();
            sender = null;
            //Debug.Log(I18n.Get(currentLanguage, "oscsenderstop"));
        }

        /*
        void UpdateEndpoints()
        {
            try
            {
                foreach (string address in registeredEndpoints.Keys)
                {
                    OscQuery.AddEndpoint(address, registeredEndpoints[address].types, registeredEndpoints[address].accessValue);
                    //Debug.Log($"Endpoint updating: {address}, {registeredEndpoints[address].types}");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            //Debug.Log($"Endpoint updated: {registeredEndpoints.Keys.Count} items updated to OSCQuery");
        }

        public Action<OscMessage> monitor;

        public class Endpoint
        {
            public Endpoint(Action<OscMessage> action, string types, Attributes.AccessValues accessValues)
            {
                this.action = action;
                totalActions = 1;
                this.types = types;
                this.accessValue = accessValues;
            }

            public Action<OscMessage> action
            {
                get;
                private set;
            }
            public uint totalActions
            {
                get;
                private set;
            }
            public string types;
            public Attributes.AccessValues accessValue;

            public static Endpoint operator +(Endpoint left, Action<OscMessage> right)
            {
                left.action += right;
                left.totalActions++;
                return left;
            }
            public static Endpoint operator -(Endpoint left, Action<OscMessage> right)
            {
                left.action -= right;
                left.totalActions--;
                return left;
            }
        }
        
        ConcurrentDictionary<string, Endpoint> registeredEndpoints = new ConcurrentDictionary<string, Endpoint>();

        void Receive(OscMessage message)
        {
            if (monitor != null)
            {
                try
                {
                    monitor.Invoke(message);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            try
            {
                if (registeredEndpoints.TryGetValue(message.Address, out Endpoint action))
                {
                    action.action.Invoke(message);
                }
            } catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public IEnumerable<string> registeredEndpointAddresses
        {
            get
            {
                return registeredEndpoints.Keys;
            }
        }

        public Endpoint GetEndpoint(string address)
        {
            return registeredEndpoints[address];
        }

        public void AddEndpoint(string address, string types, Action<OscMessage> action)
        {
            AddEndpoint(address, types, action, Attributes.AccessValues.NoValue);
        }

        public void AddEndpoint(string address, string types, Action<OscMessage> action, Attributes.AccessValues accessValue)
        {
            if (OscQuery != null)
            {
                OscQuery.AddEndpoint(address, types, accessValue);
            }
            if (registeredEndpoints.ContainsKey(address))
            {
                registeredEndpoints[address] += action;
            } else
            {
                registeredEndpoints[address] = new Endpoint(action, types, accessValue);
            }

            //Debug.Log($"Endpoint added {address}, {types}");
        }

        Endpoint _dummy;
        public void RemoveEndpoint(string address, Action<OscMessage> action)
        {
            if (OscQuery != null)
            {
                OscQuery.RemoveEndpoint(address);
            }
            if (registeredEndpoints.ContainsKey(address))
            {
                registeredEndpoints[address] -= action;
                if (registeredEndpoints[address].totalActions == 0)
                {
                    registeredEndpoints.Remove(address, out _dummy);
                }
            }
        }
        */

        const int messagePoolSize = 64;
        OscMessage[] messagePool = new OscMessage[messagePoolSize];
        int messagePoolIndex = 0;

        /// <summary>
        /// Send OSC value immediately to address, without making OscMessage instance
        /// | 주소에 값을 즉시 전송
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="value">value</param>
        public void Send(string address, int value)
        {
            _Send_Primitive(address, value);
        }
        /// <summary>
        /// Send OSC value immediately to address
        /// | 주소에 값을 즉시 전송
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="value">value</param>
        public void Send(string address, float value)
        {
            _Send_Primitive(address, value);
        }
        /// <summary>
        /// Send OSC value immediately to address
        /// | 주소에 값을 즉시 전송
        /// </summary>
        /// <param name="address">address</param>
        /// <param name="value">value</param>
        public void Send(string address, bool value)
        {
            _Send_Primitive(address, value);
        }

        void _Send_Primitive(string address, object value)
        {
            if (messagePool[messagePoolIndex] == null)
            {
                messagePool[messagePoolIndex] = new OscMessage(address, value);
            } else
            {
                messagePool[messagePoolIndex].Address = address;
                messagePool[messagePoolIndex].Set(0, value);
            }
            Send(messagePool[messagePoolIndex]);
            messagePoolIndex = (messagePoolIndex + 1) % messagePoolSize;
        }
        /// <summary>
        /// Send OSC data immediately 
        /// | OSCMessage를 전송
        /// </summary>
        /// <param name="oscMessage">message</param>
        public void Send(OscMessage oscMessage)
        {
            if (oscMessage.Address == "") return;
            byte[] packet = oscMessage.ToByteArray();
            SendPacket(packet, packet.Length);
        }

        private void SendPacket(byte[] packet, int length)
        {
            if (!senderConnected) ConnectSender();
            try
            {
                sender.Send(packet, length);
            } catch (Exception e)
            {
                Debug.LogException(new Exception(I18n.Format(currentLanguage, "oscsendfail", "%0", SenderIP, "%1", SenderPort.ToString()), e));
                DisconnectSender();
            }
        }
        #endregion
    }
}
