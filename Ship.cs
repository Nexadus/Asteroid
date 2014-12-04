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
    class Ship
    {
        #region Variables

        private const float SPEED = 5f;
        private const float ROTATE = (float)(Math.PI / 90);
        private Rectangle source;
        private Texture2D shipTexture;
        private Vector2 position, velocity, origin;
        private Random generate;
        private float height, width, rotation, scale;
        public bool boosting, invincible;
        public Color[] imageData;

        #endregion

        #region GetSet

        public Texture2D Texture { get { return shipTexture; } }
        public Rectangle Source {
            get { return source; }
            set {
                source = value;
                height = source.Height * scale;
                width = source.Width * scale;
            }
        }
        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }
        public Vector2 Velocity {
            get { return velocity; }
            set { velocity = value; }
        }
        public int Height {
            get { return (int)height; }
        }
        public int Width {
            get { return (int)width;  }
        }
        public float PosX {
            get { return position.X; }
            set { position.X = value; }
        }
        public float PosY {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public float Rotation {
            get { return rotation; }
            set { rotation = value; }
        }

        #endregion

        #region XNAMethods

        public void LoadContent(ContentManager contentManager) {
            shipTexture = contentManager.Load<Texture2D>(@"Sprites\Spaceship");
            height = shipTexture.Height;
            width = shipTexture.Width;
            source = new Rectangle(0, 0, shipTexture.Width, shipTexture.Height);
            origin = new Vector2(shipTexture.Width, shipTexture.Height) * 0.5f;
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(shipTexture, position, source, Color.White,
                             rotation, origin, scale, SpriteEffects.None, 0);

        }

        public void Update(GameTime gameTime, KeyboardState currKS) {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currKS.IsKeyDown(Keys.Up) || currKS.IsKeyDown(Keys.W))
                Accelerate(SPEED);
            else
                boosting = false;
            if (currKS.IsKeyDown(Keys.Left) || currKS.IsKeyDown(Keys.A))
                rotation -= ROTATE;
            if (currKS.IsKeyDown(Keys.Right) || currKS.IsKeyDown(Keys.D))
                rotation += ROTATE;
            
            
            position += velocity * elapsed;
        }

        #endregion

        #region ShipMethods

        public Ship(ContentManager content, Vector2 start) {
            LoadContent(content);
            generate = new Random();
            rotation = 0;
            scale = 1.0f;
            origin = Vector2.Zero;
            imageData = new Color[shipTexture.Height * shipTexture.Width];
            shipTexture.GetData(imageData);
            position = start;
        }

        public void Accelerate(float magnitutde) {
            Vector2 thrust;
            thrust.X = (float)Math.Sin(rotation) * magnitutde;
            thrust.Y = -(float)Math.Cos(rotation) * magnitutde;
            boosting = true;

            velocity += thrust;
        }

        public void Jump() {
            position = new Vector2(generate.Next(800), generate.Next(600));
        }
        
        public Matrix Transform() {
           return Matrix.CreateTranslation(new Vector3(new Vector2((int)-width / 2, (int)-height / 2), 0.0f)) *
                        Matrix.CreateRotationZ(rotation) *
                        Matrix.CreateTranslation(new Vector3(position, 0.0f));

        }

        #endregion
    }
}
