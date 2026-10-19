using OscCore;
using PrismStudio;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ExampleModule : Module
{
    public override string description(I18n.SupportedLanguage lang)
    {
        return lang switch
        {
            I18n.SupportedLanguage.Korean => "예제 모듈입니다. 직접 모듈을 만들때, 이 예제 모듈처럼 만드시면 됩니다!",
            I18n.SupportedLanguage.Japanese => "例題モジュールです。 直接モジュールを作る時、この例題モジュールのように作ってください！",
            _ => "Example Module. If you're going to make a module for PrismStudio, use this script as an example!",
        };
    }

    public override bool OnSend(StudioSystem system, bool immediate)
    {
        // Access to StudioSystem with 'system' variable
        // 'system' 변수를 통해서 StudioSystem에 접근
        // 'system' 変数を通じてStudioSystemにアクセス
        // Debug.Log("This is Example Module!!! PrismStudio Version: " + system.version);

        // Send OSC values by calling 'system.Send()'
        // 'system.Send()' 호출해서 OSC 신호 전송
        // 'system.Send()' を呼び出してOSC信号を送信
        system.Send("/test/address", 123);
        // Multiple values can be sent
        system.Send(new OscMessage("/test/address/test", true, 12, 0.53f));

        // Return true when module execution is successed
        // 모듈 실행에 성공한 경우 true를 반환
        // モジュール実行に成功した場合、trueを返却
        return true;

        // Return false wheen module execution is failed
        // 모듈 실행에 실패한 경우 false를 반환
        // モジュール実行に失敗した場合、falseを返却
        // return false;
    }
}
