using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public struct MapData
    {
        public bool[] obstacleArray;
        public List<KnightPiece> KnightPiecesList;
        public Vector3 startPos;
        public Vector3 endPos;
    }
}