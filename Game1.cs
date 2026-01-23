using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        Playing,
        End
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
        //Vector2 square1speed = new Vector2(-6, 0);
        Vector2 dog2speed = new Vector2(-6, 0);
        Vector2 knifespeed = new Vector2(-6, 0);
        Random generator;
        //List<Rectangle> square1rectangle;
        List<Rectangle> cat1rectangle;
        List<Rectangle> square2rectangle;
        List<Rectangle> dog2rectangle;
        List<Rectangle> kniferectangle;
        SoundEffect jumpSound;
        List<Texture2D> square2textures;
        Random random = new Random();
        SpriteFont scorefont;
        SpriteFont gamefont;
        SoundEffect gamesound;
        SoundEffectInstance gameSoundInstance;
        SoundEffect gameover;
        Texture2D runTexture;
        Texture2D endScreenTexture;
        int deathScore = 0;
        Rectangle sourceRect;  
        bool isJumping = false;

        Rectangle window;
        Texture2D bgtexture;
        GameState screen;
        Texture2D startscreentexture;
        //Texture2D square1texture;
        Texture2D dog2texture;
        Texture2D knifetexture;
        Texture2D square2texture;
        float respawntime;
        float seconds;
        float timer = 0f;
        int score = 0;       
        int highScore = 0;
        float scoreTimer = 0f;
        float difficultyTimer = 0f;
        Rectangle ground;
        KeyboardState keyboardState;
        float gravity = 0.3f;
        float jumpSpeed = 6.5f;
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
            dog2rectangle = new List<Rectangle>();
           
            kniferectangle = new List<Rectangle>();
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
           

            dog2texture = Content.Load<Texture2D>("Images/redsquare");


            square2texture = Content.Load<Texture2D>("Images/brownsquare");
            bgtexture = Content.Load<Texture2D>("Images/kitchentable");
           
            startscreentexture = Content.Load<Texture2D>("Images/catstartscreen");
            endScreenTexture = Content.Load<Texture2D>("Images/catendscreen");
            jumpSound = Content.Load<SoundEffect>("soundeffects/catmeow");
            gamesound = Content.Load<SoundEffect>("soundeffects/retrosound");
            knifetexture = Content.Load<Texture2D>("Images/knife");
            dog2texture = Content.Load<Texture2D>("Images/dog2");
            gameover = Content.Load<SoundEffect>("soundeffects/mariodeath");
            gameSoundInstance = gamesound.CreateInstance();
            gamefont = Content.Load<SpriteFont>("fonts/font1");





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
                    score = 0;
                    scoreTimer = 0f;
                    difficultyTimer = 0f;

                    dog2speed.X = -6;
                    knifespeed.X = -6;
                    seconds = 0f;
                    gameSoundInstance.Play();
                }

                
            }
            else if (screen == GameState.Playing)
            {
               

                scoreTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (scoreTimer >= 1f)
                {
                    score += 1;
                    scoreTimer = 0f;
                }
                difficultyTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (difficultyTimer >= 20f)
                {
                   
                    dog2speed.X -= 3;
                    knifespeed.X -= 3;

                  
                    respawntime -= 0.25f;

                    difficultyTimer = 0f;
                }
               
                if (seconds >= respawntime)
                {

                    if (seconds >= respawntime)
                    {
                        int roll = generator.Next(0, 2);

                        if (roll == 0)
                        {
                            dog2rectangle.Add(new Rectangle(
                                generator.Next(825, 850),
                                275,
                                40,
                                40
                            ));
                        }
                        else
                        {
                            kniferectangle.Add(new Rectangle(
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
                for (int i = 0; i < dog2rectangle.Count; i++)
                {
                    temp = dog2rectangle[i];
                    temp.X += (int)dog2speed.X;
                    dog2rectangle[i] = temp;
                }
                for (int i = 0; i < kniferectangle.Count; i++)
                {
                    temp = kniferectangle[i];
                    temp.X += (int)knifespeed.X;
                    kniferectangle[i] = temp;
                }

                speed.Y += gravity;



                if (keyboardState.IsKeyDown(Keys.Space) && onGround)
                {
                    speed.Y = -jumpSpeed;
                    onGround = false;
                    jumpSound.Play();

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
                for (int i = 0; i < dog2rectangle.Count; i++)
                {
                    if (player.Intersects(dog2rectangle[i]))
                    {
                        deathScore = score;
                        gameover.Play();
                        gameSoundInstance.Stop();



                        screen = GameState.End;
                    }
                }

                
                for (int i = 0; i < kniferectangle.Count; i++)
                {
                    if (player.Intersects(kniferectangle[i]))
                    {
                        if (player.Intersects(kniferectangle[i]))
                        {
                            deathScore = score;      
                            gameover.Play();
                            screen = GameState.End;
                            gameSoundInstance.Stop();

                        }
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
                   gamefont,
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
                _spriteBatch.DrawString(
                 gamefont,
                            "Score: " + score,
                        new Vector2(20, 20),
                     Color.Black
                            );

               


                _spriteBatch.Draw(runTexture, player, Color.White);
                for (int i = 0; i < dog2rectangle.Count; i++)
                {
                    _spriteBatch.Draw(dog2texture, dog2rectangle[i], Color.White);
                }
                for (int i = 0; i < kniferectangle.Count; i++)
                {
                    _spriteBatch.Draw(knifetexture, kniferectangle[i], Color.White);
                }



                _spriteBatch.Draw(rectangleTexture, ground, Color.SandyBrown);
            }
            else if (screen == GameState.End)
            {
                _spriteBatch.Draw(
                    endScreenTexture,
                    new Rectangle(0, 0, 800, 480),
                    Color.White
                );
                _spriteBatch.DrawString(
                   gamefont,
                    "YOUR SCORE: " + deathScore,
                    new Vector2(300, 250),
                    Color.Black
                );
                
                
               
            }





            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
