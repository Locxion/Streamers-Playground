using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Objects
{
    public class CandidateMap
    {
        public MapGrid _grid;
        private int _numberOfPieces = 0;
        public bool[] _obstaclesArray = null;
        private Vector3 _startPoint, _endPoint;
        private List<KnightPiece> _knightPiecesList;

        public CandidateMap(MapGrid mapGrid, int pieces)
        {
            _grid = mapGrid;
            _numberOfPieces = pieces;
        }

        public void CreateMap(Vector3 startPosition, Vector3 endPosition, bool autoRepair = false)
        {
            _startPoint = startPosition;
            _endPoint = endPosition;
            _obstaclesArray = new bool[_grid.Width * _grid.Length];
            _knightPiecesList = new List<KnightPiece>();
            RandomlyPlaceKnightPieces(_numberOfPieces);
            PlaceObstacles();
        }

        private bool CheckIfPositionCanBeObstacle(Vector3 position)
        {
            if (position == _startPoint || position == _endPoint)
                return false;

            int index = _grid.CalcIndexFromCoords(position.x, position.z);
            return _obstaclesArray[index] = false;
        }

        private void RandomlyPlaceKnightPieces(int numberOfPieces)
        {
            var count = numberOfPieces;
            var knightPlacementTryLimit = 100;
            while (count > 0 && knightPlacementTryLimit > 0)
            {
                var randomIndex = Random.Range(0, _obstaclesArray.Length);
                if (_obstaclesArray[randomIndex] == false)
                {
                    var coords = _grid.CalcCoordinatesFromIndex(randomIndex);
                    if (coords == _startPoint || coords == _endPoint)
                        continue;
                    _obstaclesArray[randomIndex] = true;
                    _knightPiecesList.Add(new KnightPiece(coords));
                    count--;
                }
                knightPlacementTryLimit--;
            }
        }

        private void PlaceObstaclesForKnight(KnightPiece knight)
        {
            foreach (var position in KnightPiece.listOfPossibleMoves)
            {
                var newPos = knight.Position + position;
                if (_grid.IsCellValid(newPos.x, newPos.z) && CheckIfPositionCanBeObstacle(newPos))
                {
                    _obstaclesArray[_grid.CalcIndexFromCoords(newPos.x, newPos.z)] = true;
                }
            }
        }

        private void PlaceObstacles()
        {
            foreach (var knight in _knightPiecesList)
            {
                PlaceObstaclesForKnight(knight);
            }
        }

        public MapData ReturnMapData()
        {
            return new MapData
            {
                obstacleArray = _obstaclesArray,
                KnightPiecesList = _knightPiecesList,
                startPos = _startPoint,
                endPos = _endPoint
            };
        }
    }
}