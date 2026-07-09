using static System.Runtime.InteropServices.JavaScript.JSType;

const double Kelvin = 273.15;
const int AbsoluteRounder = 3;
const int ExactRounder = 0;
const int DARounder = -1;
const int TARounder = -1;
const int TASRounder = -1;
const int TRRounder = 0;
const int TATRounder = 0;
const double RAD = Math.PI / 180.0;
const double DEG = 180.0/ Math.PI;


List<string> exercises = new List<string>();
List<string> solutions = new List<string>();

GenerateExercises(exercises, solutions, 5);
PrintExercises(exercises, solutions);

//PrintTestsB();

static double CalculateDA(double PA, double TAT)
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

static void GenerateCalculateDA(List<string> exercises, List<string> solutions)
{
    var PA = GetRandom(0, 80000, -3);
    var TAT = GetRandom(-80, 50, -1);
    var text = "Find Density Altitude, given PA: " + PA + "ft, TAT: " + TAT + "°C";
    exercises.Add(text);
    Console.WriteLine(text);
    var solution = "DA: " + Rounder(CalculateDA(PA, TAT), -2) + "ft";
    solutions.Add(solution);
    Console.WriteLine(solution);
}

static double CalculateTA(double PA, double TAT, double CA, double SA = 0)
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

static void GenerateCalculateTA(List<string> exercises, List<string> solutions)
{
    var PA = GetRandom(0, 35000, -3);
    var CA = PA + GetRandom(-3000, 3000, -2);
    var SA = GetRandom(0, (int)(0.9 * Math.Abs(CA)), -3);
    var TAT = GetRandom(-80, 40, -1);
    var text = "Find True Altitude, given PA: " + PA + "ft, TAT: " + TAT + "°C, CA: " + CA + "ft, SA: " + SA + "ft";
    exercises.Add(text);
    Console.WriteLine(text);
    var solution = "TA: " + Rounder(CalculateTA(PA, TAT, CA, SA), -2) + "ft";
    solutions.Add(solution);
    Console.WriteLine(solution);
}

static double CalculateMach(double CAS, double PA)
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

    double mach = Math.Sqrt((2.0 / (gamma - 1.0)) *  (Math.Pow((qc / p) + 1.0, (gamma - 1.0) / gamma) - 1.0));
    return mach;
}

static void GenerateCalculateMach(List<string> exercises, List<string> solutions)
{
    var PA = GetRandom(0, 50000, -3);
    var CAS = GetRandom(100, 900, -1);
    var text = "Find Mach, given PA: " + PA + "ft, CAS: " + CAS + "kt";
    exercises.Add(text);
    Console.WriteLine(text);
    var solution = "Mach: " + Rounder(CalculateMach(CAS, PA), 2);
    solutions.Add(solution);
    Console.WriteLine(solution);
}

static double CalculateTASFromIAT(double CAS, double PA, double IAT, double RC = 1)
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

static void GenerateCalculateTASFromIAT(List<string> exercises, List<string> solutions)
{
    var PA = GetRandom(0, 35000, -3);
    var CAS = GetRandom(120, 700, -1);
    var IAT = GetRandom(-80, 120, -1);
    var RC = GenerateRC();
    var text = "Find TAS, given PA: " + PA + "ft, CAS:" + CAS + "kt, IAT: " + IAT + "°C, RC: " + RC;
    exercises.Add(text);
    Console.WriteLine(text);
    var solution = "TAS: " + Rounder(CalculateTASFromIAT(CAS, PA, IAT, RC), 0) + "kt";
    solutions.Add(solution);
    Console.WriteLine(solution);
}

static double CalculateTR(double CAS, double PA, double IAT, double RC = 1)
{
    double mach = CalculateMach(CAS, PA);

    double indicatedTempK = IAT + 273.15;
    double staticTempK = indicatedTempK / (1.0 + 0.2 * RC * mach * mach);
    double tempRiseK = indicatedTempK - staticTempK;

    return tempRiseK;
}

static double CalculateTASFromTAT(double PA, double TAT, double CAS)
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

static void GenerateCalculateTASFromTAT(List<string> exercises, List<string> solutions)
{
    var PA = GetRandom(0, 20000, -3);
    var CAS = GetRandom(50, 220, 0);
    var TAT = GetRandom(-80, 50, -1);
    var text = "Find TAS, given PA: " + PA + "ft, CAS:" + CAS + "kt, TAT: " + TAT + "°C";
    exercises.Add(text);
    Console.WriteLine(text);
    var solution = "TAS: " + Rounder(CalculateTASFromTAT(PA, TAT, CAS), 0) + "kt";
    solutions.Add(solution);
    Console.WriteLine(solution);
}

