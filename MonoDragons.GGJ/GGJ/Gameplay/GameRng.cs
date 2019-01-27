using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoDragons.GGJ.Gameplay
{
    public class GameRng
    {
        private GameData _gameData;
        private Random _instance;

        public GameRng(GameData gameData)
        {
            _gameData = gameData;
            _instance = new Random(gameData.InitialSeed);
            for (var i = 0; i < gameData.TimesSeedHasBeenUsed; i++)
                _instance.Next();
        }

        public bool Bool()
        {
            return Int(2) == 1;
        }

        public int Int()
        {
            return Int(int.MaxValue);
        }

        public int Int(int max)
        {
            _gameData.TimesSeedHasBeenUsed++;
            return _instance.Next(max);
        }

        public int Int(int min, int max)
        {
            _gameData.TimesSeedHasBeenUsed++;
            return _instance.Next(min, max);
        }

        public double Dbl()
        {
            _gameData.TimesSeedHasBeenUsed++;
            return _instance.NextDouble();
        }

        public KeyValuePair<T, T2> Random<T, T2>(Dictionary<T, T2> dictionary)
        {
            return dictionary.ElementAt(Int(dictionary.Count));
        }

        public T Random<T>(T[] array)
        {
            return array[Int(array.Length)];
        }

        public T Random<T>(List<T> list)
        {
            return list[Int(list.Count)];
        }

        public T Between<T>(T primary, T other, double primaryWeight)
        {
            return Dbl() <= primaryWeight ? primary : other;
        }

        public void Shuffle<T>(IList<T> list)
        {
            for (var n = list.Count; n > 1; n--)
            {
                _gameData.TimesSeedHasBeenUsed++;
                var k = _instance.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
