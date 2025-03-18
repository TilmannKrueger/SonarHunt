using System.Media;

// This is a Windows-only game, due to the use of SoundPlayer, so we disable the warning
#pragma warning disable CA1416

namespace Sonar
{
    internal class SonarHuntGame
    {
        // the SonarGame data 
        private SoundPlayer pingSound;
        private SoundPlayer typeSound;
        private int targetX = 0;
        private int targetY = 0;
        private int playerX = 0;
        private int playerY = 0;
        private int steps = 0;
        private static int fieldSizeX = 10; // the playing field parameters
        private static int fieldSizeY = 10;

        // property that is 'true' when the player reached the target
        private bool Finished { get => playerX == targetX && playerY == targetY; }

        // property that is true when the user selected to play again
        private static bool PlayAgain
        {
            get
            {   
                ClearConsoleInputQueue();
                Console.WriteLine();
                Console.Write("Play again (y/n)?");
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                return keyInfo.Key == ConsoleKey.Y;
            }
        }

        private static void ClearConsoleInputQueue ()
        {
            // swallow everything that is in the input queue
            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }
        }

        private void Teletyper(string line = "")
        {
            if (line != string.Empty)
            {
                typeSound.PlayLooping();
                foreach (char c in line)
                {
                    Console.Write(c);
                    Thread.Sleep(2);
                }
                typeSound.Stop();
            }
            Console.WriteLine();
            Thread.Sleep(10);
        }

        // Print the game instructions
        private void PrintInstructions()
        {
            Teletyper("1996-03-17T08:31:21-11:00 USS Greenville (SSN-772), Bering Strait");
            Teletyper();
            Teletyper("Welcome aboard, Captain! Your mission is to find an enemy submarine hiding in a");
            Teletyper(string.Format("{0}nm by {1}nm area. To detect it, you can use your sonar rangefinder. It emits", fieldSizeX, fieldSizeY));
            Teletyper("a ping that is reflected by the sub. Move your boat across the search area and");
            Teletyper("listen carefully how fast the echo comes back. It is absolutely important that you");
            Teletyper("find the enemy as soon as possible!");
            Teletyper();
            Teletyper("Good Luck!");
            Teletyper();
        }

        // "Give me a ping, Vasili. One ping only, please!"
        private void Ping()
        {
            double distance = Math.Sqrt(Math.Pow(targetX - playerX, 2.0) + Math.Pow(targetY - playerY, 2.0));

            int deltaT = 100 + (int)(distance * 40);

            pingSound.Play();
            Thread.Sleep(deltaT);
            pingSound.Play();
        }

        // let the player make his next move
        private void MakeTurn()
        {
            Console.WriteLine("Our current position is {0} North and {1} East. Awaiting your orders, Sir.", playerY, playerX);
            Console.WriteLine("Move (n, e, s, w) or ping (p)?");
            ClearConsoleInputQueue();
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.WriteLine();
            bool moved = false;
            bool dontMove = false;
            // x direction is East <-> West
            // y direction is North <-> South
            switch (keyInfo.Key)
            {
                case ConsoleKey.N:
                    // move north
                    if (playerY < fieldSizeY - 1)
                    {
                        playerY += 1;
                        moved = true;
                    }
                    break;
                case ConsoleKey.E:
                    // move east
                    if (playerX < fieldSizeX - 1)
                    {
                        playerX += 1;
                        moved = true;
                    }
                    break;
                case ConsoleKey.S:
                    // move south
                    if (playerY > 0)
                    {
                        playerY -= 1;
                        moved = true;
                    }
                    break;
                case ConsoleKey.W:
                    // move west
                    if (playerX > 0)
                    {
                        playerX -= 1;
                        moved = true;
                    }
                    break;
                case ConsoleKey.P:
                    // ping the target, don't move
                    Ping();
                    dontMove = true;
                    break;
                default:
                    // invalid key pressed, don't move
                    Console.WriteLine("I don't understand, Sir! Try again.");
                    dontMove = true;
                    break;
            }

            // in case of error or ping, skip step update
            if (!dontMove)
            {
                // if player tried to leave the area, skip step update and print warning
                if (!moved)
                {
                    Console.WriteLine("We are leaving the search area, Sir! Turn around!");
                }
                else
                {
                    Console.WriteLine("Aye, aye, Sir!");
                    steps++;
                    Ping();
                }
            }
            Console.WriteLine();
        }

