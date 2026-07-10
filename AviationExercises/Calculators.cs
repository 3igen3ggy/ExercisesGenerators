using CF = AviationExercises.CommonFunctions;

namespace AviationExercises
{
    public class Calculators
    {
        const int AbsoluteRounder = 3;
        const int ExactRounder = 0;
        const int DARounder = -1;
        const int TARounder = -1;
        const int TASRounder = -1;
        const int TRRounder = 0;
        const int TATRounder = 0;

        const double Kelvin = 273.15;
        const double RAD = Math.PI / 180.0;
        const double DEG = 180.0 / Math.PI;
        public double CalculateDA(double PA, double TAT)
        {
            const double R = 287.05287;
            const double G0 = 9.80665;
            const double T0 = 288.15;
            const double P0 = 101325.0;
            const double L = 0.0065;

            double h = PA * 0.3048;
            double oatK = TAT + 273.15;

            double pressurePa;
            if (h <= 11000.0)
            {
                double t = T0 - L * h;
                pressurePa = P0 * Math.Pow(t / T0, G0 / (R * L));
            }
            else
            {
                double t11 = T0 - L * 11000.0;
                double p11 = P0 * Math.Pow(t11 / T0, G0 / (R * L));
                pressurePa = p11 * Math.Exp(-G0 * (h - 11000.0) / (R * t11));
            }

            double rhoTarget = pressurePa / (R * oatK);

            double low = 0.0;
            double high = 80000.0;

            for (int i = 0; i < 80; i++)
            {
                double mid = (low + high) / 2.0;

                double tMid;
                double pMid;

                if (mid <= 11000.0)
                {
                    tMid = T0 - L * mid;
                    pMid = P0 * Math.Pow(tMid / T0, G0 / (R * L));
                }
                else
                {
                    double t11 = T0 - L * 11000.0;
                    double p11 = P0 * Math.Pow(t11 / T0, G0 / (R * L));
                    tMid = t11;
                    pMid = p11 * Math.Exp(-G0 * (mid - 11000.0) / (R * tMid));
                }

                double rhoMid = pMid / (R * tMid);

                if (rhoMid > rhoTarget)
                    low = mid;
                else
                    high = mid;
            }

            return ((low + high) / 2.0) / 0.3048;
        }

        public double CalculateTA(double PA, double TAT, double CA, double SA = 0)
        {
            double heightAboveStationFeet = CA - SA;
            if (heightAboveStationFeet < 0)
                heightAboveStationFeet = 0;

            // ISA temperature at pressure altitude, valid in the troposphere up to 35,000 ft
            double isaTemperatureC = 15.0 - 2.0 * (PA / 1000.0);

            // ISA deviation: negative when colder than standard
            double isaDeviationC = TAT - isaTemperatureC;

            // Standard true-altitude correction:
            // True Altitude = Calibrated Altitude + (ISA deviation * heightAboveStation / 273.15)
            double correctionFeet = isaDeviationC * heightAboveStationFeet / 273.15;

            return CA + correctionFeet;
        }

        public double CalculateMach(double CAS, double PA)
        {
            var gamma = 1.4;
            var R = 287.05;
            var g0 = 9.80665;
            var p0 = 101325.0;
            var T0 = 288.15;
            var L = 0.0065;
            var a0 = Math.Sqrt(gamma * R * T0);

            var CASM = CAS * 0.514444;
            var PAM = PA * 0.3048;

            double p;
            if (PAM <= 11000.0)
            {
                p = p0 * Math.Pow(1.0 - L * PAM / T0, g0 / (R * L));
            }
            else
            {
                double T11 = T0 - L * 11000.0;
                double p11 = p0 * Math.Pow(1.0 - L * 11000.0 / T0, g0 / (R * L));
                p = p11 * Math.Exp(-g0 * (PAM - 11000.0) / (R * T11));
            }

            double qc = p0 * (Math.Pow(1.0 + (gamma - 1.0) / 2.0 * Math.Pow(CASM / a0, 2.0), gamma / (gamma - 1.0)) - 1.0);

            double mach = Math.Sqrt((2.0 / (gamma - 1.0)) * (Math.Pow((qc / p) + 1.0, (gamma - 1.0) / gamma) - 1.0));
            return mach;
        }

        public double CalculateTASFromIAT(double CAS, double PA, double IAT, double RC = 1)
        {
            var gamma = 1.4;
            var R = 287.05;

            var CASM = CAS * 0.514444;
            var PAM = PA * 0.3048;
            IAT += Kelvin;

            double mach = CalculateMach(CAS, PA);

            double tStatic = IAT / (1.0 + RC * (gamma - 1.0) / 2.0 * mach * mach);

            double a = Math.Sqrt(gamma * R * tStatic);
            double tasMps = mach * a;
            double tasKt = tasMps / 0.514444;
            return tasKt;
        }

        public double CalculateTR(double CAS, double PA, double IAT, double RC = 1)
        {
            double mach = CalculateMach(CAS, PA);

            double indicatedTempK = IAT + 273.15;
            double staticTempK = indicatedTempK / (1.0 + 0.2 * RC * mach * mach);
            double tempRiseK = indicatedTempK - staticTempK;

            return tempRiseK;
        }

        public double CalculateTASFromTAT(double PA, double TAT, double CAS)
        {
            const double gamma = 1.4;
            const double R = 287.05287;
            const double mpsToKt = 1.94384617179;

            double mach = CalculateMach(CAS, PA);

            double tatK = TAT + 273.15;
            double satK = tatK / (1.0 + ((gamma - 1.0) / 2.0) * mach * mach);

            double a = Math.Sqrt(gamma * R * satK);
            double tasMps = mach * a;

            return tasMps * mpsToKt;
        }

