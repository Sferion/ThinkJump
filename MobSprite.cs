using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;



namespace ThinkJump
{
    class MobSprite : Sprite
    {
        bool jumping, walking, falling, jumpIsPressed, attacking, patroleDirection;
        const float jumpSpeed = 4f;
        const float walkSpeed = 50f;
        public int lives = 3;
        SoundEffect jumpSound, bumpSound;
        Vector2 patroleLocetion1, patroleLocetion2;

        public MobSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation, Vector2 newPatrole, SoundEffect newjumpSound)
            : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            jumpSound = newjumpSound;
            patroleLocetion1 = newLocation;
            patroleLocetion2 = newPatrole;

            spriteOrigin = new Vector2(0.5f, 2f);
            isColliding = true;
            //drawCollision = true;
            collisionInsetMin = new Vector2(0.25f, 0.3f);
            collisionInsetMax = new Vector2(0.25f, 0.05f);

            frameTime = 0.5f;
            animations = new List<List<Rectangle>>();       // player animations

            animations.Add(new List<Rectangle>());              // idal animations
            animations[0].Add(new Rectangle(30, 19, 40, 50));
            animations[0].Add(new Rectangle(130, 20, 40, 50));
            animations[0].Add(new Rectangle(227, 19, 40, 50));
            animations[0].Add(new Rectangle(332, 20, 40, 50));


            animations.Add(new List<Rectangle>());            // runing animations          
            animations[1].Add(new Rectangle(30, 19, 40, 50));
            animations[1].Add(new Rectangle(128, 20, 40, 50));
            animations[1].Add(new Rectangle(231, 19, 40, 50));
            animations[1].Add(new Rectangle(330, 20, 40, 50));

            animations.Add(new List<Rectangle>());              // attack animations 
            animations[2].Add(new Rectangle(8, 94, 40, 50));
            animations[2].Add(new Rectangle(104, 94, 40, 50));
            animations[2].Add(new Rectangle(203, 94, 40, 50));
            animations[2].Add(new Rectangle(310, 94, 40, 50));
            animations[2].Add(new Rectangle(405, 94, 40, 50));
            animations[2].Add(new Rectangle(502, 94, 40, 50));


            animations.Add(new List<Rectangle>());              // death animations
            animations[3].Add(new Rectangle(53, 6, 48, 52));
            animations[3].Add(new Rectangle(236, 6, 48, 52));
            animations[3].Add(new Rectangle(416, 6, 56, 52));
            animations[3].Add(new Rectangle(596, 6, 56, 52));
            animations[3].Add(new Rectangle(775, 6, 56, 52));         //to be worked on


            jumping = false;
            walking = false;
            falling = true;
            jumpIsPressed = false;
        }

        public void Update(GameTime gameTime)
        {

            if (patroleDirection)
            {
                if (spritePos.X < (patroleLocetion2.X - (walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds))) spritePos.X += walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;                
                else if (spritePos.X > (patroleLocetion2.X + (walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds))) spritePos.X -= walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else patroleDirection = !patroleDirection;
            }
            else
            {
                if (spritePos.X < (patroleLocetion1.X - (walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds))) spritePos.X += walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else if (spritePos.X > (patroleLocetion1.X + (walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds))) spritePos.X -= walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else patroleDirection = !patroleDirection;
            }
          


           // if (walking && Math.Abs(spriteVelocity.Y) < 0.35) setAnim(1);
           // else if (falling) setAnim(3);
           // else if (jumping) setAnim(2);
           // else if (attacking) setAnim(4);
           // else
                setAnim(0);



        }
        
    }
}
