using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LunarLander
{
    public class Lander
    {

        float fuel;
        public Vector2 position;
        public Texture2D texture;
        float speed;
        public Rectangle hitbox;

        public SoundEffect boost;
        // position = position + thrust - gravity
        // Need to check collision, hitbox, movement, and make a draw function for the game view
        // Also need a function to return the amount of fuel left as Full, mid, and empty. 
        // Make the rectangle here
        // Add the texture here
        // Check collision every update

        public Lander() {
            speed = 15f;
            position = new Vector2((int)1920/2, (int)200);
            fuel = 100000f;
            hitbox = new Rectangle((int)position.X,(int) position.Y, 40, 40);
        }


        public void resetPositon()
        {
            
            position = new Vector2((int)1920 / 2, (int)200);
            fuel = 100000f;
            hitbox = new Rectangle((int)position.X, (int)position.Y, 40, 40);
        }


        
        

        public bool colision(List<Vector3> terrain , List<Vector3> safeZone)
        {

            for (int i = 0; i < terrain.Count; i++)
            {
                if((int)position.X <= (int)terrain[i].X   && (int)position.Y == (int)terrain[i].Y +32 )
                {
                    if (safeZone.Contains(terrain[i]))
                    {
                        return true;
                    }

                }
            }

            return false;

        }
        public bool colision2(List<Vector3> terrain, List<Vector3> safeZone)
        {

            for (int i = 0; i < terrain.Count; i++)
            {
                if ((int)position.X == (int)terrain[i].X && (int)position.Y == (int)terrain[i].Y)
                {
                    if (!safeZone.Contains(terrain[i]))
                    {
                        return true;
                    }
                    

                }
            }

            return false;

        }

        public void applyGrav()
        {
            position += new Vector2(0, .8f);
            hitbox = new Rectangle((int)position.X, (int) position.Y, 32, 32);

        }

        public void moveUP()  //Boost 
        {
            position += new Vector2(0,-1f);
            fuel -= 2;
            
            
        }
        public void rotateRight() {
            position += new Vector2(.5f, 0);
        }
        public void rotateLeft() {
            position += new Vector2(-.5f, 0); 
            

        }


    }
}
