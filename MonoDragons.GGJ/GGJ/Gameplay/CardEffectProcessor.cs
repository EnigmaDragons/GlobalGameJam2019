using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Data;

namespace MonoDragons.GGJ.Gameplay
{
    public class CardEffectProcessor
    {
        private readonly GameData _data;
        private int _processedTurn;

        public CardEffectProcessor(GameData data)
        {
            _data = data;
            Event.Subscribe<AllCardsRevealed>(OnCardsRevealed, this);
        }

        private void OnCardsRevealed(AllCardsRevealed e)
        {
            if (_processedTurn >= e.TurnNumber)
                return;
            
            _processedTurn = e.TurnNumber;
            Cards.Execute(_data, _data.AllCards[e.CowboyCard].CardName);
            Cards.Execute(_data, _data.AllCards[e.HouseCard].CardName);
        }
    }
}