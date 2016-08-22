using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class CardValueGenerator
    {//separate as a class to make making changes easier

        private int sumRow;
        private int[] sumCol = { 0, 0, 0, 0 };

        //set lower and upper limit for the sum of values of cards
        private readonly int[] limLw = { 1, 4, 10, 13, 17 };
        private readonly int[] limUp = { 9, 17, 23, 27, 29 };

        private int lw, up, rng;

        private Random random;

        public CardValueGenerator() { }

        public CardValueGenerator(ref int[,] cardVal, int numOfCard)
        {

            random = new System.Random();

            for (int i = 0; i < numOfCard; i++)
            {
                sumRow = 0;
                for (int j = 0; j < 4; j++)
                {
                    lw = Math.Max(Math.Max(1, limLw[j] - sumRow), limLw[i] - sumCol[j]);
                    up = Math.Min(Math.Min(9, limUp[j] - sumRow), limUp[i] - sumCol[j]);

                    rng = random.Next(lw, up);
                    //rng = up; //force lower|upper limit via lw|up, for debugging purpose only
                    sumRow += rng;
                    sumCol[j] += rng;
                    cardVal[i, j] = rng;

                    if (j < 3) Debug.Write(cardVal[i, j] + ","); else Debug.WriteLine(cardVal[i, j] + ",");
                }
            }
        }

        //set seed for fixed random value
        public CardValueGenerator(ref int[,] cardVal, int numOfCard, int seed)
        {
            
            random = new System.Random(seed);

            for (int i = 0; i < numOfCard; i++)
            {
                sumRow = 0;
                for (int j = 0; j < 4; j++)
                {
                    lw = Math.Max(Math.Max(1, limLw[j] - sumRow), limLw[i] - sumCol[j]);
                    up = Math.Min(Math.Min(9, limUp[j] - sumRow), limUp[i] - sumCol[j]);

                    rng = random.Next(lw, up);
                    //rng = up; //force lower|upper limit via lw|up, for debugging purpose only
                    sumRow += rng;
                    sumCol[j] += rng;
                    cardVal[i, j] = rng;

                    if (j < 3) Debug.Write(cardVal[i, j] + ","); else Debug.WriteLine(cardVal[i, j] + ",");
                }
            }
        }


    }//end class========================================================
}//end namespace========================================================
