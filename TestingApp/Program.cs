using GreatMachineCalculator;

namespace TestingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<CARD_TYPES, CARD_STATUS> deck = new Dictionary<CARD_TYPES, CARD_STATUS>();
            deck[CARD_TYPES.MOVE_LEFT] = CARD_STATUS.DANGER_SERVENT_1;
            deck[CARD_TYPES.STAY_STILL] = CARD_STATUS.SAFE;
            deck[CARD_TYPES.ROSE] = CARD_STATUS.DANGER_SERVENT_1;
            deck[CARD_TYPES.MAINTENENCE_1] = CARD_STATUS.SAFE;

            double prob = OutcomeCalculator.CalculateProbibilityOfDetainment(deck);
            Console.WriteLine("there is a %" + prob + " chance of detainment");

        }
    }
}
