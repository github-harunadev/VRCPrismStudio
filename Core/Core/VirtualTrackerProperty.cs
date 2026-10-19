/*
using UnityEngine;

namespace PrismStudio
{
    /// <summary>
    /// PrismStudio Core: VirtualTrackerProperty
    /// (c) harunadev
    /// </summary>
    [ExecuteInEditMode]
    public class VirtualTrackerProperty : MonoBehaviour
    {
        VirtualTrackerManager manager;

        /// <summary>
        /// Index of this Virtual Tracker Property
        /// Virtual Tracker Property가 맡은 트래커 넘버
        /// </summary>
        public int virtualTrackerIndex = 0;
        /// <summary>
        /// Weight of Virtual Tracker LookAt, requires Local avatar look at to be enabled (higher than 0);
        /// 버추얼 트래커를 바라보는 강도, 로컬 아바타 LookAt이 활성화 되어있어야 함 (0보다 큰)
        /// </summary>
        public float weight = 0;
        /// <summary>
        /// Use this tracker as PrismStudio root position
        /// | 이 트래커를 PrismStudio 기준 위치로 사용
        /// </summary>
        public float cameraRootP = 0;
        /// <summary>
        /// Use this tracker as PrismStudio root rotation
        /// | 이 트래커를 PrismStudio 기준 각도로 사용
        /// </summary>
        public float cameraRootR = 0;
        /// <summary>
        /// Force stop tracking &amp; reset transform
        /// | 트래킹을 강제 정지하고 위치 및 각도를 초기화
        /// </summary>
        public bool resetFlag = false;

        /// <summary>
        /// Is VirtualTracker tracking?
        /// | VirtualTracker가 추적되고 있나?
        /// </summary>
        public bool isTracking
        {
            get
            {
                for (int i = 0; i < 6; i++)
                {
                    if (rawTrackingData[i] < 0.1f) return false;
                }
                return true;
            }
        }

        /// <summary>
        /// VirtualTracker's raw tracking data (used to detect tracking status)
        /// | VirtualTracker의 트래킹 데이터 (추적 상태를 인식하기 위한 용도)
        /// </summary>
        public float[] rawTrackingData 
        {
            get
            {
                return _rawTrackingData;
            } private set
            {
                _rawTrackingData = value;
            }
        }
        float[] _rawTrackingData = new float[6];

        private void OnEnable()
        {
            if (!manager) manager = transform.parent.GetComponent<VirtualTrackerManager>();
        }

        /// <summary>
        /// Internal
        /// </summary>
        /// <param name="manager"></param>
        public void SetManager(VirtualTrackerManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Internal
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetRawTrackingData(byte index, float value)
        {
            rawTrackingData[index] = value;
        }

        /// <summary>
        /// Toggle tracker activation
        /// 트래커 활성화 토글
        /// </summary>
        public void ToggleActive()
        {
            gameObject.SetActive(gameObject.activeSelf);
        }
    }
}
*/