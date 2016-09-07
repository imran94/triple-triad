using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game4
{
    class CardValueGenerator
    {
        private int sumRow;
        private int[] sumCol = { 0, 0, 0, 0 };

        //set lower and upper limit for the sum of values of cards
        private readonly int[] limLw = { 1, 4, 9, 15, 22 };
        private readonly int[] limUp = { 9, 17, 23, 27, 29 };

        private int lw, up, rng;

        private Random random;
        private int seed = 0; //any value, to need to fix random pattern if necessary

        public CardValueGenerator() { }

        public CardValueGenerator(ref int[,] cardVal, int numOfCard)
        {

            random = new System.Random();

            for (int i = 0; i < numOfCard; i++)
            {
                sumRow = 0;
                for (int j = 0; j < 4; j++)
                {
                    lw = Math.Max(1, Math.Max(limLw[j] - sumRow, limLw[i] - sumCol[j]));
                    up = Math.Min(9, Math.Min(limUp[j] - sumRow, limUp[i] - sumCol[j]));

                    if (up <= lw)
                    {//contingency fix
                        j--;
                        break;
                    }
                    rng = random.Next(lw, up+1);
                    if (rng < 1) rng = 1; //contingency fix 2
                    if (rng > 9) rng = 9;
                    //rng = up; //force lower|upper limit via lw|up, for debugging purpose only
                    sumRow += rng;
                    sumCol[j] += rng;
                    cardVal[i, j] = rng;

                    if (j < 3) Debug.Write(cardVal[i, j] + ","); else Debug.WriteLine(cardVal[i, j] + ",");
                }
            }
            Debug.WriteLine("============================================\n");
        }


    }//end class========================================================
}//end namespace========================================================
