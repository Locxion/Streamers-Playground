using Assets.Scripts.Enum;
using Assets.Scripts.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.MapGeneration
{
    public static class MapHelper
    {
        public static void SetRandomStartAndExit(MapGrid grid, ref Vector3 startPosition, ref Vector3 endPosition,
            bool randomPlacement, DirectionEnum startDir, DirectionEnum endDir)
        {
            if (randomPlacement)
            {
                startPosition = ChooseRandomPosOnTheEdge(grid, startPosition);
                endPosition = ChooseRandomPosOnTheEdge(grid, startPosition);
            }
            else
            {
                startPosition = ChooseRandomPosOnTheEdge(grid, startPosition, startDir);
                endPosition = ChooseRandomPosOnTheEdge(grid, startPosition, endDir);
            }

            grid.SetCell(startPosition.x, startPosition.z, CellObjectTypeEnum.StartPosition);
            grid.SetCell(endPosition.x, endPosition.z, CellObjectTypeEnum.EndPosition);
        }

        private static Vector3 ChooseRandomPosOnTheEdge(MapGrid grid, Vector3 startPosition, DirectionEnum direction = DirectionEnum.None)
        {
            if (direction == DirectionEnum.None)
            {
                direction = (DirectionEnum)Random.Range(1, 5);
            }

            Vector3 position = Vector3.zero;
            switch (direction)
            {
                case DirectionEnum.Right:
                    do
                    {
                        position = new Vector3(grid.Width - 1, 0, Random.Range(0, grid.Length));
                    } while (Vector3.Distance(position, startPosition) <= 1);
                    break;

                case DirectionEnum.Left:
                    do
                    {
                        position = new Vector3(0, 0, Random.Range(0, grid.Length));
                    } while (Vector3.Distance(position, startPosition) <= 1);
                    break;

                case DirectionEnum.Up:
                    do
                    {
                        position = new Vector3(Random.Range(0, grid.Length), 0, grid.Length - 1);
                    } while (Vector3.Distance(position, startPosition) <= 1);
                    break;

                case DirectionEnum.Down:
                    do
                    {
                        position = new Vector3(Random.Range(0, grid.Length), 0, 0);
                    } while (Vector3.Distance(position, startPosition) <= 1);
                    break;
            }

            return position;
        }
    }
}