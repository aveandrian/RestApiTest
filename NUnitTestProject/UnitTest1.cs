using NUnit.Framework;
using NUnitTestProject.Utilities;
using NUnitTestProject.Models;
using NUnitTestProject;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace Tests
{
  public class Tests : TestBase
  {
   
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestGet_WhenSendTestResults_ShouldReturnOk()
    {
      
      var _request = new ApiRequest<List<TestResult>>(BaseUrl.AbsoluteUri);
      _request.SetEndpoint("testresults");
      _request.SetMethod(Method.GET);
      _request.Execute();

      Assert.AreEqual(true, _request.IsSuccessful());
      Assert.AreNotEqual(0, _request.GetResponse().Count);
    }

    [Test]
    public void TestPost_WhenSendTestResult_ShouldCreateNewResult()
    {
      var _request = new ApiRequest<TestResult>(BaseUrl.AbsoluteUri);
      _request.SetEndpoint("testresults");

      var newTestResult = new TestResultPostRequestDto
      {
        Name = "Test name"
      };
      
      _request.SetMethod(Method.POST);
      _request.SetJsonBody(newTestResult);
      _request.Execute();

      var request = new ApiRequest<List<TestResult>>(BaseUrl.AbsoluteUri);
      request.SetEndpoint("testresults");
      request.SetMethod(Method.GET);
      request.Execute();

      Assert.That(request.GetResponse().Any(x => x.Id == _request.GetResponse().Id));
      Assert.That(request.GetResponse().Any(x => x.Name == _request.GetResponse().Name));
    }

    [Test]
    public void TestPut_WhenSendTestResults_ShouldRefreshWholeResult()
    {
      var _request = new ApiRequest<TestResult>(BaseUrl.AbsoluteUri);
      _request.SetEndpoint("testresults");
      var newTestResult = new TestResultPutRequestDto
      {
        Name = "Test name",
        RandomField = "Random test field"
       };

      _request.SetMethod(Method.POST);
      _request.SetJsonBody(newTestResult);
      _request.Execute();

      Assert.AreEqual(true, _request.IsSuccessful());

      var request = new ApiRequest<TestResultPutRequestDto>(BaseUrl.AbsoluteUri);
      request.SetEndpoint("testresults/{id}");
      request.SetUrlSegment("id", _request.GetResponse().Id.ToString());
      request.SetMethod(Method.PUT);
      request.SetJsonBody(new TestResult { Name = "Edited name" });
      request.Execute();

      Assert.That(request.GetResponse().Name, Is.EqualTo("Edited name"));
      Assert.That(request.GetResponse().RandomField, Is.Null);
    }

    [Test]
    public void TestPatch_WhenSendTestResults_ShouldRefreshOnlyExatField() {
      var _request = new ApiRequest<TestResult>(BaseUrl.AbsoluteUri);
      _request.SetEndpoint("testresults");
      var newTestResult = new TestResultPutRequestDto
      {
        Name = "Test name",
        RandomField = "Random test field"
      };

      _request.SetMethod(Method.POST);
      _request.SetJsonBody(newTestResult);
      _request.Execute();

      Assert.AreEqual(true, _request.IsSuccessful());

      var request = new ApiRequest<TestResultPutRequestDto>(BaseUrl.AbsoluteUri);
      request.SetEndpoint("testresults/{id}");
      request.SetUrlSegment("id", _request.GetResponse().Id.ToString());
      request.SetMethod(Method.PATCH);
      request.SetJsonBody(new TestResult
      {
        Name = "Patched name"
      });

      request.Execute();
      Assert.That(request.GetResponse().Name, Is.EqualTo("Patched name"));
      Assert.That(request.GetResponse().RandomField, Is.EqualTo("Random test field"));
    }

    [Test]
    public void TestDelete_WhenSendTestResults_ShouldDeleteExisting() {
      var _request = new ApiRequest<TestResult>(BaseUrl.AbsoluteUri);
      _request.SetEndpoint("testresults");
      var newTestResult = new TestResultPostRequestDto
      {
        Name = "Test name"
      };
      _request.SetMethod(Method.POST);
      _request.SetJsonBody(newTestResult);
      _request.Execute();

      Assert.AreEqual(true, _request.IsSuccessful());

      var request = new ApiRequest<TestResult>(BaseUrl.AbsoluteUri);
      request.SetEndpoint("testresults/{id}");

      request.SetUrlSegment("id", _request.GetResponse().Id.ToString());
      request.SetMethod(Method.DELETE);
      request.Execute();

      Assert.AreEqual(true, request.IsSuccessful());

      request.SetMethod(Method.GET);
      request.Execute();

      Assert.That(request.IsSuccessful, Is.False);
      Assert.That(request.GetStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public void TestGet_WhenSendTestresultsWithHeader_ShouldReturnOk()
    {
      var _request = new ApiRequest<List<TestResult>>(BaseUrl.AbsoluteUri);
      _request.SetEndpoint("testresults");
      _request.SetMethod(Method.GET);
      _request.SetHeader("Authorization", "Basic Some string");
      _request.Execute();
      Assert.AreEqual(true, _request.IsSuccessful());
    }

    [Test]
    public void TestGet_WhenSendTestresultsWithQueryParam_ShouldReturnFilteredList()
    {
      var _request = new ApiRequest<List<TestResult>>(BaseUrl.AbsoluteUri);
      _request.SetEndpoint("testresults");
      _request.SetMethod(Method.GET);
      _request.SetQueryParameter("id", 0.ToString());
      _request.Execute();
      Assert.AreEqual(true, _request.IsSuccessful());
      Assert.That(_request.GetResponse().Count, Is.EqualTo(0));
    }
  }
}