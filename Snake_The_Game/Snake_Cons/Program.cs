using Snake_Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake_Cons
{
    class Program
    {
        List<Circle> Snake;
        Circle food;
        bool eating = false;
        Circle FoodEating;

        public void PrintSnake()
        {
            Console.Clear();

            Console.SetCursorPosition(food.x, food.y);
            Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("¤");
            Console.ForegroundColor = ConsoleColor.White;

            if (Snake != null)
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        try
                        {
                            Console.SetCursorPosition(Snake[i].x, Snake[i].y);
                            Console.ForegroundColor = ConsoleColor.DarkGreen; Console.WriteLine("@");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("GameOver");
                            CheckScore();
                            Console.ReadKey();

                            Environment.Exit(-1);
                           
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(Snake[i].x, Snake[i].y);
                        Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("o");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
        }
        static void Main(string[] args)
        {
            Console.SetWindowSize(30, 30);
            Program p = new Program();
            p.Game();
        }
        public void Game()
        {
            Thread dirChange = new Thread(ChangeDirection);
            dirChange.Start();
            StartGame();
            while (true)
            {
                PrintSnake();
                MoveSnake();

                SnakeEat();
                Thread.Sleep(100);
                Colosion();
            }
        }
        public void StartGame()
        {
            new Settings();
            Snake = new List<Circle>();
            Circle Head = new Circle(10, 10);
            Snake.Add(Head);
            MakeFood();
        }
        private static void CheckScore()
        {
            Console.SetCursorPosition(1, 1);
            Console.WriteLine("      Your Score is: " + Settings.score);
            
        }
        public void SnakeEat()
        {
            if (eating)
            {
                Snake.Add(FoodEating);
                MakeFood();
                eating = false;
            }
            if (Snake[0].x == food.x && Snake[0].y == food.y)
            {
                FoodEating = new Circle(food.x, food.y);
                eating = true;
                Settings.score += Settings.points;
            }
        }
        public void MakeFood()
        {
            Random random = new Random();
            int x = random.Next(1, 30);
            int y = random.Next(1, 30);
            food = new Circle(x, y);
        }
        public void MoveSnake()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i > 0)
                {
                    Snake[i].x = Snake[i - 1].x;
                    Snake[i].y = Snake[i - 1].y;
                }
                if (i == 0)
                {
                    try
                    {
                        if (Settings.direction == Direction.Down) { Snake[i].y++; }
                        else if (Settings.direction == Direction.Up) { Snake[i].y--; }
                        else if (Settings.direction == Direction.Left) { Snake[i].x--; }
                        else if (Settings.direction == Direction.Right) { Snake[i].x++; }
                        else if (Settings.direction == Direction.Stop) { CheckScore(); }
                    }
                    catch
                    {
                        Console.Clear();
                        Console.WriteLine("GameOver");
                        CheckScore();
                        Console.ReadKey();
                        Environment.Exit(-1);
                    }
                }
            }
        }
        public static void ChangeDirection()
        {
            while (true)
            {
                ConsoleKeyInfo key;
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Spacebar:
                        Settings.direction = Direction.Stop;

                        break;
                    case ConsoleKey.Enter:
                        break;
                    case ConsoleKey.LeftArrow:
                        if (Settings.direction != Direction.Right)
                        {
                            Settings.direction = Direction.Left;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (Settings.direction != Direction.Down)
                        {
                            Settings.direction = Direction.Up;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (Settings.direction != Direction.Left)
                        {
                            Settings.direction = Direction.Right;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (Settings.direction != Direction.Up)
                        {
                            Settings.direction = Direction.Down;
                        }
                        break;
                    default:
                        break;
                }

            }
        }
        public void Colosion()
        {
            for (int i = 1; i < Snake.Count; i++)
            {
                if (Snake[0].x == Snake[i].x && Snake[0].y == Snake[i].y)
                {
                    Console.Clear();
                    Console.WriteLine("GameOver");
                    CheckScore();
                    Console.ReadKey();
                    Environment.Exit(-1);
                }
            }
        }

    }
}
