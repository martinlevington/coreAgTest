using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Moq;
using Nest;
using RPS.Data.Elasticsearch.ProfileCompleteness;
using RPS.Data.Elasticsearch.Tests.Helpers;
using RPS.Domain.ProfileCompleteness;
using Xunit;

namespace RPS.Data.Elasticsearch.Tests.ProfileCompleteness
{
    public class ScoringRepositoryTests : IDisposable
    {
        public ScoringRepositoryTests()
        {
            _client = new Mock<IElasticClient>();
            _searchContext = new Mock<IElasticSearchContext>();
            _searchContext.Setup(x => x.GetClient()).Returns(_client.Object);
            _options = new Mock<IOptions<ElasticSearchConfiguration>>();

            SystemClock.Set(new DateTime(2018,2,3));
        }

        private readonly Mock<IElasticClient> _client;
        private readonly Mock<IElasticSearchContext> _searchContext;
        private readonly Mock<IOptions<ElasticSearchConfiguration>> _options;
 



        private List<Scoring> SimpleData()
        {
            return new List<Scoring>
            {
                new Scoring
                {
                    Change = 1.1,
                    CompanyFk = 1,
                    Created = SystemClock.Now,
                    Explanations = new List<string>(),
                    Id = 1,
                    Modifed = SystemClock.Now,
                    RecordedOn = SystemClock.Now,
                    RuleVersion = 1,
                    Score = 12
                },
                new Scoring
                {
                    Change = 1.2,
                    CompanyFk = 2,
                    Created = SystemClock.Now,
                    Explanations = new List<string>(),
                    Id = 2,
                    Modifed = SystemClock.Now,
                    RecordedOn = SystemClock.Now,
                    RuleVersion = 1,
                    Score = 13
                },
                new Scoring
                {
                    Change = 1.3,
                    CompanyFk = 3,
                    Created = SystemClock.Now,
                    Explanations = new List<string>(),
                    Id = 3,
                    Modifed = SystemClock.Now,
                    RecordedOn = SystemClock.Now,
                    RuleVersion = 1,
                    Score = 14
                },
                new Scoring
                {
                    Change = 1.4,
                    CompanyFk = 4,
                    Created = SystemClock.Now,
                    Explanations = new List<string>(),
                    Id = 4,
                    Modifed = SystemClock.Now,
                    RecordedOn = SystemClock.Now,
                    RuleVersion = 1,
                    Score = 15
                },
                new Scoring
                {
                    Change = 1.5,
                    CompanyFk = 5,
                    Created = SystemClock.Now,
                    Explanations = new List<string>(),
                    Id = 5,
                    Modifed = SystemClock.Now,
                    RecordedOn = SystemClock.Now,
                    RuleVersion = 1,
                    Score = 16
                },
                new Scoring
                {
                    Change = 1.6,
                    CompanyFk = 6,
                    Created = SystemClock.Now,
                    Explanations = new List<string>(),
                    Id = 6,
                    Modifed = SystemClock.Now,
                    RecordedOn = SystemClock.Now,
                    RuleVersion = 1,
                    Score = 17
                }
            };
        }

        private void ClientSetup(List<Scoring> data)
        {
            // Arrange 
          
            var searchResponse = new Mock<ISearchResponse<Scoring>>();
            searchResponse.Setup(x => x.Documents).Returns(data);

            _client.Setup(x => x.Search(It.IsAny<Func<SearchDescriptor<Scoring>, SearchDescriptor<Scoring>>>()))
                .Returns(searchResponse.Object);
            _client.Setup(x => x.Search(It.IsAny<Func<SearchDescriptor<Scoring>, ISearchRequest>>()))
                .Returns(searchResponse.Object);

     
        }

        [Fact]
        public void GetShouldReturnData()
        {
            // Arrange 
            var data = SimpleData();
            ClientSetup(data);
            var resultSize = 5;
            var sut = new ScoringRepository(_searchContext.Object, _options.Object);
           
            // Act
            var result = sut.Get(resultSize);

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(data.First(), result.First());
        }


        [Fact]
        public void GetMonthlyAverageShouldReturnData()
        {
            // Arrange 
            var data = SimpleData();
            ClientSetup(data);
            var resultSize = 5;
            var numberOfMonths = 12;
            var sut = new ScoringRepository(_searchContext.Object, _options.Object);
           
            // Act
            var result = sut.GetMonthlyAverage(resultSize,numberOfMonths);

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(data.First(), result.First());
        }

        [Fact]
        public void GetTopImproversShouldReturnData()
        {
            // Arrange 
            var data = SimpleData();
            ClientSetup(data);
            var resultSize = 5;
            var startPeriod = SystemClock.Now;
            var sut = new ScoringRepository(_searchContext.Object, _options.Object);
           
            // Act
            var result = sut.GetTopImprovers(resultSize,startPeriod);

            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(data.First(), result.First());
        }

        public void Dispose()
        {
            SystemClock.Reset();
        }
    }
}