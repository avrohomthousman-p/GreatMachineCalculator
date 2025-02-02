using GreatMachineCalculator;

namespace TestingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CARD_TYPES[] deckContents = new CARD_TYPES[] { CARD_TYPES.MOVE_UP, CARD_TYPES.MOVE_RIGHT,
                                                    CARD_TYPES.MOVE_LEFT, CARD_TYPES.MAINTENENCE_1 };

            List<CARD_TYPES> deck = new List<CARD_TYPES>(deckContents);

            Dictionary<CARD_TYPES, CARD_STATUS> criteria = new Dictionary<CARD_TYPES, CARD_STATUS>();
            criteria[CARD_TYPES.MOVE_LEFT] = CARD_STATUS.DANGER_SERVENT_1;

            double prob = OutcomeCalculator.CalculateProbibilityOfDetainment(deck, criteria);
            Console.WriteLine("there is a %" + prob + " chance of detainment");

        }
    }
}
