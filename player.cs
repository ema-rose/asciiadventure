using System;

namespace asciiadventure {
    class Player : MovingGameObject {
        public Player(int row, int col, Screen screen, string name) : base(row, col, "웃", screen) {
            Name = name;
        }
        public string Name {
            get;
            protected set;
        }

        //keep track of a players noms!
        static public int Noms {
            get;
            set;
        }

        //keep track of remaining noms
        static public int NomsLeft{
            get;
            set;
        }

        //create treasure chest
        static public int Chest {
            get;
            set;
        }
        //keep track of remaining treasure
        static public int TLeft{
            get;
            set;
        }


        public override Boolean IsPassable(){
            return true;
        }

        public String Action(int deltaRow, int deltaCol){
            int newRow = Row + deltaRow;
            int newCol = Col + deltaCol;
            if (!Screen.IsInBounds(newRow, newCol)){
                return "nope";
            }
            GameObject other = Screen[newRow, newCol];
            if (other == null){
                return "negative";
            }
            // TODO: Interact with the object
            if (other is Treasure){
                //add the treasure to the chest
                Player.Chest += 1;
                other.Delete();
                return "Yay, we got the treasure!";
            }

            if(other is Teleport) {
                this.Delete();
                this.Row = 1;
                this.Col = 8;
                return "꧁     Whooooosh -- off we go!\nNow step off the transport!";
            }

            if(other is Nomnom) {
                Player.Noms += 1;
                other.Delete();
                return "Yummy!";
            }


            return "ouch";
        }
    }
}