using System;
using System.Collections.Generic;
using Nest;
using FakeItEasy;
using Microsoft.Extensions.Options;
using Moq;
using RPS.Data.Elasticsearch.ProfileCompleteness;
using RPS.Domain.ProfileCompleteness;
using Xunit;

namespace RPS.Data.Elasticsearch.Tests
{
    public class ScoringRepositoryTests
    {
        [Fact]
        public void GetShouldReturnData()
        {
            // Arrange 
            var data = new List<Scoring>();
            data.Add(new Scoring()
            {
                Change = 1.1,
                CompanyFK = 1,
                Created = DateTime.Now,
                Explanations = new List<string>(),
                Id = 1,
                Modifed = DateTime.Now,
                RecordedOn = DateTime.Now,
                RuleVersion = 1,
                Score = 12
            });

            var searchResponse = new Mock<ISearchResponse<Scoring>>();
            searchResponse.Setup(x => x.Documents).Returns(data);
  
   

          //  var client = A.Fake<ElasticClient>();
           // A.CallTo(() => client.Search<Scoring>(A<SearchDescriptor<Scoring>>.Ignored)).Returns(searchResponse.Object);

            var client = new Mock<IElasticClient>();
            client.Setup(x => x.Search<Scoring>(It.IsAny<Func<SearchDescriptor<Scoring>,SearchDescriptor<Scoring>>>())).Returns(searchResponse.Object);

            var obj = client.Object;

            var searchContext = new Mock<IElasticSearchContext>();
            searchContext.Setup(x => x.GetClient()).Returns(client.Object);
            var options = new Mock<IOptions<ElasticSearchConfiguration>>();

            var sut = new ScoringRepository(searchContext.Object, options.Object);
            // Act
            var result = sut.Get(5);

            //Assert
            Assert.NotEmpty(result); 
           

        }


    }
}
