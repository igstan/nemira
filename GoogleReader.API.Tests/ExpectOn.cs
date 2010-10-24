using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Mocks;

namespace GoogleReader.API.Tests
{
    class ExpectOn<T>
    {
        private string expectedMethodCall;
        private List<object> expectedArguments = new List<object>();
        private DynamicMock mock;

        public ExpectOn()
        {
            mock = new DynamicMock(typeof(T));
        }

        public ExpectOn<T> MethodCall(string methodName)
        {
            expectedMethodCall = methodName;
            return this;
        }

        public ExpectOn<T> WithArgument(object arg)
        {
            expectedArguments.Add(arg);
            return this;
        }

        public T ToReturn(object returnValue)
        {
            mock.ExpectAndReturn(expectedMethodCall, returnValue, expectedArguments.ToArray());
            return (T) mock.MockInstance;
        }
    }
}
