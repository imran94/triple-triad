using System;

namespace Game4
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    /// 

    

    public static class Program
    {
        public static Boolean restart;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            do
            {
                restart = false;
                using (var game = new Game1())
                    game.Run();
            } while (restart == true);
        }
    }
#endif
}