        public double[] CalculateCrabMHGS(double TAS, double MC, double variation, double[] wind)
        {
            var TC = CF.Rev(MC + variation);
            var diff = CF.Rev(TC - wind[0]);
            var diffRad = diff * RAD;
            var crosswind = -wind[1] * Math.Sin(diffRad);
            var headwind = -wind[1] * Math.Cos(diffRad);
            var crab = Math.Asin(crosswind / TAS) * DEG;

            if (diff < 0)
            {
                crab *= -1;
            }

            var MH = MC + crab;
            var ETAS = TAS * Math.Cos(crab * RAD);
            var GS = ETAS + headwind;

            TC = CF.Rounder(TC, 0);
            headwind = CF.Rounder(headwind, 0);
            crosswind = CF.Rounder(crosswind, 0);
            crab = CF.Rounder(crab, 0);
            MH = CF.Rounder(MH, 0);
            ETAS = CF.Rounder(ETAS, 0);
            GS = CF.Rounder(GS, 0);

            return [TC, headwind, crosswind, crab, MH, ETAS, GS];
        }
        
        public double[] CalculateWind(double TAS, double TC, double TH, double GS)
        {
            var crab = TH - TC;
            var ETAS = TAS * Math.Cos(crab * RAD);
            var headwind = GS - ETAS;
            var crosswind = TAS * Math.Sin(crab * RAD);
            var windMag = Math.Sqrt(headwind * headwind + crosswind * crosswind);
            var windDir = CF.Rev(Math.Atan2(crosswind, -headwind) * DEG + TC);
            //Console.WriteLine("Crab: " + crab + ", ETAS: " + ETAS + ", headwind: " + headwind + ", crosswind: " + crosswind + ", windMag: " + windMag + ", windDir: " + windDir);
            return [windDir, windMag];
        }

        public void PrintTestsA()
        {
            Console.WriteLine("#");
            Solved(CalculateDA(3000, 25), 5000, DARounder);
            Solved(CalculateDA(1500, 35), 4100, DARounder);
            Solved(CalculateDA(0, 40), 2820, DARounder);
            Solved(CalculateDA(8000, -10), 6890, DARounder);
            Console.WriteLine("#");
            Solved(CalculateTA(10000, -20, 9000, 5000), 8780, TARounder);
            Solved(CalculateTA(10000, -20, 9000), 8500, TARounder);
            Solved(CalculateTA(10000, 25, 11400, 4200), 12200, TARounder);
            Solved(CalculateTA(5000, 0, 6000), 5890, TARounder);
            Solved(CalculateTA(7000, 10, 7400, 1900), 7580, TARounder);
            Solved(CalculateTA(20000, -15, 21000), 21810, TARounder);
            Console.WriteLine("#");
            Solved(CalculateTASFromIAT(400, 15000, 30), 500, TASRounder);
            Solved(CalculateTASFromIAT(180, 5000, -5), 188, TASRounder);
            Solved(CalculateTASFromIAT(276, 16000, -15), 339, TASRounder);
            Solved(CalculateTASFromIAT(355, 20000, 5), 470, TASRounder);
            Console.WriteLine("#");
            Solved(CalculateTR(276, 10000, 0), 13, TRRounder);
            Solved(CalculateTR(190, 5000, 0), 5, TRRounder);
            Solved(CalculateTR(350, 17000, -10), 24, TRRounder);
            Console.WriteLine("#");
            Solved(CalculateTASFromTAT(5000, 10, 166), 180, TATRounder);
            Solved(CalculateTASFromTAT(7000, 0, 210), 232, TATRounder);
            Solved(CalculateTASFromTAT(10000, -20, 188), 212, TATRounder);
        }

        public void PrintTestsB()
        {
            Console.WriteLine("#");
            SolvedArr(CalculateCrabMHGS(180, 140, -10, [100, 40]), [130, -35, -20, -6, 134, 179, 144], 0);
            SolvedArr(CalculateCrabMHGS(310, 254, 6, [240, 30]), [260, -28, -10, -2, 252, 310, 282], 0);
            SolvedArr(CalculateCrabMHGS(165, 130, -5, [270, 20]), [125, 16, 11, 4, 134, 165, 181], 0);
            SolvedArr(CalculateCrabMHGS(130, 350, 11, [290, 30]), [1, -10, -28, -13, 337, 127, 117], 0);
            Console.WriteLine("#");
            SolvedArr(CalculateWind(180, 175, 160, 144), [118, 55], 0);
            SolvedArr(CalculateWind(240, 106, 102, 220), [65, 26], 0);
            SolvedArr(CalculateWind(130, 320, 309, 142), [200, 29], 0);
            SolvedArr(CalculateWind(210, 164, 175, 222), [276, 43], 0);
        }

        void Solved(double value, double expectedValue, int round)
        {
            var valueRounded = CF.Rounder(value, round);
            var expectedValueRounded = CF.Rounder(expectedValue, round);

            var relative = expectedValueRounded - valueRounded;
            var absolute = CF.Rounder(Math.Abs(relative) / expectedValueRounded * 100, AbsoluteRounder);
            Console.WriteLine("Exact: " + CF.Rounder(value, ExactRounder) + ", \tVal: " + valueRounded + ", \texpVal: " + expectedValueRounded + ", \trel: " + relative + ", \tabs: " + absolute + "%");
        }

        void SolvedArr(double[] value, double[] expectedValue, int round)
        {
            var length = value.Length;
            var expectedlength = expectedValue.Length;

            if (length != expectedlength)
            {
                Console.WriteLine("Different lengths of arrays, valuesLength: " + length + ", expectedValuesLength: " + expectedlength);
                return;
            }

            for (int i = 0; i < length; i++)
            {
                Solved(value[i], expectedValue[i], round);
            }
        }
    }
}
