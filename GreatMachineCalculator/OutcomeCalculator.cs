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
    /// Stores the number of detentions that drawing a specific card will cause.
    /// </summary>
    public class CardEffects
    {
        readonly int servent1Detainments;
        readonly int servent2Detainments;
        readonly int servent3Detainments;


        public CardEffects()
        {
            this.servent1Detainments = 0;
            this.servent2Detainments = 0;
            this.servent3Detainments = 0;
        }

        public CardEffects(int servent1Detainments, int servent2Detainments, int servent3Detainments)
        {
            this.servent1Detainments = servent1Detainments;
            this.servent2Detainments = servent2Detainments;
            this.servent3Detainments = servent3Detainments;
        }


        public int TotalDetainments()
        {
            return servent1Detainments + servent2Detainments + servent3Detainments;
        }


        public int GetDetainmentsByServent(int serventNumber)
        {
            switch (serventNumber)
            {
                case 1: 
                    return servent1Detainments;
                case 2:
                    return servent2Detainments;
                case 3:
                    return servent3Detainments;
                default:
                    throw new ArgumentException($"There is no servent {serventNumber}. All servents are numbered 1-3.");
            }
        }

    }



    /// <summary>
    /// Contains utilities to calculate the likelyhood of a move resulting in detainment.
    /// </summary>
    public static class OutcomeCalculator
    {

        /// <summary>
        /// Given a deck of cards and the results that each card would produce if drawn,
        /// this function generates and returns a breakdown of all the possible results 
        /// and thier liklyhood.
        /// 
        /// The data is formatted as an array of doubles with a length of 10, such that
        /// array[i] = (percent chance that there are i detainments)
        /// </summary>
        public static double[] CalculateProbibilityOfOutcome(Dictionary<CARD_TYPES, CardEffects> deck)
        {
            List<int> detainmentCounts = GetNumberOfDetainmentsPerPossibility(deck);

            //Map the number of detainments to the number of possibilities that result in
            //that number of detainments.
            Dictionary<int, int> detainmentsToPossibilities = new Dictionary<int, int>();
            foreach (int item in detainmentCounts)
            {
                if (detainmentsToPossibilities.ContainsKey(item))
                { 
                    detainmentsToPossibilities[item]++;
                }
                else
                {
                    detainmentsToPossibilities[item] = 1;
                }
            }

            //Now copy the results to an output array that has 0's where there are no possibilities
            double[] output = new double[10];

            for(int i = 0; i < output.Length; i++)
            {
                if (detainmentsToPossibilities.ContainsKey(i))
                {
                    output[i] = (detainmentsToPossibilities[i] / (double)detainmentCounts.Count()) * 100;
                }
            }

            return output;
        }


        /// <summary>
        /// Given a deck of cards and the results that each card would produce if drawn,
        /// this function generates and returns a list of the number of detainments
        /// that would be caused for each possible set of cards drawn.
        /// 
        /// E.g: the return of [0, 0, 1, 2] means there are a total of 4 possible ways
        /// the cards can be drawn. Two of them result in 0 detainments, one results
        /// in 1 detainment and one results in 2 detainments.
        /// 
        /// No gaurentee is made about the order the results are returned in, and the 
        /// results include only the number of detainments, but not the cards drawn 
        /// that led to that result.
        /// </summary>
        public static List<int> GetNumberOfDetainmentsPerPossibility(Dictionary<CARD_TYPES, CardEffects> deck)
        {
            CARD_TYPES[] cardsInDeck = deck.Keys.ToArray();
            List<CARD_TYPES[]> drawOptions = GetDrawPossibilities(cardsInDeck);
            List<int> detainments = new List<int>();

            foreach (var cardsDrawn in drawOptions)
            {
                detainments.Add(NumberOfDetainments(cardsDrawn));
            }

            return detainments;


            int NumberOfDetainments(CARD_TYPES[] cardsDrawn)
            {
                if (cardsDrawn.Length != 3)
                    throw new ArgumentException("Only 3 cards can be drawn");

                int numDetainments = 0;
                for (int i = 0; i < cardsDrawn.Length; i++)
                {
                    var currentCard = cardsDrawn[i];
                    var cardEffects = deck[currentCard];

                    numDetainments += cardEffects.GetDetainmentsByServent(i + 1);
                }

                return numDetainments;
            }
        }



        /// <summary>
        /// Given an array of cards that are still in the deck, generates all possible draws that 
        /// can happen.
        /// </summary>
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
        /// All cards are set to not cause any detainments.
        /// </summary>
        public static Dictionary<CARD_TYPES, CardEffects> GetFullDeck(bool includeMaintenenceCards = true)
        {
            Dictionary<CARD_TYPES, CardEffects> fullDeck = new Dictionary<CARD_TYPES, CardEffects>();

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

                fullDeck[cardType] = new CardEffects(0, 0, 0);
            }

            return fullDeck;
        }
    }

}
