namespace DevsDNA.Application.Messages
{
    public class OrientationMessage
    {
        public OrientationMessage(Orientation orientation)
        {
            Orientation = orientation;
        }

        public Orientation Orientation { get; }
    }
}
