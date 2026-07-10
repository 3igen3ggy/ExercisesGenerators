using CF = AviationExercises.CommonFunctions;

using AviationExercises;

var calculators = new Calculators();
var generators = new Generators(calculators);

List<string> exercises = new List<string>();
List<string> solutions = new List<string>();

generators.GenerateExercises(exercises, solutions, 5);
CF.PrintExercises(exercises, solutions);

calculators.PrintTestsB();
