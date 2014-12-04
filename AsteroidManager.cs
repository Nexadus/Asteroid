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
    class AsteroidManager
    {
        #region Variables

        public List<Asteroid> asteroids;
        public Random generate;
        public Texture2D smAsteroid, medAsteroid, lgAsteroid;
        private Rectangle screenBounds;
        private Vector2 changeMed, changeLarge;
        enum Size { SMALL, MEDIUM, LARGE }

        #endregion

        #region XNAMethods

        public void Draw(SpriteBatch spriteBatch) {
            foreach(Asteroid a in asteroids) {
                a.Draw(spriteBatch);
            }
        }

        public bool Update() {
            if (asteroids.Count == 0)
                return false;
            foreach(Asteroid a in asteroids) {
                if (a.position.X < -59)
                    a.position.X = screenBounds.Width + a.width;
                if (a.position.X > screenBounds.Width + a.width)
                    a.position.X = -49;
                if (a.position.Y < -59)
                    a.position.Y = screenBounds.Height + a.height;
                if (a.position.Y > screenBounds.Height + a.height)
                    a.position.Y = -49;
                a.Update();
            }
            return true;
        }

        #endregion

        #region AsteroidManagerMethods

        public AsteroidManager(ContentManager content, Rectangle screen) {
            smAsteroid = content.Load<Texture2D>(@"Sprites\SAsteroid");
            medAsteroid = content.Load<Texture2D>(@"Sprites\MAsteroid");
            lgAsteroid = content.Load<Texture2D>(@"Sprites\LAsteroid");
            generate = new Random();
            asteroids = new List<Asteroid>();
            screenBounds = screen;
            changeLarge = new Vector2(20, 20);
            changeMed = new Vector2(10, 10);
            Start(2);
        }

        public void Start(int round) {
            asteroids.Clear();
            for( int i = 0; i < round; i++) {
                Vector2 position = Vector2.Zero;
                int rand = generate.Next(2);
                if( rand == 1) {
                    position.X = generate.Next(-49, screenBounds.Width);
                    position.Y = (generate.Next(2) == 1 ? -49 : screenBounds.Height);
                }
                else {
                    position.Y = generate.Next(-49, screenBounds.Height);
                    position.X = (generate.Next(2) == 1 ? -49 : screenBounds.Width);
                }
                asteroids.Add(new Asteroid(lgAsteroid, position, 
                              new Vector2(-1, 1), (int)Size.LARGE));

            }
        }

        public int collision(Asteroid a) {
            asteroids.Remove(a);
            if( a.size == (int)Size.LARGE) {
                asteroids.Add(new Asteroid(medAsteroid, a.position + changeLarge,
                              new Vector2(generate.Next(-3, -3), generate.Next(-3, 3)), (int)Size.MEDIUM));
                asteroids.Add(new Asteroid(medAsteroid, a.position - changeLarge,
                              new Vector2(generate.Next(-3, -3), generate.Next(-3, 3)), (int)Size.MEDIUM));
                return 20;
            }
            else if ( a.size == (int)Size.MEDIUM ) {
                asteroids.Add(new Asteroid(smAsteroid, a.position + changeMed,
                              new Vector2(generate.Next(-7, 7), generate.Next(-7, 7)), (int)Size.SMALL));
                asteroids.Add(new Asteroid(smAsteroid, a.position - changeMed,
                              new Vector2(generate.Next(-7, 7), generate.Next(-7, 7)), (int)Size.SMALL));
                return 50;
            }
            return 100;
        }
        

        #endregion
    }
}
