using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/*
 Cool ass stuff people could implement:
 > jumping
 > attacking
 > randomly moving monsters
 > smarter moving monsters
*/
namespace asciiadventure {
    public class Game {
        Player player;
        private Random random = new Random();
        private static Boolean Eq(char c1, char c2){
            return c1.ToString().Equals(c2.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        private static string Menu() {
            return "WASD to move\nIJKL to attack/interact\nQ to quit\n\nNomnoms: " + Player.Noms + "\nTreasure Chest: " + Player.Chest + "\n\nEnter command: \n";
        }

        private static void PrintScreen(Screen screen, string message, string menu) {
            Console.Clear();
            Console.WriteLine(screen);
            Console.WriteLine($"\n{message}");
            Console.WriteLine($"\n{menu}");
        }
        public void Run() {
            Console.ForegroundColor = ConsoleColor.Green;

            Screen screen = new Screen(10, 10);
            // add a couple of walls
            for (int i=0; i < 3; i++){
                new Wall(1, 2 + i, screen);
            }
            for (int i=0; i < 4; i++){
                new Wall(3 + i, 4, screen);
            }
            
            // add a player
            player = new Player(0, 0, screen, "Zelda");
            
            // add a treasure
            Treasure treasure = new Treasure(5, 2, screen);
            //set initial value left to 1
            Player.TLeft = 1;

            //adding teleport
            Teleport teleport1 = new Teleport(9, 0, screen);

            //adding nomnoms
            Nomnom nom1 = new Nomnom(8, 3, screen);
            Nomnom nom2 = new Nomnom(6, 7, screen);
            Nomnom nom3 = new Nomnom(2, 6, screen);
            //set inital val left to 3
            Player.NomsLeft = 3;


            // add some monsters
            List<Monster> mons = new List<Monster>();
            mons.Add(new Monster(9, 9, screen));
            
            // initially print the game board
            String setup = "Welcome!\nCollect at least 2 Nomnoms ✧ ";
            setup += "and 1 Treasure 𓇽  to win.\nMake sure not to ";
            setup += "let the monster get you or the items!\n\n";
            setup += "Use the teleportation device Ⓞ  to travel\n";
            setup += "from the bottom of the board to the top.\n";
            setup += "And *one* more thing -- make sure not to ";
            setup += "step\non items and squish them!\n\nGood luck ＞ᴗ＜";
            PrintScreen(screen, setup, Menu());
            
            Boolean gameOver = false;
            
            while (!gameOver) {
                char input = Console.ReadKey(true).KeyChar;

                String message = "";

                if (Eq(input, 'q')) {
                    break;
                } else if (Eq(input, 'w')) {
                    player.Move(-1, 0);
                } else if (Eq(input, 's')) {
                    player.Move(1, 0);
                } else if (Eq(input, 'a')) {
                    player.Move(0, -1);
                } else if (Eq(input, 'd')) {
                    player.Move(0, 1);
                } else if (Eq(input, ' ')) {
                    //hop foward
                    player.Move(-2, 0);
                }else if (Eq(input, 'i')) {
                    message += player.Action(-1, 0) + "\n";
                } else if (Eq(input, 'k')) {
                    message += player.Action(1, 0) + "\n";
                } else if (Eq(input, 'j')) {
                    message += player.Action(0, -1) + "\n";
                } else if (Eq(input, 'l')) {
                    message += player.Action(0, 1) + "\n";
                } else if (Eq(input, 'v')) {
                    // TODO: handle inventory
                    message = "You have nothing\n";
                } else {
                    message = $"Unknown command: {input}";
                }

                // OK, now move the monster
                foreach (Monster mon in mons){
                    // TODO: Make monsters smarter, so they jump on the player, if it's possible to do so
                    List<Tuple<int, int>> moves = screen.GetLegalMoves(mon.Row, mon.Col);
                    if (moves.Count == 0){
                        continue;
                    }
                    //monsters move randomly
                    var (deltaRow, deltaCol) = moves[random.Next(moves.Count)];
                    
                    if (screen[mon.Row + deltaRow, mon.Col + deltaCol] is Player){
                        // the monster got the player!
                        mon.Token = "💀";
                        message += "Oh no! A monster got you *෴ *\nGAME OVER\n";
                        gameOver = true;
                    }
                    else if (screen[mon.Row + deltaRow, mon.Col + deltaCol] is Treasure){
                        // the monster got the Treasure
                        Player.TLeft -= 1;
                        if(Player.TLeft == 0) {
                            message += "Oh no! The Treasure was stolen!";
                            message += "\nGAME OVER  *෴ *\n\n";
                            gameOver = true;
                        }

                    }
                    else if (screen[mon.Row + deltaRow, mon.Col + deltaCol] is Nomnom){
                        // the monster got a Nomnom
                        Player.NomsLeft -= 1;
                        if(Player.NomsLeft < 2) {
                            message += "Oh no! A Nomnom is gone and there\naren't enough left for you!";
                            message += "\nGAME OVER  *෴ *\n\n";
                            gameOver = true;
                            break;
                        }
                        else {
                            message += "Oh no! A Nomnom was eaten!";
                            message += "\nThere are " + Player.NomsLeft + " remaining.";
                        }
                    }
                    mon.Move(deltaRow, deltaCol);
                }

                //win game
                if (Player.Noms == 2 && Player.Chest == 1) {
                    message = "Great job; you won the game!\n\n~(‾▿‾~)(~‾▿‾)~";
                    gameOver = true;
                    PrintScreen(screen, message, "");
                    break;
                }

                PrintScreen(screen, message, Menu());
            }
        }

        public static void Main(string[] args){
            Game game = new Game();
            game.Run();
        }
    }
}