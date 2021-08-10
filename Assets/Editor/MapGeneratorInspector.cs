using Assets.Scripts.MapGeneration;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorInspector : UnityEditor.Editor
    {
        private MapGenerator map;

        private void OnEnable()
        {
            map = (MapGenerator)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying)
            {
                if (GUILayout.Button("Generate Map"))
                {
                    map.GenerateNewMap();
                }
            }
        }
    }
}