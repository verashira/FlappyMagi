/******************************************************************************
 * Copyright 2014-2014 Chenchen Xu. All rights reserved.
 *     See the full license in License.txt.
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyMagi
{
    /// <summary>
    /// Background class controls some moving sprites to tile the whole window.
    /// </summary>
    class Background
    {
        private FlappyMagi _game;
        private ResourceManager _resourceManager;

        private float _dayTime;

        public bool IsDay
        {
            get { return _isDay; }
            set { _isDay = value; }
        }
        private bool _isDay;

        // x-axis position
        private float _x1;
        private float _x2;
        private float _x3;

        // |----------|---------|------------
        // x1         x2        x3
        //          |-----------------| Screen   520pixel
        // |----------------------------------
        // ground                                 80pixel
        private Texture2D _texDay;
        private Texture2D _texNight;
        private Texture2D _texGround;

        private Texture2D _texture1;
        private Texture2D _texture2;
        private Texture2D _texture3;

        private readonly int _velocity = Config.GetConfigInteger("BackgroundVelocity");
        private readonly int _oneDayTime = Config.GetConfigInteger("OneDayTime");

        public Background(FlappyMagi game)
        {
            this._game = game;
            this._resourceManager = game.ResourceManager;

            this._dayTime = 0.0f;
            this._isDay = true;

            _texDay = _resourceManager.GetTexture("Back/day");
            _texNight = _resourceManager.GetTexture("Back/night");
            _texGround = _resourceManager.GetTexture("Back/ground");
            _texture1 = _texture2 = _texture3 = _texDay;

            this._x1 = 0;
            this._x2 = _texDay.Width;
            this._x3 = 2 * _texDay.Width;
        }

        /// <summary>
        /// Reset the background.
        /// </summary>
        public void Reset()
        {
            this._isDay = true;
            this._x1 = 0;
            this._x2 = _texDay.Width;
            this._x3 = 2 * _texDay.Width;
            _texture1 = _texture2 = _texture3 = _texDay;
        }

        /// <summary>
        /// Update the background, with its movement and adjust of the offscreen background sprite.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            int elapsed = gameTime.ElapsedGameTime.Milliseconds;
            _dayTime += elapsed;

            if (_dayTime > _oneDayTime)
            {
                _dayTime -= _oneDayTime;
                _isDay = !IsDay;
            }

            float movement = elapsed / 1000.0f * _velocity;
            _x1 += movement;
            _x2 += movement;
            _x3 += movement;

            if (_x1 + _texDay.Width < -10)
            {
                // This can be implemented delicately with Queue, to be exact, Dequeue(Double-ended-queue)
                // but take it sample here.
                _x1 = _x2;
                _x2 = _x3;
                _x3 = _x2 + _texDay.Width;

                _texture1 = _texture2;
                _texture2 = _texture3;
                _texture3 = _isDay ? _texDay : _texNight;
            }
        }

        /// <summary>
        /// Draw the background.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture1, new Rectangle((int)_x1, 0, _texDay.Width, 520), Color.White);
            spriteBatch.Draw(_texture2, new Rectangle((int)_x2, 0, _texDay.Width, 520), Color.White);
            spriteBatch.Draw(_texture3, new Rectangle((int)_x3, 0, _texDay.Width, 520), Color.White);
            spriteBatch.Draw(_texGround, new Rectangle((int)_x1, 520, _texDay.Width, 80), Color.White);
            spriteBatch.Draw(_texGround, new Rectangle((int)_x2, 520, _texDay.Width, 80), Color.White);
            spriteBatch.Draw(_texGround, new Rectangle((int)_x3, 520, _texDay.Width, 80), Color.White);

        }
    }
}
