using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MidpointDisplacement
{
    internal class Displace
    {
        public List<Vector3> displaceList = new List<Vector3>();
        float curving_value = -.7f;
        float startDisplacement = 300;
        float currentDisplacement;
        private List<Vector3> displaceListL;
        private List<Vector3> displaceListR;
        public List<Vector3> displaceListSafe;

        private Random random = new Random();

        
        public void initList()
        {
            displaceListL = new List<Vector3>
            {
                new Vector3(0, 900, 0)
            };
            displaceListSafe = new List<Vector3>();

            CS5410.MyRandom temp = new CS5410.MyRandom();
            float x = temp.nextRange(50, 1700);
            float y = temp.nextRange(600, 900);

            displaceListL.Add( new Vector3(x, y, 0));

            

            for(int i = 1; i < 100; i++)
            {
                displaceListSafe.Add(new Vector3(x + i, y, 0));
            }

            

            displaceListR = new List<Vector3>();
            displaceListR.Add(new Vector3(x + 100, y, 0));
            displaceListR.Add(new Vector3(1919, 900, 0));
            currentDisplacement = startDisplacement;
        }

        public void testDis()
        {
             

            displaceListL = midpointDiplace(displaceListL, 8);
            currentDisplacement = startDisplacement;
            displaceListR = midpointDiplace(displaceListR, 8);
            displaceList = new List<Vector3>(displaceListL.Count + displaceListR.Count + displaceListSafe.Count);
            displaceList.AddRange(displaceListL);
            displaceList.AddRange(displaceListSafe);
            displaceList.AddRange(displaceListR);
            
        }


        public List<Vector3> midpointDiplace(List<Vector3> points, int iterationCount)
        {

            if(iterationCount < 1)
            {
                return points;
            }

            var oldList = points;
            points = new List<Vector3>();
            for( int i = 0; i < oldList.Count -1; i++ )
            {
                var midpoint = (oldList[i] + oldList[i + 1]) /2;

                midpoint.Y = midpoint.Y + (float) (currentDisplacement * Math.Pow(-1, random.Next() % 2)) ;

                if(midpoint.Y >= 1000)
                {
                    midpoint.Y = 900 ;
                }

                points.Add(oldList[i]); /// First 
                points.Add(midpoint); // mid 

            }
            currentDisplacement *= (float) Math.Pow(2.0, curving_value);
            points.Add(oldList[oldList.Count -1]); // Adds the last point 


            

            return midpointDiplace(points, iterationCount - 1); 

        }


        public void debugPoints()
        {
            foreach(Vector3 vector in displaceList)
            {
                Debug.WriteLine(vector);
                
            }
            Debug.WriteLine(displaceList.Count);
        }



    }
}
