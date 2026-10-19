using OscCore;
using PrismStudio.OSC;
using UnityEngine;
using static PrismStudio.I18n;

namespace PrismStudio
{
    /// <summary>
    /// PrismStudio Core: LocalPlayerTracker
    /// (c) harunadev
    /// </summary>
    [ExecuteInEditMode, RequireComponent(typeof(OscReceiver))]
    public class LocalPlayerTracker : Module
    {
        public override string description(SupportedLanguage lang)
        {
            switch (lang)
            {
                case SupportedLanguage.Korean:
                    {
                        return "PrismStudio의 기본 기능; ローカル プレーヤーを追跡します";
                    }
                case SupportedLanguage.Japanese:
                    {
                        return "PrismStudioの基本機能; VirtualTrackerを管理します";
                    }
            }
            return "Core function of PrismStudio; Track local player";
        }

        OscReceiver oscReceiver;

        public float smoothFactor = 20f;

        public Transform LeftHandTransform
        {
            get;
            private set;
        }
        public Transform RightHandTransform
        {
            get;
            private set;
        }
        public Transform HeadTransform
        {
            get;
            private set;
        }

        volatile float[] x = new float[3];
        volatile float[] y = new float[3];
        volatile float[] z = new float[3];
        float[] sx = new float[3];
        float[] sy = new float[3];
        float[] sz = new float[3];
        float[] bx = new float[3];
        float[] by = new float[3];
        float[] bz = new float[3];
        Vector3[] r = new Vector3[3];
        volatile float[] px = new float[3];
        volatile float[] py = new float[3];
        volatile float[] pz = new float[3];
        volatile bool[] fx = new bool[3];
        volatile bool[] fy = new bool[3];
        volatile bool[] fz = new bool[3];

        volatile public float[] debug_x = new float[3];
        volatile public float[] debug_y = new float[3];
        volatile public float[] debug_z = new float[3];
        volatile public bool[] debug_fx = new bool[3];
        volatile public bool[] debug_fy = new bool[3];
        volatile public bool[] debug_fz = new bool[3];

        /*
        private void initializeMessages(StudioSystem.ClientCameraType cameraType)
        {
            if (!leftHandTransform)
            {
                leftHandTransform = this.transform.Find("LeftHand");
            }
            if (!rightHandTransform)
            {
                rightHandTransform = this.transform.Find("RightHand");
            }

            if (_receiveActions.Count < 1)
            {
                _receiveActions = new Dictionary<string, Action<OscMessage>>()
                {
                    { "/avatar/parameters/PrismStudio_STrack_l_x",  Receive_l_x },
                    { "/avatar/parameters/PrismStudio_STrack_l_y",  Receive_l_y },
                    { "/avatar/parameters/PrismStudio_STrack_l_z",  Receive_l_z },
                    { "/avatar/parameters/PrismStudio_STrack_l_xp", Receive_l_xp },
                    { "/avatar/parameters/PrismStudio_STrack_l_yp", Receive_l_yp },
                    { "/avatar/parameters/PrismStudio_STrack_l_zp", Receive_l_zp },
                    { "/avatar/parameters/PrismStudio_STrack_r_x",  Receive_r_x },
                    { "/avatar/parameters/PrismStudio_STrack_r_y",  Receive_r_y },
                    { "/avatar/parameters/PrismStudio_STrack_r_z",  Receive_r_z },
                    { "/avatar/parameters/PrismStudio_STrack_r_xp", Receive_r_xp },
                    { "/avatar/parameters/PrismStudio_STrack_r_yp", Receive_r_yp },
                    { "/avatar/parameters/PrismStudio_STrack_r_zp", Receive_r_zp },
                };
            }
        }
        */

        void Awake()
        {
            oscReceiver = GetComponent<OscReceiver>();
        }

        void Update()
        {
            if (!LeftHandTransform)
            {
                LeftHandTransform = this.transform.Find("LeftHand");
            }
            if (!RightHandTransform)
            {
                RightHandTransform = this.transform.Find("RightHand");
            }
            if (!HeadTransform)
            {
                HeadTransform = this.transform.Find("Head");
            }

            /*
            for (ushort i = 0; i < 3; i++)
            {
                if (sx[i] > 0.2f) 
                {
                    bx[i] = x[i];
                    sx[i] = 0;
                } else if (Mathf.Abs(bx[i] - x[i]) < 0.1f)
                {
                    bx[i] = x[i];
                } else
                {
                    sx[i] += Time.unscaledDeltaTime;
                }

                if (sy[i] > 0.2f) 
                {
                    by[i] = y[i];
                    sy[i] = 0;
                } else if (Mathf.Abs(by[i] - y[i]) < 0.1f)
                {
                    by[i] = y[i];
                } else
                {
                    sy[i] += Time.unscaledDeltaTime;
                }

                if (sz[i] > 0.2f) 
                {
                    bz[i] = z[i];
                    sz[i] = 0;
                } else if (Mathf.Abs(bz[i] - z[i]) < 0.1f)
                {
                    bz[i] = z[i];
                } else
                {
                    sz[i] += Time.unscaledDeltaTime;
                }
            }
            */
            for (ushort i = 0; i < 3; i++)
            {
                px[i] = Mathf.Lerp(px[i], x[i], Time.deltaTime * smoothFactor);
                py[i] = Mathf.Lerp(py[i], y[i], Time.deltaTime * smoothFactor);
                pz[i] = Mathf.Lerp(pz[i], z[i], Time.deltaTime * smoothFactor);
            }

            HeadTransform.SetLocalPositionAndRotation(new Vector3(px[0], py[0], pz[0]), LeftHandTransform.localRotation);
            LeftHandTransform.SetLocalPositionAndRotation(new Vector3(px[1], py[1], pz[1]), RightHandTransform.localRotation);
            RightHandTransform.SetLocalPositionAndRotation(new Vector3(px[2], py[2], pz[2]), HeadTransform.localRotation);
        }

