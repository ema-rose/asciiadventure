using System;

namespace asciiadventure {
    public class Monster : MovingGameObject {
        public Monster(int row, int col, Screen screen) : base(row, col, "☿ ", screen) {}
    }
}