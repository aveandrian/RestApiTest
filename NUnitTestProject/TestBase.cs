using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework; 
namespace NUnitTestProject
{
  [TestFixture]
  public class TestBase
  {
    protected Settings Setting;
    protected Uri BaseUrl;
    [OneTimeSetUp]
    public void SetSettings()
    {
      var json = File.ReadAllText("config.json");
      Setting = JsonConvert.DeserializeObject<Settings>(json);
      BaseUrl = new Uri($"{Setting.Protocol}://{Setting.Url}:{Setting.Port}");
    }
  }
}
