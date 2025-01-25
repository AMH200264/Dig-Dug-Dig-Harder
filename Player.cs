using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.PointOfService;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Final_Project_CIS_297
{
    internal class Player : IDrawable, ICollidable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public enum Direction { Left, Right, Up, Down }

        public Direction direction { get; set; }

        public List<CanvasBitmap> playerSprites { get; set; }

        public int Lives {  get; set; }

        public Player(int x, int y, int lives)
        {
            X = x;
            Y = y;
            playerSprites = new List<CanvasBitmap>();
            Lives = lives;
        }

        private async void playerDeath()
        {
            if (Lives > 0)
            {
                X = 0;
                Y = 0;
                --Lives;
            }
            else
            {
                MediaPlayerManager.StopMedia();
                //UMGPT: The application called an interface that was marshalled for a different thread, what does this error mean in c#
                var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    Frame frame = Window.Current.Content as Frame;
                    if (frame != null)
                    {
                        frame.Navigate(typeof(BlankPage1));
                    }
                    else
                    {
                        throw new InvalidOperationException("Unable to navigate to main menu, Frame is null.");
                    }
                });
            }
        }

        public bool DidCollide(ICollidable other)
        {
            if (this.GetGridPosition().SequenceEqual(other.GetGridPosition()))
            {
                if (other is Enemy)
                {
                    playerDeath();
                }
                return true;
            }
            return false;
        }

        public void Draw(CanvasDrawingSession session)
        {
            if (direction == Direction.Down)
            {
                session.DrawImage(playerSprites[0], X, Y);
            }
            else if (direction == Direction.Up) 
            {
                session.DrawImage(playerSprites[1], X, Y);
            }
            else if (direction == Direction.Right)
            {
                session.DrawImage(playerSprites[2], X, Y);
            }
            else if (direction == Direction.Left)
            {
                session.DrawImage(playerSprites[3], X, Y);
            }

        }

        public List<double> GetGridPosition()
        {
            List<double> gridPos = new List<double>();

            gridPos.Add(X / 100.0);
            gridPos.Add(Y / 100.0);

            return gridPos;
        }
    }
}
