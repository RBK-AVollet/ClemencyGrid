using System;
using TMPro;
using UnityEngine;

namespace Clemency.Grid
{
    public class GridSystem2D<T> {
        int _width;
        int _height;
        float _cellSize;
        Vector3 _origin;
        T[,] _gridArray;

        CoordinateConverter _coordinateConverter;

        public event Action<int, int, T> OnValueChanged;

        public GridSystem2D(int width, int height, float cellSize, Vector3 origin, CoordinateConverter coordinateConverter, bool debug) {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _origin = origin;
            _coordinateConverter = coordinateConverter ?? new VerticalConverter();

            _gridArray = new T[_width, _height];

            if (debug) {
                DrawDebugLines();
            }
        }

        public T GetValue(Vector3 worldPosition) {
            Vector2Int pos = _coordinateConverter.WorldToGrid(worldPosition, _cellSize, _origin);
            return GetValue(pos.x, pos.y);
        }
        
        public T GetValue(int x, int y) {
            return IsValid(x, y) ? _gridArray[x, y] : default;
        }
        
        public void SetValue(Vector3 worldPosition, T value) {
            Vector2Int pos = _coordinateConverter.WorldToGrid(worldPosition, _cellSize, _origin);
            SetValue(pos.x, pos.y, value);
        }
        
        public void SetValue(int x, int y, T value) {
            if (IsValid(x, y)) {
                _gridArray[x, y] = value;
                OnValueChanged?.Invoke(x, y, value);
            }
        }
        
        bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < _width && y < _height;

        public Vector2Int GetXY(Vector3 worldPosition) => _coordinateConverter.WorldToGrid(worldPosition, _cellSize, _origin);
        
        public Vector3 GetWorldPositionCenter(int x, int y) => _coordinateConverter.GridToWorldCenter(x, y, _cellSize, _origin);
        
        Vector3 GetWorldPosition(int x, int y) => _coordinateConverter.GridToWorld(x, y, _cellSize, _origin);
        
        void DrawDebugLines() {
            const float k_duration = 100f;
            var parent = new GameObject("Debugging");

            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    CreateWorldText(parent, x + "," + y, GetWorldPositionCenter(x, y), _coordinateConverter.Forward);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, k_duration);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, k_duration);
                }
            }
            
            Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, k_duration);
            Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, k_duration);
        }

        TextMeshPro CreateWorldText(GameObject parent, string text, Vector3 position, Vector3 dir,
            int fontSize = 2, Color color = default, TextAlignmentOptions textAnchor = TextAlignmentOptions.Center, int sortingOrder = 0)
        {
            GameObject gameObject = new GameObject("DebugText_" + text, typeof(TextMeshPro));
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.position = position;
            gameObject.transform.forward = dir;

            TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
            textMeshPro.text = text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = color;
            textMeshPro.alignment = textAnchor;
            textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMeshPro;
        }
    }
}
