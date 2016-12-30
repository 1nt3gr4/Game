using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Bullet : IBase
    {
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public string Shape { get; private set; }
        public static char shape = '*';

        public void ChangeX(int value)
        {
            PositionX = value;
        }

        public void ChangeY(int value)
        {
            throw new Exception();
        }

        public void Draw()
        {
            if(PositionX < 78 && PositionY < 24)
            {
                Console.SetCursorPosition(PositionX, PositionY);
                Console.CursorVisible = false;
                Console.Write(Shape);
            }
        }

        public Bullet(int posY)
        {
            Shape = "*";
            PositionX = 0;
            PositionY = posY;
        }


    }
}
