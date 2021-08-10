using Assets.Scripts.Enum;
using Assets.Scripts.Objects;
using UnityEngine;

namespace Assets.Scripts.MapGeneration
{
    public class MapGenerator : MonoBehaviour
    {
        public GridVisualizer GridVisualizer;
        public MapVisualizer MapVisualizer;
        public DirectionEnum startDir;
        public DirectionEnum endDir;
        public bool RandomPlacement;
        private Vector3 startPos;
        private Vector3 endPos;

        [Range(1, 10)]
        public int numberOfPieces;

        [Range(3, 20)]
        public int width = 11;

        [Range(3, 20)]
        public int length = 11;

        private MapGrid grid;

        // Start is called before the first frame update
        private void Start()
        {
            GridVisualizer.VisualizeGrid(width, length);

            GenerateNewMap();
        }

        public void GenerateNewMap()
        {
            MapVisualizer.ClearMap();
            grid = new MapGrid(width, length);
            MapHelper.SetRandomStartAndExit(grid, ref startPos, ref endPos, RandomPlacement, startDir, endDir);
            CandidateMap map = new CandidateMap(grid, numberOfPieces);
            map.CreateMap(startPos, endPos);
            MapVisualizer.VisualizeMap(grid, map.ReturnMapData(), false);
        }
    }
}