﻿using System;
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
using System.Media;

namespace testgame
{
    public partial class GameScreen : UserControl
    {
        //player1 button control keys - DO NOT CHANGE
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown, bDown, nDown, mDown, spaceDown;

        //player2 button control keys - DO NOT CHANGE
        Boolean aDown, sDown, dDown, wDown, cDown, vDown, xDown, zDown;

        //TODO create your global game variables here
        int heroX, heroY, heroSize, heroSpeed, monX, monY, monSize;
        SolidBrush heroBrush = new SolidBrush(Color.Black);
        Pen linePen = new Pen(Color.Black);
        SolidBrush bulletBrush = new SolidBrush(Color.Black);
        SolidBrush scoreBrush = new SolidBrush(Color.Black);
        Font drawFont = new Font("Arial", 16, FontStyle.Bold);
        Font scoreFont = new Font("Arial", 8, FontStyle.Bold);          

        SolidBrush monBrush = new SolidBrush(Color.Black);
        List<Rectangle> monRectList = new List<Rectangle>();
        List<Rectangle> bulletList = new List<Rectangle>();

        int bulletSpeed = 10;
        int monSpeed = 1;
        int shootCool = 0;
        int startTimer = 0;
        int level = 1;
        int xValue;
        int yValue;
        int score = 0;

        Random randGen = new Random();
        SoundPlayer player = new SoundPlayer(Properties.Resources.bullet);


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
            heroSpeed = 6;

            xValue = randGen.Next(400, 550);
            yValue = randGen.Next(0, this.Height - 15);
            Rectangle R = new Rectangle(xValue, yValue, 15, 15);
            monRectList.Add(R);

            xValue = randGen.Next(400, 550);
            yValue = randGen.Next(0, this.Height - 15);
            Rectangle R1 = new Rectangle(xValue, yValue, 15, 15);
            monRectList.Add(R1);

            xValue = randGen.Next(400, 550);
            yValue = randGen.Next(0, this.Height - 15);
            Rectangle R2 = new Rectangle(xValue, yValue, 15, 15);
            monRectList.Add(R2);
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
                case Keys.Space:
                    spaceDown = false;
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
            if (startTimer > 50)
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

                if (spaceDown == true)
                {
                    if (shootCool == 0)
                    {
                        Rectangle p = new Rectangle(heroX, heroY, 15, 10);
                        bulletList.Add(p);
                        shootCool = 10;
                        player.Play();
                    }

                }


                #endregion

                if (shootCool > 0)
                {
                    shootCool--;
                }

                for (int i = 0; i < bulletList.Count; i++)
                {
                    Rectangle q = new Rectangle(bulletList[i].X + bulletSpeed, bulletList[i].Y, bulletList[i].Width, bulletList[i].Height);
                    bulletList[i] = q;
                }

                //TODO move npc characters
                for (int i = 0; i < monRectList.Count; i++)
                {
                    if (monRectList[i].X > 50)
                    {
                        Rectangle r = new Rectangle(monRectList[i].X - monSpeed, monRectList[i].Y, monRectList[i].Width, monRectList[i].Height);
                        monRectList[i] = r;
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

                bool monRemoved = false;
                bool bulletRemoved = false;

                for (int i = 0; i < monRectList.Count(); i++)
                { 
                    for (int j = 0; j < bulletList.Count; j++)
                    {
                        if (monRectList[i].IntersectsWith(bulletList[j]))
                        {
                            monRectList.Remove(monRectList[i]);
                            monRemoved = true;
                            bulletList.Remove(bulletList[j]);
                            bulletRemoved = true;
                            score++;
                            break;
                        }
                    }
                }


                //calls the GameScreen_Paint method to draw the screen.
                if (monRectList.Count < 1)
                {
                    level++;
                    startTimer = 0;
                    //create some more monsters
                    xValue = randGen.Next(400, 500);
                    yValue = randGen.Next(0, this.Height - 15);
                    Rectangle R = new Rectangle (xValue , yValue, 15, 15);
                    monRectList.Add(R);

                    xValue = randGen.Next(400, 500);
                    yValue = randGen.Next(0, this.Height - 15);
                    Rectangle R1 = new Rectangle(xValue, yValue, 15, 15);
                    monRectList.Add(R1);

                    xValue = randGen.Next(400, 500);
                    yValue = randGen.Next(0, this.Height - 15);
                    Rectangle R2 = new Rectangle(xValue, yValue, 15, 15);
                    monRectList.Add(R2);

                    monSpeed = monSpeed + 1;
                }
            }
            else
            {
                startTimer++;
            }
            Refresh();
        }

        //Everything that is to be drawn on the screen should be done here
        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            if (startTimer > 50)
            {
                //draw rectangle to screen
                e.Graphics.DrawImage(Properties.Resources.fortnite_default, heroX, heroY, heroSize, heroSize);
                e.Graphics.DrawLine(linePen, 50, 0, 50, this.Height);
                e.Graphics.DrawString("Score: " + score, scoreFont, scoreBrush, 1, 10);

                foreach (Rectangle r in bulletList)
                {
                    e.Graphics.FillRectangle(monBrush, r.X, r.Y, r.Width, r.Height);
                }

                for (int i = 0; i < monRectList.Count; i++)
                {
                    e.Graphics.DrawImage(Properties.Resources.shrek_monsters, monRectList[i].X, monRectList[i].Y, monRectList[i].Width, monRectList[i].Height);
                }
            }
            else
            {
                e.Graphics.DrawImage(Properties.Resources.fortnite_default, heroX, heroY, heroSize, heroSpeed); 
                e.Graphics.DrawLine(linePen, 50, 0, 50, this.Height);
                e.Graphics.DrawString("Level : " + level, drawFont, heroBrush, this.Width / 2 - 45, this.Height / 2 - 30);
                e.Graphics.DrawString("Score: " + score, scoreFont, scoreBrush, 1, 10);
            }

            if (startTimer < 50 && level == 1)
            {
                e.Graphics.DrawString("Arrow Keys : Move" +
                    "\nSpace Bar : Shoot", drawFont, heroBrush, this.Width / 2 - 65, this.Height / 2 + 40);
            }
        }
    }


}

