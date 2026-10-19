#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using static PrismStudio.CameraPoint;

/// <summary>
/// PostProcessor Example 예제
/// </summary>

// The actual example PostProcessor. This class will going to modify the finalized camera transform!
// PostProcessor의 실제 예제 클래스입니다. 이 클래스가 카메라의 최종 위치와 각도를 변환할꺼에요!
// PostProcessorの実際の例のClassです。 このClassがカメラの最終位置と角度を変換します！
public class ExamplePostProcessor : PostProcessor
{
    [SerializeField] public float example_value = 0.123f;

    public override ProcessData Process(ProcessData data)
    {
        Debug.Log("This is an example PostProcessor!");

        // Add Vector3(0, 10, example_value) to the output
        // Vector3(0, 10, value)를 최종 카메라 위치에 추가
        // Vector3(0, 10, value)を最終カメラ位置に追加
        data.position += new Vector3(0, 10, example_value);

        // Always set EulerAngle.z to 0
        // EulerAngle.z를 항상 0으로 설정
        // EulerAngle.zを常に0に設定
        data.rotation.eulerAngles = new Vector3(data.rotation.eulerAngles.x, data.rotation.eulerAngles.y, 0);

        // Return the finalized value
        // 최종 데이터를 반환
        // 最終データを返還
        return data;
    }

    // The editor class of ExamplePostProcessor. This class will show an editor GUI for user to edit easily! If there's no editor supplied, there will be a default editor.
    // ExamplePostProcessor의 에디터 클래스입니다. 이 클래스는 사용자에게 보여질 에디터 GUI를 담당합니다! 만약 이 클래스가 없다면, 기본 에디터가 사용될꺼에요.
    // ExamplePostProcessorのエディタClassです。 このClassは、ユーザーに表示されるエディターGUIを担当します！ もしこのClassがなければ、基本エディターが使われるでしょう。
#if UNITY_EDITOR
    [CustomEditor(typeof(ExamplePostProcessor))]
    public class ExamplePostProcessorPropertyDrawer : Editor
    {
        public override void OnInspectorGUI()
            {
                DrawDefaultInspector();
            }
    }
#endif
}