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
        public CoinSprite(Texture2D newSpriteSheet, Texture2D newCollisionTxr, Vector2 newLocation)
            : base(newSpriteSheet, newCollisionTxr, newLocation)
        {
            spriteOrigin = new Vector2(0.5f, 1f);
            isColliding = true;

            animations = new List<List<Rectangle>>();
            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(48, 48, 48, 48));
            animations[0].Add(new Rectangle(96, 48, 48, 48));
            animations[0].Add(new Rectangle(144, 48, 48, 48));
            animations[0].Add(new Rectangle(96, 48, 48, 48));
        }
    }
}
