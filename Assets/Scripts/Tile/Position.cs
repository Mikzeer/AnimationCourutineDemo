namespace PositionerDemo
{
    public class Position
    {
        public int posX { get; private set; }
        public int posY { get; private set; }

        public Position(int posX, int posY)
        {
            this.posX = posX;
            this.posY = posY;
        }
    }
}