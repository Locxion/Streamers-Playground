using Assets.Scripts.Enum;
using Assets.Scripts.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    public class MapVisualizer : MonoBehaviour
    {
        private Transform parent;
        public Color startColor;
        public Color endColor;

        private Dictionary<Vector3, GameObject> obstalceDict = new Dictionary<Vector3, GameObject>();

        private void Awake()
        {
            parent = this.transform;
        }

        public void VisualizeMap(MapGrid grid, MapData data, bool usePrefabs)
        {
            if (usePrefabs)
            {
            }
            else
            {
                VisualizePrimitive(grid, data);
            }
        }

        private void VisualizePrimitive(MapGrid grid, MapData data)
        {
            PlaceStartAndExitPoints(data);
            for (int i = 0; i < data.obstacleArray.Length; i++)
            {
                if (data.obstacleArray[i])
                {
                    var posOnGrid = grid.CalcCoordinatesFromIndex(i);
                    if (posOnGrid == data.startPos || posOnGrid == data.endPos)
                        continue;

                    grid.SetCell(posOnGrid.x, posOnGrid.z, CellObjectTypeEnum.Obstacle);
                    if (PlaceKnightObstacle(data, posOnGrid))
                        continue;
                    if (obstalceDict.ContainsKey(posOnGrid) == false)
                    {
                        CreateIndicator(posOnGrid, Color.white, PrimitiveType.Cube);
                    }
                }
            }
        }

        private bool PlaceKnightObstacle(MapData data, Vector3 posOnGrid)
        {
            foreach (var knight in data.KnightPiecesList)
            {
                if (knight.Position == posOnGrid)
                {
                    CreateIndicator(posOnGrid, Color.red, PrimitiveType.Cube);
                    return true;
                }
            }

            return false;
        }

        private void PlaceStartAndExitPoints(MapData data)
        {
            CreateIndicator(data.startPos, startColor, PrimitiveType.Sphere);
            CreateIndicator(data.endPos, endColor, PrimitiveType.Sphere);
        }

        private void CreateIndicator(Vector3 position, Color color, PrimitiveType sphere)
        {
            var element = GameObject.CreatePrimitive(sphere);
            obstalceDict.Add(position, element);
            element.transform.position = position + new Vector3(.5f, .5f, .5f);
            element.transform.parent = parent;
            var renderer = element.GetComponent<Renderer>();
            renderer.material.SetColor("_Color", color);
        }

        public void ClearMap()
        {
            foreach (var obstacle in obstalceDict.Values)
            {
                Destroy(obstacle);
            }
            obstalceDict.Clear();
        }
    }
}