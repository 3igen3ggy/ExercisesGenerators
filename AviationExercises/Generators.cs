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
            var windDir = CF.GetRandom(0, 360, 0) - 180;
            var text = "Find TC, HW, CW, Crab, MH, ETAS, GS, \n\tGiven TAS: " + TAS + ", MC: " + MC + "°, var: " + variation + "°, windDir: " + windDir + "°, windMag: " + windMag + "kt";
            exercises.Add(text);
            Console.WriteLine(text);
            var solutionArray = calculators.CalculateCrabMHGS(TAS, MC, variation, [windDir, windMag]);
            var solution = "TC: " + solutionArray[0] + "°, HW: " + solutionArray[1] + ", CW: " + solutionArray[2] + ", crab: " + solutionArray[3] + "°, MH: " + solutionArray[4] + "°, ETAS: " + solutionArray[5] + ", GS: " + solutionArray[6];
            solutions.Add(solution);
            Console.WriteLine(solution);
        }



        void GenerateCalculateWind(List<string> exercises, List<string> solutions)
        {
            var TAS = CF.GetRandom(100, 800, -1);
            var TC = CF.GetRandom(0, 360, 0);
            var TH = TC + CF.GetRandom(0, 180, 0) - 90;
            var GS = TAS + CF.GetRandom(0, 140, -1);
            var text = "Find wind components Dir and Mag, given TAS: " + TAS + "kt, TC: " + TC + "°, TH: " + TH + "°, GS: " + GS + "kt";
            Console.WriteLine(text);
            var solutionArray = calculators.CalculateWind(TAS, TC, TH, GS);
            var solution = "windDir: " + solutionArray[0] + "°, windMag: " + solutionArray[1] + "kt";
            solutions.Add(solution);
            Console.WriteLine(solution);
        }
    }
}
