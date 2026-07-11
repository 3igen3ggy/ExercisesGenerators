namespace Classic
{
    static class CommonFunctions
    {
        public static string GetFractionBar(int i)
        {
            var bar = "";
            for (int j = 0; j < i; j++)
            {
                bar += "-";
            }
            return bar;
        }

        public static double GetNextNumber()
        {
            double nextNumber = GetRandom(0, 1000, 0);
            var multiplicator = GetRandom(-4, 3, 0);
            nextNumber *= Math.Pow(10, multiplicator);
            return Rounder(nextNumber, 3);
        }

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
    }
}