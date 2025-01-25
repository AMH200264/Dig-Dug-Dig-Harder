using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Final_Project_CIS_297
{
    internal class LevelManager : IDrawable
    {
        Random random = new Random();

        public CanvasBitmap blockImage { get; set; }
        public CanvasBitmap damagedBlockImage { get; set; }

        public int currentLevel = 1;

        public int[,] dirtMap = { {0, 2, 2, 0, 0, 2, 0, 0, 0, 2},
                                  {0, 2, 0, 0, 0, 0, 0, 0, 0, 0},
                                  {2, 2, 0, 0, 0, 2, 0, 2, 2, 0},
                                  {0, 0, 0, 0, 2, 2, 0, 2, 0, 0},
                                  {0, 0, 0, 2, 0, 2, 0, 0, 0, 2},
                                  {2, 0, 2, 2, 0, 0, 0, 2, 0, 0},
                                  {2, 0, 0, 0, 0, 0, 0, 2, 0, 2}};


        public void levelChange(int level)
        {
            currentLevel = level;

            int blockChance;

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        dirtMap[i,j] = 0;
                        blockChance = random.Next(6);
                        if (blockChance < 3)
                            dirtMap[i,j] = 2;
                    }
                }
            }
        }

        public int getRandomEmpty(int row)
        {
            List<int> emptyPositions = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                if (dirtMap[row,i] == 0)
                {
                    emptyPositions.Add(i);
                }
            }

            if (emptyPositions.Count == 0)
                return -1;
            else
                return (emptyPositions[random.Next(emptyPositions.Count())]);
        }

        public int getRandomFilled(int row)
        {
            List<int> fullPositions = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                if (dirtMap[row, i] == 2)
                {
                    fullPositions.Add(i);
                }
            }

            if (fullPositions.Count == 0)
                return -1;
            else
                return (fullPositions[random.Next(fullPositions.Count())]);
        }

        public int MovementCheck(int x, int y, int direction, bool isPlayer)
        {

            

            // If the entity is eligible to move in the desired direction, return the movement distance
            // If the entity is the player and their path is blocked, weaken the dirt block
            if (direction == 0)
            {
                if (x > 0)
                    if (dirtMap[y, x - 1] == 0)
                        return 100;
                    else if (isPlayer == true)
                        dirtMap[y, x - 1]--;
            }
            else if (direction == 1)
            {
                if (x < 9)
                    if (dirtMap[y, x + 1] == 0)
                        return 100;
                    else if (isPlayer == true)
                        dirtMap[y, x + 1]--;
            }
            else if (direction == 2)
            {
                if (y > 0)
                    if (dirtMap[y - 1, x] == 0)
                        return 100;
                    else if (isPlayer == true)
                        dirtMap[y - 1, x]--;
            }
            else // direction > 2
            {
                if (y < 6)
                    if (dirtMap[y + 1, x] == 0)
                        return 100;
                    else if (isPlayer == true)
                        dirtMap[y + 1, x]--;
            }

            // if the entity cannot move, return 0
            return 0;
        }

        public void Draw(CanvasDrawingSession session)
        {

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (dirtMap[i,j] > 1)
                    {
                        if (blockImage != null)
                            session.DrawImage(blockImage, j*100, i*100);
                    }
                    else if (dirtMap[i,j] == 1)
                    {
                        if (damagedBlockImage != null)
                            session.DrawImage(damagedBlockImage, j*100, i*100);
                    }
                }
            }
        }

        public void DrawFog(CanvasDrawingSession session, int x, int y)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (Math.Abs(j - x) + Math.Abs (i - y) > 3)
                    {
                        session.FillRectangle(j * 100, i * 100, 100, 100, Colors.Black);
                    }
                }
            }
        }
    }
}
