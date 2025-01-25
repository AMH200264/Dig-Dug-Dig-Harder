using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Final_Project_CIS_297
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private CanvasBitmap dirtImage, damagedDirtImage;

        private List<CanvasBitmap> playerSprites;
        private List<CanvasBitmap> enemySprites;
        private List<CanvasBitmap> angrySprites;

        LevelManager levelManager;
        bool fog;
        Player player;
        Enemy enemy1;
        Enemy enemy2;
        Enemy enemy3;
        Enemy enemy4;
        Powerup powerup1;
        private TimeSpan lastUpdateTime = TimeSpan.Zero;
        private double enemyDelay = 0.5;
        Random random = new Random();
        Frame rootframe = new Frame();
        public int score = 0;
        CoreDispatcher dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;

        int row;

        List<int> XorY = new List<int>();

        private GamepadButtons previousButtons = GamepadButtons.None;

        public int boxSize; //the size of the boxes on the grid

        List<IDrawable> drawables;
        List<ICollidable> collidables;

        List<Enemy> enemies;
        public GamePage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += Canvas_KeyDown;

            PlayMusic();

            drawables = new List<IDrawable>();
            collidables = new List<ICollidable>();
            enemies = new List<Enemy>();

            levelManager = new LevelManager();
            fog = false;

            player = new Player(0, 0, 3);

            enemy1 = new Enemy(300, 300, 2);
            enemy2 = new Enemy(700, 100, 2);
            enemy3 = new Enemy(100, 500, 2);
            enemy4 = new Enemy(500, 600, 2);

            powerup1 = new Powerup(200, 200);

            boxSize = 100;

            drawables.Add(levelManager);
            drawables.Add(player);
            drawables.Add(enemy1);
            drawables.Add(enemy2);
            drawables.Add(enemy3);
            drawables.Add(enemy4);

            collidables.Add(enemy1);
            collidables.Add(enemy2);
            collidables.Add(enemy3);
            collidables.Add(enemy4);
            collidables.Add(player);

            enemies.Add(enemy1);
            enemies.Add(enemy2);
            enemies.Add(enemy3);
            enemies.Add(enemy4);

            playerSprites = new List<CanvasBitmap>();
            enemySprites = new List<CanvasBitmap>();
            angrySprites = new List<CanvasBitmap>();

            rootframe = Window.Current.Content as Frame;
            ScoreBlock.Text = $"Score: {score}";
        }

        private async void PlayMusic()
        {
            var musicFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///digdugsong.mp3"));
            MediaPlayerManager.PlayMedia(musicFile);
        }


        /*UMGPT Prompt: Hello, I'm designing a game using c# in visual studio, The game is played on a grid,
            meaning the player moves on a grid using the movement logic below: ...
            How do I ensure that the player stays within the bounds of the screen without using fixed labels
        as the screen may be subject to change?*/
        private void Canvas_KeyDown(CoreWindow sender, KeyEventArgs args)
        {

            //Convert the player's X and Y coordinates to the X and Y coordinates of the dirtMap matrix
            int playerGridX = player.X / 100;
            int playerGridY = player.Y / 100;

            int newX = player.X;
            int newY = player.Y;

            //Check to see if the player is not on any of the screens edges, then if the location they wish to move to is not occupied
            //If both conditions are satisfied, the player is moved to the position they wished to move to
            if (args.VirtualKey == Windows.System.VirtualKey.Left)
            {
                newX -= levelManager.MovementCheck(playerGridX, playerGridY, 0, true);
                player.direction = Player.Direction.Left;
            }
            else if (args.VirtualKey == Windows.System.VirtualKey.Right)
            {
                newX += levelManager.MovementCheck(playerGridX, playerGridY, 1, true);
                player.direction = Player.Direction.Right;
            }
            else if (args.VirtualKey == Windows.System.VirtualKey.Up)
            {
                newY -= levelManager.MovementCheck(playerGridX, playerGridY, 2, true);
                player.direction = Player.Direction.Up;
            }
            else if (args.VirtualKey == Windows.System.VirtualKey.Down)
            {
                newY += levelManager.MovementCheck(playerGridX, playerGridY, 3, true);
                player.direction = Player.Direction.Down;
            }

            if (args.VirtualKey == Windows.System.VirtualKey.Space)
            {
                attackEnemy();
            }

            if (args.VirtualKey == Windows.System.VirtualKey.M)
            {
                MediaPlayerManager.StopMedia();
                rootframe.Navigate(typeof(BlankPage1));
            }

            // Update player position
            player.X = newX;
            player.Y = newY;

            if (args.VirtualKey == Windows.System.VirtualKey.F)
                fog = !fog;
        }

        private void attackEnemy()
        {
            List<double> spaceinFront = new List<double>();
            List<double> spaceinFront2 = new List<double>();

            if (player.direction == Player.Direction.Left)
            {
                spaceinFront.Add((player.X / 100.0) - 1);
                spaceinFront.Add(player.Y / 100.0);

                spaceinFront2.Add(spaceinFront[0] - 1);
                spaceinFront2.Add(spaceinFront[1]);

                checkHit(spaceinFront, spaceinFront2);
            }
            else if (player.direction == Player.Direction.Right)
            {
                spaceinFront.Add((player.X / 100.0) + 1);
                spaceinFront.Add(player.Y / 100.0);

                spaceinFront2.Add(spaceinFront[0] + 1);
                spaceinFront2.Add(spaceinFront[1]);


                checkHit(spaceinFront, spaceinFront2);
            }
            else if (player.direction == Player.Direction.Up)
            {
                spaceinFront.Add(player.X / 100.0);
                spaceinFront.Add((player.Y / 100.0) - 1);

                spaceinFront2.Add(spaceinFront[0]);
                spaceinFront2.Add(spaceinFront[1] - 1);

                checkHit(spaceinFront, spaceinFront2);
            }
            else if (player.direction == Player.Direction.Down)
            {
                spaceinFront.Add(player.X / 100.0);
                spaceinFront.Add((player.Y / 100.0) + 1);

                spaceinFront2.Add(spaceinFront[0]);
                spaceinFront2.Add(spaceinFront[1] + 1);

                checkHit(spaceinFront, spaceinFront2);
            }
        }

        private async void checkHit(List<double> spaceinFront, List<double> spaceinFront2)
        {
            foreach (var enemy in enemies.ToList())
            {
                if (enemy.GetGridPosition().SequenceEqual(spaceinFront) || enemy.GetGridPosition().SequenceEqual(spaceinFront2))
                {
                    enemy.DamageEnemy();
                    score += 50;
        
                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        ScoreBlock.Text = $"Score: {score}";
                    });
                }
            }
        }

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            foreach (var drawable in drawables)
            {
                drawable.Draw(args.DrawingSession);
            }

            if (fog)
                levelManager.DrawFog(args.DrawingSession, player.X / 100, player.Y / 100);
        }

        private async void Canvas_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            TimeSpan totalTime = args.Timing.TotalTime;
            int playerGridX = player.X / 100;
            int playerGridY = player.Y / 100;
            int nextX = player.X;
            int nextY = player.Y;

            var windowBounds = sender.Size;
            int maxX = (int)windowBounds.Width - 95;
            int maxY = (int)windowBounds.Height - 95;


            if (Gamepad.Gamepads.Count > 0)
            {
                Gamepad controller = Gamepad.Gamepads.First();
                var reading = controller.GetCurrentReading();

                //These two lines are from UMGPT, Prompt: In the code above I am attempting to read a
                //gamepad to update player actions in a c# game in visual studio. How do I get the actions
                //to only take place once per press rather than every frame?
                var buttonsChanged = reading.Buttons ^ previousButtons; // XOR to find changed buttons
                var buttonsPressed = buttonsChanged & reading.Buttons; // AND to find currently pressed buttons

                if (buttonsPressed.HasFlag(GamepadButtons.DPadLeft))
                {
                    nextX -= levelManager.MovementCheck(playerGridX, playerGridY, 0, true);
                    player.direction = Player.Direction.Left;
                }
                else if (buttonsPressed.HasFlag(GamepadButtons.DPadRight))
                {
                    nextX += levelManager.MovementCheck(playerGridX, playerGridY, 1, true);
                    player.direction = Player.Direction.Right;
                }
                else if (buttonsPressed.HasFlag(GamepadButtons.DPadUp))
                {
                    nextY -= levelManager.MovementCheck(playerGridX, playerGridY, 2, true);
                    player.direction = Player.Direction.Up;
                }
                else if (buttonsPressed.HasFlag(GamepadButtons.DPadDown))
                {
                    nextY += levelManager.MovementCheck(playerGridX, playerGridY, 3, true);
                    player.direction = Player.Direction.Down;
                }

                if (buttonsPressed.HasFlag(GamepadButtons.A))
                {
                    attackEnemy();
                }

                if (buttonsPressed.HasFlag(GamepadButtons.Menu))
                {
                    MediaPlayerManager.StopMedia();
                    //UMGPT: The application called an interface that was marshalled for a different thread, what does this error mean in c#
                    await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        rootframe.Navigate(typeof(BlankPage1));
                    });
                }

                if (buttonsPressed.HasFlag(GamepadButtons.LeftShoulder))
                {
                    fog = !fog;
                }

                player.X = nextX;
                player.Y = nextY;

                previousButtons = reading.Buttons;
            }

            foreach (var collision in collidables)
            {
                for (int i = 0; i < collidables.Count; ++i)
                {
                    collision.DidCollide(collidables[i]);
                }        
            }

            //UMGPT: Now I am attempting to have my enemies move randomly, but only once a second,
            //how do i tell my code to wait a second per each move?
            if (totalTime - lastUpdateTime >= TimeSpan.FromSeconds(enemyDelay)) 
            {
                foreach (var enemy in enemies)
                {
                    int newX = enemy.X;
                    int newY = enemy.Y;
                    int direction = random.Next(0, 4);
                    

                    if (enemy.Angry)
                    {
                        if (player.GetGridPosition()[0] < enemy.GetGridPosition()[0] &&
                            player.GetGridPosition()[1] < enemy.GetGridPosition()[1])
                        {
                            XorY.Add(0);
                            XorY.Add(2);
                            direction = XorY[random.Next(0, 2)];
                        }
                        else if (player.GetGridPosition()[0] > enemy.GetGridPosition()[0] &&
                                 player.GetGridPosition()[1] > enemy.GetGridPosition()[1])
                        {
                            XorY.Add(1);
                            XorY.Add(3);
                            direction = XorY[random.Next(0, 2)];
                        }
                        else if (player.GetGridPosition()[0] < enemy.GetGridPosition()[0] &&
                                 player.GetGridPosition()[1] > enemy.GetGridPosition()[1])
                        {
                            XorY.Add(0);
                            XorY.Add(3);
                            direction = XorY[random.Next(0, 2)];
                        }
                        else if (player.GetGridPosition()[0] > enemy.GetGridPosition()[0] &&
                                 player.GetGridPosition()[1] < enemy.GetGridPosition()[1])
                        {
                            XorY.Add(1);
                            XorY.Add(2);
                            direction = XorY[random.Next(0, 2)];
                        }
                        else if (player.GetGridPosition()[0] < enemy.GetGridPosition()[0])
                        {
                            direction = 0;
                        }
                        else if (player.GetGridPosition()[0] > enemy.GetGridPosition()[0])
                        {
                            direction = 1;
                        }
                        else if (player.GetGridPosition()[1] < enemy.GetGridPosition()[1])
                        {
                            direction = 2;
                        }
                        else if (player.GetGridPosition()[1] > enemy.GetGridPosition()[1])
                        {
                            direction = 3;
                        }

                        XorY.Clear();
                    }

                    if (direction == 0)
                    {
                        newX -= levelManager.MovementCheck(enemy.X / 100, enemy.Y / 100, direction, false);
                        enemy.direction = Enemy.Direction.Left;
                    }
                    else if (direction == 1)
                    {
                        newX += levelManager.MovementCheck(enemy.X / 100, enemy.Y / 100, direction, false);
                        enemy.direction = Enemy.Direction.Right;
                    }
                    else if (direction == 2)
                    {
                        newY -= levelManager.MovementCheck(enemy.X / 100, enemy.Y / 100, direction, false);
                        enemy.direction = Enemy.Direction.Up;
                                              
                    }
                    else if (direction == 3)
                    {
                        newY += levelManager.MovementCheck(enemy.X / 100, enemy.Y / 100, direction, false);
                        enemy.direction = Enemy.Direction.Down;
                    }

                    // Update enemy position if within bounds
                    enemy.X = newX;
                    enemy.Y = newY;
                }
                lastUpdateTime = totalTime;

                foreach (var enemy in enemies.ToList())
                {
                    if (enemy.Angry)
                    {
                        enemy.enemySprites = angrySprites;
                    }
                    if (enemy.Dead)
                    {
                        enemies.Remove(enemy);
                        drawables.Remove(enemy);
                        collidables.Remove(enemy);
                    }
                }

                if (enemies.Count == 0)
                {
                    player.X = 0;
                    player.Y = 0;

                    levelManager.levelChange(levelManager.currentLevel + 1);

                    RandomEnemyPos(enemy1);
                    RandomEnemyPos(enemy2);
                    RandomEnemyPos(enemy3);
                    RandomEnemyPos(enemy4);
                }
            }  
        }

        private void RandomEnemyPos(Enemy enemy)
        {
            row = random.Next(0, 7);
            enemy.Y = row * 100;
            enemy.X = levelManager.getRandomEmpty(row) * 100;
            while (enemy.X < 0 || (enemy.X == 0 && enemy.Y == 0) || (enemy.X == 0 && enemy.Y == 100)
                || (enemy.X == 100 && enemy.Y == 0) || (enemy.X == 100 && enemy.Y == 100))
            {
                row = random.Next(0, 7);
                enemy.Y = row * 100;
                enemy.X = levelManager.getRandomEmpty(row) * 100;
            }

            enemies.Add(enemy);
            drawables.Add(enemy);
            collidables.Add(enemy);
            enemy.Health = 2;
            enemy.Dead = false;
            enemy.Angry = false;
            enemy.enemySprites = enemySprites;
        }

        private void Canvas_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(CreateResources(sender).AsAsyncAction());
        }

        async Task CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl sender)
        {
            dirtImage = await CanvasBitmap.LoadAsync(sender, "Assets/RockBlock.png");
            damagedDirtImage = await CanvasBitmap.LoadAsync(sender, "Assets/DamagedRockBlock.png");
            levelManager.blockImage = dirtImage;
            levelManager.damagedBlockImage = damagedDirtImage;

            playerSprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/playersprite_1.png"));
            playerSprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/playersprite_2.png"));
            playerSprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/playersprite_3.png"));
            playerSprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/playersprite_4.png"));

            enemySprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/enemysprite_1.png"));
            enemySprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/enemysprite_2.png"));
            enemySprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/enemysprite_3.png"));
            enemySprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/enemysprite_4.png"));

            angrySprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/angrysprite_1.png"));
            angrySprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/angrysprite_2.png"));
            angrySprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/angrysprite_3.png"));
            angrySprites.Add(await CanvasBitmap.LoadAsync(sender, "Assets/angrysprite_4.png"));

            player.playerSprites = playerSprites;
            enemy1.enemySprites = enemySprites;
            enemy2.enemySprites = enemySprites;
            enemy3.enemySprites = enemySprites;
            enemy4.enemySprites = enemySprites;
        }


        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
