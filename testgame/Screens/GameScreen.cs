using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameSystemServices;
using System.Threading;

namespace testgame
{
    public partial class GameScreen : UserControl
    {
        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown, bDown, nDown, mDown, spaceDown;

        //player2 button control keys - DO NOT CHANGE
        Boolean aDown, sDown, dDown, wDown, cDown, vDown, xDown, zDown;

        //TODO create your global game variables here
        int heroX, heroY, heroSize, heroSpeed, monX, monY, monSize, monSpeed;
        SolidBrush heroBrush = new SolidBrush(Color.Black);
        Pen linePen = new Pen(Color.Black);

        List<int> monXList = new List<int>();
        List<int> monYList = new List<int>();
        List<int> monSizeList = new List<int>();
        List<int> monSpeedList = new List<int>();
        SolidBrush monBrush = new SolidBrush(Color.Black);
        

        public GameScreen()
        {
            InitializeComponent();
            InitializeGameValues();
        }

        public void InitializeGameValues()
        {
            //TODO - setup all your initial game values here. Use this method
            // each time you restart your game to reset all values.
            heroX = 25;
            heroY = this.Height / 2;
            heroSize = 20;
            heroSpeed = 4;

            monXList.Add(394);
            monYList.Add(20);
            monSizeList.Add(15);
            monSpeedList.Add(1);

            monXList.Add(300);
            monYList.Add(98);
            monSizeList.Add(15);
            monSpeedList.Add(1);

            monXList.Add(434);
            monYList.Add(219);
            monSizeList.Add(15);
            monSpeedList.Add(1);
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // opens a pause screen is escape is pressed. Depending on what is pressed
            // on pause screen the program will either continue or exit to main menu
            if (e.KeyCode == Keys.Escape && gameTimer.Enabled)
            {
                gameTimer.Enabled = false;
                rightArrowDown = leftArrowDown = upArrowDown = downArrowDown = false;

                DialogResult result = PauseForm.Show();

                if (result == DialogResult.Cancel)
                {
                    gameTimer.Enabled = true;
                }
                else if (result == DialogResult.Abort)
                {
                    MainForm.ChangeScreen(this, "MenuScreen");
                }
            }

            //TODO - basic player 1 key down bools set below. Add remainging key down
            // required for player 1 or player 2 here.

            //player 1 button presses
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Space:
                    spaceDown = true;
                    break;
                case Keys.M:
                    mDown = true;
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            //TODO - basic player 1 key up bools set below. Add remainging key up
            // required for player 1 or player 2 here.

            //player 1 button releases
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
            }
        }

        /// <summary>
        /// This is the Game Engine and repeats on each interval of the timer. For example
        /// if the interval is set to 16 then it will run each 16ms or approx. 50 times
        /// per second
        /// </summary>
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //TODO move main character 
            #region player movements


            if (leftArrowDown == true)
            {
                heroX = heroX - heroSpeed;

                if (heroX < 0)
                {
                    heroX = 0;
                }
            }

            if (rightArrowDown == true)
            {
                heroX = heroX + heroSpeed;

                if (heroX > 50 - heroSize)
                {
                    heroX = 50 - heroSize;
                }
            }

            if (downArrowDown == true)
            {
                heroY = heroY + heroSpeed;

                if (heroY > this.Height - heroSize)
                {
                    heroY = this.Height - heroSize;
                }
            }

            if (upArrowDown == true)
            {
                heroY = heroY - heroSpeed;

                if (heroY < 0)
                {
                    heroY = 0;
                }
            }

            #endregion

            //TODO move npc characters
            for (int i = 0; i < monXList.Count; i++)
            {
                if (monXList[i] > 50)
                {
                    monXList[i] = monXList[i] - monSpeedList[i];
                }
                else
                {
                    gameTimer.Enabled = false;
                    Thread.Sleep(1000);

                    Font drawFont = new Font("Arial", 30, FontStyle.Bold);
                    SolidBrush drawBrush = new SolidBrush(Color.Black);
                    Graphics g = this.CreateGraphics();
                    g.DrawString("YOU LOSE", drawFont, drawBrush, this.Width / 4, this.Height / 4);
                    Thread.Sleep(4000);
                    MainForm.ChangeScreen(this, "MenuScreen");
                }
            }

            //TODO collisions checks 


            //calls the GameScreen_Paint method to draw the screen.
            Refresh();
        }


        //Everything that is to be drawn on the screen should be done here
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //draw rectangle to screen
            e.Graphics.FillRectangle(heroBrush, heroX, heroY, heroSize, heroSize);
            e.Graphics.DrawLine(linePen, 50, 0, 50, this.Height);

            for (int i = 0; i < monXList.Count; i++)
            {
                //if (monXList[i] > 50)
                //{
                    e.Graphics.FillRectangle(monBrush, monXList[i], monYList[i], monSizeList[i], monSizeList[i]);
                //}
            }
            
        }
    }

}
