using MonoDragons.Core.EventSystem;

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
            _data.Card(e.CowboyCard).Resolve();
            _data.Card(e.HouseCard).Resolve();
        }
    }
}