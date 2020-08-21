using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NUnit.Framework.Constraints;
using RestSharp;
using NUnitTestProject.Models;

namespace NUnitTestProject.Utilities
{
  public class ApiRequest<T> where T : new()
  {
    private readonly RestClient _client;
    private readonly RestRequest _request;
    private IRestResponse<T> _response;

    public ApiRequest(string baseUrl)
    {

      _client = new RestClient(baseUrl);
      _request = new RestRequest();

    }

    public void SetEndpoint(string url)
    {
      _request.Resource = url;
    }

    public void SetMethod(Method method)
    {
      _request.Method = method;
    }

    public void Execute()
    {
      _response = _client.Execute<T>(_request);
    }

    public T GetResponse()
    {
      return _response.Data;
    }

    public bool IsSuccessful()
    {
      return _response.IsSuccessful;
    }

    public void SetJsonBody(object body)
    {
      _request.AddJsonBody(body);
    }

    public void SetUrlSegment(string id, string value) {
      _request.AddUrlSegment(id, value);
    }

    public HttpStatusCode GetStatusCode()
    {
      return _response.StatusCode;
    }

    public void SetHeader(string name, string value) { 
      _request.AddHeader(name, value);
    }

    public void SetQueryParameter(string name, string value) {
      _request.AddQueryParameter(name, value);
    }

  }

}
