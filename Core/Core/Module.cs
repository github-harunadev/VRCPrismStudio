using OscCore;
using System;
using UnityEngine;
using static PrismStudio.I18n;

namespace PrismStudio
{
    /// <summary>
    /// PrismStudio Core: Module
    /// (c) harunadev
    /// </summary>
    [ExecuteInEditMode]
    public abstract class Module : MonoBehaviour
    {
        /// <summary>
        /// Registered StudioSystem of this Module
        /// | 이 Module에 등록된 StudioSystem
        /// </summary>
        public StudioSystem studioSystem
        {
            get;
            protected set;
        }

        /// <summary>
        /// Get description of module, multilanguage support required when making your own module
        /// | 이 모듈의 설명 반환, 직접 모듈을 만들시 다국어 지원 필수
        /// </summary>
        /// <param name="lang">language</param>
        /// <returns></returns>
        public abstract string description(SupportedLanguage lang);

        /// <summary>
        /// Internal
        /// </summary>
        /// <param name="studioSystem"></param>
        internal void StudioSystemInitialized(StudioSystem studioSystem)
        {
            this.studioSystem = studioSystem;
            OnStudioSystemInitialized(studioSystem);
        }

        /// <summary>
        /// Called when StudioSystem is initialized
        /// | StudioSystem이 생성되었을때 호출
        /// </summary>
        /// <param name="studioSystem"></param>
        public virtual void OnStudioSystemInitialized(StudioSystem studioSystem)
        {

        }

        /// <summary>
        /// Called when StudioSystem's camera type has changed
        /// | StudioSystem의 카메라 종류가 변경되었을때 호출
        /// </summary>
        /// <param name="newCameraType"></param>
        public virtual void OnCameraTypeChanged(StudioSystem.ClientCameraType newCameraType)
        {

        }

        /// <summary>
        /// Called from StudioSystem
        /// StudioSystem에서 호출하는 함수
        /// </summary>
        /// <param name="system">StudioSystem</param>
        /// <param name="immediate">send OSC now?</param>
        /// <returns>is success?</returns>
        public abstract bool OnSend(StudioSystem system, bool immediate);
    }
}
