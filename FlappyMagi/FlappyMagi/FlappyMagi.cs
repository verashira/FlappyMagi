/*******************************************************************************\
 * Copyright 2014-2014 Chenchen Xu                                              *
 * All rights reserved.                                                         *
 * Redistribution and use in source and binary forms, with or without           *
 * modification, are permitted provided that the following conditions are met:  *
 *                                                                              *
 * 1. Redistributions of source code must retain the above copyright notice,    *
 *    this list of conditions and the following disclaimer.                     *
 * 2. Redistributions in binary form must reproduce the above copyright notice, *
 *    this list of conditions and the following disclaimer in the documentation *
 *    and/or other materials provided with the distribution.                    *
 * 3. Neither the name of the copyright holder nor the names of its             *
 *    contributors may be used to endorse or promote products derived from this *
 *    software without specific prior written permission.                       *
 *                                                                              *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"  *
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE    *
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE   *
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE    *
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR          *
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF         *
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS     *
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN      *
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)      *
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE   *
 * POSSIBILITY OF SUCH DAMAGE.                                                  *
 *                                                                              *
 * IF WITHOUT ANY FURTHER DECLARATION, EVERY SOURCE CODE FILE IN THIS PROJECT   *
 * OF COPYRIGHT TO CHENCHEN XU IS UNDER THE SAME LICENSE DOCUMENTED HERE.       *
\*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FlappyMagi
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FlappyMagi : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont _font;

        private Magi _magi;
        private Background _back;
        private PipeWave _pipeWave;

        private readonly Vector2 _magiStartPos = new Vector2(100, 100);

        public enum GameState { InGame, GameOver }
        private GameState _state;

        public ResourceManager ResourceManager
        {
            get { return _resourceManager; }
        }
        private ResourceManager _resourceManager;

        public KeyboardState KeyboardInput
        {
            get { return _keyboardInput; }
        }
        private KeyboardState _keyboardInput;

        public FlappyMagi()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 600;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0 / 60);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _resourceManager = new ResourceManager(Content);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _resourceManager.LoadAll();
            _font = _resourceManager.GetFont("DefaultFont");

            _magi = new Magi(this, _magiStartPos, Vector2.Zero);
            _back = new Background(this);
            _pipeWave = new PipeWave(this);

            _state = GameState.InGame;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _keyboardInput = Keyboard.GetState();

            // TODO: Add your update logic here
            switch (_state)
            {
                case GameState.InGame:
                    {   // In Game.
                        if (!_magi.IsOnGround)
                        {
                            _back.Update(gameTime);
                            _pipeWave.Update(gameTime);
                            _magi.Update(gameTime);

                            if (_pipeWave.Collide(_magi))
                                _magi.Die();

                            _pipeWave.JudgeScore(_magi);
                        }
                        else
                            _state = GameState.GameOver;
                    }
                    break;
                case GameState.GameOver:
                    {
                        if (_keyboardInput.IsKeyDown(Keys.R))
                        {
                            _back.Reset();
                            _pipeWave.Reset();
                            _magi.Reset(_magiStartPos, Vector2.Zero);
                            _state = GameState.InGame;
                        }
                    }
                    break;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            _back.Draw(spriteBatch);
            _pipeWave.Draw(spriteBatch);
            _magi.Draw(spriteBatch);

            if (_state == GameState.InGame)
            {
            }
            else
                spriteBatch.DrawString(_font, "Dead, Press R to restart.", new Vector2(250 - 135, 300 - 10), Color.Red);

            spriteBatch.DrawString(_font, "Score: " + _pipeWave.Score.ToString("D2"), new Vector2(15, 15), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
