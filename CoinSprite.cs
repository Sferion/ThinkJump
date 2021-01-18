using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace ThinkJump
{
    class CoinSprite : Sprite
    {
        public CoinSprite(Texture2D doorTxr, Texture2D newCollisionTxr, Vector2 newLocation)
            : base(doorTxr, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;
            //drawCollision = true;

            animations = new List<List<Rectangle>>();
            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(2, 2, 20, 25));

        }
    }
}
