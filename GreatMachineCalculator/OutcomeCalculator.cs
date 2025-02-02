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

        public static double CalculateProbibilityOfDetainment(List<CARD_TYPES> deck, Dictionary<CARD_TYPES, CARD_STATUS> criteria)
        {
            List<CARD_TYPES[]> drawOptions = GetDrawPossibilities(deck);
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

                    //Get the status of the current card, default=SAFE
                    CARD_STATUS cardStatus;
                    if (!criteria.TryGetValue(currentCard, out cardStatus))
                        cardStatus = CARD_STATUS.SAFE;

                    if (TriggersDetainment(cardStatus, i + 1))
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
        /// Given a list of cards that are still in the deck, generates all possible draws that 
        /// can happen.
        /// </summary>
        /// <param name="deck"></param>
        /// <exception cref="ArgumentException"></exception>
        public static List<CARD_TYPES[]> GetDrawPossibilities(List<CARD_TYPES> deck)
        {
            if (deck.Count < 4)
                throw new ArgumentException("There cannot be fewer than 4 cards in the deck. Did you forget to shuffle?");


            List<CARD_TYPES[]> posibilities = new List<CARD_TYPES[]>();

            for (int i = 0; i < deck.Count(); i++)
            {
                for (int j = 0; j < deck.Count(); j++)
                {
                    if (i == j)
                        continue;

                    for (int k = 0; k < deck.Count(); k++)
                    {
                        if (k == i || k == j)
                            continue;

                        posibilities.Add(new CARD_TYPES[] { deck[i], deck[j], deck[k] });
                    }
                }
            }


            return posibilities;
        }


        public static List<CARD_TYPES> GetFullDeck(bool includeMaintenenceCards = true)
        {
            CARD_TYPES[] allCardsAsArray = (CARD_TYPES[])Enum.GetValues(typeof(CARD_TYPES));
            List<CARD_TYPES> allCards = new List<CARD_TYPES>(allCardsAsArray);

            if (!includeMaintenenceCards)
            {
                var excludedCards = new List<CARD_TYPES>
            {
                CARD_TYPES.MAINTENENCE_1,
                CARD_TYPES.MAINTENENCE_2,
                CARD_TYPES.MAINTENENCE_3,
            };

                allCards = allCards.Where(card => !excludedCards.Contains(card)).ToList();
            }

            return allCards;
        }
    }


}
