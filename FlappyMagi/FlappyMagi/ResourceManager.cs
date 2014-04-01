/******************************************************************************
 * Copyright 2014-2014 Chenchen Xu. All rights reserved.
 *     See the full license in License.txt.
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlappyMagi
{
    /// <summary>
    /// Resource manager manage the loading and caching of game resources.
    /// It's simply implemented here, with all resources loaded at the beginning.
    /// </summary>
    public class ResourceManager
    {
        private Dictionary<string, object> _resources = new Dictionary<string, object>();
        private ContentManager _content;

        private static string[] _texResourcesList = new []
        {
            // Magi sprites
            "Magi/up.0",
            "Magi/up.20",
            "Magi/up.-20",
            "Magi/up.-90",
            "Magi/down.0",
            "Magi/down.20",
            "Magi/down.-20",
            "Magi/down.-90",
            "Magi/mid.0",
            "Magi/mid.20",
            "Magi/mid.-20",
            "Magi/mid.-90",
            
            // Background sprites
            "Back/day",
            "Back/night",
            "Back/ground",

            // Pipe sprites
            "Pipe/top",
            "Pipe/bottom"
        };

        private static string[] _fontResourcesList = new []
        {
            "DefaultFont"
        };

        public ResourceManager(ContentManager content)
        {
            _content = content;
        }

        /// <summary>
        /// Load all registered resources.
        /// </summary>
        public void LoadAll()
        {
            foreach (string r in _texResourcesList)
                _resources.Add(r, _content.Load<Texture2D>(r));
            foreach (string r in _fontResourcesList)
                _resources.Add(r, _content.Load<SpriteFont>(r));
        }

        /// <summary>
        /// Fetch a texture resource.
        /// </summary>
        /// <param name="name">Texture name.</param>
        /// <returns>Fetched texture.</returns>
        public Texture2D GetTexture(string name)
        {
            if (!_resources.ContainsKey(name))
                return null;

            return _resources[name] as Texture2D;
        }

        public SpriteFont GetFont(string name)
        {
            if (!_resources.ContainsKey(name))
                return null;

            return _resources[name] as SpriteFont;
        }
    }
}
