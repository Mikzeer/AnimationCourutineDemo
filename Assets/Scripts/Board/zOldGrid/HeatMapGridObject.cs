using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PositionerDemo
{
    public class HeatMapGridObject
    {
        private const int MIN = 0;
        private const int MAX = 100;

        private Grid<HeatMapGridObject> grid;
        public int x;
        public int y;
        public int value;

        public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void AddValue(int addValue)
        {
            value += addValue;
            value = Mathf.Clamp(value, MIN, MAX);
            grid.TriggerGridObjectChanged(x, y);
        }

        public Vector3 GetRealWorldLocation()
        {            
            return grid.GetGridObjectRealWorldPositionByArrayPosition(x, y);
        }

        public float GetValueNormalized()
        {
            return (float)value / MAX;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

}