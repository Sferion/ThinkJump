using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ThinkJump
{
    class Sprite
    {

        Texture2D spriteSheet, collisionTexture;                // visible texture sheet, texture used to draw collision boxes

        public Vector2 spritePos, spriteVelocity,               // location of the sprite on-screen, velocity of the sprite in pixels per second
            spriteOrigin,                                       // where on the sprite is it's origin? (0f,0f) = top-left, (0.5f,1f) = bottom-middle, (0.5f,0.5f) = centre, etc
            spriteScale,                                        // should the sprite be resized? (1f,1f) = normal size, (2f,2f) = double size, (0.5f,1f) = half as wide, etc
            collisionInsetMin, collisionInsetMax;               // the extents of the collision box relative to the visible sprite, e.g. min = (0.25f,0.5f) would shave a quarter off on the left and half from the top

        public bool flipped,                                    // whether the sprite should appear flipped horizontally
            isDead,                                             // whether the sprite should be marked for deletion
            isColliding, drawCollision;                         // whether collision is active, whether the collision box will be drawn on screen

        int collPadding = 5;                                    // tweaks collision sensitivity for each edge, recommended = 5

        public List<List<Rectangle>> animations;                // a 2D table of animation frames, each stored as a rectangle to be selected from a sprite sheet
        public int currentAnim, currentFrame;                   // integers for tracking animation state
        public float frameTime, frameCounter;                   // floats for timing animation, frameTime is the duration of each frame in seconds


        //
        // Constructor
        //
        public Sprite(Texture2D newSpritesheet, Texture2D newCollisionTexture, Vector2 newLocation)
        {
            // assign the parameters to the member variables:
            spriteSheet = newSpritesheet;
            collisionTexture = newCollisionTexture;
            spritePos = newLocation;

            // assign some default values, will be overridden by child classes
            isColliding = false;
            drawCollision = false;
            isDead = false;
            flipped = false;
            spriteOrigin = new Vector2(0f, 0f);
            collisionInsetMin = new Vector2(0f, 0f);
            collisionInsetMax = new Vector2(0f, 0f);
            spriteScale = new Vector2(1f, 1f);
            currentAnim = 0;
            currentFrame = 0;
            frameTime = 0.6f;
            frameCounter = frameTime;

            // initialise animation lists and add a single frame a default animation
            animations = new List<List<Rectangle>>();
            animations.Add(new List<Rectangle>());
            animations[0].Add(new Rectangle(5, 5, 246, 315));
        }

        //
        // Update()
        // virtual method, to be overridden by child classes
        //
        public virtual void Update(GameTime gameTime) { }

        //
        // Draw()
        // advanced animation and draws frame to spriteBatch
        //
        public void Draw(SpriteBatch spriteBatch, GameTime gametime)
        {
            // advance the animation timer and frames
            if (animations[currentAnim].Count > 1) // only advance animation if the current animation has multiple frames
            {
                frameCounter -= (float)gametime.ElapsedGameTime.TotalSeconds; // reduce the timer by whatever fraction of a second has passed since the last frame
                if (frameCounter <= 0)  // has the timer ran out for this frame?
                {
                    frameCounter = frameTime; // reset the timer
                    currentFrame++; // advance to the next frame
                }
                if (currentFrame >= animations[currentAnim].Count) currentFrame = 0; // if we have reached the end of the animation, start again from frame 0
            }

            // declare a SpriteEffects variable to flip the sprite if required by the flipped bool
            SpriteEffects drawEffect;
            if (flipped) drawEffect = SpriteEffects.FlipHorizontally;
            else drawEffect = SpriteEffects.None;

            // draw the visible sprite to the spriteBatch
            spriteBatch.Draw(
                spriteSheet,                            // using the spriteSheet that was assigned in the constructor
                getRectangleForDraw(),                  // the DESTINATION rectangle is defined by the getRectangleForDraw() method below
                animations[currentAnim][currentFrame],  // the SOURCE rectangle is the current frame of the current animation
                Color.White,                            // white = no tint
                0f,                                     // rotation of the sprite around the origin
                spriteOrigin,                           // the origin that was assigned in the constructor
                drawEffect,                             // the SpriteEffect used above to define whether flipped or not
                1f);                                    // draw-order, not used in this project

            // if required, draw a simple box over the collision area
            if (drawCollision) spriteBatch.Draw(
                collisionTexture,                       // using the collision texture that was assigned in the constructor
                getRectangleForCollision(),             // the destination rectangle is defined by the same method that provides the shape for actual collision
                Color.Red);                             // tint red for visibility
        }

        //
        // setAnim()
        // used to change which animation is playing
        //
        public void setAnim(int newAnim) // && newAnim < animations.Count//)
        {
            if (currentAnim != newAnim) // only if the new animation is not already playing
            {
                // change the animation int, and reset the counters
                currentAnim = newAnim;
                currentFrame = 0;
                frameCounter = frameTime;
            }
        }

        //
        //
        //
        // The following methods are all different ways of checking for collisions:
        // checkCollision()
        // checkCollisionBelow()
        // checkCollisionAbove()
        // checkCollisionLeft()
        // checkCollisionRight()
        //
        //
        //

        //
        // checkCollision()
        // compares this sprite with another and returns true if they interset
        // uses getRectangleForCollision() to build the correct collision shape
        //
        public bool checkCollision(Sprite otherSprite)
        {
            if (!isColliding || !otherSprite.isColliding) return false; // if either sprite is NOT set to collide, ignore collision check
            else return getRectangleForCollision().Intersects(otherSprite.getRectangleForCollision()); // get the getRectangleForCollision() method of each sprite and return whether or not they intersect
        }

        //
        // checkCollisionBelow()
        // a specific collision check that returns true if there is a collision caused by this sprite touching another one from below
        // uses getRectangleForCollision() to build the correct collision shape, and the getEdge....ForCollision() methods to compare the edges of the collision shapes
        //
        public bool checkCollisionBelow(Sprite otherSprite)
        {
            return checkCollision(otherSprite) && getEdgeBottomForCollision() < otherSprite.getEdgeBottomForCollision() && getEdgeBottomForCollision() > otherSprite.getEdgeTopForCollision()
                && (getEdgeLeftForCollision() + collPadding < otherSprite.getEdgeRightForCollision() && getEdgeRightForCollision() - collPadding > otherSprite.getEdgeLeftForCollision());
        }

        //
        // checkCollisionAbove()
        // a specific collision check that returns true if there is a collision caused by this sprite touching another one from above
        // uses getRectangleForCollision() to build the correct collision shape, and the getEdge....ForCollision() methods to compare the edges of the collision shapes
        //
        public bool checkCollisionAbove(Sprite otherSprite)
        {
            return checkCollision(otherSprite) && getEdgeTopForCollision() > otherSprite.getEdgeTopForCollision() && getEdgeTopForCollision() < otherSprite.getEdgeBottomForCollision()
                && (getEdgeLeftForCollision() + collPadding < otherSprite.getEdgeRightForCollision() && getEdgeRightForCollision() - collPadding > otherSprite.getEdgeLeftForCollision());
        }

        //
        // checkCollisionLeft()
        // a specific collision check that returns true if there is a collision caused by this sprite touching another one from the left
        // uses getRectangleForCollision() to build the correct collision shape, and the getEdge....ForCollision() methods to compare the edges of the collision shapes
        //
        public bool checkCollisionLeft(Sprite otherSprite)
        {
            return checkCollision(otherSprite) && getEdgeRightForCollision() < otherSprite.getEdgeRightForCollision() && getEdgeRightForCollision() > otherSprite.getEdgeLeftForCollision()
                && (getEdgeTopForCollision() + collPadding < otherSprite.getEdgeBottomForCollision() && getEdgeBottomForCollision() - collPadding > otherSprite.getEdgeTopForCollision());
        }

        //
        // checkCollisionRight()
        // a specific collision check that returns true if there is a collision caused by this sprite touching another one from the right
        // uses getRectangleForCollision() to build the correct collision shape, and the getEdge....ForCollision() methods to compare the edges of the collision shapes
        //
        public bool checkCollisionRight(Sprite otherSprite)
        {
            return checkCollision(otherSprite) && getEdgeLeftForCollision() > otherSprite.getEdgeLeftForCollision() && getEdgeLeftForCollision() < otherSprite.getEdgeRightForCollision()
                && (getEdgeTopForCollision() + collPadding < otherSprite.getEdgeBottomForCollision() && getEdgeBottomForCollision() - collPadding > otherSprite.getEdgeTopForCollision());
        }

        //
        //
        //
        // The following methods are all used in calculating the desintation rectangle for DRAWING the visible sprite:
        // getRectangleForDraw()
        // getWidthForDraw()
        // getEdgeLeftForDraw()
        // getEdgeRightForDraw()
        // getHieghtForDraw()
        // getEdgeTopForDraw()
        // getEdgeBottomForDraw()
        //
        //
        //

        //
        // getRectangleForDraw()
        // builds the rectangle that is used as the destination rectangle when drawing the sprite
        // seperates the calculations out into individual methods
        //
        public Rectangle getRectangleForDraw()
        {
            return new Rectangle(getEdgeLeftForDraw(), getEdgeTopForDraw(), getWidthForDraw(), getHeightForDraw());
        }

        //
        // getWidthForDraw()
        // finds the desired draw width of the visible sprite
        //
        public int getWidthForDraw()
        {
            // draw width is the width of the current frame, multiplied by the resized scale of the sprite
            return (int)(animations[currentAnim][currentFrame].Width * spriteScale.X);
        }

        //
        // getEdgeLeftForDraw()
        // finds the x co-ord of the left edge of the visible sprite
        //
        public int getEdgeLeftForDraw()
        {
            // left edge is the x co-ord, minus the width scaled by the x component of the origin
            return (int)(spritePos.X - (getWidthForDraw() * spriteOrigin.X));
        }

        //
        // getEdgeRightForDraw()
        // finds the x co-ord of the right edge of the visible sprite
        //
        public int getEdgeRightForDraw()
        {
            // right edge is the left edge plus the whole (scaled) width
            return getEdgeLeftForDraw() + getWidthForDraw();
        }

        //
        // getHieghtForDraw()
        // finds the desired draw height of the visible sprite
        //
        public int getHeightForDraw()
        {
            // draw height is the height of the current frame, multiplied by the resized scale of the sprite
            return (int)(animations[currentAnim][currentFrame].Height * spriteScale.Y);
        }

        //
        // getEdgeTopForDraw()
        // finds the y co-ord of the top edge of the visible sprite
        //
        public int getEdgeTopForDraw()
        {
            // letopft edge is the y co-ord, minus the height scaled by the y component of the origin
            return (int)(spritePos.Y - (getHeightForDraw() * spriteOrigin.Y));
        }

        //
        // getEdgeBottomForDraw()
        // finds the y co-ord of the bottom edge of the visible sprite
        //
        public int getEdgeBottomForDraw()
        {
            // right edge is the top edge plus the whole (scaled) height
            return getEdgeTopForDraw() + getHeightForDraw();
        }

        //
        //
        //
        // The following methods are all used in calculating the rectangle for COLLISION detection:
        // getRectangleForCollision()
        // getWidthForCollision()
        // getEdgeLeftForCollision()
        // getEdgeRightForCollision()
        // getHieghtForCollision()
        // getEdgeTopForCollision()
        // getEdgeBottomForCollision()
        // getCentreForCollision()
        //
        //
        //

        //
        // getRectangleForCollision()
        // builds the rectangle that is used for checking collions
        // seperates the calculations out into individual methods
        //
        public Rectangle getRectangleForCollision()
        {
            return new Rectangle(getEdgeLeftForCollision(), getEdgeTopForCollision(), getWidthForCollision(), getHeightForCollision());
        }

        //
        // getWidthForCollision()
        // finds the desired width of the collision rectangle
        //
        public int getWidthForCollision()
        {
            // the width is the right edge minus the left edge
            return getEdgeRightForCollision() - getEdgeLeftForCollision();
        }

        //
        // getEdgeLeftForCollision()
        // finds the x co-ord of the left edge of the collision rectangle
        //
        public int getEdgeLeftForCollision()
        {
            // left edge is the x co-ord, plus the width scaled by the x component of the MIN inset
            return getEdgeLeftForDraw() + (int)(collisionInsetMin.X * getWidthForDraw());
        }

        //
        // getEdgeRightForCollision()
        // finds the x co-ord of the right edge of the collision rectangle
        //
        public int getEdgeRightForCollision()
        {
            // right edge is the x co-ord, minus the width scaled by the x component of the MAX inset
            return getEdgeRightForDraw() - (int)(collisionInsetMax.X * getWidthForDraw());
        }

        //
        // getHeightForCollision()
        // finds the desired height of the collision rectangle
        //
        public int getHeightForCollision()
        {
            // the height is the bottom edge minus the top edge
            return getEdgeBottomForCollision() - getEdgeTopForCollision();
        }

        //
        // getEdgeTopForCollision()
        // finds the y co-ord of the top edge of the collision rectangle
        //
        public int getEdgeTopForCollision()
        {
            // top edge is the y co-ord, plus the height scaled by the y component of the MIN inset
            return getEdgeTopForDraw() + (int)(collisionInsetMin.Y * getHeightForDraw());
        }

        //
        // getEdgeBottomForCollision()
        // finds the y co-ord of the bottom edge of the collision rectangle
        //
        public int getEdgeBottomForCollision()
        {
            // top edge is the y co-ord, minus the height scaled by the y component of the MAX inset
            return getEdgeBottomForDraw() - (int)(collisionInsetMax.Y * getHeightForDraw());
        }

        //
        // getCentreForCollision()
        // finds the x and y co-ords of the centre of the collision rectangle
        //
        public Vector2 getCentreForCollision()
        {
            // the centre is the top-left plus half of the width and height
            return new Vector2(getEdgeLeftForCollision() + (getWidthForCollision() * 0.5f), getEdgeTopForCollision() + (getHeightForCollision() * 0.5f));
        }
    }
}