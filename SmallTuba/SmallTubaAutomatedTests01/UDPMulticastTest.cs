// <copyright file="UDPMulticastTest.cs">Copyright ©  2011</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallTuba.Network.UDP;

namespace SmallTuba.Network.UDP
{
    [TestClass]
    [PexClass(typeof(UDPMulticast))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class UDPMulticastTest
    {
        [PexMethod]
        public object Receive([PexAssumeUnderTest]UDPMulticast target, long timeOut)
        {
            object result = target.Receive(timeOut);
            return result;
            // TODO: add assertions to method UDPMulticastTest.Receive(UDPMulticast, Int64)
        }
    }
}
