using Classic;

List<string> exercises = new List<string>();
List<string> solutions = new List<string>();

var generators = new Generators();

generators.GenerateExercises(exercises, solutions, 100, 5);


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