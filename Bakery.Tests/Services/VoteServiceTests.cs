using Bakery.Data.Models;
using BakeryServices.Service.Votes;
using Bakery.Tests.Mock;

using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bakery.Tests.Services
{
    public class VoteServiceTests
    {
        [Fact]
        public void GetValueShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var votes = CreateListVotes();

            data.Votes.AddRange(votes);

            data.SaveChanges();

            var voteService = new VoteService(data);

            var result = voteService.GetValue("My id3", 1);

            Assert.Equal(3, result);
        }

        [Theory]
        [InlineData("My id100", 1)]
        [InlineData("My id1", 6)]
        [InlineData(null, 6)]
        public void GetValueShouldReturnIncorectResulWhenMethodParameterIsNotValid(string userId, byte productid)
        {
            using var data = DatabaseMock.Instance;

            var votes = CreateListVotes();

            data.Votes.AddRange(votes);

            data.SaveChanges();

            var voteService = new VoteService(data);

            var result = voteService.GetValue(userId, productid);

            Assert.Equal(0, result);

        }

        [Fact]
        public void GetAverageShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var votes = CreateListVotes();

            votes[0].Value = 3;

            data.Votes.AddRange(votes);

            data.SaveChanges();

            var voteService = new VoteService(data);

            var result = voteService.GetAverage(1);

            Assert.Equal(4, result);
        }

        [Fact]
        public void GetAverageShouldReturnZeroIfMrthodParameterIsUncorect()
        {
            using var data = DatabaseMock.Instance;

            var votes = CreateListVotes();            

            data.Votes.AddRange(votes);

            data.SaveChanges();

            var voteService = new VoteService(data);

            var result = voteService.GetAverage(6);

            Assert.Equal(0.0, result);
        }

        [Fact]
        public async Task SetVoteShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var votes = CreateListVotes();            

            await data.Votes.AddAsync(votes[1]);

            await data.SaveChangesAsync();

            var voteService = new VoteService(data);

            await voteService.SetVote("Another user", 1, 4);

            var result = voteService.GetAverage(1);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task SetVoteShouldReturnCorectResultIfUserTryToVoteAgain()
        {
            using var data = DatabaseMock.Instance;

            var votes = CreateListVotes();           

            await data.Votes.AddAsync(votes[0]);

            await data.SaveChangesAsync();

            var voteService = new VoteService(data);

            await voteService.SetVote("My id1", 1, 4);

            var result = voteService.GetAverage(1);

            Assert.Equal(1, result);
        }        

        private List<Vote> CreateListVotes()
        {
            var votes = new List<Vote>();

            for (int i = 1; i <= 5; i++)
            {
                var vote = new Vote
                {
                    Id = i,
                    ProductId = 1,
                    Value = (byte)i,
                    UsreId = $"My id{i}",
                };

                votes.Add(vote);
            }

            for (int i = 6; i <= 10; i++)
            {
                var vote = new Vote
                {
                    Id = i,
                    ProductId = 2,
                    Value = 4,
                    UsreId = $"My id{i - 5}",
                };

                votes.Add(vote);
            }

            return votes;
        }
    }
}
