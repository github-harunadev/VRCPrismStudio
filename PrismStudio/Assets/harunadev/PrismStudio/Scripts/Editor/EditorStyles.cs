#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class EditorStyles
{
    public static GUIStyle header()
    {
        GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
        style.fontSize = 18;
        return style;
    }
    public static GUIStyle subheader()
    {
        GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
        style.fontSize = 14;
        return style;
    }
    public static GUIStyle normalrich()
    {
        GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
        style.wordWrap = true;
        style.richText = true;
        style.fontSize = 12;
        return style;
    }
    public static GUIStyle normal()
    {
        GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
        style.wordWrap = true;
        style.fontSize = 12;
        return style;
    }
    public static GUIStyle box()
    {
        GUIStyle g = new GUIStyle("box");
        g.richText = true;
        return g;
    }


    public static GUIStyle foldoutHeader()
    {
        GUIStyle style = new GUIStyle(GUI.skin.GetStyle("foldout"));
        style.fontSize = 14;
        return style;
    }


    public static GUIStyle itemName()
    {
        GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
        style.wordWrap = true;
        style.fontSize = 14;
        return style;
    }
    public static GUIStyle itemDescription()
    {
        GUIStyle style = new GUIStyle(GUI.skin.GetStyle("label"));
        style.wordWrap = true;
        style.fontSize = 11;
        return style;
    }
}
#endif