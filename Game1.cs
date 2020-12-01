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

        Texture2D backgroundtxr, playerSheetTxr, platformSheetTxr, whiteBox;
        SpriteFont uiTextFout, heartFont;
        SoundEffect jumpSound, bumpSound, fanfareSound;

        int levelNumber = 0;
        Point screenSize = new Point(800, 450);

        PlayerSprite playerSprite;
        CoinSprite coinSprite;


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
            backgroundtxr = Content.Load<Texture2D>("JumpThing_background");
            playerSheetTxr = Content.Load<Texture2D>("heroSprite");
            platformSheetTxr = Content.Load<Texture2D>("JumpThing_spriteSheet2");
            uiTextFout = Content.Load<SpriteFont>("UiText");
            heartFont = Content.Load<SpriteFont>("HeartFont");
            jumpSound = Content.Load<SoundEffect>("jump");
            bumpSound = Content.Load<SoundEffect>("tap");
            fanfareSound = Content.Load<SoundEffect>("fanfare");

            whiteBox = new Texture2D(GraphicsDevice, 1, 1);
            whiteBox.SetData(new[] { Color.White });

            playerSprite = new PlayerSprite(playerSheetTxr, whiteBox, new Vector2(50, 50), jumpSound, bumpSound);
            coinSprite = new CoinSprite(playerSheetTxr, whiteBox, new Vector2(200, 200));

            BuildLevels();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            playerSprite.Update(gameTime, levels[levelNumber]);

            if (playerSprite.spritePos.Y > screenSize.Y + 50)
            {
                playerSprite.lives--;
                if (playerSprite.lives <= 0)
                {
                    playerSprite.lives = 3;
                    levelNumber = 0;
                }
                playerSprite.ResetPlayer(new Vector2(50, 50));
            }

            if (playerSprite.checkCollision(coinSprite))
            {
                levelNumber++;
                if (levelNumber >= levels.Count) levelNumber = 0;
                coinSprite.spritePos = coins[levelNumber];
                playerSprite.ResetPlayer(new Vector2(50, 50));
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            string livesString = "";
            _spriteBatch.Begin();

            _spriteBatch.Draw(backgroundtxr, new Rectangle(0, 0, screenSize.X, screenSize.Y), Color.White);  //creating background

            playerSprite.Draw(_spriteBatch, gameTime);
            //coinSprite.Draw(_spriteBatch, gameTime);
            foreach (PlatformSprite platform in levels[levelNumber]) platform.Draw(_spriteBatch, gameTime);


            for (int i = 0; i < playerSprite.lives; i++) livesString += "p";

            _spriteBatch.DrawString(heartFont, livesString, new Vector2(15, 10), Color.White);

            uiTextFout.MeasureString("level " + levelNumber);
            _spriteBatch.DrawString(uiTextFout, "level " + (levelNumber + 1), new Vector2(screenSize.X - 15 - uiTextFout.MeasureString("level " + levelNumber).X, 5), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void BuildLevels()
        {
            levels.Add(new List<PlatformSprite>());
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(100, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(70, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(250, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(280, 300)));
            levels[0].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(310, 300)));
            coins.Add(new Vector2(0, 0));

            levels.Add(new List<PlatformSprite>());
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(100, 350)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(130, 350)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(160, 350)));
            levels[1].Add(new PlatformSprite(platformSheetTxr, whiteBox, new Vector2(200, 400)));
            coins.Add(new Vector2(0, 0));
        }
    }
}
