using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameOfCode2016.Models
{
    public class CityCarPark
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string QuartierFR { get; set; }
        //public string QuartierDE { get; set; }
        public int? Total { get; set; }
        public int? Actuel { get; set; }
        //public int? Tendance { get; set; }
        public int? Ouvert { get; set; }
        public int? Complet { get; set; }


        //Localisation
        //public string LocalisationEntree { get; set; }
        //public string LocalisationSortie { get; set; }
        public double? LocalisationLatitude { get; set; }
        public double? LocalisationLongitude { get; set; }

        //Filtering
        //public int? NominalSurface { get; set; }
        //public int? NominalCouvertes { get; set; }
        //public int? NominalHandicapes { get; set; }
        //public int? NominalFemmes { get; set; }

        //public string PictureUrl { get; set; }

    }
}