static double[] CalculateCrabMHGS(double TAS, double MC, double variation, double[] wind)
{
    var TC = Rev(MC + variation);
    var diff = Rev(TC - wind[1]);
    var diffRad = diff * RAD;
    var crosswind = -wind[0] * Math.Sin(diffRad);
    var headwind = -wind[0] * Math.Cos(diffRad);
    var crab = Math.Atan(crosswind / TAS) * DEG;

    if (diff < 0)
    {
        crab *= -1;
    }

    var MH = MC + crab;
    var ETAS = TAS * Math.Cos(crab * RAD);
    var GS = ETAS + headwind;

    TC = Rounder(TC, 0);
    headwind = Rounder(headwind, 0);
    crosswind = Rounder(crosswind, 0);
    crab = Rounder(crab, 0);
    MH = Rounder(MH, 0);
    ETAS = Rounder(ETAS, 0);
    GS = Rounder(GS, 0);

    return [TC, headwind, crosswind, crab, MH, ETAS, GS];
}

static void GenerateCalculateCrabMHGS(List<string> exercises, List<string> solutions)
{
    var TAS = GetRandom(100, 800, -1);
    var MC = GetRandom(0, 360, 0);
    var variation = GetRandom(0, 360, 0) - 180;
    var windMag = GetRandom(0, 140, -1);
    var windDir = GetRandom(0, 360, 0) - 180;
    var text = "Find TC, HW, CW, Crab, MH, ETAS, GS, \n\tGiven TAS: " + TAS + ", MC: " + MC + "°, variation: " + variation + "°, windMag: " + windMag + "kt, windDir: " + windDir + "°";
    exercises.Add(text);
    Console.WriteLine(text);
    var solutionArray = CalculateCrabMHGS(TAS, MC, variation, [windMag, windDir]);
    var solution = "TC: " + solutionArray[0] + "°, HW: " + solutionArray[1] + ", CW: " + solutionArray[2] + ", crab: " + solutionArray[3] + "°, MH: " + solutionArray[4] + "°, ETAS: " + solutionArray[5] + ", GS: " + solutionArray[6];
    solutions.Add(solution);
    Console.WriteLine(solution);
}


static void GenerateExercises(List<string> exercises, List<string> solutions, int exercisesAmount)
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
    }
}

static void PrintExercises(List<string> exercises, List<string> solutions)
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

static void PrintTestsA()
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

static void PrintTestsB()
{
    Console.WriteLine("#");
    SolvedArr(CalculateCrabMHGS(180, 140, -10, [40, 100]), [130, -35, -20, -6, 134, 179, 144], 0);
    SolvedArr(CalculateCrabMHGS(310, 254, 6, [30, 240]), [260, -28, -10, -2, 252, 310, 282], 0);
    SolvedArr(CalculateCrabMHGS(165, 130, -5, [20, 270]), [125, 16, 11, 4, 134, 165, 181], 0);
    SolvedArr(CalculateCrabMHGS(130, 350, 11, [30, 290]), [1, -10, -28, -13, 337, 127, 117], 0);
}

static void Solved(double value, double expectedValue, int round)
{
    var valueRounded = Rounder(value, round);
    var expectedValueRounded = Rounder(expectedValue, round);

    var relative = expectedValueRounded - valueRounded;
    var absolute = Rounder(Math.Abs(relative) / expectedValueRounded * 100, AbsoluteRounder);
    Console.WriteLine("Exact: " + Rounder(value, ExactRounder) + ", \tVal: " + valueRounded + ", \texpVal: " + expectedValueRounded + ", \trel: " + relative + ", \tabs: " + absolute + "%");
}

static void SolvedArr(double[] value, double[] expectedValue, int round)
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

static int GetRandom(int min, int max, int rounder)
{
    Random rnd = new();
    var random = rnd.Next(min, max);
    return (int)Rounder(random, rounder);
}

static double Rounder(double a, int dec)
{
    var pow = Math.Pow(10, dec);
    return Math.Round(a * pow) / pow;
}

static double Rev(double x)
{
    return x - Math.Floor(x / 360.0) * 360;
}

static double GenerateRC()
{
    Random rnd = new();
    var random = rnd.Next(0, 5);
    return (6 + random) / 10.0;
}