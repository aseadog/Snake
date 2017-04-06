using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;


namespace snake
{
    public partial class Form1 : Form
    {
        const int SCREEN_WIDTH = 800;
        const int SCREEN_HEIGHT = 600;

        const int gameTimeInterval = 1;
        const int snakeStartSpeed = 5;
        const int snakeIncreaseSpeedRate = 1;

        int snakeSpeedX = snakeStartSpeed;
        int snakeSpeedY = 0;
        int foodCount = 0;
        int snakeLength = 10;

        PictureBox food;
        TextBox endGame;
        Label scoreBox;
        Timer gameTime;
        Random rad;

        Size sizeSnake = new Size(30, 30);
        Size sizeFood = new Size(10, 10);
        Size sizeGameOver = new Size(400, 400);

        List<PictureBox> snakes = new List<PictureBox>();

        public Form1()
        {
            InitializeComponent();

            this.Width = SCREEN_WIDTH;
            this.Height = SCREEN_HEIGHT;
            this.StartPosition = FormStartPosition.Manual;
            this.BackColor = Color.Coral;
            this.DoubleBuffered = true;                 // Prevents flickering
            
            food = new PictureBox();

            scoreBox = new Label();                     // Create score box
            scoreBox.Size = new Size(100, 20);
            scoreBox.Location = new Point(700, 50);
            scoreBox.BackColor = Color.Black;
            scoreBox.ForeColor = Color.White;
            this.Controls.Add(scoreBox);

            for (int i = 0; i < snakeLength; i++)       // Create a List of x Pictureboxes called snakes
            {
                snakes.Add(new PictureBox());
            }

            for (int i = 0; i < snakeLength; i++)       // Add each picturebox to the form, separated by the start speed
            {
                snakes[i].Size = sizeSnake;
                snakes[i].Location = new Point(SCREEN_WIDTH / 2 - sizeSnake.Width / 2 - i*snakeStartSpeed, SCREEN_HEIGHT / 2 - sizeSnake.Height / 2);
                snakes[i].BackColor = Color.BlueViolet;
                this.Controls.Add(snakes[i]);
            }


            food.Size = sizeFood;                       // Add the global picture box food to the form
            food.Location = new Point(500, 300);
            food.BackColor = Color.Green;
            this.Controls.Add(food);

            gameTime = new Timer();
            gameTime.Interval = gameTimeInterval;
            gameTime.Tick += new EventHandler(gameTime_Tick);

            rad = new Random();
           
            gameTime.Enabled = true;
        }

        void gameTime_Tick(object sender, EventArgs e)           // Event that occurs on each tick
        {
            int[,] xy = new int[snakes.Count(),2];               // Create a 2d array for xy coordinates of each box   

            
            snakes[0].Location = new Point(snakes[0].Location.X + snakeSpeedX, snakes[0].Location.Y + snakeSpeedY);

            xy[0, 0] = snakes[0].Location.X;                    // Set first box xy coordinates adjusted fpr speed
            xy[0, 1] = snakes[0].Location.Y;
            
            for (int i = snakes.Count()-1; i > 0; i--)          // Starting from last box, set each boxe's xy coordinates to one in front
            {
                for (int j = 0; j < 2; j++)
                {
                    if (j == 0)

                        xy[i, j] = snakes[i -1].Location.X;
                    else
                        xy[i, j] = snakes[i -1].Location.Y;

                }

            }

            for (int i = 1; i<snakes.Count() ; i++)
                {
                    snakes[i].Location = new Point(xy[i, 0],xy[i,1]);       // Actually set each new position based on the array above
                }

            checkFood();                                                    // Check if snake intercepts food
            checkCollision();                                               // Check if snake hits wall
          //  checkselfcollision();
        }

        private void checkCollision()             // Have amended the boundaries to take into account a small margin on the sides
        {
            if (snakes[0].Location.X < 0 || snakes[0].Location.X > SCREEN_WIDTH - sizeSnake.Width - 17 || snakes[0].Location.Y < 0 || snakes[0].Location.Y > SCREEN_HEIGHT - sizeSnake.Height - 34)
            {
                gameOver();
            }
        }

        private void checkselfcollision()
        {
            PictureBox edge = new PictureBox();

            if (snakeSpeedX > 0)
            {
                edge.Location = new Point(snakes[0].Bounds.Right, snakes[0].Bounds.Top);
                edge.Size = new Size(snakeSpeedX, snakes[0].Height);
            }

        }


        private void checkFood()
        {
            if (snakes[0].Bounds.IntersectsWith(food.Bounds))
            {
                food.Location = new Point(new Random().Next(0, SCREEN_WIDTH/2), new Random().Next(0, SCREEN_HEIGHT/2));
                foodCount++;
                scoreBox.Text = "Score: " + Convert.ToString(foodCount);

                PictureBox addBox = new PictureBox();
                addBox.BackColor = Color.BlueViolet;
                addBox.Size = sizeSnake;
                addBox.Location = new Point(snakes[snakes.Count()-1].Location.X - snakeSpeedX, snakes[snakes.Count()-1].Location.Y - snakeSpeedY);
                this.Controls.Add(addBox);
                snakes.Add(addBox);
                
               
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            

            switch (keyData)
            {
                
                case Keys.Left:
                    moveLeft();
                    break;
                case Keys.Up:
                    moveUp();
                    break;
                case Keys.Right:
                    moveRight();
                    break;
                case Keys.Down:
                    moveDown();
                    break;
                                
                default:
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        
        private void moveLeft()
        {
            if (snakeSpeedX == 0)
            {
                snakeSpeedX = -Math.Abs(snakeSpeedY);
                snakeSpeedY = 0;
            }
         }

        private void moveRight()
        {
            if (snakeSpeedX == 0)
            {
                snakeSpeedX = Math.Abs(snakeSpeedY);
                snakeSpeedY = 0;
            }
        }

        private void moveUp()
        {
            if (snakeSpeedY == 0)
            {
                snakeSpeedY = -Math.Abs(snakeSpeedX);
                snakeSpeedX = 0;
                
            }
        }

        private void moveDown()
        {
            if (snakeSpeedY == 0)
            {
                snakeSpeedY = Math.Abs(snakeSpeedX);
                snakeSpeedX = 0;
                
            }
        }

        

        private void gameOver()
        {
            
            endGame = new TextBox();
            endGame.Location = new Point(SCREEN_WIDTH/2 - sizeGameOver.Width/2, SCREEN_HEIGHT/2 - sizeGameOver.Height/2);
            endGame.Size = sizeGameOver;
            endGame.BackColor = Color.Blue;
            this.Controls.Add(endGame);
            endGame.Text = "Game Over";
            
            
            snakeSpeedX = 0;
            snakeSpeedY = 0;

        }

       
    }
}