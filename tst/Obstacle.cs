using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Obstacle : IBase
    {
        public int PositionX { get; private set; }
        public int PositionY { get; private set; }
        public static char baseShape = '▌';

        public void ChangeX(int value)
        {
            PositionX = value;
        }

        public void ChangeY(int value)
        {
            PositionY = value;
        }

        public string Shape { get; set; }
        public int HealthPoints { get; set; }
        Random rand = new Random();

        public Obstacle()
        {
            HealthPoints = rand.Next(1, 4);
            if (HealthPoints == 1)
                Shape = "▌";
            else if (HealthPoints == 2)
                Shape = "▌▌";
            else
                Shape = "▌▌▌";
            PositionX = 0;
            PositionY = 0;
        }

        public Obstacle(int healthPoints)
        {
            HealthPoints = healthPoints;
            if (healthPoints == 1)
                Shape = "▌";
            else if (healthPoints == 2)
                Shape = "▌▌";
            else
                Shape = "▌▌▌";
            PositionX = 0;
            PositionY = 0;
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

        private void DrawWall()
        {
            Console.CursorVisible = false;
            for(int i = 0; i < 24; i++)
            {
                Console.SetCursorPosition(29, i);
                Console.Write("|");
            }
        }

        public void ChangeHP(int value)
        {
            HealthPoints = value;
            if(HealthPoints == 3)
                Shape = "▌▌▌";
            else if(HealthPoints == 2)
                Shape = "▌▌";
            else
                Shape = "▌";
        }
    }
}
