using GameOfCode2016.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GameOfCode2016.Models.Factory
{
    public class ParkOccupancyFactory
    {
        GameOfCode2016Entities context = null;
        public ParkOccupancyFactory(GameOfCode2016Entities context)
        {
            this.context = context;
        }
        public void SaveOccupancy(Occupancy[] occupancy)
        {
            context.Occupancy.AddRange(occupancy);
            context.SaveChanges();
        }

        public void SaveForecast(Forecast[] forecasts)
        {
            using (var transaction = context.Database.BeginTransaction())
            {

                context.Database.ExecuteSqlCommand("DELETE FROM Forecast");
                context.SaveChanges();

                context.Forecast.AddRange(forecasts);
                context.SaveChanges();
            transaction.Commit();
        }
    }

        public OccupancyForecast GetForecast(int parkingId, DateTime targetDate)
        {
            DateTime now = DateTime.Now;
            if (targetDate < now.AddMinutes(5))
            {
                return context.Occupancy.Where(o => o.ParkingId == parkingId)
                                        .OrderByDescending(o => o.Date)
                                        .Select(o => new OccupancyForecast() {
                                            FreeSlots = o.FreeSlots,
                                            Rate = o.Rate,
                                        })
                                        .First();
            }

            return context.Forecast.Where(o => o.ParkingId == parkingId && targetDate > now)
                        .OrderByDescending(o => o.ForecastId)
                        .Select(o => new OccupancyForecast()
                        {
                            FreeSlots = o.FreeSlots,
                            Rate = o.Rate,
                        })
                        .First();
        }

        public Occupancy[] GetForecastsSince(DateTime startDate, int parkingId)
        {
            return context.Occupancy.Where(o => o.Date > startDate && o.ParkingId == parkingId)
                                    .OrderBy(o => o.Date)
                                    .ToArray();
        }

    }
}