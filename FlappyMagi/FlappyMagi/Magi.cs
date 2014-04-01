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
using Microsoft.Xna.Framework.Input;

namespace FlappyMagi
{
    /// <summary>
    /// Magi is magi, ugh, also the bird.
    /// </summary>
    class Magi
    {
        private FlappyMagi _game;
        private ResourceManager _resourceManager;
        private Vector2 _pos;
        private Vector2 _velocity;
        private int _currentFrame;
        private float _time;

        /// <summary>
        /// Magi is dead, but it can be not over yet, it's maybe on falling.
        /// </summary>
        public bool IsDead { get { return _isDead; } }
        private bool _isDead;

        /// <summary>
        /// Magi is completely dead only when it hits the ground.
        /// </summary>
        public bool IsOnGround { get { return _isOnGround; } }
        private bool _isOnGround;

        private const double _degreeToRadian = 57.2957795131;
        private readonly float _flapVelocity = Config.GetConfigInteger("FlapVelocity");
        private readonly float _gravityAcc = Config.GetConfigInteger("GravityAcceleration"); // 980;
        private readonly float _deadVelocity = Config.GetConfigInteger("DeadVelocity");
        private readonly float _backVelocity = Config.GetConfigInteger("BackgroundVelocity");

        private Texture2D[] _tex0  ;
        private Texture2D[] _texM20;
        private Texture2D[] _tex20 ;
        private Texture2D[] _tex90 ;
        
        /// <summary>
        /// Return which direction magi is looking at, up, forward or down.
        /// </summary>
        private int LookDirection
        {
            get
            {
                // Reverse the y value for the y-axis is pointing down.
                // Reverse the x value for the magi is moving in the reverse direction of
                // the background, background moves to the left so that the magi "looks"
                // moving to the right.
                double angle = Math.Atan2(-_velocity.Y, -_backVelocity);

                if (angle > 0)
                    return 0;
                if (angle > -15 * _degreeToRadian)
                    return 1;
                if (angle > -75 * _degreeToRadian)
                    return 2;
                return 3;
            }
        }

        /// <summary>
        /// Return the texture corresponed for the current frame and direction.
        /// </summary>
        private Texture2D CurrentTexture
        {
            get 
            {
                int lookDirection = LookDirection;
                switch (lookDirection)
                {
                    case 0:
                        return _tex20[_currentFrame];
                    case 1:
                        return _tex0[_currentFrame];
                    case 2:
                        return _texM20[_currentFrame];
                    case 3:
                        return _tex90[_currentFrame];
                    default:
                        return null; // Which is impossible
                }
            }
        }

        /// <summary>
        /// Return the bounding rectangle of the magi.
        /// </summary>
        public Rectangle Bound
        {
            get
            {
                return new Rectangle((int)_pos.X, (int)_pos.Y, _tex0[0].Width - 4, _tex0[0].Height - 4);
            }
        }

        public Magi(FlappyMagi game, Vector2 pos, Vector2 velocity)
        {
            this._game = game;
            this._resourceManager = game.ResourceManager;
            this._pos = pos;
            this._velocity = velocity;
            this._isDead = false;
            this._isOnGround = false;
            
            this._currentFrame = 0;
            this._time = 0.0f;

            _tex0 = new Texture2D[]
            {
                _resourceManager.GetTexture("Magi/up.0"),
                _resourceManager.GetTexture("Magi/mid.0"),
                _resourceManager.GetTexture("Magi/down.0")
            };
            _texM20 = new Texture2D[]
            {
                _resourceManager.GetTexture("Magi/up.-20"),
                _resourceManager.GetTexture("Magi/mid.-20"),
                _resourceManager.GetTexture("Magi/down.-20")
            };
            _tex20 = new Texture2D[]
            {
                _resourceManager.GetTexture("Magi/up.20"),
                _resourceManager.GetTexture("Magi/mid.20"),
                _resourceManager.GetTexture("Magi/down.20")
            };
            _tex90 = new Texture2D[]
            {
                _resourceManager.GetTexture("Magi/up.-90"),
                _resourceManager.GetTexture("Magi/mid.-90"),
                _resourceManager.GetTexture("Magi/down.-90")
            };
        }

        /// <summary>
        /// Reset the magi to certain position and to be of the velocity.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="velocity"></param>
        public void Reset(Vector2 pos, Vector2 velocity)
        {
            this._pos = pos;
            this._velocity = velocity;
            this._isDead = false;
            this._isOnGround = false;

            this._currentFrame = 0;
            this._time = 0.0f;
        }

        /// <summary>
        /// Let the magi die.
        /// </summary>
        public void Die()
        {
            if (_isDead)
                return;

            _isDead = true;
            _velocity = new Vector2(0, _deadVelocity);
        }

        /// <summary>
        /// Update the magi.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            float elapsed = gameTime.ElapsedGameTime.Milliseconds;

            // If the magi is already on the ground, let it be here silently.
            if (_isOnGround)
                return;

            // The magi is controllable and influenced by gravity only when
            // it is still alive.
            if (!_isDead)
            {
                _velocity += new Vector2(0, _gravityAcc * elapsed / 1000.0f);

                if (_game.KeyboardInput.IsKeyDown(Keys.Space))
                    _velocity.Y = _flapVelocity;
            }

            _pos += (float)(elapsed / 1000.0) * _velocity;

            // Check whether the magi is on the ground now.
            if (_pos.Y > 525)
            {
                Die();
                _isOnGround = true;
            }

            // Update the texture frame.
            _time += elapsed;
            if (_time > 100)
            {
                _currentFrame = (_currentFrame + 1) % _tex0.Length;
                _time -= 100.0f;
            }
        }

        /// <summary>
        /// Draw the magi.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CurrentTexture, _pos, Color.White);
        }
    }
}
