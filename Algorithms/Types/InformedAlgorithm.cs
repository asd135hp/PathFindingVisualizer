namespace Algorithms.Types
{
    public abstract class InformedAlgorithm : SearchingAlgorithm
    {
        public InformedAlgorithm(int mapRows, int mapCols) : base(mapRows, mapCols) { }

        public override string Name => "Informed Algorithm";
        public override AlgorithmType Type => AlgorithmType.Informed;
    }
}
