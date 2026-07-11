using CF = Classic.CommonFunctions;

namespace Classic
{
    public class Generators
    {
        public void GenerateExercises(List<string> exercises, List<string> solutions, int exercisesNo, int items)
        {
            for (int i = 0; i < exercisesNo; i++)
            {
                GenerateExerciseFormatted(exercises, solutions, items);
            }
        }

        public void GenerateExercise(List<string> exercises, List<string> solutions, int items)
        {
            var sol = 0.0;
            do
            {
                sol = CF.GetNextNumber();
            } while (sol == 0);

            var text = sol.ToString();
            for (int i = 1; i < items; i++)
            {
                var next = 0.0;
                do
                {
                    next = CF.GetNextNumber();
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

        public void GenerateExerciseFormatted(List<string> exercises, List<string> solutions, int items)
        {
            var sol = CF.GetNextNumber();

            string numerator = sol.ToString();

            string denominator = null;


            for (int i = 1; i < items; i++)
            {
                var next = CF.GetNextNumber();

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
                    }
                    else
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
    }
}
