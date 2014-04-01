/******************************************************************************
 * Copyright 2014-2014 Chenchen Xu. All rights reserved.
 *     See the full license in License.txt.
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FlappyMagi
{
    /// <summary>
    /// Pipe class represents one pipe, which can be on top or bottom
    /// based on the texture passed in.
    /// </summary>
    class Pipe
    {
        private Texture2D _texPipe;

        private Vector2 _pos;

        /// <summary>
        /// Represent whether this pipe has been counted to add scores.
        /// </summary>
        public bool IsScored 
        {
            set { _isScored = true; }
            get { return _isScored; }
        }
        private bool _isScored;

        // Keeping this same with the velocity of background is fine.
        private readonly int _velocity = -500;

        public Pipe(Vector2 pos, Texture2D _tex)
        {
            this._pos = pos;
            this._texPipe = _tex;
            this._isScored = false;
        }
        
        /// <summary>
        /// Reset the pipe to certain position.
        /// </summary>
        /// <param name="pos"></param>
        public void Reset(Vector2 pos)
        {
            this._pos = pos;
            this._isScored = false;
        }

        /// <summary>
        /// Update the pipe.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _pos.X += gameTime.ElapsedGameTime.Milliseconds / 1000.0f * _velocity;
        }

        /// <summary>
        /// Draw the pipe.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texPipe, new Rectangle((int)_pos.X, (int)_pos.Y, _texPipe.Width, 300), Color.White);
        }

        /// <summary>
        /// Return the bounding rectangle of the pipe.
        /// </summary>
        public Rectangle Bound
        {
            get
            {
                return new Rectangle((int)_pos.X, (int)_pos.Y, _texPipe.Width - 7, 300 - 4);
            }
        }
    }
}
