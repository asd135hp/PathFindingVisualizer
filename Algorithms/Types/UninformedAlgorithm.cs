namespace Algorithms.Types
{
    public abstract class UninformedAlgorithm : SearchingAlgorithm
    {
        public UninformedAlgorithm(int mapRows, int mapCols) : base(mapRows, mapCols)
        {

        }

        public override string Name => "Uninformed Algorithm";
        public override AlgorithmType Type => AlgorithmType.Uninformed;
    }
}
