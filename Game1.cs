using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace ThinkJump
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D backgroundtxr, playerSheetTxr, platformSheetTxr, whiteBox, mobSheetTxr, doorTxr;
        SpriteFont uiTextFout, heartFont;
        SoundEffect jumpSound, bumpSound, fanfareSound, slashSound, ghostSound, gruntSound, backgroundMusic;

        int levelNumber = 0;
        Point screenSize = new Point(800, 450);

        PlayerSprite playerSprite;
        CoinSprite coinSprite;
        MobSprite mobSprite;

        List<List<PlatformSprite>> levels = new List<List<PlatformSprite>>();
        List<List<MobSprite>> mob = new List<List<MobSprite>>();        
        List<Vector2> coins = new List<Vector2>();


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = screenSize.X;
            _graphics.PreferredBackBufferHeight = screenSize.Y;
            _graphics.ApplyChanges();



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundtxr = Content.Load<Texture2D>("ThinkJump_background");
            playerSheetTxr = Content.Load<Texture2D>("heroSprite");
            platformSheetTxr = Content.Load<Texture2D>("JumpThing_spriteSheet2");
            mobSheetTxr = Content.Load<Texture2D>("mobSprite");
            uiTextFout = Content.Load<SpriteFont>("UiText");
            heartFont = Content.Load<SpriteFont>("HeartFont");
            jumpSound = Content.Load<SoundEffect>("jump");
            bumpSound = Content.Load<SoundEffect>("tap");
            fanfareSound = Content.Load<SoundEffect>("gateSound");
            doorTxr = Content.Load<Texture2D>("door");
            slashSound = Content.Load<SoundEffect>("slash");
            ghostSound = Content.Load<SoundEffect>("ghostsound");
            gruntSound = Content.Load<SoundEffect>("grunt");
            backgroundMusic = Content.Load<SoundEffect>("music");


            whiteBox = new Texture2D(GraphicsDevice, 1, 1);
            whiteBox.SetData(new[] { Color.White });

            playerSprite = new PlayerSprite(playerSheetTxr, whiteBox, new Vector2(50, 50), jumpSound, bumpSound, slashSound, gruntSound, ghostSound);
            
            coinSprite = new CoinSprite(doorTxr, whiteBox, new Vector2(760, 300));

            BuildLevels();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            playerSprite.Update(gameTime, levels[levelNumber],mob[levelNumber]);
            foreach (MobSprite thisMob in mob[levelNumber])
            {
              if (thisMob.isDead == false)
                  thisMob.Update(gameTime);
            }

            if (playerSprite.spritePos.Y > screenSize.Y + 50)
            {
                playerSprite.lives--;
                if (playerSprite.lives <= 0)
                {
                    //playerSprite.lives = 3;
                    levelNumber = 0;
                }
                gruntSound.Play();
                playerSprite.ResetPlayer(new Vector2(50, 50));
            }

            if (playerSprite.checkCollision(coinSprite))
            {
                levelNumber++;              
                if (levelNumber >= levels.Count) levelNumber = 0;
                coinSprite.spritePos = coins[levelNumber];
                playerSprite.ResetPlayer(new Vector2(50, 50));
                fanfareSound.Play();
            }
            base.Update(gameTime);
           if (playerSprite.lives <= 0)
            {
                
            }

        }


        protected override void Draw(GameTime gameTime)
        {
            if (playerSprite.lives > 0)
            {
                string livesString = "";
                
                _spriteBatch.Draw(backgroundtxr, new Rectangle(0, 0, screenSize.X, screenSize.Y), Color.White);  //creating background
                playerSprite.Draw(_spriteBatch, gameTime);                                                      //drawing player character           

                foreach (MobSprite thisMob in mob[levelNumber])                                             //dawing mobs
                    if (thisMob.isDead == false)
                        thisMob.Draw(_spriteBatch, gameTime);

                coinSprite.Draw(_spriteBatch, gameTime);
                foreach (PlatformSprite platform in levels[levelNumber]) platform.Draw(_spriteBatch, gameTime);

                for (int i = 0; i < playerSprite.lives; i++) livesString += "p";                            //drawing lives on top of screen

                _spriteBatch.DrawString(heartFont, livesString, new Vector2(15, 10), Color.White);

                uiTextFout.MeasureString("level " + levelNumber);
                _spriteBatch.DrawString(uiTextFout, "level " + (levelNumber + 1), new Vector2(screenSize.X - 15 - uiTextFout.MeasureString("level " + levelNumber).X, 5), Color.White);
            }
            else
            {
                _spriteBatch.Begin();
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.DrawString(uiTextFout, "Game Over", new Vector2(300, 200), Color.Red);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void BuildLevels()
        {
            //level 1
            backgroundMusic.Play();
            levels.Add(new List<PlatformSprite>());
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(100, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(70, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(120, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(250, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(280, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(310, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(340, 300)));

            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(355, 350)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(385, 350)));

            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(490, 260)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(520, 260)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(550, 260)));

            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(750, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(770, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(790, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(730, 300)));
            coins.Add(new Vector2(700, 100));

            mob.Add(new List<MobSprite>());
            mob[0].Add(new MobSprite(mobSheetTxr, whiteBox, new Vector2(260, 335), new Vector2(350, 295), ghostSound));
            
            //level 2
            levels.Add(new List<PlatformSprite>());
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(50, 100)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(80, 100)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(110, 100)));
            
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(610, 100)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(640, 100)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(660, 100)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(675, 100)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(690, 100)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(710, 100)));

            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(140, 400)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(170, 400)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(200, 400)));

            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(250, 380)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(280, 380)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(310, 380)));

            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(340, 340)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(370, 340)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(400, 340)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(430, 340)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(430, 340)));

            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(490, 300)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(520, 300)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(550, 300)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(580, 300)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(610, 300)));

            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(400, 230)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(370, 230)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(430, 230)));

            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(530, 160)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(560, 160)));


            coins.Add(new Vector2(700, 100));           
            mob.Add(new List<MobSprite>());
            mob[1].Add(new MobSprite(mobSheetTxr, whiteBox, new Vector2(630, 130), new Vector2(705, 130), jumpSound));

            //level 3

            coins.Add(new Vector2(700, 420));
            mob[1].Add(new MobSprite(mobSheetTxr, whiteBox, new Vector2(630, 130), new Vector2(705, 130), jumpSound));

        }
    }
}
