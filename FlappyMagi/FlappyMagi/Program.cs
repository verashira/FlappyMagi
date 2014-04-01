/******************************************************************************
 * Copyright 2014-2014 Chenchen Xu. All rights reserved.
 *     See the full license in License.txt.
******************************************************************************/

using System;

namespace FlappyMagi
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (FlappyMagi game = new FlappyMagi())
            {
                game.Run();
            }
        }
    }
#endif
}

