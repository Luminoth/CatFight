using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace CatFight.Util
{
    public static class VoteHelper
    {
        private static Random _random = new Random();

        [CanBeNull]
        public static TK GetWinner<TK>(IReadOnlyDictionary<TK, int> votes) where TK: class
        {
            var topVoteIds = new List<TK>();
            int topVoteCount = int.MinValue;

            foreach(var kvp in votes) {
                if(kvp.Value > topVoteCount) {
                    topVoteCount = kvp.Value;

                    topVoteIds.Clear();
                    topVoteIds.Add(kvp.Key);
                } else if(kvp.Value == topVoteCount) {
                    topVoteIds.Add(kvp.Key);
                }
            }

            return topVoteIds.Any() ? _random.RandomEntry(topVoteIds) : null;
        }

        public static int GetWinner(IReadOnlyDictionary<int, int> votes)
        {
            var topVoteIds = new List<int>();
            int topVoteCount = int.MinValue;

            foreach(var kvp in votes) {
                if(kvp.Value > topVoteCount) {
                    topVoteCount = kvp.Value;

                    topVoteIds.Clear();
                    topVoteIds.Add(kvp.Key);
                } else if(kvp.Value == topVoteCount) {
                    topVoteIds.Add(kvp.Key);
                }
            }

            return topVoteIds.Any() ? _random.RandomEntry(topVoteIds) : -1;
        }
    }
}
