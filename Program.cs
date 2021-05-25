using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Snake
{
    public class Snake
    {
        public class Part
        {
            public int x, y, oldx, oldy;
        }
        public int HeadX, HeadY;
        public List<Part> parts = new List<Part>();
    }
    class Program
    {

        public static bool isStarted;
        public static int width = 80, height = 30;
        public enum Move 
        {
            up,
            down,
            left,
            right,
            stop
        }
        public static Move dirrection = Move.stop;

        public static Snake snake;
        static void Main()
        {
            Console.SetWindowSize(width, height + 5);
            Start();
        }
        public static void Start()
        {
            snake = new Snake()
            {
                HeadX = width / 2,
                HeadY = height / 2,
                parts = new List<Snake.Part>()
                {
                      new Snake.Part()
                      {
                      x = (width / 2) - 1,
                      y = height / 2,
                      oldx = (width / 2) - 1,
                      oldy = height / 2
                      }
                }
            };
            Console.CursorVisible = false;
            isStarted = true;
            dirrection = Move.stop;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Console.SetCursorPosition(j, i);
                    if (j == 0 && i == 0)
                    {
                        Console.Write("╔"); 
                        continue; 
                    }
                    if (j == width - 1 && i == 0)
                    {
                        Console.Write("╗");
                        continue;
                    }
                    if (j == 0 && i == height - 1) 
                    { 
                        Console.Write("╚"); 
                        continue;
                    }
                    if (j == width - 1 && i == height - 1)
                    {
                        Console.Write("╝");
                        continue;
                    }

                    if (i == 0 || i == height - 1)
                    {
                        Console.Write("═");
                        continue;
                    }
                    if ((j == 0 || j == width - 1))
                    {
                        Console.Write("║");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
            }
            SetFruit();
            Game();
        }



        public static int frutX = 0, frutY = 0;
        public static void SetFruit()
        {
            Random randomfood = new Random();
            frutX = randomfood.Next(1, width - 1);
            frutY = randomfood.Next(1, height - 1);
        }




        public static void Draw()
        {
            Console.SetCursorPosition(snake.HeadX, snake.HeadY);
            Console.Write("*");
            for (int i = 0; i < snake.parts.Count; i++)
            {
                Console.SetCursorPosition(snake.parts[i].oldx, snake.parts[i].oldy);
                Console.Write(" ");
                Console.SetCursorPosition(snake.parts[i].x, snake.parts[i].y);
                Console.Write("*");
            }
            Console.SetCursorPosition(frutX, frutY);
            Console.Write("+");
        }



        public static void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (dirrection != Move.down)
                            dirrection = Move.up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (dirrection != Move.up)
                            dirrection = Move.down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (dirrection != Move.right)
                            dirrection = Move.left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (dirrection != Move.left)
                            dirrection = Move.right;
                        break;
                }
            }
        }



        public static void Logic()
        {
            if (dirrection != Move.stop)//хвост
            {
                if (snake.parts.FindAll(x => (x.x == snake.HeadX && x.y == snake.HeadY)).Count > 0)
                {
                    isStarted = false;
                }
            }

            int oldX = snake.HeadX, oldY = snake.HeadY;

            if (dirrection == Move.up)
            {
                snake.HeadY--;
            }
            if (dirrection == Move.down)
            {
                snake.HeadY++;
            }
            if (dirrection == Move.left)
            {
                snake.HeadX--;
            }
            if (dirrection == Move.right)
            {
                snake.HeadX++;
            }


            if (dirrection != Move.stop)
            {

                if (snake.HeadX == width || snake.HeadX == 0 || snake.HeadY == 0 || snake.HeadY == height - 1)//стены
                {
                    isStarted = false;
                }
                if (snake.HeadX == frutX && snake.HeadY == frutY)//еда
                {
                    snake.parts.Add(new Snake.Part() { x = snake.parts[^1].oldx, y = snake.parts[^1].oldy });
                    SetFruit();
                }
                for (int i = 0; i < snake.parts.Count; i++)
                {
                    if (i == 0)
                    {
                        snake.parts[i].oldx = snake.parts[i].x;
                        snake.parts[i].oldy = snake.parts[i].y;
                        snake.parts[i].x = oldX;
                        snake.parts[i].y = oldY;
                        continue;
                    };

                    snake.parts[i].oldx = snake.parts[i].x;
                    snake.parts[i].oldy = snake.parts[i].y;

                    snake.parts[i].x = snake.parts[i - 1].oldx;
                    snake.parts[i].y = snake.parts[i - 1].oldy;
                }
                Console.SetCursorPosition(0, height + 2);
                Console.Write(snake.parts.Count + "\n\nУправление на стрелках ибо мне лень было ебошить еще кейсы");
            }
        }


        public static void Game()
        {
            while (isStarted)
            {
                Draw();
                Input();
                Logic();
                Thread.Sleep(200);
            }
            Console.Clear();
            Console.WriteLine("Для повтора хуярь по ENTER'у, а для выхода выйди в окно(ESC, нет блят DSC OFF)");

            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Start();
                    Game();
                    break;
                }
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    break;
                }
            }


        }
    }
}