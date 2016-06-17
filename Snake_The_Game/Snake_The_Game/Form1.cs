using Snake_Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake_The_Game
{
    public partial class Form1 : Form
    {
        System.Media.SoundPlayer PlaySound = new System.Media.SoundPlayer(Properties.Resources.GameSound);
        System.Media.SoundPlayer WinnerSound = new System.Media.SoundPlayer(Properties.Resources.WinSound);
        System.Media.SoundPlayer EatSound = new System.Media.SoundPlayer(Properties.Resources.EatSound);
        System.Media.SoundPlayer Eat10Sound = new System.Media.SoundPlayer(Properties.Resources.ray_gun);

        List<Circle> Snake;
        Circle food;
        List<Score> HighScore;
        bool GameDone = true;
        bool ReadyToMove = true;
        bool Sound = true;
        int PlayerPoints = 10;      

        public Form1()
        {
            InitializeComponent();
            new Settings();
            lblSpeed.Text = "Medium (10 points)";                 
            PlayMainMusik();
            timerGame.Tick += TimerGame_Tick;
            timerMove.Tick += TimerMove_Tick;
            pbCanvas.Paint += PbCanvas_Paint;
            HighScore = LoadAndSaveGame.LoadG();
            SetScoresOnBoard();
            timerMove.Start();
        }
        private void SetScoresOnBoard()
        {
            for (int i = 0; i < HighScore.Count; i++)
            {
                lblName1.Text = HighScore[0]._name;
                lblScore1.Text = HighScore[0]._score.ToString();
                lblName2.Text = HighScore[1]._name;
                lblScore2.Text = HighScore[1]._score.ToString();
                lblName3.Text = HighScore[2]._name;
                lblScore3.Text = HighScore[2]._score.ToString();
            }
        }
        private void TimerMove_Tick(object sender, EventArgs e)
        {
            this.KeyDown += new KeyEventHandler(ChangeDirection);          
            if (Settings.Name != "")
            {
                AddNewPlayerToHighScore();
                if(Sound)
                PlayMainMusik();
                
            }
        }
        private void TimerGame_Tick(object sender, EventArgs e)
        {
            if (Settings.direction != Direction.Stop)
            {
                MovePlayer();
                pbCanvas.Invalidate();
            }
        }
        private void StartGame()
        {
            SetPoints();     
            PlaySound.Stop();
            timerMusik.Stop();

            pbCanvas.Image = Properties.Resources.untitled;
            Snake = new List<Circle>();
            new Settings();
            Settings.points = PlayerPoints;
            ReadyToMove = true;
            GameDone = false;
            Circle head = new Circle();
            head.x = 15;
            head.y = 12;
            Snake.Add(head);
            lblScore.Text = Settings.score.ToString();
            GenerateFood();
            timerGame.Start();

        }
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / 15;
            int maxYPos = pbCanvas.Size.Height / 15;
            Random random = new Random();
            food = new Circle();
            do
            {
                food.x = random.Next(0, maxXPos);
                food.y = random.Next(0, maxYPos);
            } while (!CheckFreeSpot(food)); // Check so food dont spawn in snake
        }
        private bool CheckFreeSpot(Circle food)
        {

            foreach (var item in Snake)
            {
                if (item.x == food.x && item.y == food.y)
                {
                    return false;
                }
            }
            return true;
        }
        private void PbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!GameDone)
            {
                Brush snakeColor;
                //Draw snake
                for (int i = Snake.Count - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        snakeColor = Brushes.Black;
                    }
                    else if (i == 10|| i ==20||i == 30|| i == 40|| i == 50)
                    {
                        snakeColor = Brushes.BlueViolet;
                    }
                    else
                    {
                        snakeColor = Brushes.Green;
                    }

                    //Paint Snake
                    canvas.FillEllipse(snakeColor,
                         new Rectangle(Snake[i].x * Settings.Width,
                                       Snake[i].y * Settings.Height,
                                       Settings.Width, Settings.Height));


                    //Paint food
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.x * Settings.Width,
                        food.y * Settings.Height, Settings.Width, Settings.Height));
                }

            }
        }
        private void ChangeDirection(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // handle up/down/left/right
                case Keys.Up:
                    if (Settings.direction != Direction.Down && ReadyToMove == true)
                    {
                        Settings.direction = Direction.Up;
                        ReadyToMove = false;
                    }
                    ReadyToMove = false;
                    break;
                case Keys.Left:
                    {
                        if (Settings.direction != Direction.Right && ReadyToMove == true)
                            Settings.direction = Direction.Left;
                        ReadyToMove = false;
                    }
                    break;
                case Keys.Right:
                    if (Settings.direction != Direction.Left && ReadyToMove == true)
                    {
                        Settings.direction = Direction.Right;
                        ReadyToMove = false;
                    }
                    break;
                case Keys.Down:
                    if (Settings.direction != Direction.Up && ReadyToMove == true)
                    {
                        Settings.direction = Direction.Down;
                        ReadyToMove = false;
                    }
                    break;
                case Keys.Space:
                    if (Settings.Pause == false)
                    {
                        Settings.direction = Direction.Stop;
                        Settings.Pause = true;
                    }
                    else
                    {          
                            lblWarning.Text = "You cant pause more this game!";
                            timerWarning.Start();                      
                    }
                    break;
                //case Keys.F10:
                //    StartGame();
                //    pbCanvas.Invalidate();
                //    break;
                default: return;
            }
        }
        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //MoveHead
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Left:
                            Snake[i].x--;
                            break;
                        case Direction.Right:
                            Snake[i].x++;
                            break;
                        case Direction.Up:
                            Snake[i].y--;
                            break;
                        case Direction.Down:
                            Snake[i].y++;
                            break;
                        default:
                            break;
                    }
                    //Get Canvas border
                    var MaxX = pbCanvas.Size.Width / Settings.Width;
                    var MaxY = pbCanvas.Size.Height / Settings.Height;

                    //Keeps snake inside Canvas
                    if (Snake[i].x == MaxX || Snake[i].y == MaxY || Snake[i].x < 0 || Snake[i].y < 0)
                    {
                        Die();
                    }
                    //Colotion with its body
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].x == Snake[j].x && Snake[i].y == Snake[j].y)
                        {
                            Die();
                        }
                    }
                    // food colotion
                    if (Snake[0].x == food.x && Snake[0].y == food.y)
                    {
                        Eat();
                    }


                }
                else
                {
                    //Move body
                    Snake[i].x = Snake[i - 1].x;
                    Snake[i].y = Snake[i - 1].y;
                }
            }
            ReadyToMove = true;
        }
        private void Eat()
        {
            Settings.eat++;
            if (Settings.eat == 10)
            {
                if(Sound)
                Eat10Sound.Play();


                Settings.eat = 0;
            }
        else
            {
                if (Sound)
                    EatSound.Play();
            }         
            Circle food = new Circle();
            food.x = Snake[Snake.Count - 1].x;
            food.y = Snake[Snake.Count - 1].y;
            Snake.Add(food);
            Settings.score += Settings.points;
            lblScore.Text = Settings.score.ToString();
            GenerateFood();         
        }
        private void Die()
        {
            timerMusik.Stop();
            PlaySound.Stop();
            timerGame.Stop();          
            GameDone = true;
            int a = Convert.ToInt32(lblScore3.Text);
            if (Settings.score > a)
            {
                if (Sound)
                    WinnerSound.Play();


                HighScoreScreen hs = new HighScoreScreen();
                hs.Show();

            }
            else
            {
                if (Sound)
                    PlayMainMusik();       
                
                     
                pbCanvas.Image = Properties.Resources.Snake;
            }
            Settings.direction = Direction.Stop;
        }
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes ==
            MessageBox.Show("Are you sure?", "Exit Snake The Game", MessageBoxButtons.YesNo))
            {
                this.Close();
            }
        }
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartGame();
            pbCanvas.Invalidate();
        }
        private void AddNewPlayerToHighScore()
        {
            Score score = new Score(Settings.Name, Settings.score);
            HighScore = LoadAndSaveGame.CheckHighScore(HighScore, score);
            LoadAndSaveGame.SaveG(HighScore);
            HighScore = LoadAndSaveGame.LoadG();
            SetScoresOnBoard();
            Settings.Name = "";
            pbCanvas.Image = Properties.Resources.Snake;
        }
        private void resetHighScoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes ==
            MessageBox.Show("Are you sure?" + Environment.NewLine + "You want to reset ALL highScores??", "Reset highScores", MessageBoxButtons.YesNo))
            {
                for (int i = 0; i < HighScore.Count; i++)
                {
                    HighScore[i]._name = "PlayerName";
                    HighScore[i]._score = 0;
                    LoadAndSaveGame.SaveG(HighScore);
                    SetScoresOnBoard();
                }
            }
        }
        private void superSlowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GameDone)
            {
                timerGame.Interval = 250;
                lblSpeed.Text = "Super Slow (1 points)";
            }
            else
            {
                lblWarning.Text = "You cant change speed under a game!";
                timerWarning.Start();
            }
        }
        private void slowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GameDone)
            {
                timerGame.Interval = 150;
                lblSpeed.Text = "Slow (5 points)";
            }
            else
            {
                lblWarning.Text = "You cant change speed under a game!";
                timerWarning.Start();
            }
        }
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GameDone)
            {
                timerGame.Interval = 100;
                lblSpeed.Text = "Medium (10 points)";
            }
            else
            {
                lblWarning.Text = "You cant change speed under a game!";
                timerWarning.Start();
            }
        }
        private void fastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GameDone)
            {
                timerGame.Interval = 60;
                lblSpeed.Text = "Fast (15 points)";
            }
            else
            {
                lblWarning.Text = "You cant change speed under a game!";
                timerWarning.Start();
            }
        }
        private void tURBOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GameDone)
            {
                timerGame.Interval = 30;
                lblSpeed.Text = "TURBO! (50 points)";
            }
            else
            {
                lblWarning.Text = "You cant change speed under a game!";
                timerWarning.Start();
            }
        }
        private void SetPoints()
        {
            if (timerGame.Interval == 250) { PlayerPoints = 1; }
           else if (timerGame.Interval == 150) { PlayerPoints = 5; }
                else if (timerGame.Interval == 100) { PlayerPoints = 10;  }
                else if (timerGame.Interval == 60) { PlayerPoints = 15;  }
            else if (timerGame.Interval == 30) { PlayerPoints = 50; }
        }
        private void pauseSpaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.direction = Direction.Stop;
        }
        private void keysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Up-Arrow = Walk up" + Environment.NewLine+"Down-Arrow = Walk Down" + Environment.NewLine +
                "Left-Arrow = Walk Left" + Environment.NewLine +"RIght-Arrow = Walk Right" + Environment.NewLine +
                "Space = Pause" + Environment.NewLine +"F10 = New Game","Game keys");
        }
        private void rulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dont hit sides! Your snake dies!" + Environment.NewLine + "Dont hit your tail! Your snake dies!" +
                Environment.NewLine + "You may only Pause one time each game!"+Environment.NewLine +"Get the highScore!","Game rules");
        }
        private void timerWarning_Tick(object sender, EventArgs e)
        {
            lblWarning.Text = "";
            timerWarning.Stop();
        }
        private void PlayMainMusik()
        {
            PlaySound.Play();
            timerMusik.Start();
        }
        private void timerMusik_Tick(object sender, EventArgs e)
        {
            PlaySound.Play();           
        }
        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sound = true;
            if (GameDone)
                PlaySound.Play();
        }
        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sound = false;
            PlaySound.Stop();
            WinnerSound.Stop();
            EatSound.Stop();
            Eat10Sound.Stop();
        }

    
    }
}
