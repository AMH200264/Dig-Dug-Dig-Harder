using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Final_Project_CIS_297
{
    internal class Powerup : IDrawable, ICollidable
    {
        //Class elements
        public int speed { get; set; }
        public bool checkPowerUp { get; set; }
        public bool checkCollision { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        //Constructor for the powerup class
        public Powerup(int x, int y)
        {
            X = x;
            Y = y;
        }

        //Speed powerup method, returns updated speed if the player encounters the powerup
        public int speedPowerUp (int speed, bool collisionCheck)
        {
            if (collisionCheck == false)
            {
                return speed;
            }
            else
            {
                return speed + 10; //or whatever value makes sense, 10 is just a placeholder lol
            }
        }
        //Freeze powerup method, returns updated speed for the enemy if the player encounters a powerup
        public int freezePowerUp(int speed, bool collisioncheck)
        {
            if (collisioncheck == false)
            {
                return speed;
            }
            else
            {
                speed = 0;
                return speed;
            }
        }
        //Powerup to kill random enemy, kills a random enemy from the enemy
        //list and returns the index from the list
        public int killRandomPowerUp(List<Enemy> enemies,  bool collisionCheck)
        {
            int enemyCount = 0;
            foreach(Enemy enemy in enemies)
            {
                enemyCount++;
            }
            Random rand = new Random();
            int listIndex = rand.Next(0, enemyCount); 

            if (collisionCheck == false)
            {
                return 0;
            }
            else
            {
                return listIndex;
            }
        }

        public void Draw(CanvasDrawingSession session)
        {
            throw new NotImplementedException();
        }

        public List<double> GetGridPosition()
        {
            throw new NotImplementedException();
        }

        public bool DidCollide(ICollidable other)
        {
            throw new NotImplementedException();
        }
    }
}
