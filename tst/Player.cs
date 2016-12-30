using System;

namespace Game
{
    class Player : IBase
    {
        public string Shape { get; private set; }
        public static char shape = '╠';

        public int PositionX { get; private set; }
        public int PositionY { get; private set; }

        public void ChangeX(int value)
        {
            throw new Exception("Can't change player's PositionX.");
        }
        
        public void ChangeY(int value)
        {
            PositionY = value;
        }

        public Player()
        {
            PositionX = 0;
            Shape = "╠";
        }

        public void Draw()
        {
            if (PositionX < 78 && PositionY < 24)
            {
                Console.SetCursorPosition(PositionX, PositionY);
                Console.CursorVisible = false;
                Console.Write(Shape);
            }
        }
    }
}
