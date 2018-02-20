using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Moq;
using Nest;
using RPS.Data.Elasticsearch.ProfileCompleteness;
using RPS.Domain.ProfileCompleteness;
using Xunit;

namespace RPS.Data.Elasticsearch.Tests.ProfileCompleteness
{
    public class ScoringRepositoryTests
    {
        private readonly Mock<IElasticClient> _client;
        private readonly Mock<IElasticSearchContext> _searchContext;
        private readonly Mock<IOptions<ElasticSearchConfiguration>> _options;

        public ScoringRepositoryTests()
        {
            _client = new Mock<IElasticClient>();
            _searchContext = new Mock<IElasticSearchContext>();
            _searchContext.Setup(x => x.GetClient()).Returns(_client.Object);
            _options = new Mock<IOptions<ElasticSearchConfiguration>>();
        }
        
        
        [Fact]
        public void GetShouldReturnData()
        {
            // Arrange 
            var data = SimpleData();
            var searchResponse = new Mock<ISearchResponse<Scoring>>();
            searchResponse.Setup(x => x.Documents).Returns(data);
          
            _client.Setup(x => x.Search<Scoring>(It.IsAny<Func<SearchDescriptor<Scoring>,SearchDescriptor<Scoring>>>())).Returns(searchResponse.Object);
            _client.Setup(x => x.Search<Scoring>(It.IsAny<Func<SearchDescriptor<Scoring>,ISearchRequest>>())).Returns(searchResponse.Object);


            var sut = new ScoringRepository(_searchContext.Object, _options.Object);
            // Act
            var result = sut.Get(5);

            //Assert
            Assert.NotEmpty(result); 
            Assert.Equal(data.First(),result.First()); 
           

        }


        private List<Scoring> SimpleData()
        {
            return new List<Scoring>
            {
                new Scoring()
                {
                    Change = 1.1,
                    CompanyFk = 1,
                    Created = DateTime.Now,
                    Explanations = new List<string>(),
                    Id = 1,
                    Modifed = DateTime.Now,
                    RecordedOn = DateTime.Now,
                    RuleVersion = 1,
                    Score = 12
                },
                new Scoring()
                {
                    Change = 1.2,
                    CompanyFk = 2,
                    Created = DateTime.Now,
                    Explanations = new List<string>(),
                    Id = 2,
                    Modifed = DateTime.Now,
                    RecordedOn = DateTime.Now,
                    RuleVersion = 1,
                    Score = 13
                },
                new Scoring()
                {
                    Change = 1.3,
                    CompanyFk = 3,
                    Created = DateTime.Now,
                    Explanations = new List<string>(),
                    Id = 3,
                    Modifed = DateTime.Now,
                    RecordedOn = DateTime.Now,
                    RuleVersion = 1,
                    Score = 14
                }
            };
        }


    }
}
