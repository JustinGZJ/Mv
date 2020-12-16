using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mv.Modules.P99.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mv.Modules.P99.Service.Tests
{
    [TestClass()]
    public class SerializeTests
    {
        [TestMethod()]
        public void ToJsonTest()
        {
            var json = new UploadData().ToJson();
            Assert.IsTrue(json.Contains(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }
    }
}