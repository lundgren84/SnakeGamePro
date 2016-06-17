using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Logic
{
    public static class LoadAndSaveGame
    {
        public static List<Score> LoadG()
        {
            StreamReader sr = new StreamReader("Save.txt");
            string line;
            List<Score> HighScore = new List<Score>();
            while ((line = sr.ReadLine()) != null)
            {
                var input = line.Split('\t');
                HighScore.Add(new Score(input[0], Convert.ToInt32(input[1])));
                HighScore.Add(new Score(input[2], Convert.ToInt32(input[3])));
                HighScore.Add(new Score(input[4], Convert.ToInt32(input[5])));
            }
            sr.Close();
            return HighScore;
        }
        public static void SaveG(List<Score> input)
        {
            StreamWriter sw = new StreamWriter("Save.txt");
            foreach (var item in input)
            {
                sw.Write(item._name + "\t" + item._score + "\t");
            }
            // sw.Write(points.ToString());
            sw.Close();

        }
        public static List<Score> CheckHighScore(List<Score> HighScore,Score score)
        {        
            HighScore.Add(new Score(score._name, score._score)); 
            bool flag = true;
            Score temp;
            int numCount = HighScore.Count;

            //sorting an array
            for (int i = 1; (i <= (numCount - 1)) && flag; i++)
            {
                flag = false;
                for (int j = 0; j < (numCount - 1); j++)
                {
                    if (HighScore[j + 1]._score > HighScore[j]._score)
                    {
                        temp = HighScore[j];
                        HighScore[j] = HighScore[j + 1];
                        HighScore[j + 1] = temp;
                        flag = true;
                    }
                }
            }
            return HighScore;
        }
    }
}
