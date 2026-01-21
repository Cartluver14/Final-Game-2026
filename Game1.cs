using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Final_Game_2026
{
    enum GameState
    {
        Start,
        Playing
    }

    
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D rectangleTexture;
        GameState currentState = GameState.Start;


        Rectangle player;
        Vector2 playerPosition;
        Vector2 speed;
        Vector2 square1speed = new Vector2(-6, 0);
        Vector2 square2speed = new Vector2(-6, 0);
        Random generator;
        List<Rectangle> square1rectangle;
        List<Rectangle> cat1rectangle;
        List<Rectangle> square2rectangle;
       
        List<Texture2D> square2textures;
        Random random = new Random();
        SpriteFont scorefont;
        // CAT ANIMATION TEXTURES
        Texture2D runTexture;
        Texture2D jumpTexture;
        Texture2D duckTexture;

        // FRAME INFO
        int currentFrame = 0;
        int frameWidth = 50;
        int frameHeight = 50;

        float animationTimer = 0f;
        float animationSpeed = 0.12f; // lower = faster

        Rectangle sourceRect;

        // STATES
        bool isJumping = false;

        Rectangle window;
        Texture2D bgtexture;
        GameState screen;
        Texture2D startscreentexture;
        Texture2D square1texture;
        Texture2D square2texture;
        float respawntime;
        float seconds;
        float timer = 0f;
        int score = 0;



       




        Rectangle ground;



        KeyboardState keyboardState;

        float gravity = 0.3f;
        float jumpSpeed = 7f;
        bool onGround = false;
        int standHeight = 50;
        int duckHeight = 25;
        bool isDucking = false;

        public Game1()
        {
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screen = GameState.Start;
            generator = new Random();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            square1rectangle = new List<Rectangle>();
           
            square2rectangle = new List<Rectangle>();
            square2textures = new List<Texture2D>();

            respawntime = 2f;
            speed = Vector2.Zero;

            playerPosition = new Vector2(10, 10);
            player = new Rectangle(10, 10, 50, 50);


            
            ground = new Rectangle(0, 310, 800, 20);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rectangleTexture = Content.Load<Texture2D>("images/brick");
            runTexture = Content.Load<Texture2D>("Images/catrun1");
           

            square1texture = Content.Load<Texture2D>("Images/redsquare");


            square2texture = Content.Load<Texture2D>("Images/brownsquare");
            bgtexture = Content.Load<Texture2D>("Images/kitchentable");
            scorefont = Content.Load<SpriteFont>("fonts/scorefont");
            startscreentexture = Content.Load<Texture2D>("Images/catstartscreen");




            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

            keyboardState = Keyboard.GetState();
            if (screen == GameState.Start)
            {
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    screen = GameState.Playing;
                    seconds = 0f;
                }

                
            }
            else if (screen == GameState.Playing)
            {
                // Game playing logic
                if (seconds >= respawntime)
                {

                    if (seconds >= respawntime)
                    {
                        int roll = generator.Next(0, 2);

                        if (roll == 0)
                        {
                            square1rectangle.Add(new Rectangle(
                                generator.Next(825, 850),
                                275,
                                40,
                                40
                            ));
                        }
                        else
                        {
                            square2rectangle.Add(new Rectangle(
                                generator.Next(825, 850),
                                235,
                                35,
                                35
                            ));
                        }

                        seconds = 0f;
                    }
                    seconds = 0f;



                }
                Rectangle temp;
                for (int i = 0; i < square1rectangle.Count; i++)
                {
                    temp = square1rectangle[i];
                    temp.X += (int)square1speed.X;
                    square1rectangle[i] = temp;
                }
                for (int i = 0; i < square2rectangle.Count; i++)
                {
                    temp = square2rectangle[i];
                    temp.X += (int)square2speed.X;
                    square2rectangle[i] = temp;
                }

                speed.Y += gravity;



                if (keyboardState.IsKeyDown(Keys.Space) && onGround)
                {
                    speed.Y = -jumpSpeed;
                    onGround = false;
                }

                playerPosition.Y += speed.Y;
                player.Location = playerPosition.ToPoint();
                onGround = false;

                if (player.Intersects(ground))
                {

                    if (speed.Y > 0)
                    {
                        onGround = true;
                        speed.Y = 0;
                        playerPosition.Y = ground.Y - player.Height;
                        player.Location = playerPosition.ToPoint();

                    }
                    if (keyboardState.IsKeyDown(Keys.S) && onGround)
                    {
                        if (!isDucking)
                        {
                            isDucking = true;
                            player.Height = duckHeight;
                            playerPosition.Y = ground.Y - player.Height;
                        }
                    }
                    else
                    {
                        if (isDucking)
                        {
                            isDucking = false;
                            player.Height = standHeight;
                            playerPosition.Y = ground.Y - player.Height;
                        }
                    }

                    player.Location = playerPosition.ToPoint();

                }
                for (int i = 0; i < square1rectangle.Count; i++)
                {
                    if (player.Intersects(square1rectangle[i]))
                    {
                        Exit();
                    }
                }

                // collision with square2 obstacles
                for (int i = 0; i < square2rectangle.Count; i++)
                {
                    if (player.Intersects(square2rectangle[i]))
                    {
                        Exit();
                    }
                }
            }
            

            




            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            if (screen == GameState.Start)
            {
                _spriteBatch.Draw(
                    startscreentexture,
                    new Rectangle(0, 0, 800, 480),
                    Color.White
                );

                _spriteBatch.DrawString(
                   scorefont,
                    "PRESS ENTER TO START",
                    new Vector2(300, 300),
                    Color.Black
                );
            }

            else if (screen == GameState.Playing)
            {
                _spriteBatch.Draw(
                    bgtexture,
                    new Rectangle(0, 0, 800, 480),
                    Color.White
                );


                _spriteBatch.Draw(runTexture, player, Color.White);
                for (int i = 0; i < square1rectangle.Count; i++)
                {
                    _spriteBatch.Draw(square1texture, square1rectangle[i], Color.White);
                }
                for (int i = 0; i < square2rectangle.Count; i++)
                {
                    _spriteBatch.Draw(square2texture, square2rectangle[i], Color.White);
                }



                _spriteBatch.Draw(rectangleTexture, ground, Color.SandyBrown);
            }


                
            

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
