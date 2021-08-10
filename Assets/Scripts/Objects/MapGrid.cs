using Assets.Scripts.Enum;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class MapGrid
    {
        public int Width { get; set; }
        public int Length { get; set; }
        public Cell[,] cellGrid { get; set; }

        public MapGrid(int width, int length)
        {
            Width = width;
            Length = length;
            CreateGrid();
        }

        private void CreateGrid()
        {
            cellGrid = new Cell[Length, Width];
            for (int row = 0; row < Length; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    cellGrid[row, col] = new Cell(col, row);
                }
            }
        }

        public void SetCell(int x, int z, CellObjectTypeEnum objectType, bool isTaken = true)
        {
            cellGrid[z, x].ObjectType = objectType;
            cellGrid[z, x].IsTaken = isTaken;
        }

        public void SetCell(float x, float z, CellObjectTypeEnum objectType, bool isTaken = true)
        {
            SetCell((int)x, (int)z, objectType, isTaken);
        }

        public bool IsCellTaken(int x, int z)
        {
            return cellGrid[z, x].IsTaken;
        }

        public bool IsCellTaken(float x, float z)
        {
            return cellGrid[(int)z, (int)x].IsTaken;
        }

        public bool IsCellValid(float x, float z)
        {
            if (x >= Width || x < 0 || z >= Length || z < 0)
            {
                return false;
            }

            return true;
        }

        public Cell GetCell(int x, int z)
        {
            if (IsCellValid(x, z) == false)
            {
                return null;
            }

            return cellGrid[z, x];
        }

        public Cell GetCell(float x, float z)
        {
            return GetCell((int)x, (int)z);
        }

        public int CalcIndexFromCoords(float x, float z)
        {
            return (int)x + (int)z * Width;
        }

        public void CheckCoors()
        {
            for (int i = 0; i < cellGrid.GetLength(0); i++)
            {
                StringBuilder b = new StringBuilder();
                for (int j = 0; j < cellGrid.GetLength(1); j++)
                {
                    b.Append(j + "," + i + " ");
                }

                Debug.Log(b.ToString());
            }
        }

        public int CalcIndexFromCoords(int x, int z)
        {
            return x + z * Width;
        }

        public Vector3 CalcCoordinatesFromIndex(int randomIndex)
        {
            int x = randomIndex % Width;
            int z = randomIndex / Width;
            return new Vector3(x, 0, z);
        }
    }
}