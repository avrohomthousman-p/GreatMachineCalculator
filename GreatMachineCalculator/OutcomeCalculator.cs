using System;

namespace GreatMachineCalculator
{
    /// <summary>
    /// These represent each of the cards in the movement deck for
    /// the game City of The Great Machine.
    /// </summary>
    public enum CARD_TYPES
    {
        MOVE_UP,
        MOVE_RIGHT,
        MOVE_DOWN,
        MOVE_LEFT,
        STAY_STILL,
        STAY_AND_MAKE_GAURD,
        MAINTENENCE_1,
        MAINTENENCE_2,
        MAINTENENCE_3,
        CENTRAL_SQUARE,
        ROSE,
        GOGGLES,
        BIRD
    }



    /// <summary>
    /// Used to specify which servent drawing a certain card will cause detainment
    /// </summary>
    public enum CARD_STATUS
    {
        SAFE,
        DANGER_SERVENT_1,
        DANGER_SERVENT_2,
        DANGER_SERVENT_3,
        DANGER_SERVENT_1_2,
        DANGER_SERVENT_1_3,
        DANGER_SERVENT_2_3,
        DANGER_ALL
    }



    /// <summary>
    /// Contains utilities to calculate the likelyhood of a move resulting in detainment.
    /// </summary>
    public static class OutcomeCalculator
    {

        public static double CalculateProbibilityOfDetainment(Dictionary<CARD_TYPES, CARD_STATUS> deck)
        {
            CARD_TYPES[] cardsInDeck = deck.Keys.ToArray();
            List<CARD_TYPES[]> drawOptions = GetDrawPossibilities(cardsInDeck);
            int detainments = 0;
            foreach (var cardsDrawn in drawOptions)
            {
                if (IsDetained(cardsDrawn))
                {
                    detainments++;
                }
            }

            return (detainments / (double)drawOptions.Count()) * 100;


            bool IsDetained(CARD_TYPES[] cardsDrawn)
            {
                for (int i = 0; i < cardsDrawn.Length; i++)
                {
                    var currentCard = cardsDrawn[i];

                    if (TriggersDetainment(deck[currentCard], i + 1))
                    {
                        return true;
                    }
                }

                return false;
            }
        }


        public static bool TriggersDetainment(CARD_STATUS status, int serventNumber)
        {
            if (status == CARD_STATUS.SAFE)
                return false;
            if (status == CARD_STATUS.DANGER_ALL)
                return true;

            switch (serventNumber)
            {
                case 1:
                    return status == CARD_STATUS.DANGER_SERVENT_1
                            || status == CARD_STATUS.DANGER_SERVENT_1_2
                            || status == CARD_STATUS.DANGER_SERVENT_1_3;
                case 2:
                    return status == CARD_STATUS.DANGER_SERVENT_2
                            || status == CARD_STATUS.DANGER_SERVENT_1_2
                            || status == CARD_STATUS.DANGER_SERVENT_2_3;

                case 3:
                    return status == CARD_STATUS.DANGER_SERVENT_3
                            || status == CARD_STATUS.DANGER_SERVENT_1_3
                            || status == CARD_STATUS.DANGER_SERVENT_2_3;

                default:
                    throw new ArgumentException("There are only 3 servents");
            }
        }


        /// <summary>
        /// Given an array of cards that are still in the deck, generates all possible draws that 
        /// can happen.
        /// </summary>
        /// <param name="deck"></param>
        /// <exception cref="ArgumentException"></exception>
        public static List<CARD_TYPES[]> GetDrawPossibilities(CARD_TYPES[] deck)
        {
            if (deck.Length < 4)
                throw new ArgumentException("There cannot be fewer than 4 cards in the deck. Did you forget to shuffle?");


            List<CARD_TYPES[]> posibilities = new List<CARD_TYPES[]>();

            for (int i = 0; i < deck.Length; i++)
            {
                for (int j = 0; j < deck.Length; j++)
                {
                    if (i == j)
                        continue;

                    for (int k = 0; k < deck.Length; k++)
                    {
                        if (k == i || k == j)
                            continue;

                        posibilities.Add(new CARD_TYPES[] { deck[i], deck[j], deck[k] });
                    }
                }
            }


            return posibilities;
        }


        /// <summary>
        /// Utility to create a deck containing all the appropriate cards for a full deck.
        /// All cards have the default status of SAFE.
        /// </summary>
        /// <param name="includeMaintenenceCards"></param>
        /// <returns></returns>
        public static Dictionary<CARD_TYPES, CARD_STATUS> GetFullDeck(bool includeMaintenenceCards = true)
        {
            Dictionary<CARD_TYPES, CARD_STATUS> fullDeck = new Dictionary<CARD_TYPES, CARD_STATUS>();

            CARD_TYPES[] maintenenceCards = new CARD_TYPES[] 
            {
                CARD_TYPES.MAINTENENCE_1,
                CARD_TYPES.MAINTENENCE_2, 
                CARD_TYPES.MAINTENENCE_3
            };

            foreach (CARD_TYPES cardType in Enum.GetValues(typeof(CARD_TYPES)))
            {
                if (!includeMaintenenceCards && maintenenceCards.Contains(cardType))
                    continue;

                fullDeck[cardType] = CARD_STATUS.SAFE;
            }

            return fullDeck;
        }
    }


}
