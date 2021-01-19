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
        bool patroleDirection;
        const float walkSpeed = 50f;
        public int lives = 3;
        SoundEffect ghostSound ;
        Vector2 patroleLocetion1, patroleLocetion2;

        public MobSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation, Vector2 newPatrole, SoundEffect newghostSound)
            : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            ghostSound = newghostSound;
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

        }

        public void Update(GameTime gameTime)
        {
            
            if (patroleDirection)
            {
                if (spritePos.X < (patroleLocetion2.X - (walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds)))
                {
                    spritePos.X += walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    flipped = false;
                }
                else if (spritePos.X > (patroleLocetion2.X + (walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds))) spritePos.X -= walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else patroleDirection = !patroleDirection;
            }
            else
            {
                if (spritePos.X < (patroleLocetion1.X - (walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds))) spritePos.X += walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else if (spritePos.X > (patroleLocetion1.X + (walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds)))
                {
                    spritePos.X -= walkSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    flipped = true;
                }
                else patroleDirection = !patroleDirection;
            }
          
                setAnim(0);



        }
        
    }
}
