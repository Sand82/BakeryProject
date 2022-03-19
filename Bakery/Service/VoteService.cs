using Bakery.Data;
using Bakery.Data.Models;
using System;

namespace Bakery.Service
{
    public class VoteService : IVoteService
    {
        private readonly BackeryDbContext data;

        public VoteService(BackeryDbContext data)
        {
            this.data = data;
        }

        public double GetAverage(int productId)
        {
            var averageData = this.data.Votes
                .Where(p => p.ProductId == productId).ToList();

            double averageVote;

            if (!averageData.Any())
            {
                return 0.0;
            };

            averageVote = averageData.Average(v => v.Value);
                        
            return averageVote;
        }

        public void SetVote(string userId, int productId, byte value)
        {
            var vote = this.data.Votes
                .Where(v => v.ProductId == productId && v.UsreId == userId)
                .FirstOrDefault();

            if (vote == null)
            {
                vote = new Vote
                {
                    ProductId = productId,
                    UsreId = userId,
                };

                this.data.Votes.Add(vote);

                vote.Value = value;

                Save(vote);
            }            
        } 
        
        private void Save(Vote vote)
        {
            this.data.Votes.Add(vote);

            this.data.SaveChanges();
        }
    }
}
