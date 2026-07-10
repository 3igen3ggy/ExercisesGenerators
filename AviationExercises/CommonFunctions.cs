namespace AviationExercises
{
    static class CommonFunctions
    {
        public static int GetRandom(int min, int max, int rounder)
        {
            Random rnd = new();
            var random = rnd.Next(min, max);
            return (int)Rounder(random, rounder);
        }

        public static double Rounder(double a, int dec)
        {
            var pow = Math.Pow(10, dec);
            return Math.Round(a * pow) / pow;
        }

        public static double Rev(double x)
        {
            return x - Math.Floor(x / 360.0) * 360;
        }

        public static double GenerateRC()
        {
            Random rnd = new();
            var random = rnd.Next(0, 5);
            return (6 + random) / 10.0;
        }

        public static void PrintExercises(List<string> exercises, List<string> solutions)
        {
            Console.WriteLine();

            for (int i = 1; i <= exercises.Count(); i++)
            {
                Console.WriteLine(i + ":\t " + exercises[i - 1]);
            }

            Console.WriteLine();

            for (int i = 1; i <= solutions.Count(); i++)
            {
                Console.WriteLine(i + ":\t " + solutions[i - 1]);
            }
        }
    }
}
