using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;


namespace LunarLander
{
    internal class Terrain
    {
        // Where we generate and draw the terrain
        // Max height of the terrain is half of the screen
        // Min height of the terrain is 0
        // safe zone is 1/4 of the screen width
        private VertexPositionColor[] _strip;
        private int[] _indexStrip;
        private int maxHeight;
        private int pointCount;
        private int minHeight;
        

        public Terrain(int x , int y) {

            maxHeight = y / 3;
            minHeight = y;
            pointCount = x;
            
            
            // 1 point for every 2 pixels 
            
           // generateTerrarin(0,x);
            makeTriStrip();
        }


        // Make makeTriStrip use an array of Vector 3 for this. 
            // Call make strip then chose a point on he strip
            // That point will now be the safe zone and will be saved in an array
            // and drawn flat based on first index
            // AKA clone the vector 3 but change the x for every other point

        private void generateTerrarin(int Start, int End)
        {
            Console.WriteLine("generated Terrarain");

            

            
        }

        private void makeTriStrip()
        {
            //_strip = new VertexPositionColor[1920];

            // will need change to include different hight levels 
            /*_strip[0].Position = new Vector3(0,1080,0);
            _strip[0].Color = Color.Blue;
            for (int i = 1; i < 1920-1; i++)
             {
                 _strip[i].Position = new Vector3(i* 64, 600, 0);
                 _strip[i].Color = Color.Blue; 
                 
                 i++;
                 _strip[i].Position = new Vector3(i * 64, 1080, 0);
                 _strip[i].Color = Color.Blue;
                 

             }

             _indexStrip = new int[1922];
             for (int i = 0; i < 1920; i++)
             {
                 _indexStrip[i] = i;
             }*/

            /*_strip = new VertexPositionColor[5];
            _strip[0].Position = new Vector3(0, 1080, 0);
            _strip[0].Color = Color.Red;
            _strip[1].Position = new Vector3(0, 600, 0);
            _strip[1].Color = Color.Red;
            _strip[2].Position = new Vector3(1920, 600, 0);
            _strip[2].Color = Color.Red;

            _strip[3].Position = new Vector3(1920, 1080, 0);
            _strip[3].Color = Color.Blue;
            *//*_strip[4].Position = new Vector3(4000, 600, 0);
            _strip[4].Color = Color.Blue;*//*



            _indexStrip = new int[6];
            _indexStrip[0] = 0;
            _indexStrip[1] = 1;
            _indexStrip[2] = 2;
            _indexStrip[3] = 3;
            //_indexStrip[4] = 4;*/


            _strip = new VertexPositionColor[7];
            _strip[0].Position = new Vector3(200, 600, 0);
            _strip[0].Color = Color.Red;
            _strip[1].Position = new Vector3(300, 400, 0);
            _strip[1].Color = Color.Green;
            _strip[2].Position = new Vector3(400, 600, 0);
            _strip[2].Color = Color.Blue;
            _strip[3].Position = new Vector3(500, 400, 0);
            _strip[3].Color = Color.Red;
            _strip[4].Position = new Vector3(600, 600, 0);
            _strip[4].Color = Color.Green;
            _strip[5].Position = new Vector3(700, 400, 0);
            _strip[5].Color = Color.Red;
            _strip[6].Position = new Vector3(800, 600, 0);
            _strip[6].Color = Color.Green;

            _indexStrip = new int[7];
            _indexStrip[0] = 0;
            _indexStrip[1] = 1;
            _indexStrip[2] = 2;
            _indexStrip[3] = 3;
            _indexStrip[4] = 4;
            _indexStrip[5] = 5;
            _indexStrip[6] = 6;


        }



        public VertexPositionColor[] getStrip()
        {
            return _strip;
        }
        public int[] getIndex() { return _indexStrip; }

    }
}
