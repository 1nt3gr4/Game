using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Game
{
    public interface IBase
    {
        void Draw();
        int PositionX { get; }
        int PositionY { get; }
        string Shape { get; }
        void ChangeX(int value);
        void ChangeY(int value);
    }

    class Game
    {
        private static int FieldWidth = 78;
        private static int FieldHeight = 24;
        private static int _border = 30;
        static int spawnTime = 3500;

        public void Start(int speed)
        {
            TimeSpan[] t = new TimeSpan[3];
            bool con = true;
            int score = 0;
            int fps = speed;
            int delay = 100;
            int bulletDelay = delay / 3;
            Player p = new Player();
            ConsoleKey k;
            bool check = true;
            List<Obstacle> Ob = new List<Obstacle>();
            List<Bullet> b = new List<Bullet>();
            
            while(check)
            {
                if((int)t[0].TotalMilliseconds > bulletDelay)
                {
                    foreach(Bullet _b in b)
                    {
                        _b.ChangeX(_b.PositionX + 1);
                    }
                    t[0] = t[0].Subtract(new TimeSpan(0, 0, 0, 0, bulletDelay));
                    Console.Clear();
                }
                if ((int)t[1].TotalMilliseconds > delay)
                {
                    foreach (Obstacle o in Ob)
                        o.ChangeX(o.PositionX - 1);
                    t[1] = t[1].Subtract(new TimeSpan(0, 0, 0, 0, delay));
                    Console.Clear();
                }

                if (t[2].TotalMilliseconds > spawnTime) 
                {
                    CreateObstacle(ref Ob);
                    t[2] = t[2].Subtract(new TimeSpan(0, 0, 0, 0, spawnTime));
                }
                if(Console.KeyAvailable)
                {
                    Console.Clear();
                    k = Console.ReadKey(false).Key;
                    switch (k)
                    {
                        case ConsoleKey.UpArrow:
                            if (p.PositionY > 0)
                                p.ChangeY(p.PositionY - 1);
                            break;
                        case ConsoleKey.DownArrow:
                            if (p.PositionY < 23)
                                p.ChangeY(p.PositionY + 1);
                            break;
                        case ConsoleKey.Spacebar:
                            CreateBullet(ref b, p.PositionY);
                            break;
                        case ConsoleKey.Escape:
                            check = false;
                            break;
                    }
                }
                DestroyObstacles(ref b, ref Ob, ref score);
                Delete(ref Ob, ref b, p, out con);
                if (!con)
                {
                    Console.Clear();
                    Console.WriteLine("You lose.");
                    Console.WriteLine($"Your score: {score}");
                    Console.ReadKey();
                    break;
                }
                foreach (Obstacle o in Ob)
                    o.Draw();
                foreach (Bullet _b in b)
                    _b.Draw();
                p.Draw();
                for (int i = 0; i < t.Length; i++)
                    t[i] = t[i].Add(new TimeSpan(0, 0, 0, 0, fps));
                Thread.Sleep(fps);
            }
        }
        
        public void CreateObstacle(ref List<Obstacle> Ob)
        {
            Random rand = new Random();
            Ob.Add(new Obstacle());
            Ob[Ob.Count - 1].ChangeX(rand.Next(_border, FieldWidth - 2));
            Ob[Ob.Count - 1].ChangeY(rand.Next(FieldHeight));

            foreach (Obstacle o in Ob)
            {
                if (o.PositionX == Ob[Ob.Count - 1].PositionX && o != Ob[Ob.Count - 1])
                    o.ChangeX(rand.Next(_border, FieldWidth - 2));
                if (o.PositionY == Ob[Ob.Count - 1].PositionY && o != Ob[Ob.Count - 1])
                    o.ChangeY(rand.Next(FieldHeight));
            }
        }

        public void CreateBullet(ref List<Bullet> b, int startPosition)
        {
            b.Add(new Bullet(startPosition));
            b[b.Count - 1].ChangeX(1);
        }

        private void DestroyObstacles(ref List<Bullet> b, ref List<Obstacle> ob, ref int score)
        {
            for (int i = 0; i < b.Count; i++)
                for (int j = 0; j < ob.Count; j++)
                {
                    int x1 = -1, x2 = -2, y1 = -1, y2 = -2;
                    if(i >= 0 && j >= 0 && i < b.Count && j < ob.Count)
                    {
                        x1 = b[i].PositionX;
                        x2 = ob[j].PositionX;
                        y1 = b[i].PositionY;
                        y2 = ob[j].PositionY;
                    }
                    if ((x1 == x2 || x2 == x1 + 1) && y1 == y2)
                    {
                        score += 1;
                        if (ob[j].HealthPoints == 1)
                        {
                            ob.Remove(ob[j]);
                            if (ob.Count == 0)
                                CreateObstacle(ref ob);
                            b.Remove(b[i]);
                            continue;
                        }

                        if (ob[j].HealthPoints == 2 && j < ob.Count)
                        {
                            ob[j].HealthPoints = 1;
                            ob[j].Shape = "▌";
                            b.Remove(b[i]);
                            continue;
                        }
                        if (ob[j].HealthPoints == 3 && j < ob.Count)
                        {
                            ob[j].HealthPoints = 2;
                            ob[j].Shape = "▌▌";
                            b.Remove(b[i]);
                            continue;
                        }
                    }
                }
        }

        private void Delete(ref List<Obstacle> ob, ref List<Bullet> b, Player p, out bool con)
        {
            con = true;
            for(int i = 0; i < b.Count; i++)
            {
                if (b[i].PositionX <= 0 || b[i].PositionX >= FieldWidth - 1)
                        b.Remove(b[i]);
            }
            for(int i = 0; i < ob.Count; i++)
            {
                if (ob[i].PositionX <= 0 || ob[i].PositionX == p.PositionX && ob[i].PositionY == 1)
                {
                    ob.Remove(ob[i]);
                    con = false;
                }
            }
        }

        public void Menu()
        {
            int speed = 45;
            bool check = true;
            int matched = 1;
            ConsoleKey k = ConsoleKey.Spacebar;

            List<string> menu = new List<string>();
            menu.Add("MENU");
            menu.Add("start");
            menu.Add("select game speed");
            menu.Add("select difficult");
            menu.Add("exit");

            while (check)
            {
                switch(k)
                {
                    case ConsoleKey.DownArrow:
                        if (matched < menu.Count - 1)
                            matched += 1;
                        break;
                    case ConsoleKey.UpArrow:
                        if (matched > 1)
                            matched -= 1;
                        break;
                    case ConsoleKey.Enter:
                        switch (matched)
                        {
                            case 1:
                                this.Start(speed);
                                Console.Clear();
                                break;
                            case 2:
                                Console.Clear();
                                List<string> s = new List<string>();
                                s.Add("Select the game speed. Less value for higher speed.");
                                s.Add("Lesser values may cause blinking screen.");
                                s.Add("Enter the value from 20 to 200:(50 is recommended)");
                                for(int i = 0; i < s.Count; i++)
                                {
                                    Console.SetCursorPosition(30, 14 + i);
                                    Console.WriteLine(s[i]);
                                }
                                Console.SetCursorPosition(30, s.Count + 14);
                                int x;
                                int.TryParse(Console.ReadLine(), out x);

                                if (x >= 20 && x <= 200)
                                {
                                    speed = x / 2;
                                    Console.Clear();
                                }
                                else
                                {
                                    Console.SetCursorPosition(30, s.Count + 15);
                                    Console.WriteLine("Wrong value!");
                                    Console.ReadKey();
                                    Console.Clear();
                                }
                                break;
                            case 3:
                                Console.Clear();
                                Console.SetCursorPosition(30, 14 + matched);
                                Console.WriteLine("Type 1 for easy, 2 for medium and 3 for hard mode.");
                                Console.SetCursorPosition(30, 15 + matched);
                                switch(int.Parse(Console.ReadLine()))
                                {
                                    case 1:
                                        spawnTime = 2500;
                                        break;
                                    case 2:
                                        spawnTime = 2000;
                                        break;
                                    case 3:
                                        spawnTime = 1500;
                                        break;
                                    default:
                                        break;
                                }
                                Console.Clear();
                                break;
                            case 4:
                                Console.Clear();
                                Console.SetCursorPosition(30, 14 + matched);
                                Console.WriteLine("Are you sure?Y/N");
                                Console.SetCursorPosition(30, 15 + matched);
                                if (Console.ReadLine() == "Y")
                                    check = false;
                                else
                                    Console.Clear();
                                break;
                        }
                        break;
                    default:
                        break;
                }

                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < menu.Count; i++)
                {
                    if (matched == i)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.SetCursorPosition(30, 14 + i);
                        Console.Write(menu[i]);
                        Console.ForegroundColor = ConsoleColor.White;
                        continue;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(30, 14 + i);
                        Console.Write(menu[i]);
                    }
                }
                k = Console.ReadKey(false).Key;

                
            }
        }
    }
}
