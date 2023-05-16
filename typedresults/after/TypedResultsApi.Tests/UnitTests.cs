using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;
using TypedResultsApi.Apis;
using TypedResultsApi.Controllers;
using TypedResultsApi.Data;

namespace TypedResultsApi.Tests;

public class UnitTests
{

  [Fact]
  public void EnsurePersonReturnsSuccess()
  {
    var retVal = PeopleApi.GetPerson(1, new DataFactory());
    var result = retVal as Ok<Person>; //await GetResponse<Person>();

    Assert.NotNull(result);

    Assert.Equal(200, result.StatusCode);

    Assert.NotNull(result.Value);

    Assert.IsType<Person>(result.Value);

    Assert.True(result.Value.Id == 1);
  }

  [Fact]
  public void EnsurePersonReturnsFailedResult()
  {
    var result = PeopleApi.GetPerson(1000, new DataFactory()) as NotFound;

    Assert.NotNull(result);

    Assert.Equal(404, result.StatusCode);
  }

  [Fact]
  public async Task EnsurePeopleReturnsSuccess()
  {
    var result = await GetResponse<IEnumerable<Person>>(PeopleApi.GetAllPeople(new DataFactory()));

    Assert.Equal(HttpStatusCode.OK, result.StatusCode);

    Assert.NotNull(result.Value);

    Assert.IsAssignableFrom<IEnumerable<Person>>(result.Value);

    Assert.True(result.Value.Count() == 25);
  }

  [Fact]
  public void EnsureJobsReturnsSuccess()
  {
    var ctrl = new JobsController(new DataFactory());
    
    var result = ctrl.Get().Result as Ok<IEnumerable<Job>>;

    Assert.NotNull(result);

    Assert.Equal(200, result.StatusCode);

    Assert.NotNull(result.Value);

    Assert.IsAssignableFrom<IEnumerable<Job>>(result.Value);

    Assert.True(result.Value.Count() == 100);

  }


  async Task<(T? Value, HttpStatusCode StatusCode)> GetResponse<T>(IResult result)
    where T : class
  {
    var tempContext = new DefaultHttpContext
    {
      // RequestServices needs to be set so the IResult implementation can log.
      RequestServices = new ServiceCollection().AddLogging().BuildServiceProvider(),
      Response =
        {
            // The default response body is Stream.Null which throws away anything that is written to it.
            Body = new MemoryStream(),
        },
    };

    await result.ExecuteAsync(tempContext);

    var status = tempContext.Response.StatusCode;

    tempContext.Response.Body.Position = 0;
    var rdr = new StreamReader(tempContext.Response.Body);
    var body = rdr.ReadToEnd();

    T? data = null;

    if (body is not null && body.Length > 0)
    {
      var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
      data = JsonSerializer.Deserialize<T>(body, jsonOptions);
    }

    return (data, (HttpStatusCode)status);
  }

}