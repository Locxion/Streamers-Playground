using Assets.Scripts.Enum;

namespace Assets.Scripts.Objects
{
    public class Cell
    {
        public int X { get; }
        public int Z { get; }
        public bool IsTaken { get; set; }
        public CellObjectTypeEnum ObjectType { get; set; }

        public Cell(int x, int z)
        {
            X = x;
            Z = z;
            ObjectType = CellObjectTypeEnum.Empty;
        }
    }
}