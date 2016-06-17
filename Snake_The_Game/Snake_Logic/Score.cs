using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Logic
{
   public class Score
    {
        public string _name { get; set; }
        public int _score { get; set; }

        public Score(string name, int score)
        {
            this._name = name;
            this._score = score;
        }
    }
}