        void ReceiveP(ushort index, char axis, float value)
        {
            if (value == 0) return;
            switch (axis)
            {
                case 'x':
                    {
                        x[index] = (1 - value) * (fx[index] ? -2000 : 2000);
                        debug_x[index] = value;
                    }
                    break;
                case 'y':
                    {
                        y[index] = (1 - value) * (fy[index] ? -2000 : 2000);
                        debug_y[index] = value;
                    }
                    break;
                case 'z':
                    {
                        z[index] = (1 - value) * (fz[index] ? -2000 : 2000);
                        debug_z[index] = value;
                    }
                    break;
            }
        }
        void ReceivePm(ushort index, char axis, bool value)
        {
                switch (axis)
                {
                    case 'x':
                        {
                            fx[index] = value;
                            debug_fx[index] = value;
                        }
                        break;
                    case 'y':
                        {
                            fy[index] = value;
                            debug_fy[index] = value;
                        }
                        break;
                    case 'z':
                        {
                            fz[index] = value;
                            debug_fz[index] = value;
                        }
                        break;
                }
        }
        
        void ReceiveR(ushort index, OscMessage message)
        {
            r[index] = new Vector3((float)message[3], (float)message[4], (float)message[5]);
        }
        void ReceiveR_h(OscMessage message) { ReceiveR(0, message); }
        void ReceiveP_h_x(OscMessage message) {  ReceiveP(0,  'x', (float)message[0]); }
        void ReceiveP_h_y(OscMessage message) {  ReceiveP(0,  'y', (float)message[0]); }
        void ReceiveP_h_z(OscMessage message) {  ReceiveP(0,  'z', (float)message[0]); }
        void ReceiveP_h_xm(OscMessage message) { ReceivePm(0, 'x', (bool)message[0]); }
        void ReceiveP_h_ym(OscMessage message) { ReceivePm(0, 'y', (bool)message[0]); }
        void ReceiveP_h_zm(OscMessage message) { ReceivePm(0, 'z', (bool)message[0]); }

        void ReceiveR_lh(OscMessage message) { ReceiveR(1, message); }
        void ReceiveP_lh_x(OscMessage message) {  ReceiveP(1, 'x', (float)message[0]); }
        void ReceiveP_lh_y(OscMessage message) {  ReceiveP(1, 'y', (float)message[0]); }
        void ReceiveP_lh_z(OscMessage message) {  ReceiveP(1, 'z', (float)message[0]); }
        void ReceiveP_lh_xm(OscMessage message) { ReceivePm(1, 'x', (bool)message[0]); }
        void ReceiveP_lh_ym(OscMessage message) { ReceivePm(1, 'y', (bool)message[0]); }
        void ReceiveP_lh_zm(OscMessage message) { ReceivePm(1, 'z', (bool)message[0]); }
        void ReceiveR_rh(OscMessage message) {  ReceiveR(2, message); }
        void ReceiveP_rh_x(OscMessage message) {  ReceiveP(2, 'x', (float)message[0]); }
        void ReceiveP_rh_y(OscMessage message) {  ReceiveP(2, 'y', (float)message[0]); }
        void ReceiveP_rh_z(OscMessage message) {  ReceiveP(2, 'z', (float)message[0]); }
        void ReceiveP_rh_xm(OscMessage message) { ReceivePm(2, 'x', (bool)message[0]); }
        void ReceiveP_rh_ym(OscMessage message) { ReceivePm(2, 'y', (bool)message[0]); }
        void ReceiveP_rh_zm(OscMessage message) { ReceivePm(2, 'z', (bool)message[0]); }

        public override bool OnSend(StudioSystem system, bool immediate)
        {
            // try
            // {
            //     //initializeMessages(system.cameraType);
            // } catch (Exception e)
            // {
            // 
            // }
            // 
            return true;
        }

        void OnEnable()
        {
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_h_x", ReceiveP_h_x);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_h_y", ReceiveP_h_y);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_h_z", ReceiveP_h_z);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_h_xm", ReceiveP_h_xm);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_h_ym", ReceiveP_h_ym);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_h_zm", ReceiveP_h_zm);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_lh_x", ReceiveP_lh_x);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_lh_y", ReceiveP_lh_y);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_lh_z", ReceiveP_lh_z);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_lh_xm", ReceiveP_lh_xm);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_lh_ym", ReceiveP_lh_ym);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_lh_zm", ReceiveP_lh_zm);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_rh_x", ReceiveP_rh_x);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_rh_y", ReceiveP_rh_y);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_rh_z", ReceiveP_rh_z);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_rh_xm", ReceiveP_rh_xm);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_rh_ym", ReceiveP_rh_ym);
            oscReceiver.AddEndpoint("/avatar/parameters/ps_lt_rh_zm", ReceiveP_rh_zm);
        }
    }
}