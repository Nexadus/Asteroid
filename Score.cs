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
    class Score
    {
        public Vector2 scorePos, livesPos, gameOver;

        public int score, lives;

        public Score(Rectangle boundary) {
            scorePos = new Vector2(1, 1);
            livesPos = new Vector2(boundary.Width - (10 * 5), 1);
            gameOver = new Vector2(50 , 100);
        }
        public SpriteFont Font
        {
            get;
            set;
        }

        public SpriteFont gameOverFont {
            get;
            set;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, "Score: " + score.ToString(), scorePos, Color.White);
            spriteBatch.DrawString(Font, "Lives: " + lives.ToString(), livesPos, Color.White);
            if (lives < 0)
                spriteBatch.DrawString(gameOverFont, "GAME OVER", gameOver, Color.White, 0.0f, Vector2.Zero, 6, SpriteEffects.None, 1f);
        }
    }
}
