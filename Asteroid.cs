using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HW5
{
    class Asteroid
    {
        #region Variables

        private Texture2D asteroidTexture;
        public Color[] imageData;
        public Vector2 position, velocity;

        public float angle;
        public int size, height, width;

        #endregion

        #region XNAMethods

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(asteroidTexture, position, null, Color.White,
                             angle, new Vector2(width / 2f, height / 2f), 1, SpriteEffects.None, .8f); 
        }

        public void Update() {
            position += velocity;
            angle += (float)(velocity.Y / 180.0 * MathHelper.Pi);
            
        }

        #endregion

        #region AsteroidMethods

        public Asteroid(Texture2D img, Vector2 pos, Vector2 vel, int s) {
            asteroidTexture = img;
            position = pos;
            velocity = vel;
            angle = 0;
            size = s;
            if (velocity.X == 0)
                velocity.X = 1;
            if (velocity.Y == 0)
                velocity.Y = 1;
            height = asteroidTexture.Height;
            width = asteroidTexture.Width;
            imageData = new Color[height * width];
            asteroidTexture.GetData(imageData);
        }

        public Matrix Transform() {
            return Matrix.CreateTranslation(new Vector3(new Vector2(-width / 2, -height /2), 0.0f)) *
                        Matrix.CreateRotationZ(angle) *
                        Matrix.CreateTranslation(new Vector3(position, 0.0f));
        }

        #endregion

    }
}
