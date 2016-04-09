using GameOfCode2016.Analytics.ARIMAModel;
using GameOfCode2016.Connectivity;
using GameOfCode2016.Models.DAL;
using GameOfCode2016.Models.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfCode2016.Worker
{
    class Program
    {
        private static bool continueToWork = true;
        static void Main(string[] args)
        {
            var workerThread = new Thread(new ThreadStart(Worker));
            workerThread.Start();
            Console.WriteLine("Ready... (press Enter to quit)\n");
            Console.ReadLine();
            continueToWork = false;
        }

        static int frequencyInMinutes = 1;


        private static void Worker()
        {
            Thread.CurrentThread.Name = "Worker Thread";
            DateTime lastUpdate = DateTime.MinValue;
            CityParksClient parksClient = new CityParksClient();
            ARIMAEngine2 engine = new ARIMAEngine2();

            while (continueToWork)
            {
                var now = DateTime.Now;
                if (now > lastUpdate.AddMinutes(frequencyInMinutes))
                {
                    // Refresh Occupancy
                    var items = parksClient.GetCityParks().ToDictionary(k => k.Id ?? 666, k => k);
                    var parkingIds = items.Keys.ToArray();

                    var occupancy = items.Values.Select(cp => new Occupancy()
                    {
                        Date = now,
                        FreeSlots = cp.Actuel ?? 666,
                        Rate = (1.0 - (cp.Actuel ?? 666.0) / (cp.Total ?? 666.0)) * 100.0,
                        ParkingId = cp.Id ?? 666,
                    }).ToArray();
                    ParkOccupancyFactory factory = new ParkOccupancyFactory(new GameOfCode2016Entities());
                    factory.SaveOccupancy(occupancy);
                    Console.WriteLine("Saving Status");

                    // Forecast
                    DateTime dataStartDate = DateTime.Now.AddMonths(-1);
                    var forecasts = parkingIds.SelectMany(parkingId =>
                    {
                        var sourceData = factory.GetForecastsSince(dataStartDate, parkingId).Select(f => f.Rate).ToArray();
                        var occ = engine.ForeCast(sourceData, 2 * 60 / frequencyInMinutes); // 2 hours with 5min interval

                        var toSave = new List<Forecast>();

                        int count = 0;
                        foreach (double item in occ)
                        {
                            var rate = Math.Max(Math.Min(item, 100.0), 0);
                            count++;
                            toSave.Add(new Forecast()
                            {
                                Date = now.AddMinutes(frequencyInMinutes * count),
                                ParkingId = parkingId,
                                Rate = rate,
                                FreeSlots = (int)(items[parkingId].Total * (100 - rate) / 100),
                            });
                        }
                        return toSave;
                    }).ToArray();

                    ParkOccupancyFactory factory2 = new ParkOccupancyFactory(new GameOfCode2016Entities());
                    factory2.SaveForecast(forecasts);
                    Console.WriteLine("Forecast saved");

                    lastUpdate = now;
                    
                }
                Thread.Sleep(5000);
            }
        }
    }
}
