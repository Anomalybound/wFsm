using UnityEditor;
using UnityEngine;
using wLib.Fsm;

namespace wLib
{
    [CustomEditor(typeof(FsmContainer), true)]
    public class FsmContainerEditor : Editor
    {
        private FsmContainer _fsm;

        private IState _activeState;

        private void OnEnable()
        {
            _fsm = target as FsmContainer;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (_fsm.Root == null)
            {
                EditorGUILayout.LabelField("Root state not initialized.");
                return;
            }

            _activeState = null;

            DrawState("Root", _fsm.Root);
        }

        private void DrawState(string stateName, IState state)
        {
            var guiCol = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = GetStateColor(state);
            EditorGUILayout.LabelField($"Name: {stateName}[{state.GetType().FullName}] - Children: [{state.Children.Count}] - Active: [{state.ActiveStates.Count}]");
            EditorStyles.label.normal.textColor = guiCol;

            if (state.Children.Count > 0) { DrawChildren(state); }
        }

        private void DrawChildren(IState state)
        {
            EditorGUI.indentLevel++;

            if (state.ActiveStates.Count > 0)
            {
                var activeState = state.ActiveStates.Peek();
                if (_activeState != activeState)
                {
                    Repaint();
                    _activeState = activeState;
                }
            }

            if (state.Children.Count > 0)
            {
                foreach (var pair in state.Children)
                {
                    var stateName = pair.Key;
                    var childState = pair.Value;

                    DrawState(stateName, childState);
                }
            }

            EditorGUI.indentLevel--;
        }

        private Color GetStateColor(IState state)
        {
            var col = EditorStyles.label.normal.textColor;
            if (_activeState == state) { col = Color.green; }

            return col;
        }
    }
}