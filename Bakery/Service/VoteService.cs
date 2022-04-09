using Bakery.Data;
using Bakery.Data.Models;
using System;

namespace Bakery.Service
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
            var value = new Byte();

            Task.Run(() =>
            {
                 value = this.data.Votes
                .Where(p => p.ProductId == productId && p.UsreId == userId)
                .Select(p => p.Value)
                .FirstOrDefault();

            }).GetAwaiter().GetResult();
                                 
            return value;
        }

        public double GetAverage(int productId)
        {
            var averageData = new List<Vote>();

            Task.Run(() =>
            {
                 averageData = this.data.Votes
                .Where(p => p.ProductId == productId)
                .ToList();

            }).GetAwaiter().GetResult();           

            double averageVote;

            if (!averageData.Any())
            {
                return 0.0;
            };

            averageVote = averageData.Average(v => v.Value);
                        
            return Math.Ceiling(averageVote);
        }

        
        public void SetVote(string userId, int productId, byte value)
        {
           
            var vote = new Vote();           

            Task.Run(() =>
            {
                 vote = this.data.Votes
                .Where(v => v.ProductId == productId && v.UsreId == userId)
                .FirstOrDefault();                

            }).GetAwaiter().GetResult();            

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
            Task.Run(() =>
            {
                this.data.Votes.Add(vote);

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();            
        }
    }
}
