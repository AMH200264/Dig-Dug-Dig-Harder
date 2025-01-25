using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Final_Project_CIS_297
{
    internal class Enemy : IDrawable, ICollidable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public List<CanvasBitmap> enemySprites { get; set; }

        public enum Direction { Left, Right, Up, Down }

        public Direction direction { get; set; }

        public int Health { get; set; }

        public bool Dead { get; set; }

        public bool Angry { get; set; }

        public Enemy(int x, int y, int health)
        {
            X = x;
            Y = y;
            enemySprites = new List<CanvasBitmap>();
            Health = health;
            Dead = false;
            Angry = false;
        }

        public bool DidCollide(ICollidable other)
        {
            if (this.GetGridPosition() == other.GetGridPosition())
            {
                return true;

            }
            return false;
        }

        public void Draw(CanvasDrawingSession session)
        {
            if (direction == Direction.Down) 
            {
                session.DrawImage(enemySprites[0], X, Y);
            }
            else if (direction == Direction.Up)
            {
                session.DrawImage(enemySprites[1], X, Y);
            }
            else if (direction == Direction.Right)
            {
                session.DrawImage(enemySprites[2], X, Y);
            }
            else if (direction == Direction.Left)
            {
                session.DrawImage(enemySprites[3], X, Y);
            }
        }

        public List<double> GetGridPosition()
        {
            List<double> gridPos = new List<double>();
            gridPos.Add(X / 100.0);
            gridPos.Add(Y / 100.0);

            return gridPos;
        }

        internal void DamageEnemy()
        {
            if (Health > 0)
            {
                --Health;
            }
            else
            {
                Dead = true;
            }
            if (Health == 0)
            {
                Angry = true;
            }
            
        }
    }
}
