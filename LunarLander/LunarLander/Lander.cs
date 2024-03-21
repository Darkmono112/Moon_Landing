using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LunarLander
{
    public class Lander
    {

        float fuel;
        Vector2 gravity;
        Vector2 thrust;
        Vector2 position;

        // position = position + thrust - gravity
        // Need to check collision, hitbox, movement, and make a draw function for the game view
        // Also need a function to return the amount of fuel left as Full, mid, and empty. 
        // Make the rectangle here
        // Add the texture here
        // Check collision every update

        public Lander() { }
        
        

        public void moveUP() { }
        public void rotateRight() { }
        public void rotateLeft() { }


    }
}
