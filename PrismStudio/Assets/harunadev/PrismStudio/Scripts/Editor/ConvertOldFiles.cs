#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

[InitializeOnLoad, DefaultExecutionOrder(1000)]
public class ConvertOldFiles
{
    static ConvertOldFiles()
    {
        EditorApplication.update += FindOldFiles;
    }

    static void FindOldFiles()
    {
        // Based on v1.15

        bool DoNotConvertOldFiles = SessionState.GetBool("DoNotConvertOldFiles", false);
        bool updated = false;
        
        string oldCorePath = Application.dataPath + "/harunadev/PrismStudio/Scripts/Plugins/Core.dll";
        string oldCoreDirPath = Application.dataPath + "/harunadev/PrismStudio/Scripts/Plugins";
        string newCoreDirPath = Application.dataPath + "/harunadev/PrismStudio/Scripts/Runtime";

        string oldMonoBehaviourDirPath = Application.dataPath + "/harunadev/PrismStudio/Scripts/MonoBehaviour";
        string oldMonoBehaviourPath = Application.dataPath + "/harunadev/PrismStudio/Scripts/MonoBehaviour/FPSCounter.cs";
        string newMonoBehaviourPath = Application.dataPath + "/harunadev/PrismStudio/Scripts/Runtime/FPSCounter.cs";

        if (!DoNotConvertOldFiles)
        {
            if (File.Exists(oldCorePath))
            {
                Directory.Move(oldCoreDirPath, newCoreDirPath);
                File.Move(oldCoreDirPath + ".meta", newCoreDirPath + ".meta");
                updated = true;
            }

            if (File.Exists(oldMonoBehaviourPath))
            {
                File.Move(oldMonoBehaviourPath, newMonoBehaviourPath);
                File.Move(oldMonoBehaviourPath + ".meta", newMonoBehaviourPath + ".meta");
                Directory.Delete(oldMonoBehaviourDirPath, true);
                File.Delete(oldMonoBehaviourDirPath + ".meta");

                updated = true;
            }

            if (updated)
            {
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                CompilationPipeline.RequestScriptCompilation();
            }

            EditorApplication.update -= FindOldFiles;
        } else
        {
            SessionState.SetBool("DoNotConvertOldFiles", false);
        }
    }
}
#endif