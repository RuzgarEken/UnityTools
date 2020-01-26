using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Editor.Hierarchy
{

    [InitializeOnLoad]
    public class ComponentHierarchyIconEditor
    {
        public static Dictionary<Type, Texture2D> Icons
            = new Dictionary<Type, Texture2D>()
            {
                //{typeof(Button), AssetDatabase.LoadAssetAtPath ("Assets/Textures/ButtonHierarchyIcon.png", typeof(Texture2D)) as Texture2D },
            };

        public static Dictionary<int, List<Texture2D>> MarkedObjects;

        static ObjectHierarchyView()
        {
            MarkedObjects = new Dictionary<int, List<Texture2D>>();

            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            EditorApplication.hierarchyChanged += OnHierarchyChanged;

            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyUpdate;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyUpdate;

            OnHierarchyChanged();
        }

        #region Listeners

        static void OnHierarchyUpdate(int instanceId, Rect rect)
        {
            if(MarkedObjects.TryGetValue(instanceId, out var icons))
            {
                float offset = 10;  

                foreach (var icon in icons)
                {
                    Rect r = new Rect(rect.position.x + rect.width - offset, rect.y, 18, 18);
                    GUI.Label(r, icon);

                    offset += 20;
                }
            }
        }

        public static void OnHierarchyChanged()
        {
            MarkedObjects.Clear();

            foreach (var icon in Icons)
            {
                var objects = Object.FindObjectsOfType(icon.Key);

                foreach (var obje in objects)
                {
                    int instanceId = (obje as MonoBehaviour).gameObject.GetInstanceID();
                    if (!MarkedObjects.TryGetValue(instanceId, out var list))
                    {
                        list = new List<Texture2D>();
                        MarkedObjects.Add(instanceId, list);
                    }

                    list.Add(icon.Value);
                }
            }
        }

        #endregion

    }

}
