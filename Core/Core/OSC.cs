using OscCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using UnityEngine;


namespace PrismStudio.OSC
{

    /// <summary>
    /// PrismStudio Core: OscServer
    /// (c) harunadev
    /// </summary>
    public class OscServer : IDisposable
    {
        readonly Socket socket;
        readonly Thread thread;

        bool isDisposed = false;
        bool isOpen = false;

        byte[] buffer;
        
        /// <summary>
        /// Port
        /// </summary>
        public int Port
        {
            get { return _port; }
        }
        readonly int _port;

        /// <summary>
        /// Buffer size (default = 4096)
        /// | 버퍼 크기
        /// </summary>
        public int BufferSize
        {
            get { return _bufferSize; }
        }
        readonly int _bufferSize = 4096;

        /// <summary>
        /// Receive action
        /// | 리시버 Action(OscMessage)
        /// </summary>
        public Action<OscMessage> receieveAction
        {
            get { return _receieveAction; }
        }
        readonly Action<OscMessage> _receieveAction;

        /// <summary>
        /// Log almost everything
        /// | 거의 모든 메세지를 로그로 출력
        /// </summary>
        public bool verbose = false;
        
        /// <summary>
        /// Create &amp; start OSC server with given port, receiverAction, and buffer size
        /// | 주어진 정보를 가지고 OSC 서버를 생성 및 시작
        /// </summary>
        /// <param name="port">port</param>
        /// <param name="receiverAction">receiverAction</param>
        /// <param name="bufferSize">bufferSize</param>
        public OscServer(int port, Action<OscMessage> receiverAction, int bufferSize = 4096)
        {
            _port = port;
            _bufferSize = bufferSize;
            _receieveAction = receiverAction;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = int.MaxValue
            };
            thread = new Thread(WorkThread);
        }

        /// <summary>
        /// Open receiver
        /// | 리시버 열기
        /// </summary>
        public void Open()
        {
            if (isOpen) return;

            isDisposed = false;
            if (!socket.IsBound)
            {
                socket.Bind(new IPEndPoint(IPAddress.Any, Port));
            }

            thread.Start();

            isOpen = true;
        }

        private void WorkThread()
        {
            buffer = new byte[BufferSize];

            while (!isDisposed)
            {
                try
                {
                    int receiveCount = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    if (receiveCount == 0) continue;

                    OscMessage packet = OscMessage.Read(buffer, 0, buffer.Length);

                    _receieveAction.Invoke(packet);
                }
                catch (SocketException e) 
                {
                    if (verbose && !isDisposed) UnityEngine.Debug.LogException(e);
                }
                catch (ThreadAbortException) { }
                catch (Exception e)
                {
                    if (verbose && !isDisposed) UnityEngine.Debug.LogException(e);
                }
            }

        }

