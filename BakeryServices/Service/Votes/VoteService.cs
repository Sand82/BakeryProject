using Bakery.Data;
using Bakery.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bakery.Service.Votes
{
    public class VoteService : IVoteService
    {
        private readonly BakeryDbContext data;

        public VoteService(BakeryDbContext data)
        {
            this.data = data;
        }

        public int GetValue(string userId, int productId)
        {
            var value = this.data.Votes
            .Where(p => p.ProductId == productId && p.UsreId == userId)
            .Select(p => p.Value)
            .FirstOrDefault();

            return value;
        }

        public double GetAverage(int productId)
        {
            var averageData = this.data.Votes
            .Where(p => p.ProductId == productId)
            .ToList();

            double averageVote;

            if (!averageData.Any())
            {
                return 0.0;
            };

            averageVote = averageData.Average(v => v.Value);

            return Math.Ceiling(averageVote);
        }

        public async Task SetVote(string userId, int productId, byte value)
        {

            var vote = await this.data.Votes
            .Where(v => v.ProductId == productId && v.UsreId == userId)
            .FirstOrDefaultAsync();

            if (vote == null)
            {
                vote = new Vote
                {
                    ProductId = productId,
                    UsreId = userId,
                };

                await this.data.Votes.AddAsync(vote);

                vote.Value = value;

                await Save(vote);
            }
        }

        private async Task Save(Vote vote)
        {
            await this.data.Votes.AddAsync(vote);

            await this.data.SaveChangesAsync();
        }
    }
}
