using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;



namespace ThinkJump
{
    class PlayerSprite : Sprite
    {
        bool jumping, walking, falling, jumpIsPressed, attacking;       
        const float jumpSpeed = 4f;
        const float walkSpeed = 100f;
        public int lives = 3;
        SoundEffect jumpSound, bumpSound, slashSound, gruntSound;

        public PlayerSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation, SoundEffect newjumpSound, SoundEffect newbumpSound, SoundEffect newslashSound, SoundEffect newgruntSound)
            : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            gruntSound = newgruntSound;
            slashSound = newslashSound;
            jumpSound = newjumpSound;
            bumpSound = newbumpSound;


            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;
            //drawCollision = true;
            collisionInsetMin = new Vector2(0.25f, 0.3f);
            collisionInsetMax = new Vector2(0.25f, 0.05f);

            frameTime = 0.1f;
            animations = new List<List<Rectangle>>();       // player animations

            animations.Add(new List<Rectangle>());              // idal animations
            animations[0].Add(new Rectangle(72, 156, 48, 52));
            animations[0].Add(new Rectangle(253, 156, 48, 52));
            animations[0].Add(new Rectangle(433, 156, 48, 52));
            animations[0].Add(new Rectangle(614, 156, 48, 52));
            animations[0].Add(new Rectangle(792, 156, 48, 52));
            animations[0].Add(new Rectangle(973, 156, 48, 52));
            animations[0].Add(new Rectangle(1153, 156, 48, 52));
            animations[0].Add(new Rectangle(1334, 156, 48, 52));
            animations[0].Add(new Rectangle(1514, 156, 48, 52));

            animations.Add(new List<Rectangle>());            // runing animations
            animations[1].Add(new Rectangle(163, 244, 48, 52));
            animations[1].Add(new Rectangle(339, 244, 48, 52));
            animations[1].Add(new Rectangle(524, 244, 48, 52));
            animations[1].Add(new Rectangle(707, 244, 48, 52));
            animations[1].Add(new Rectangle(886, 244, 48, 52));
            animations[1].Add(new Rectangle(1060, 244, 48, 52));
            animations[1].Add(new Rectangle(1246, 244, 48, 52));
            animations[1].Add(new Rectangle(1426, 244, 48, 52));

            animations.Add(new List<Rectangle>());              // jumping animations
            animations[2].Add(new Rectangle(103, 465, 48, 52));
            animations[2].Add(new Rectangle(283, 465, 48, 52));
            animations[2].Add(new Rectangle(463, 465, 48, 52));

            animations.Add(new List<Rectangle>());              // falling animations
            animations[3].Add(new Rectangle(208, 347, 48, 52));
            animations[3].Add(new Rectangle(390, 347, 48, 52));
            animations[3].Add(new Rectangle(569, 347, 48, 52));

            
            animations.Add(new List<Rectangle>());              // attack animations            
            animations[4].Add(new Rectangle(68, 50, 56, 63));
            animations[4].Add(new Rectangle(244, 50, 56, 63));
            animations[4].Add(new Rectangle(420, 50, 56, 63));                  //current work
            animations[4].Add(new Rectangle(614, 50, 56, 63));
            animations[4].Add(new Rectangle(795, 50, 56, 63));
            animations[4].Add(new Rectangle(976, 50, 56, 63));
            animations[4].Add(new Rectangle(1157, 50, 56, 63));
            animations[4].Add(new Rectangle(1157, 50, 56, 63));

            animations.Add(new List<Rectangle>());              // geting hit animations
            animations[5].Add(new Rectangle(39, 577, 48, 52));
            animations[5].Add(new Rectangle(220, 577, 48, 52));
            animations[5].Add(new Rectangle(394, 577, 48, 52));
            animations[5].Add(new Rectangle(573, 577, 48, 52));

