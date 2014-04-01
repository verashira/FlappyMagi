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
    /// Pipe wave controls many pipes and manage their movement and function.
    /// </summary>
    class PipeWave
    {
        private FlappyMagi _game;
        private ResourceManager _resourceManager;

        // Double-ended-queue is better, but that it's for me to write is bad.
        private Queue<Pipe> _pipes = new Queue<Pipe>(10);
        private Random rnd = new Random();

        Texture2D _texPipeTop;
        Texture2D _texPipeBottom;

        private readonly int _startX = Config.GetConfigInteger("FirstPipeX"); // The first pipe starts here.
        private readonly int _pipeStride = Config.GetConfigInteger("PipeStride"); // Distance between two pipes.
        // The vertical distance between two pipes in a group, with one top one bottom.
        private readonly int _pipeIntervalMin = Config.GetConfigInteger("PipeIntervalMin"); 

        public int Score
        {
            get { return _score; }
        }
        private int _score;

        /// <summary>
        /// Generate the y position for two pipes, of which one will be at the top,
        /// the other at the bottom.
        /// </summary>
        /// <returns></returns>
        private Tuple<float, float> SelectPipePos()
        {
            float y1 = (float)rnd.NextDouble() * 200 - 200;
            float y2 = (float)rnd.NextDouble() * 240 + 240;
            
            // 300 is for the vertical length of the top pipe
            if (y2 - y1 < 300 + _pipeIntervalMin)
                y2 = y1 + 300 + _pipeIntervalMin;
            //if (y2 - y1 < 400)
            //    y2 = y1 + 450;
            if (y2 < 300)
                y2 = 301;

            return new Tuple<float, float>(y1, y2);
        }

        public PipeWave(FlappyMagi game)
        {
            this._game = game;
            this._resourceManager = game.ResourceManager;

            _texPipeTop = _resourceManager.GetTexture("Pipe/top");
            _texPipeBottom = _resourceManager.GetTexture("Pipe/bottom");

            int startX = _startX;
            for (int i = 0; i < 3; ++i)
            {
                var ys = SelectPipePos();
                float y1 = ys.Item1;
                float y2 = ys.Item2;

                Pipe pipeTop = new Pipe(new Vector2(startX + i * _pipeStride, y1), _texPipeTop);
                Pipe pipeBottom = new Pipe(new Vector2(startX + i * _pipeStride, y2), _texPipeBottom);
                _pipes.Enqueue(pipeTop);
                _pipes.Enqueue(pipeBottom);
            }
        }

        /// <summary>
        /// Reset the pipe wave and all pipes the same.
        /// </summary>
        public void Reset()
        {
            _score = 0;

            _pipes.Clear();
            int startX = _startX;
            for (int i = 0; i < 3; ++i)
            {
                var ys = SelectPipePos();
                float y1 = ys.Item1;
                float y2 = ys.Item2;

                Pipe pipeTop = new Pipe(new Vector2(startX + i * _pipeStride, y1), _texPipeTop);
                Pipe pipeBottom = new Pipe(new Vector2(startX + i * _pipeStride, y2), _texPipeBottom);
                _pipes.Enqueue(pipeTop);
                _pipes.Enqueue(pipeBottom);
            }
        }

        /// <summary>
        /// Update the pipe wave and adjust all pipes.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            foreach (Pipe p in _pipes)
                p.Update(gameTime);

            Pipe p1 = _pipes.Peek();
            Rectangle bound = p1.Bound;
            if (bound.X + bound.Width < -5)
            {
                Pipe lastP = _pipes.Last();

                // Dequeue the first two pipes, within which one is on top and another bottom,
                // as they are now out of the screen.
                _pipes.Dequeue(); // p1
                Pipe p2 = _pipes.Dequeue();

                var ys = SelectPipePos();
                float y1 = ys.Item1;
                float y2 = ys.Item2;

                p1.Reset(new Vector2(lastP.Bound.X + _pipeStride, y1));
                p2.Reset(new Vector2(lastP.Bound.X + _pipeStride, y2));
                _pipes.Enqueue(p1);
                _pipes.Enqueue(p2);
            }
        }

        /// <summary>
        /// Draw all pipes.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Pipe p in _pipes)
                p.Draw(spriteBatch);
        }

        /// <summary>
        /// Judge whether any pipe is collided with the cute magi(the bird).
        /// </summary>
        /// <param name="magi"></param>
        /// <returns></returns>
        public bool Collide(Magi magi)
        {
            Rectangle boundMagi = magi.Bound;
            foreach (Pipe p in _pipes)
            {
                Rectangle boundPipe = p.Bound;
                if (boundMagi.Intersects(boundPipe))
                     return true;
            }
            return false;
        }

        /// <summary>
        /// Update the score, one point will be added the player for when
        /// the magi passed every two pipes, top and bottom.
        /// You need to access the Score property to get the current score.
        /// </summary>
        /// <param name="magi"></param>
        public void JudgeScore(Magi magi)
        {
            Rectangle boundMagi = magi.Bound;

            int count = 0;
            foreach (Pipe p in _pipes)
            {
                if (p.IsScored) continue;

                Rectangle bound = p.Bound;
                if (boundMagi.X > bound.X + bound.Width + 5)
                {
                    p.IsScored = true;
                    count++;
                }
            }
            _score += count / 2; // Every two pipes (top and bottom) for 1 score.
        }
    }
}
