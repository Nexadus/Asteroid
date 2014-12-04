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
    class Level
    {
        #region Variables

        Vector2 startPoint;
        Rectangle screenBounds;
        ContentManager content;
        KeyboardState keyboard;

        int timeSinceShot = 0;
        int timePerShot = 50;
        int timeElapsed = 0;
        int timePerFrame = 16;
        
        #endregion

        #region GameVar

        AsteroidManager am;
        Ship ship;
        MissileManager shipMissile;
        Score score;

        #endregion

        #region XNAMethods

        public void Initialize() {
            ship = new Ship(content, startPoint);
            am = new AsteroidManager(content, screenBounds);
            shipMissile = new MissileManager(content, screenBounds);
            score = new Score(screenBounds);
            score.Font = content.Load<SpriteFont>(@"Sprites\SpriteFont1");
            score.lives += 3;
            score.score = 0;
            score.gameOverFont = content.Load<SpriteFont>(@"Sprites\SpriteFont2");
        }

        public void LoadContent() {
            Initialize();
            ship.LoadContent(content);
        }

        public void Draw(SpriteBatch spriteBatch) {
            ship.Draw(spriteBatch);
            am.Draw(spriteBatch);
            shipMissile.Draw(spriteBatch);
            score.Draw(spriteBatch);

        }

        public void Update(GameTime gameTime) {
            keyboard = Keyboard.GetState();
            timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

            if ( timeElapsed > timePerFrame ) {

                timeElapsed -= timePerFrame;

                timeSinceShot++;

                ship.Update(gameTime, keyboard);
                if(keyboard.IsKeyDown(Keys.Space) && timeSinceShot > timePerShot && !ship.invincible) {
                    timeSinceShot = 0;
                    shipMissile.addMissile(ship.Rotation, ship.Position);
                }
                if( (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down)) && timeSinceShot > timePerShot) {
                    timeSinceShot = 0;
                    ship.Jump();
                }
                shipMissile.Update();
                am.Update();

                Rectangle shipRectangle = new Rectangle((int)ship.PosX, (int)ship.PosY,
                                          (int)ship.Width, (int)ship.Height);
                Matrix shipTransform = ship.Transform();

                 for( int i = 0; i < am.asteroids.Count; i++) {
                    Asteroid a = am.asteroids[i];
                    Matrix blockTransform = a.Transform();
                    Rectangle blockRectangle = 
                        CalculatingBoundingRectangle(new Rectangle(0, 0, a.width, a.height), blockTransform);                    

                    for( int j = 0; j < shipMissile.missiles.Count; j++) {
                        Missile m = shipMissile.missiles[j];
                        Rectangle missileTemp = new Rectangle((int)m.position.X,(int)m.position.Y, 
                                             m.width, m.height); 
                        Matrix missileTransform = m.Transform();
                        if ( missileTemp.Intersects(blockRectangle ) ) {
                               /*if (collision(Matrix.CreateTranslation(new Vector3(a.width / 2, a.height / 2, 0.0f)) *                                     
                                                                       Matrix.CreateRotationZ(a.angle) * Matrix.CreateTranslation(new Vector3(a.position, 0.0f)),
                                        (int)a.width, (int)a.height, a.imageData, Matrix.CreateTranslation(new Vector3(-m.width / 2, -m.height / 2, 0)) * Matrix.CreateRotationZ(m.angle) * Matrix.CreateTranslation(new Vector3(m.position, 0)),
                                        (int)m.width, (int)m.height, m.imageData)) {*/
                                if ( collision(missileTransform, m.width, m.height, m.imageData,
                                               blockTransform, a.width, a.height, a.imageData)) {
                                
                                    score.score += am.collision(a);
                                    shipMissile.collision(m);
                                } 
                        }
                        
                        
                    }
                    if (shipRectangle.Intersects(blockRectangle)) {
                        if( collision(shipTransform, ship.Width, ship.Height, ship.imageData,
                                      blockTransform, a.width, a.height, a.imageData) ) {
                            reset();
                            if (score.lives-- == 0 )
                                am.asteroids.Clear();
                        }

                    }
                }
                
                WorldWrap();
            }
        }

        #endregion

        #region LevelMethods

        public Level(ContentManager stuff, Vector2 startPos, Rectangle boundary) {
            content = stuff;
            startPoint = startPos;
            screenBounds = boundary;
        }

        public void reset() {
            ship.Position = startPoint;
            ship.Velocity = Vector2.Zero;
        }

        public bool collision(Rectangle rA, Color[] dA,
                              Rectangle rB, Color[] dB)
        {

            int top = Math.Max(rA.Top, rB.Top);
            int bottom = Math.Min(rA.Bottom, rB.Bottom);
            int left = Math.Max(rA.Left, rB.Left);
            int right = Math.Max(rA.Right, rB.Right);

            for( int y = top; y < bottom; y++ ) {
                for( int x = left; x < right; x++ ) {
                    Color colorA = dA[(x - rA.Left) + (y - rA.Top) * rA.Width];
                    Color colorB = dB[(x - rB.Left) + (y - rB.Top) * rB.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                        return true;
                }
            }
            
            return false;
        }

        public bool collision(Matrix tA, int wA, int hA, Color[] dA,
                              Matrix tB, int wB, int hB, Color[] dB) {

            Matrix AtoB = tA * Matrix.Invert(tB);

            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, AtoB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, AtoB);

            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, AtoB);

            for(int yA = 0; yA < hA; yA++) {
                Vector2 posInB = yPosInB;
                for(int xA = 0; xA < wA; xA++) {
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    if (0 <= xB && xB < wB &&
                        0 <= yB && yB < hB) {
                            Color colorA = dA[xA + yA * wA];
                            Color colorB = dB[xB + yB * wB];

                            if (colorA.A != 0 && colorB.A != 0)
                                return true;
                    }
                    posInB += stepX;
                }
                yPosInB += stepY;
            }
            return false;
        }

        public Rectangle CalculatingBoundingRectangle(Rectangle rec, Matrix transform) {
            Vector2 leftTop = new Vector2(rec.Left, rec.Top),
                    rightTop = new Vector2(rec.Right, rec.Top),
                    leftBottom = new Vector2(rec.Left, rec.Bottom),
                    rightBottom = new Vector2(rec.Right, rec.Bottom);

            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop), Vector2.Min(leftBottom, rightBottom)),
                    max = Vector2.Max(Vector2.Max(leftTop, rightTop), Vector2.Max(leftBottom, rightBottom));

            return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        public bool intersects(Rectangle a, Rectangle b)
        {
            return (a.Right > b.Left && a.Left < b.Right &&
                    a.Bottom > b.Bottom && a.Top < b.Bottom);
        }

        private void WorldWrap() {
            if (ship.PosX < 0)
                ship.PosX = screenBounds.Width + ship.Source.Width;
            if ( ship.PosX > (screenBounds.Width+ship.Source.Width) )
                ship.PosX = 0;
            if ( ship.PosY < 0 )
                ship.PosY = screenBounds.Height + ship.Source.Height;
            if (ship.PosY > screenBounds.Height+ship.Source.Height)
                ship.PosY = 0;
        }
        #endregion

    }
}
