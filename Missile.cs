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
    class Missile
    {
        #region Variables

        public float angle;
        public Texture2D missile;
        public Vector2 velocity, position;
        public int width, height;
        public int life;
        public Color[] imageData;

        #endregion

        #region XNAMethods

        public bool Update() {
            life--;
            if (life == 0)
                return true;
            position += velocity;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(missile, position, null, Color.White, angle, new Vector2(width / 2f, height / 2f), 1, SpriteEffects.None, 0.1f);
        }

        #endregion

        #region MissileMethods

        public Missile(Texture2D img, float ang, Vector2 pos) {
            missile = img;
            angle = ang;
            position = pos;
            velocity = new Vector2((float)Math.Sin(ang) * 10, -(float)Math.Cos(ang) * 10);
            width = missile.Width;
            height = missile.Height;
            imageData = new Color[height * width];
            missile.GetData(imageData);
            life = 70;
        }

        public Matrix Transform() {
            return Matrix.CreateTranslation(new Vector3(position, 0.0f));
        }

        #endregion
    }
}
