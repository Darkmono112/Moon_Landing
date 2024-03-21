using CS5410;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MidpointDisplacement;
using System;
using System.Collections.Generic;
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
        private int minHeight;
        private MyRandom random = new MyRandom();
        Displace displace;
        

        struct landscape
        {
            public Vector3 position;
            public bool safe { get; set; }
            public landscape(Vector3 position, bool safe)
            {
                this.safe = safe;
                this.position = position;
            }


        }
        List<landscape> geography = new List<landscape>();



        public Terrain(int x , int y) {

            maxHeight = y / 2;
            minHeight = y;
        
            
            
            // 1 point for every 2 pixels 
            
           generateTerrarin();
            
        }


        // Make makeTriStrip use an array of Vector 3 for this. 
            // Call make strip then chose a point on he strip
            // That point will now be the safe zone and will be saved in an array
            // and drawn flat based on first index
            // AKA clone the vector 3 but change the x for every other point

        private void generateTerrarin()
        {
            

            geography = new List<landscape>();
            displace = new Displace();

            displace.initList();
            displace.testDis();

            int j = 0;
            for (int i = 0; i < displace.displaceList.Count; i++)
            {
                geography.Add(new landscape(displace.displaceList[j], false));
                
            }

            makeTriStrip();

            /* j = 0;
             for(int i = 1;i<_strip.Length;i+=2)
             {
                 _strip[i].Position = geography[j++].position;
             }*/

        }

       



        private void makeTriStrip()
        {
            _strip = new VertexPositionColor[displace.displaceList.Count*2 +1];
            _indexStrip = new int[displace.displaceList.Count *2 + 1]; 

            _strip[0].Position = new Vector3(-1, 1080 , 0);
            _strip[0].Color = Color.White; 

            _indexStrip[0] = 0;

            int j = 0;
            for(int i = 1; j<displace.displaceList.Count; i+=2)
            {
                    _strip[i].Position = displace.displaceList[j];
                    _strip[i].Color = Color.White;
                    _indexStrip[i] = i;

                
                    _strip[i+1].Position = new Vector3(displace.displaceList[j++].X , minHeight, 0); 
                    _strip[i + 1].Color = Color.White;
                    _indexStrip[i+1] = i+1;

            }
            _strip[displace.displaceList.Count].Position = new Vector3(1920,minHeight, 0);
            _strip[displace.displaceList.Count].Color = Color.White;
            _indexStrip[displace.displaceList.Count] = displace.displaceList.Count;


            foreach (var item in _strip)
            {
                Debug.WriteLine(item.Position.ToString());
            }

        }



        public VertexPositionColor[] getStrip()
        {
            return _strip;
        }
        public int[] getIndex() { return _indexStrip; }

    }
}
