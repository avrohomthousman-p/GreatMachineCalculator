using GreatMachineCalculator;

namespace TestingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<CARD_TYPES, CardEffects> deck = new Dictionary<CARD_TYPES, CardEffects>();
            deck[CARD_TYPES.MOVE_LEFT] = new CardEffects(1, 1, 0);
            deck[CARD_TYPES.STAY_STILL] = new CardEffects(0, 0, 0);
            deck[CARD_TYPES.ROSE] = new CardEffects(0, 0, 0);
            deck[CARD_TYPES.MAINTENENCE_1] = new CardEffects(0, 0, 0);
            deck[CARD_TYPES.BIRD] = new CardEffects(0, 0, 0);
            deck[CARD_TYPES.CENTRAL_SQUARE] = new CardEffects(1, 1, 2);
            deck[CARD_TYPES.GOGGLES] = new CardEffects(0, 0, 0);

            double[] results = OutcomeCalculator.CalculateProbibilityOfOutcome(deck);
            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine($"%{results[i]} change of there bieng {i} detainments");
            }

        }
    }
}
