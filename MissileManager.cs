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
    class MissileManager
    {
        public List<Missile> missiles;
        public Texture2D missile;
        public Rectangle screenBounds;

        #region XNAMethod

        public void Draw(SpriteBatch spriteBatch) {
            foreach(Missile m in missiles) {
                m.Draw(spriteBatch);
            }
        }

        public void Update() {
            for( int i = 0; i < missiles.Count; i++) {
                if (missiles[i].position.X < 0)
                    missiles[i].position.X = screenBounds.Width + missiles[i].width;
                if (missiles[i].position.X > screenBounds.Width + missiles[i].width)
                    missiles[i].position.X = 0;
                if (missiles[i].position.Y < 0)
                    missiles[i].position.Y = screenBounds.Height + missiles[i].height;
                if (missiles[i].position.Y > screenBounds.Height + missiles[i].height)
                    missiles[i].position.Y = 0;
                if (missiles[i].Update())
                    collision(missiles[i]);
            }
        }

        #endregion

        #region MissileManagerMethod

        public MissileManager(ContentManager content, Rectangle screen) {
            missile = content.Load<Texture2D>(@"Sprites\Missile");
            missiles = new List<Missile>();
            screenBounds = screen;
        }

        public void addMissile(float a, Vector2 pos) {
            missiles.Add(new Missile(missile, a, pos));
        }

        public void collision(Missile m) {
            missiles.Remove(m);
        }

        #endregion


    }
}
