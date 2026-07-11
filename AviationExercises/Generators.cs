using CF = AviationExercises.CommonFunctions;

namespace AviationExercises
{
    public class Generators(Calculators calculators)
    {
        public void GenerateExercises(List<string> exercises, List<string> solutions, int exercisesAmount)
        {
            var i = 0;
            while (i < exercisesAmount)
            {
                //GenerateCalculateMach(exercises, solutions);
                //i++;
                //GenerateCalculateDA(exercises, solutions);
                //i++;
                //GenerateCalculateTA(exercises, solutions);
                //i++;
                //GenerateCalculateTASFromIAT(exercises, solutions);
                //i++;
                //GenerateCalculateTASFromTAT(exercises, solutions);
                //i++;
                GenerateCalculateCrabMHGS(exercises, solutions);
                i++;
                GenerateCalculateWind(exercises, solutions);
                i++;
                GenerateCalculateTCGS(exercises, solutions);
                i++;
            }
        }

        void GenerateCalculateDA(List<string> exercises, List<string> solutions)
        {
            var PA = CF.GetRandom(0, 80000, -3);
            var TAT = CF.GetRandom(-80, 50, -1);
            var text = "Find Density Altitude, given PA: " + PA + "ft, TAT: " + TAT + "°C";
            exercises.Add(text);
            Console.WriteLine(text);
            var solution = "DA: " + CF.Rounder(calculators.CalculateDA(PA, TAT), -2) + "ft";
            solutions.Add(solution);
            Console.WriteLine(solution);
        }

        void GenerateCalculateTA(List<string> exercises, List<string> solutions)
        {
            var PA = CF.GetRandom(0, 35000, -3);
            var CA = PA + CF.GetRandom(-3000, 3000, -2);
            var SA = CF.GetRandom(0, (int)(0.9 * Math.Abs(CA)), -3);
            var TAT = CF.GetRandom(-80, 40, -1);
            var text = "Find True Altitude, given PA: " + PA + "ft, TAT: " + TAT + "°C, CA: " + CA + "ft, SA: " + SA + "ft";
            exercises.Add(text);
            Console.WriteLine(text);
            var solution = "TA: " + CF.Rounder(calculators.CalculateTA(PA, TAT, CA, SA), -2) + "ft";
            solutions.Add(solution);
            Console.WriteLine(solution);
        }

        void GenerateCalculateMach(List<string> exercises, List<string> solutions)
        {
            var PA = CF.GetRandom(0, 50000, -3);
            var CAS = CF.GetRandom(100, 900, -1);
            var text = "Find Mach, given PA: " + PA + "ft, CAS: " + CAS + "kt";
            exercises.Add(text);
            Console.WriteLine(text);
            var solution = "Mach: " + CF.Rounder(calculators.CalculateMach(CAS, PA), 2);
            solutions.Add(solution);
            Console.WriteLine(solution);
        }

        void GenerateCalculateTASFromIAT(List<string> exercises, List<string> solutions)
        {
            var PA = CF.GetRandom(0, 35000, -3);
            var CAS = CF.GetRandom(120, 700, -1);
            var IAT = CF.GetRandom(-80, 120, -1);
            var RC = CF.GenerateRC();
            var text = "Find TAS, given PA: " + PA + "ft, CAS:" + CAS + "kt, IAT: " + IAT + "°C, RC: " + RC;
            exercises.Add(text);
            Console.WriteLine(text);
            var solution = "TAS: " + CF.Rounder(calculators.CalculateTASFromIAT(CAS, PA, IAT, RC), 0) + "kt";
            solutions.Add(solution);
            Console.WriteLine(solution);
        }

        void GenerateCalculateTASFromTAT(List<string> exercises, List<string> solutions)
        {
            var PA = CF.GetRandom(0, 20000, -3);
            var CAS = CF.GetRandom(50, 220, 0);
            var TAT = CF.GetRandom(-80, 50, -1);
            var text = "Find TAS, given PA: " + PA + "ft, CAS:" + CAS + "kt, TAT: " + TAT + "°C";
            exercises.Add(text);
            Console.WriteLine(text);
            var solution = "TAS: " + CF.Rounder(calculators.CalculateTASFromTAT(PA, TAT, CAS), 0) + "kt";
            solutions.Add(solution);
            Console.WriteLine(solution);
        }

