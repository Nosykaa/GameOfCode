using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameOfCode2016.Models
{
    public class ResultParkEntity
    {
        public int Id { get; set; }

        public int Rank { get; set; } 

        public string Name { get; set; }

        public int FreeSlots { get; set; }

        public double Rate { get; set; }

        public string Cost { get; set; }
        public string DistanceParkToMyAdress { get; set; }


    }
}