/******************************************************************************
 * Copyright 2014-2014 Chenchen Xu. All rights reserved.
 *     See the full license in License.txt.
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FlappyMagi
{
    static class Config
    {
        private static readonly string _configFilename = "config.ini";
        private static Dictionary<string, string> _configs = new Dictionary<string, string>();

        static Config()
        {
            if (!File.Exists(_configFilename))
            {
                LoadDefaultConfig();
                return;
            }

            try
            {
                using (FileStream fs = File.Open("config.ini", FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fs))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] s = line.Split('=');
                        _configs.Add(s[0], s[1]);
                    }
                }
            }
            catch
            {
                _configs.Clear();
                LoadDefaultConfig();
            }
        }

        /// <summary>
        /// Load all configs to default value.
        /// </summary>
        private static void LoadDefaultConfig()
        {
            _configs.Add("OneDayTime", "10000"); // Time of a day in milliseconds, then night, and then back to day.
            _configs.Add("BackgroundVelocity", "-500");
            _configs.Add("FirstPipeX", "900");
            _configs.Add("PipeStride", "400"); // Distance between two pipes.
            _configs.Add("PipeIntervalMin", "165"); // The vertical distance between two pipes in a group, with one top one bottom.

            // Magi
            _configs.Add("FlapVelocity", "-420"); // Velocity when flap the magi.
            _configs.Add("GravityAcceleration", "1850"); // Gravity acceleration.
            _configs.Add("DeadVelocity", "600"); // Velocity when it died.
        }

        public static int GetConfigInteger(string name)
        {
            return int.Parse(_configs[name]);
        }
    }
}
