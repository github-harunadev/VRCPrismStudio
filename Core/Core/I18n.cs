using System;
using System.Collections.Generic;
using UnityEngine;

namespace PrismStudio
{
    /// <summary>
    /// PrismStudio Core: I18n
    /// (c) harunadev
    /// </summary>
    public static class I18n
    {
        static StudioSystem studioSystem;

        /// <summary>
        /// Current global language
        /// </summary>
        public static SupportedLanguage currentLanguage
        {
            get
            {
                if (studioSystem == null)
                {
                    studioSystem = GameObject.FindObjectOfType<StudioSystem>();
                } else
                {
                    return studioSystem.currentLanguage;
                }

                if (studioSystem == null) 
                { 
                    return 0; 
                } else
                {
                    return studioSystem.currentLanguage;
                }
            }
        }

        /// <summary>
        /// Currently supported languages
        /// </summary>
        [Serializable]
        public enum SupportedLanguage
        {
            /// <summary>
            /// English Language set
            /// </summary>
            [InspectorName("English")]
            English,
            /// <summary>
            /// 한국어 언어셋
            /// </summary>
            [InspectorName("한국어")]
            Korean,
            /// <summary>
            /// 日本語言語セット
            /// </summary>
            [InspectorName("日本語")]
            Japanese
        }



        /// <summary>
        /// List of currently supported internalization
        /// 프리즘스튜디오가 지원하는 언어 리스트
        /// </summary>
        public static string[] SupportedLanguageNames = { "English", "한국어", "日本語" };

        /// <summary>
        /// Get string from LanguageSet
        /// LanguageSet으로부터 텍스트 가져오기
        /// </summary>
        /// <param name="lang">language</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string Get(SupportedLanguage lang, string key)
        {
            return LanguageSet[lang][key];
        }

        /// <summary>
        /// Format string from LanguageSet 
        /// LanguageSet으로부터 가져온 텍스트를 format하기
        /// (pairs = { "%1", "helloworld", ...})
        /// </summary>
        /// <param name="lang">language</param>
        /// <param name="key">key</param>
        /// <param name="pairs">format pair</param>
        /// <returns></returns>
        public static string Format(SupportedLanguage lang, string key, params string[] pairs)
        {
            string output = LanguageSet[lang][key];
            int pairCount = pairs.Length / 2;
            for (int i = 0; i < pairCount; i++)
            {
                output.Replace(pairs[i * 2], pairs[(i * 2) + 1]);
            }
            return output;
        }

        /// <summary>
        /// Direct access to LanguageSet dictionary (get only)
        /// LanguageSet에 직접 접근 (get 전용)
        /// </summary>
        public static Dictionary<SupportedLanguage, Dictionary<string, string>> LanguageSet
        {
            get
            {
                return _LanguageSet;
            }
        }
        static Dictionary<SupportedLanguage, Dictionary<string, string>> _LanguageSet = new Dictionary<SupportedLanguage, Dictionary<string, string>>()
        {
            { (SupportedLanguage.English), new Dictionary<string, string>()
                {
                    { "oscsenderstart", "Successfully started OSC Sender" },
                    { "oscsenderstop", "Successfully disposed OSC Sender" },
                    { "oscreceiverstart", "Successfully start OSC Receiver" },
                    { "oscreceiverstop", "Successfully disposed OSC Receiver" },
                    { "oscreceiverstartfail", "Cannot start OSC Receiver at UDP:%0" },
                    { "ipaddressinvalid", "IP address is invalid, falling back to '127.0.0.1'" },
                    { "oscstartfail", "Cannot establish connection to %0:%1!" },
                    { "oscsendfail", "Cannot send OSC packets to %0:%1! Connection Terminated." },
                    { "modulefail", "Module %0 failed to execute!" },
                }
            },
            { (SupportedLanguage.Korean), new Dictionary<string, string>()
                {
                    { "oscsenderstart", "OSC 전송 세션을 시작했습니다" },
                    { "oscsenderstop", "OSC 전송 세션을 정지했습니다" },
                    { "oscreceiverstart", "OSC 수신 세션을 시작했습니다" },
                    { "oscreceiverstop", "OSC 수신 세션을 정지했습니다" },
                    { "oscreceiverstartfail", "OSC 수신 세션을 시작할 수 없습니다! (UDP:%0)" },
                    { "ipaddressinvalid", "IP 주소가 올바르지 않습니다. '127.0.0.1'으로 원상복귀합니다" },
                    { "oscstartfail", "%0:%1 에 연결할 수 없습니다!" },
                    { "oscsendfail", "%0:%1으로 OSC 데이터를 전송하는데 실패했습니다! 연결을 강제종료합니다." },
                    { "modulefail", "%0 모듈 실행에 실패했습니다!" },
                }
            },
            { (SupportedLanguage.Japanese), new Dictionary<string, string>()
                {
                    { "oscsenderstart", "OSC配信セッションを開始しました" },
                    { "oscsenderstop", "OSC配信セッションを停止しました" },
                    { "oscreceiverstart", "OSC受信セッションを開始しました" },
                    { "oscreceiverstop", "OSC受信セッションを停止しました" },
                    { "oscreceiverstartfail", "OSC受信セッションの開始に失敗しました！ (UDP:%0)" },
                    { "ipaddressinvalid", "IPアドレスが正しくありません。'127.0.0.1' に原状復帰します" },
                    { "oscstartfail", "%0:%1に接続できません！" },
                    { "oscsendfail", "%0:%1でOSCデータの転送に失敗しました！ 接続が終了しました。" },
                    { "modulefail", "%0 モジュールの実行に失敗しました！" },
                }
            },
        };
    }
}
