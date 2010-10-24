using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Nemira.Tests
{
    [TestFixture]
    public class AllTests
    {
        [Test]
        public void VerifyTestingInfrastructure()
        {
            Assert.AreEqual("foo", "foo");
        }
    }
}
