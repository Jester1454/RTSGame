using System;

namespace PrototypeScripts
{
    [Serializable]
    public class DeckConfig
    {
        public int Id;
        public int DeckSize;
        public CardPrototype[] Cards;

        public DeckConfig(int size)
        {
            DeckSize = size;
            Cards = new CardPrototype[DeckSize];
            for (int i = 0; i < DeckSize; i++)
            {
                Cards[i] = new CardPrototype();
            }
        }
    }
}