using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class KnightPiece : MonoBehaviour
    {
        public Vector3 Position { get; set; }

        public static List<Vector3> listOfPossibleMoves = new List<Vector3>
        {
            new Vector3(-1, 0, 2),
            new Vector3(1, 0, 2),
            new Vector3(-1, 0, -2),
            new Vector3(1, 0, -2),
            new Vector3(-2, 0, -1),
            new Vector3(-2, 0, 1),
            new Vector3(2, 0, -1),
            new Vector3(2, 0, 1),
        };

        public KnightPiece(Vector3 position)
        {
            Position = position;
        }
    }
}