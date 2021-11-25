using System.Collections.Generic;

namespace Models.Patterns.Facade
{
    public class RandomApiFacade
    {
        private readonly RandomNumberApi _randomNumberApi;
        private readonly RandomNumberSequence _randomNumberSequence;
 
        public RandomApiFacade()
        {
            _randomNumberApi = new RandomNumberApi();
            _randomNumberSequence = new RandomNumberSequence();
        }

        public int GetNumber(int min, int max)
        {
            return _randomNumberApi.GetOneNumber(min, max);
        }

        public List<int> GetSequenceForDeck(int min, int max)
        {
            return _randomNumberSequence.GetSequence(min, max);
        }
    }
}