        void GenerateCalculateCrabMHGS(List<string> exercises, List<string> solutions)
        {
            var TAS = CF.GetRandom(100, 800, -1);
            var MC = CF.GetRandom(0, 360, 0);
            var variation = CF.GetRandom(0, 360, 0) - 180;
            var windMag = CF.GetRandom(0, 140, -1);
            var windDir = CF.GetRandom(0, 360, 0);
            var text = "Find TC, HW, CW, Crab, MH, ETAS, GS, \n\tGiven TAS: " + TAS + ", MC: " + MC + "°, var: " + variation + "°, windDir: " + windDir + "°, windMag: " + windMag + "kt";
            exercises.Add(text);
            Console.WriteLine(text);
            var solutionArray = calculators.CalculateCrabMHGS(TAS, MC, variation, [windDir, windMag]);
            var solution = "TC: " + CF.Rounder(solutionArray[0], 0) + "°, HW: " + CF.Rounder(solutionArray[1], 0) + ", CW: " + CF.Rounder(solutionArray[2], 0) + ", crab: " + CF.Rounder(solutionArray[3], 0) + "°, MH: " + CF.Rounder(solutionArray[4], 0) + "°, ETAS: " + CF.Rounder(solutionArray[5], 0) + ", GS: " + CF.Rounder(solutionArray[6], 0);
            solutions.Add(solution);
            Console.WriteLine(solution);
        }

        void GenerateCalculateWind(List<string> exercises, List<string> solutions)
        {
            var TAS = CF.GetRandom(100, 800, -1);
            var TC = CF.GetRandom(0, 360, 0);
            var TH = TC + CF.GetRandom(0, 180, 0) - 90;
            var GS = TAS + CF.GetRandom(0, 280, -1) - 140;
            var text = "Find wind components Dir and Mag, given TAS: " + TAS + "kt, TC: " + TC + "°, TH: " + TH + "°, GS: " + GS + "kt";
            Console.WriteLine(text);
            var solutionArray = calculators.CalculateWind(TAS, TC, TH, GS);
            var solution = "windDir: " + CF.Rounder(solutionArray[0], 0) + "°, windMag: " + CF.Rounder(solutionArray[1], 0) + "kt";
            solutions.Add(solution);
            Console.WriteLine(solution);
        }

        void GenerateCalculateTCGS(List<string> exercises, List<string> solutions)
        {
            var TAS = CF.GetRandom(100, 800, -1);
            var MH = CF.GetRandom(0, 360, 0);
            var variation = CF.GetRandom(0, 360, 0) - 180;
            var windMag = CF.GetRandom(0, 140, -1);
            var windDir = CF.GetRandom(0, 360, 0);

            var text = "Find TC and GS, given TAS: " + TAS + "kt, MH: " + MH + "°, var: " + variation + "°, windDir: " + windDir + "°, windMag: " + windMag;
            Console.WriteLine(text);
            var solutionArray = calculators.CalculateTCGS(TAS, MH, variation, [windDir, windMag]);
            var solution = "TC: " + CF.Rounder(solutionArray[0], 0) + "°, GS: " + CF.Rounder(solutionArray[1], 0) + "kt";
            solutions.Add(solution);
            Console.WriteLine(solution);
        }

        void GenerateCalculateTHTAS(List<string> exercises, List<string> solutions)
        {
            var GS = CF.GetRandom(100, 800, -1);
            var TC = CF.GetRandom(0, 360, 0);
            var windMag = CF.GetRandom(0, 140, -1);
            var windDir = CF.GetRandom(0, 360, 0);

            var text = "Find TAS and TH, given GS: " + GS + "kt, TC: " + TC + "°, windDir: " + windDir + "°, windMag: " + windMag;
            Console.WriteLine(text);
            var solutionArray = calculators.CalculateTHTAS(GS, TC, [windDir, windMag]);
            var solution = "TH: " + CF.Rounder(solutionArray[0], 0) + "°, TAS: " + CF.Rounder(solutionArray[1], 0) + "kt";
            solutions.Add(solution);
            Console.WriteLine(solution);
        }

        void GenerateCalculateOffCourseCorrection(List<string> exercises, List<string> solutions)
        {
            var flown = CF.GetRandom(0, 1000, 1);
            var offCourse = CF.GetRandom(0, 1000, 1);
            var toDest = CF.GetRandom(0, 1000, 1);

            var text = "Find correction angle, given flown dist: " + flown + "nm, offCourse: " + offCourse + "nm, toDest: " + toDest + "nm";
            Console.WriteLine(text);
            var solution = "TH: " + CF.Rounder(calculators.CalculateOffCourseCorrection(flown, offCourse, toDest), 0) + "°";
            solutions.Add(solution);
            Console.WriteLine(solution);
        }
    }
}