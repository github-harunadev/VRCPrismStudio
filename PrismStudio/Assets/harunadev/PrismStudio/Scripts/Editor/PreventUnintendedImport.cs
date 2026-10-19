#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.Reflection;
using UnityEditor;

[InitializeOnLoad]
public class PreventUnintendedImport
{
    static Type ImportWindowType;
    static MethodInfo HasOpenInstancesMethodInfo;

    static PreventUnintendedImport()
    {
        AssetDatabase.importPackageStarted += Check;

        ImportWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.PackageImport");
        HasOpenInstancesMethodInfo = typeof(EditorWindow).GetMethod(
            nameof(EditorWindow.HasOpenInstances), 
            BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod(ImportWindowType);
    }

    static void Check(string packageName)
    {
        bool pass = true;

        if (packageName.IndexOf("UpdatePackage") >= 0)
        {
            pass = false;

            EditorUtility.DisplayDialog(
                "Warning 경고",
                "You're about to update PrismStudio with newer version. Please delete folder 'Assets/harunadev/PrismStudio/Scripts', and retry!\n\n" +
                "PrismStudio를 업데이트하려고 합니다. 'Assets/harunadev/PrismStudio/Scripts' 폴더를 지우고 다시 시도하세요!",
                "OK");

            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath("Assets/harunadev/PrismStudio/Scripts", typeof(UnityEngine.Object)));
        } else if (packageName.IndexOf("VRCLensPatcher") >= 0 || packageName.IndexOf("VirtualLens2Patcher") >= 0 || packageName.IndexOf("IntegralPatcher") >= 0 || packageName.IndexOf("UniversalPatcher") >= 0)
        {
            pass = false;

            EditorUtility.DisplayDialog(
                "Error 에러",
                "You're about to import camera patchers, which is not allowed in this project!\n\n" +
                "PrismStudio 프로젝트에는 카메라 패쳐를 Import할 수 없습니다!\n\n" +
                $"Package Name: '{packageName}'",
                "OK");
        }

        EditorApplication.update -= CloseImportWindow;
        if (!pass)
        {
            EditorApplication.update += CloseImportWindow;
        }
    }

    static void CloseImportWindow()
    {
        if (!(bool)HasOpenInstancesMethodInfo.Invoke(null, null)) return;

        EditorApplication.update -= CloseImportWindow;
        EditorWindow.GetWindow(ImportWindowType).Close();
    }
}
#endif