        // draws the splash screen
        private static void Splash()
        {
            Console.CursorVisible = false;
            Console.Clear();

            Console.WriteLine("                                                                 -   @                              ");
            Console.WriteLine("        Sonar Hunt - A Submarine Game                            .   @                              ");
            Console.WriteLine("                                                               @ =   @                              ");
            Console.WriteLine("      Written by Tilmann Krueger (2025)                        +- @@ @@                             ");
            Console.WriteLine("                                                                . -@ @@@                         ...");
            Console.WriteLine("      *** Press any Key to continue ***                         @ @+@@@@                        .. .");
            Console.WriteLine(".             .   .. .... ..................   . .   ............:@%@+@%....... ......... ..  ......");
            Console.WriteLine("..........  ............... ........................ ..... .....@%@@@@@@...................    .....");
            Console.WriteLine("   .......  .... .........  .  ........ ............ .. .  .....%@@@@@@@. . ........... ... .  .....");
            Console.WriteLine(".........::::.::::.::.....::.....:.::.:.:.......................@@@@@@@@.. .........................");
            Console.WriteLine("...::::.::::::...:...:::..:::::...:.:....:.:::::::....:...::..:.%@@@@@@@..::.::..:..::::::.::.......");
            Console.WriteLine(":.::::.:.:..::::..:.::::.. ...:.:: .:::  .:.:......::..:..::.:::@@@@@@@@ =.::. -:::..:::.::::::::.::");
            Console.WriteLine("- :::::::::. ::    .::  :.:::::::   :::...::::  .:.:::.:..:.:..:@*@@@@@@:......  ..........:::-:.:.:");
            Console.WriteLine(".  :  ::.. :-:. .:.--  .- .  ::.. .. .:     :.  : :..: :.-    =*%#@@@@@@..:....     ...: ..:::::::..");
            Console.WriteLine(".-:==:.:-- . -    ...-:. ..: .  . :  .        . .  ... %   +@@@=%%@@@@@@+ @* %::-:----------=-------");
            Console.WriteLine("+++=+=++++-++=@===++++=+=*+=++==+=++*++++++*+*+*-+=+*+ ++ @* @@-%%@@@@@@@.@@@@@-=*#++*++*=+***+==-+=");
            Console.WriteLine("#==+--*-#@@@@@@=-#+++=+-+*:+#+##==*++@+==-#=+*+*=+*+*=@@=.@@.@@+%%@@@@@@@@.@@@++-=+%+*@=++*=*#**+*=-");
            Console.WriteLine("=*+#+**+*@@@@@#*+-**-#=++=*=**++#%*@=--:--%***-+%+==**@+=+#@+@@@@@@@@@@@@@@++@:%=%+%**+%+%@+==*=+%+=");
            Console.WriteLine("*++##+*+#@@@@@@@**+**%++*++++=%#-***###=#==+=+##@=@@##@@@*%:##@@##*@*@@=+#+*#@@%%@@@+###%:==+=*=#=##");
            Console.WriteLine("%=+-*=**#@@@@@@@=*##***%*#=+**##-+###=*+*##%@@@%@@==@@@@@@@*=q@@@@@@@@@@##+###@%@@@@#%###*@@#=#%+%++");
            Console.WriteLine("#++#+@%##@@@@@@@%*%+#+#%%-=*#=%@%*#%@@@@@##%%=*+##*%#*##%####%%#@%##**=@#@#@#%%*+@*+@*#@+:*+=@-%-+#=");
            Console.WriteLine("=***%@##+@@@@@@@@**=*#+#*@##+#+=%+@@@@#%%%@%@%%@%*#*@#@%##*%%%%**#@*+%@#@@##*+=%%+**==*+=%#%+-=#=+==");
            Console.WriteLine("%%=*@%*=:@@@@@@@@*=.+@*%##=*=*####@@@%@@@@@@@@@@@@@ =#@@@@@@%@@#*=@%+*#%@+--+#+####*+*@%-:%=@=@:+*##");
            Console.Write("@%#=++=%:@@@@@@@@:+ .+ .        .+.:*####**##%*+++%%*++=+@*@+@%@+#*==+++#*..-+@=-*:+=#%@#%+=+=-+@#-:");

            Console.ReadKey(true);
            Console.CursorVisible = true;
        }

        // constructor
        private SonarHuntGame() 
        {
            // create and set up sounds
            pingSound = new SoundPlayer
            {
                SoundLocation = "ping.wav"
            };
            pingSound.Load();

            typeSound = new SoundPlayer
            {
                SoundLocation = "teletyper.wav"
            };
            typeSound.Load();

            // initialize player and target positions
            playerX = 0;
            playerY = 0;
            Random rand = new Random();
            targetX = rand.Next(1, fieldSizeX);
            targetY = rand.Next(1, fieldSizeY);
            steps = 0;
        }

        // This is where execution starts
        static void Main(string[] args)
        {
            // first, draw the splash screen
            Splash();
            do
            {
                // create an instance of our game
                SonarHuntGame game = new SonarHuntGame();

                // clear the console and print the instructions
                Console.Clear();
                game.PrintInstructions();

                // the game loop: Get player input and advance the game, until the target is reached
                while (!game.Finished)
                {
                    game.MakeTurn();
                }

                // print the victory message
                game.Teletyper(string.Format("Congratulations! You found the enemey submarine at ({0}, {1}) in {2} steps!", game.targetY, game.targetX, game.steps));
                
            } while (PlayAgain); // start over, if player selects 'y'
        }
    }
}
