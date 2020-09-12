using System;
using UnityEngine;
namespace PositionerDemo
{
    public class Grid<TgridObject>
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
        public class OnGridObjectChangedEventArgs : EventArgs
        {
            public int x;
            public int y;
        }

        private int width;
        private int height;
        private float cellSize;
        private TgridObject[,] gridArray;
        private Vector3 originPosition;
        private TextMesh[,] debugTextArray;

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TgridObject>, int, int, TgridObject> createGridObject)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new TgridObject[width, height];
            debugTextArray = new TextMesh[width, height];
            //Debug.Log("width " + width + " height " + height);
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createGridObject(this, x, y);
                }
            }

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    //Debug.Log("x " + x + " y " + y);
                    debugTextArray[x, y] = Helper.CreateWorldText(gridArray[x, y]?.ToString(), Color.white, null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 5, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + originPosition;
        }

        private void GetXY(Vector3 worldposition, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldposition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldposition - originPosition).y / cellSize);
        }

        public void TriggerGridObjectChanged(int x, int y)
        {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }

        public TgridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            else
            {
                return default(TgridObject);
            }
        }

        public TgridObject GetGridObject(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }

        public Vector3 GetGridObjectRealWorldPosition(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);

            Vector3 offsetPosition = GetWorldPosition(x, y);

            offsetPosition = offsetPosition + new Vector3(cellSize / 2, cellSize / 2,0);

            return offsetPosition;
        }

        public Vector3 GetGridObjectRealWorldPositionByArrayPosition(int x, int y)
        {
            Vector3 offsetPosition = GetWorldPosition(x, y);

            offsetPosition = offsetPosition + new Vector3(cellSize / 2, cellSize / 2, 0);

            return offsetPosition;
        }
    }

}

