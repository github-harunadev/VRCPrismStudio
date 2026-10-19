using PrismStudio;
using System.Linq;
using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static PrismStudio.CameraPoint;
using System.Drawing;

[CustomEditor(typeof(PostProcessorCollection))]
public class PostProcessorEditor : Editor
{
    CameraPoint cameraPoint;
    Rect AddPostProcessorButtonRect;

    public override void OnInspectorGUI()
    {
        if (cameraPoint == null)
        {
            cameraPoint = ((PostProcessorCollection)target).transform.parent.GetComponent<CameraPoint>();
        }

        GUILayout.BeginVertical(new GUIStyle("box"));

        PostProcessorEditorStatic.OnInspectorGUI(cameraPoint, ref AddPostProcessorButtonRect);

        GUILayout.EndVertical();
    }

    public static class PostProcessorEditorStatic
    {
        public static void OnInspectorGUI(CameraPoint cameraPoint, ref Rect AddPostProcessorButtonRect)
        {
            int queueRemove = -1;
            int queueMoveUp = -1;
            int queueMoveDown = -1;
            for (int i = 0; i < cameraPoint.PostProcessors.Length; i++)
            {
                PostProcessor item = cameraPoint.PostProcessors[i];
                GUIContent itemicon = null;

                if (EditorGUIUtility.GetIconForObject(item) != null)
                {
                    itemicon = new GUIContent(EditorGUIUtility.GetIconForObject(item));
                }
                else
                {
                    itemicon = EditorGUIUtility.IconContent("cs Script Icon");
                }

                GUILayout.BeginHorizontal(new GUIStyle("box"));

                if (GUILayout.Button(itemicon, new GUIStyle("label"), GUILayout.Width(32), GUILayout.Height(32)))
                {
                    EditorGUIUtility.PingObject(item.gameObject);
                }

                GUILayout.BeginVertical();

                GUILayout.Label(item.GetType().Name, EditorStyles.itemName());

                Editor e = Editor.CreateEditor(item);
                e.DrawDefaultInspector();

                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.Width(24));

                if (GUILayout.Button(EditorGUIUtility.IconContent("d_winbtn_win_close_a"), EditorStyles.box(), GUILayout.Width(24)))
                {
                    queueRemove = i;
                }
                if (i != 0) if (GUILayout.Button("<color=#ffffff>ก่</color>", EditorStyles.box(), GUILayout.Width(24)))
                    {
                        queueMoveUp = i;
                    }
                if (i < cameraPoint.PostProcessors.Length - 1) if (GUILayout.Button("<color=#ffffff>ก้</color>", EditorStyles.box(), GUILayout.Width(24)))
                    {
                        queueMoveDown = i;
                    }

                GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            }

            if (queueRemove != -1)
            {
                Undo.SetCurrentGroupName("Remove PostProcessor " + cameraPoint.PostProcessors[queueRemove].name);
                int group = Undo.GetCurrentGroup();

                Undo.DestroyObjectImmediate(cameraPoint.PostProcessors[queueRemove].gameObject);

                Undo.RecordObject(cameraPoint, "Remove PostProcessor entry");
                cameraPoint.UpdatePostProcessorsList();
                EditorUtility.SetDirty(cameraPoint);

                Undo.CollapseUndoOperations(group);
            }
            else if (queueMoveUp != -1)
            {
                Undo.SetCurrentGroupName("Arrange PostProcessor " + cameraPoint.PostProcessors[queueMoveUp].name);
                int group = Undo.GetCurrentGroup();

                cameraPoint.PostProcessors[queueMoveUp].transform.SetSiblingIndex(queueMoveUp - 1);

                Undo.RecordObject(cameraPoint, "Arrange PostProcessor entry");
                cameraPoint.UpdatePostProcessorsList();
                EditorUtility.SetDirty(cameraPoint);

                Undo.CollapseUndoOperations(group);
            }
            else if (queueMoveDown != -1)
            {
                Undo.SetCurrentGroupName("Arrange PostProcessor " + cameraPoint.PostProcessors[queueMoveDown].name);
                int group = Undo.GetCurrentGroup();

                cameraPoint.PostProcessors[queueMoveDown].transform.SetSiblingIndex(queueMoveDown + 1);

                Undo.RecordObject(cameraPoint, "Arrange PostProcessor entry");
                cameraPoint.UpdatePostProcessorsList();
                EditorUtility.SetDirty(cameraPoint);

                Undo.CollapseUndoOperations(group);
            }

            if (GUILayout.Button(PointEditor.languageData[(int)cameraPoint.studioSystem.currentLanguage]["add_postprocessor"], GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
            {
                var dropdown = new PostProcessorDropdown(new AdvancedDropdownState(), cameraPoint, (int)cameraPoint.studioSystem.currentLanguage, new Vector2(AddPostProcessorButtonRect.width, EditorGUIUtility.singleLineHeight * 16));
                dropdown.Show(new Rect(new Vector2(AddPostProcessorButtonRect.x, AddPostProcessorButtonRect.y + AddPostProcessorButtonRect.height), Vector2.zero));
            }
            if (Event.current.type == EventType.Repaint) AddPostProcessorButtonRect = GUILayoutUtility.GetLastRect();

        }

        class PostProcessorDropdown : AdvancedDropdown
        {
            CameraPoint point;

            private int currentLanguage = 0;

            private Type[] postProcessorTypes;
            private string[] postProcessorTypeNames;

            public PostProcessorDropdown(AdvancedDropdownState state, CameraPoint point, int currentLanguage, Vector2 minSize) : base(state)
            {
                this.point = point;
                this.currentLanguage = currentLanguage;
                this.minimumSize = minSize;
            }

            void UpdateTypes()
            {
                postProcessorTypes = AppDomain.CurrentDomain.GetAssemblies()
                                    .SelectMany(assembly => assembly.GetTypes())
                                    .Where(type => type.IsSubclassOf(typeof(PostProcessor)))
                                    .ToArray();

                postProcessorTypeNames = postProcessorTypes.Select(type => type.Name).ToArray();
            }

            protected override AdvancedDropdownItem BuildRoot()
            {
                UpdateTypes();

                var root = new AdvancedDropdownItem(PointEditor.languageData[currentLanguage]["settings_postprocessors"]);

                foreach (string name in postProcessorTypeNames)
                {
                    root.AddChild(new AdvancedDropdownItem(name));
                }

                return root;
            }

            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                UpdateTypes();

                if (postProcessorTypeNames.Contains(item.name))
                {
                    Type t = postProcessorTypes[postProcessorTypeNames.ToList().IndexOf(item.name)];

                    Undo.SetCurrentGroupName("Add PostProcessor " + item.name);
                    int group = Undo.GetCurrentGroup();

                    GameObject g = new GameObject(item.name);
                    g.transform.SetParent(point.PostProcessorObject);
                    Component c = g.AddComponent(t);
                    EditorUtility.SetDirty(point);
                    EditorUtility.SetDirty(g);
                    EditorUtility.SetDirty(c);

                    Undo.RegisterCreatedObjectUndo(g, "Add PostProcessor GameObject");

                    Undo.RecordObject(point, "Add PostProcessor entry");
                    point.UpdatePostProcessorsList();

                    Undo.CollapseUndoOperations(group);
                }

                base.ItemSelected(item);
            }
        }

    }
}