            animations.Add(new List<Rectangle>());              // death animations
            animations[6].Add(new Rectangle(53, 687, 48, 52));
            animations[6].Add(new Rectangle(236, 687, 48, 52));
            animations[6].Add(new Rectangle(416, 687, 56, 52));
            animations[6].Add(new Rectangle(596, 687, 56, 52));
            animations[6].Add(new Rectangle(775, 687, 56, 52));
            animations[6].Add(new Rectangle(956, 687, 56, 52));
            animations[6].Add(new Rectangle(1135, 687, 56, 52));
            animations[6].Add(new Rectangle(1320, 687, 56, 52));
            animations[6].Add(new Rectangle(1504, 687, 56, 52));
            animations[6].Add(new Rectangle(1684, 687, 56, 52));
            

            jumping = false;
            walking = false;
            falling = true;
            jumpIsPressed = false;
        }

        public void Update(GameTime gameTime, List<PlatformSprite> platforms, List<MobSprite> mobs)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (!jumpIsPressed && !jumping && !falling &&
                (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Space)
                || gamePadState.IsButtonDown(Buttons.A)))
            {
                jumpIsPressed = true;
                jumping = true;
                walking = false;
                falling = false;
                attacking = false;
                spriteVelocity.Y -= jumpSpeed;
                jumpSound.Play();
            }
            else if (jumpIsPressed && !jumping && !falling &&       //                  
                  !(keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Space)
                || gamePadState.IsButtonDown(Buttons.A)))
            {               
                jumpIsPressed = false;
            }

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)
                || gamePadState.IsButtonDown(Buttons.DPadLeft))
            {
                walking = true;
                spriteVelocity.X = -walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                flipped = true;
            }
            else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right)
                || gamePadState.IsButtonDown(Buttons.DPadRight))
            {
                walking = true;
                spriteVelocity.X = walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                flipped = false;
            }
            else
            {
                walking = false;
                spriteVelocity.X = 0;
            }
            if (!jumpIsPressed && !jumping && !falling &&
                (keyboardState.IsKeyDown(Keys.E) || keyboardState.IsKeyDown(Keys.LeftShift)))
            {
                if (attacking == false)
                {
                    jumpIsPressed = false;
                    jumping = false;
                    walking = false;
                    falling = false;
                    attacking = true;
                    slashSound.Play();
                }
            }

                if ((falling || jumping) && spriteVelocity.Y < 500f) spriteVelocity.Y += 5f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            spritePos += spriteVelocity;

            bool hasCollided = false;

            foreach (PlatformSprite platform in platforms)
            {
                if (checkCollisionBelow(platform))
                {                   
                    bumpSound.Play();                  
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.Y--;
                    spriteVelocity.Y = 0;
                    jumping = false;
                    falling = false;
                }
                else if (checkCollisionAbove(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.Y++;
                    spriteVelocity.Y = 0;
                    jumping = false;
                    falling = true;
                }
                if (checkCollisionLeft(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.X--;
                    spriteVelocity.X = 0;
                }
                if (checkCollisionRight(platform))
                {
                    hasCollided = true;
                    while (checkCollision(platform)) spritePos.X++;
                    spriteVelocity.X = 0;
                }
                if (!hasCollided && walking) falling = true;
                if (jumping && spriteVelocity.Y > 0)
                {                    
                    jumping = false;
                    falling = true;
                }

            }        
            foreach (MobSprite mob in mobs)                     // check collision agienst mob
            {               
                if (!mob.isDead && checkCollision(mob))
                {
                    if (attacking) mob.isDead = true;
                    else
                    {
                        gruntSound.Play();
                        lives--;
                        //ResetPlayer(new Vector2(50, 50));
                    }
                }
            }

            if (walking && Math.Abs(spriteVelocity.Y) < 0.3335) setAnim(1);
            else if (falling) setAnim(3);
            else if (jumping) setAnim(2);
            else if (attacking) setAnim(4);
            else setAnim(0);

            if (attacking && currentFrame == 7) attacking = false;

        }
        public void ResetPlayer(Vector2 newPos)
        {           
            spritePos = newPos;
            spriteVelocity = new Vector2();
            jumping = false;
            walking = false;
            falling = true;

        }
    }
}
