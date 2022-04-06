using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EditorGUISplitView
    {
        public enum Direction
        {
            Horizontal,
            Vertical
        }

        Direction _splitDirection;
        float _splitNormalizedPosition;
        bool _resize;
        public Vector2 scrollPosition;
        Rect _availableRect;


        public EditorGUISplitView(Direction splitDirection)
        {
            _splitNormalizedPosition = 0.5f;
            this._splitDirection = splitDirection;
        }

        public void BeginSplitView()
        {
            Rect tempRect;

            if (_splitDirection == Direction.Horizontal)
                tempRect = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            else
                tempRect = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            if (tempRect.width > 0.0f)
            {
                _availableRect = tempRect;
            }

            if (_splitDirection == Direction.Horizontal)
                scrollPosition = GUILayout.BeginScrollView(scrollPosition,
                    GUILayout.Width(_availableRect.width * _splitNormalizedPosition));
            else
                scrollPosition = GUILayout.BeginScrollView(scrollPosition,
                    GUILayout.Height(_availableRect.height * _splitNormalizedPosition));
        }

        public void Split()
        {
            GUILayout.EndScrollView();
            ResizeSplitFirstView();
        }

        public void EndSplitView()
        {
            if (_splitDirection == Direction.Horizontal)
                EditorGUILayout.EndHorizontal();
            else
                EditorGUILayout.EndVertical();
        }

        private void ResizeSplitFirstView()
        {
            Rect resizeHandleRect;

            if (_splitDirection == Direction.Horizontal)
                resizeHandleRect = new Rect(_availableRect.width * _splitNormalizedPosition, _availableRect.y, 
                    2f, _availableRect.height);
            else
                resizeHandleRect = new Rect(_availableRect.x, _availableRect.height * _splitNormalizedPosition,
                    _availableRect.width, 2f);

            GUI.DrawTexture(resizeHandleRect, EditorGUIUtility.whiteTexture);

            if (_splitDirection == Direction.Horizontal)
                EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeHorizontal);
            else
                EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeVertical);

            if (Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition))
            {
                _resize = true;
            }

            if (_resize)
            {
                if (_splitDirection == Direction.Horizontal)
                    _splitNormalizedPosition = Event.current.mousePosition.x / _availableRect.width;
                else
                    _splitNormalizedPosition = Event.current.mousePosition.y / _availableRect.height;
            }

            if (Event.current.type == EventType.MouseUp)
                _resize = false;
        }
    }
}