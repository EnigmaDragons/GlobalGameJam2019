using MonoDragons.Core.EventSystem;
using MonoDragons.GGJ.Data;
using MonoDragons.GGJ.Gameplay.Events;

namespace MonoDragons.GGJ.Gameplay
{
    public class CardEffectProcessor
    {
        private readonly GameData _data;
        private int _processedTurn = -1;

        public CardEffectProcessor(GameData data)
        {
            _data = data;
            Event.Subscribe<AllCardsSelected>(OnCardsSelected, this);
            Event.Subscribe<NextLevelRequested>(e => _processedTurn = -1, this);
        }

        private void OnCardsSelected(AllCardsSelected e)
        {
            if (_processedTurn >= e.TurnNumber)
                return;
            
            _processedTurn = e.TurnNumber;
            Cards.Execute(_data, _data.AllCards[e.CowboyCard].CardName);
            Cards.Execute(_data, _data.AllCards[e.HouseCard].CardName);
            Event.Publish(new CountersProcessed());
            Event.Publish(new CardsProcessed { CowboyCard = e.CowboyCard, HouseCard = e.HouseCard, TurnNumber = e.TurnNumber });
        }
    }
}