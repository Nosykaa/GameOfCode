using RDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfCode2016.Analytics.ARIMAModel
{
    public class ARIMAEngine2
    {
        static REngine engine = null;
        public ARIMAEngine2()
        {
            if (engine == null) {
                string rhome = System.Environment.GetEnvironmentVariable("R_HOME");
                if (string.IsNullOrEmpty(rhome))
                    rhome = @"C:\Program Files\R\R-3.2.4revised";

                System.Environment.SetEnvironmentVariable("R_HOME", rhome);
                System.Environment.SetEnvironmentVariable("PATH", System.Environment.GetEnvironmentVariable("PATH") + ";" + rhome + @"\bin\i386");

                engine = REngine.GetInstance();
            }
        }
        // C:\Program Files\R\R-3.2.4revised

        public double[] ForeCast(double[] serie, int numberOfPredictions)
        {
            // source: http://stackoverflow.com/questions/14879697/retrieve-results-from-r-evaluation-using-r-net
                engine.Initialize();

                // code below to run the first time, choose yes to install in personal directory
                // engine.Evaluate("install.packages(\"forecast\")");

                NumericVector testGroup = engine.CreateNumericVector(serie);
                engine.SetSymbol("testGroup", testGroup);
                engine.Evaluate("testTs <- c(testGroup)");
                NumericVector ts = engine.GetSymbol("testTs").AsNumeric();

                engine.Evaluate("tsValue <- ts(testTs, frequency=1, start=c(2016, 4, 9))");
                engine.Evaluate("library(forecast)");
                engine.Evaluate("arimaFit <- auto.arima(tsValue)");
                engine.Evaluate("fcast <- forecast(tsValue, h=" + numberOfPredictions + ")");
                engine.Evaluate("results <- fcast$mean");
                //engine.Evaluate("plot(fcast)");

                var fcast = engine.GetSymbol("results").AsNumeric().Select(d => Math.Round(d,2)).ToArray();
                //var nv = ts.AsNumeric().ToArray().Select(a => ""+ a).Aggregate((a,b) => a + "," + b);
                //var rstring = fcast.Select(a => "" + a).Aggregate((a, b) => a + "," + b);
                ////var result = nv.ToArray<double>();


                return fcast;
            
        }
    }
}
