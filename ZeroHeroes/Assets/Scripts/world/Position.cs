/* Position.cs
 * 
 * Author:  Nicholas Ruotsalainen RUOT0003
 * Created: 17/08/2020
 * 
 * Last Edited By: Nicholas Ruotsalainen RUOT0003
 * Last Updated:   18/08/2020
 */

using UnityEngine;

namespace Assets.Scripts.world
{
    public class Position
    {
        private int x, y;

        public Position(int _x, int _y) {
            this.x = _x;
            this.y = _y;
        }

        public int X {
            get { return x; }
        }
        public int Y {
            get { return y; }
        }

        public bool Equals(Position _position) {
            return Equals(_position.X, _position.Y);
        }
        public bool Equals(int _x, int _y) {
            return x == _x && y == _y;
        }
        public string ToString() {
            return string.Format("{0}, {1}", x, y);
        }

        public Vector2 ToVector() {
            return new Vector2(x, y);
        }

        public bool IsWithinWorldBounds() {
            return x >= 0 && x < GameController.Instance.World.Width &&
                    y >= 0 && y < GameController.Instance.World.Height;
        }
    }
}
