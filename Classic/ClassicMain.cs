using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

List<string> exercises = new List<string>();
List<string> solutions = new List<string>();


GenerateExercises(exercises, solutions, 100, 5);


Console.WriteLine();

for (int i = 1; i <= exercises.Count(); i++)
{
    Console.WriteLine(i + ": " + exercises[i - 1]);
    Console.WriteLine();
}

Console.WriteLine();

for (int i = 1; i <= exercises.Count(); i++)
{
    Console.WriteLine(i + ":\t " + solutions[i - 1]);
}


static void GenerateExercises(List<string> exercises, List<string> solutions, int exercisesNo, int items)
{
    for (int i = 0; i < exercisesNo; i++)
    {
        GenerateExerciseFormatted(exercises, solutions, items);
    }
}

static void GenerateExercise(List<string> exercises, List<string> solutions, int items)
{
    var sol = 0.0;
    do {
        sol = GetNextNumber();
    } while (sol == 0);

    var text = sol.ToString();
    for (int i = 1; i < items; i++)
    {
        var next = 0.0;
        do
        {
            next = GetNextNumber();
        } while (next == 0);

        if (i % 2 == 0)
        {
            sol *= next;
            text += " * " + next;

        } 
        else
        {
            sol /= next;
            text += " / " + next;
        }
    }
    var formattedSol = string.Format("{0:0.##E+00}", sol);
    exercises.Add(text);
    solutions.Add(formattedSol);
}

static void GenerateExerciseFormatted(List<string> exercises, List<string> solutions, int items)
{
    var sol = GetNextNumber();

    string numerator = sol.ToString();

    string denominator = null;


    for (int i = 1; i < items; i++)
    {
        var next = GetNextNumber();

        if (i % 2 == 0)
        {
            sol *= next;
            numerator += " * " + next;

        }
        else
        {
            sol /= next;
            if (denominator == null)
            {
                denominator = next.ToString();
            } else
            {
                denominator += " * " + next;
            }
            
        }
    }
    var formattedSol = string.Format("{0:0.##E+00}", sol);

    var numeratorLength = numerator.Length;
    var denominatorLength = denominator.Length;

    var longer = Math.Max(numeratorLength, denominatorLength);
    var diff = numeratorLength - denominatorLength;

    var numeratorSpacebars = 0;
    var denominatorSpacebars = 0;


    if (diff > 0) //denominator shorter
    {
        denominatorSpacebars = diff / 2;
    }
    else if (diff < 0) //numerator shorter
    {
        numeratorSpacebars = -diff / 2;
    }

    var numeratorSpacebarsText = new string(' ', numeratorSpacebars);
    var denominatorSpacebarsText = new string(' ', denominatorSpacebars);

    exercises.Add("\t" + new string(' ', numeratorSpacebars) + numerator + "\n\t" + new string('-', longer) + "\n\t" + new string(' ', denominatorSpacebars) + denominator);
    solutions.Add(formattedSol);
}

static string GetFractionBar(int i)
{
    var bar = "";
    for (int j = 0; j < i; j++)
    {
        bar += "-";
    }
    return bar;
}

static double GetNextNumber()
{
    double nextNumber = GetRandom(0, 1000, 0);
    var multiplicator = GetRandom(-4, 3, 0);
    nextNumber *= Math.Pow(10, multiplicator);
    return Rounder(nextNumber, 3);
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