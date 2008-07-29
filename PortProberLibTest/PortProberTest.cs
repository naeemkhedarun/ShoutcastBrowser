using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PortProberLib;

namespace PortProberLibTest
{
    /// <summary>
    /// Summary description for PortProberTest
    /// </summary>
    [TestClass]
    public class PortProberTest
    {
        [TestMethod]
        public void PortProber_Initialise_IsNotNull()
        {
            PortProber prober = new PortProber(new IPAddress(new byte[] {21, 12, 43, 54}), 23);
            Assert.IsNotNull(prober, "Failed to initialise.");
        }

        [TestMethod, Timeout(6000)]
        public void ProbePort_WithInvalidAddress_ReturnsFalse()
        {
            PortProber prober = new PortProber(new IPAddress(new byte[]{23,43,54,23}), 32);
            Assert.IsFalse(prober.ProbeMachine(), "Should have failed.");
        }

        [TestMethod]
        public void ProbePort_WithValidAddressAndValidPort_ReturnsTrue()
        {
            PortProber prober = new PortProber(new IPAddress(new byte[]{160,79,128,242}), 8004);
            Assert.IsTrue(prober.ProbeMachine(), "Should have succeeded.");
        }

        [TestMethod]
        public void ProbePort_WithValidExternalAddressAndValidPort_ReturnsTrue()
        {
            PortProber prober = new PortProber(new IPAddress(new byte[] { 216, 239, 59, 104 }), 80);
            Assert.IsTrue(prober.ProbeMachine(), "Should have succeeded.");
        }
    }
}