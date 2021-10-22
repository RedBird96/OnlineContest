using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PolygonUse
{
    class Program
    {
        static void Main(string[] args)
        {
            Assessment assessment = new Assessment();

            CustomContest.DoContest(assessment);
            ExContest.DoContest(assessment);
            StreakContest.DoContest(assessment);

            Console.WriteLine("Score updated successfully!");
        }
    }
}
