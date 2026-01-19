using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Final_Game_2026
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D rectangleTexture;


        Rectangle player;
        Vector2 playerPosition;
        Vector2 speed;
        Vector2 square1speed = new Vector2(-4, 0);
        Vector2 square2speed = new Vector2(-4, 0);
        Random generator;
        List<Rectangle> square1rectangle;
        List<Rectangle> square2rectangle;
        List<Texture2D> square1textures;
        List<Texture2D> square2textures;
        Random random = new Random();
        Rectangle window;
        Texture2D square1texture;
        Texture2D square2texture;
        float respawntime;
        float seconds;




        Rectangle ground;



        KeyboardState keyboardState;

        float gravity = 0.3f;
        float jumpSpeed = 8f;
        bool onGround = false;  

        public Game1()
        {
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            generator = new Random();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            square1rectangle = new List<Rectangle>();
            square1textures = new List<Texture2D>();
            square2rectangle = new List<Rectangle>();
            square2textures = new List<Texture2D>();

            respawntime = 2f;
            speed = Vector2.Zero;

            playerPosition = new Vector2(10, 10);
            player = new Rectangle(10, 10, 50, 50);


            
            ground = new Rectangle(0, 400, 800, 20);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rectangleTexture = Content.Load<Texture2D>("images/bluesquare");
            square1texture = Content.Load<Texture2D>("Images/redsquare");
            square2texture = Content.Load<Texture2D>("Images/brownsquare");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            seconds += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (seconds >= respawntime)
            {

                if (seconds >= respawntime)
                {
                    int roll = generator.Next(0, 2);

                    if (roll == 0)
                    {
                        square1rectangle.Add(new Rectangle(
                            generator.Next(825, 850),
                            370,
                            40,
                            40
                        ));
                    }
                    else
                    {
                        square2rectangle.Add(new Rectangle(
                            generator.Next(825, 850),
                            270,
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




            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

     
            _spriteBatch.Draw(rectangleTexture, player, Color.Red);
            for (int i = 0; i < square1rectangle.Count; i++)
            {
                _spriteBatch.Draw(square1texture, square1rectangle[i], Color.White);
            }
            for (int i = 0; i < square2rectangle.Count; i++)
            {
                _spriteBatch.Draw(square2texture, square2rectangle[i], Color.White);
            }



            _spriteBatch.Draw(rectangleTexture, ground, Color.Black);
            

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