        /// <summary>
        /// Close &amp; dispose receiver
        /// | 닫고 버리기
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return;
            socket.Close();
            socket.Dispose();
            isDisposed = true;
        }
    }


    /// <summary>
    /// PrismStudio Core: OscReceiver
    /// (c) harunadev
    /// </summary>
    [ExecuteInEditMode]
    public class OscReceiver : MonoBehaviour
    {
        /// <summary>
        /// OSC Server
        /// </summary>
        protected OscServer server;

        /// <summary>
        /// Is OSC Receiver Opened
        /// | OSC 수신이 열려있는가
        /// </summary>
        public bool receiverOpened
        {
            get
            {
                return server != null;
            }
        }

        /// <summary>
        /// Initialize OSC Receiver on start
        /// | OSC 수신 세션을 자동 시작합니다
        /// </summary>
        [SerializeField] public bool AutoStartReceiver = true;

        /// <summary>
        /// Receiver port (UDP)
        /// | 수신 포트
        /// </summary>
        [SerializeField] public int ReceiverPort = 9003;

        /// <summary>
        /// Action to monitor all the income OSC message
        /// | 수신된 모든 OSC 메세지를 모니터링하는 함수
        /// </summary>
        public Action<OscMessage> monitor;


        private ConcurrentDictionary<string, Endpoint> registeredEndpoints = new ConcurrentDictionary<string, Endpoint>();

        /// <summary>
        /// All addresses of registered endpoints
        /// | 등록된 모든 주소
        /// </summary>
        public IEnumerable<string> registeredEndpointAddresses => registeredEndpoints.Keys;
        private Endpoint _dummy;

        bool isEditor = false;

        private void Awake()
        {
            isEditor = false;

            try
            {
                Type t = Assembly.Load("PrismStudio").GetType("IsEditor");
                FieldInfo f = t.GetField("isEditor", BindingFlags.Static | BindingFlags.Public);
                object o = (bool)f.GetValue(null);

                if (o != null)
                {
                    isEditor = (bool)o;
                }
            }
            catch (Exception)
            {
                // Debug.LogException(e);
            }

            if (!isEditor)
            {
                if (AutoStartReceiver) OpenReceiver();
            }
        }

        private void OnDisable()
        {
            if (receiverOpened) CloseReceiver();
        }

        private void OnDestroy()
        {
            if (receiverOpened) CloseReceiver();
        }

        /// <summary>
        /// Open OSC receiver session to ReceiverPort 
        /// | 타겟 포트에 OSC 수신 세션 시작
        /// </summary>
        public void OpenReceiver()
        {
            if (server != null)
            {
                server?.Dispose();
                server = null;
            }

            try
            {
                server = new OscServer(ReceiverPort, Receive, 4096);
                server.Open();
            }
            catch (Exception e)
            {
                Debug.LogException(new Exception(I18n.Format(I18n.currentLanguage, "oscreceiverstartfail", "%0", ReceiverPort.ToString()), e));
                server = null;
            }
        }

        /// <summary>
        /// Disconnect / terminate OSC receiver 
        /// | 수신 세션 닫기
        /// </summary>
        public void CloseReceiver()
        {
            if (server != null)
            {
                server?.Dispose();
                server = null;
            }
        }

        private void Receive(OscMessage message)
        {
            if (monitor != null)
            {
                try
                {
                    monitor(message);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            try
            {
                if (registeredEndpoints.TryGetValue(message.Address, out var value))
                {
                    value.action(message);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Get endpoint of given address
        /// | 주어진 주소의 Endpoint를 반환
        /// </summary>
        /// <param name="address">address</param>
        /// <returns>Endpoint</returns>
        public Endpoint GetEndpoint(string address)
        {
            return registeredEndpoints[address];
        }

        /// <summary>
        /// Add endpoint to target address
        /// | 주소에 Endpoint 추가
        /// </summary>
        /// <param name="address"></param>
        /// <param name="action"></param>
        public void AddEndpoint(string address, Action<OscMessage> action)
        {
            if (registeredEndpoints.ContainsKey(address))
            {
                registeredEndpoints[address] += action;
            }
            else
            {
                registeredEndpoints[address] = new Endpoint(action);
            }
        }

        /// <summary>
        /// Remove endpoint of target addres. If endpoint action is empty, the whole endpoint is deleted
        /// | 주소의 Endpoint를 삭제, 주소의 Endpoint가 완전히 비어있다면 Endpoint는 제거됨
        /// </summary>
        /// <param name="address"></param>
        /// <param name="action"></param>
        public void RemoveEndpoint(string address, Action<OscMessage> action)
        {
            if (registeredEndpoints.ContainsKey(address))
            {
                registeredEndpoints[address] -= action;
                if (registeredEndpoints[address].totalActions == 0)
                {
                    registeredEndpoints.Remove(address, out _dummy);
                }
            }
        }
    }

    /// <summary>
    /// PrismStudio Core: OSC Endpoint
    /// (c) harunadev
    /// </summary>
    public class Endpoint
    {
        /// <summary>
        /// Action (OscMessage)
        /// </summary>
        public Action<OscMessage> action { get; private set; }

        /// <summary>
        /// Count of total registered Action
        /// | 모든 Action의 갯수
        /// </summary>
        public uint totalActions { get; private set; }

        /// <summary>
        /// Create Endpoint with given Action
        /// | 주어진 Action을 가지고 Endpoint를 생성
        /// </summary>
        /// <param name="action"></param>
        public Endpoint(Action<OscMessage> action)
        {
            this.action = action;
            totalActions = 1u;
        }

        /// <summary>
        /// Add action to current Endpoint
        /// | Endpoint에 Action 추가
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Endpoint operator +(Endpoint left, Action<OscMessage> right)
        {
            left.action = (Action<OscMessage>)Delegate.Combine(left.action, right);
            left.totalActions++;
            return left;
        }

        /// <summary>
        /// Remove action from current Endpoint
        /// | Endpoint에 Action 제거
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Endpoint operator -(Endpoint left, Action<OscMessage> right)
        {
            left.action = (Action<OscMessage>)Delegate.Remove(left.action, right);
            left.totalActions--;
            return left;
        }
    }
    /// <summary>
    /// PrismStudio Core: OSC Utility
    /// (c) harunadev
    /// </summary>
    public static class Utility
    {
        static System.Random random = new System.Random();

        /// <summary>
        /// Generate random string with given length
        /// </summary>
        /// <param name="length">length</param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Get this device's local IP address
        /// </summary>
        /// <returns>IPAddress</returns>
        public static IPAddress GetLocalIPAddress()
        {
            return IPAddress.Loopback;
        }

        /// <summary>
        /// Get this device's local, but nonloopback IP address
        /// </summary>
        /// <returns>IPAddress</returns>
        public static IPAddress GetLocalIPAddressNonLoopback()
        {
            string hostName = Dns.GetHostName();

            foreach (IPAddress ip in Dns.GetHostEntry(hostName).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return null;
        }
    }
}