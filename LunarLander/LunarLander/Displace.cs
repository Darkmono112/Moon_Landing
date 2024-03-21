using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        private Random random = new Random();

        
        public void initList()
        {
            displaceList.Add(new Vector3(1, 900, 0));
            displaceList.Add(new Vector3(1919, 900, 0));
            currentDisplacement = startDisplacement;
        }

        public void testDis()
        {
            displaceList = midpointDiplace(displaceList, 8);
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
