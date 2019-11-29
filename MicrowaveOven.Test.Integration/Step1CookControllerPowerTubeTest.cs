using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.Callbacks;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;
using NUnit.Framework;


namespace MicrowaveOven.Test.Integration
{
    [TestFixture]
    public class Step1CookControllerPowerTubeTest
    {
        private CookController _cookController;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IDisplay _display;
        private IOutput _output;
        private IUserInterface _ui;



        [SetUp]
        public void SetUp()
        {
            // Laver en substitute instans for følgende fake modulerne
            _timer = Substitute.For<ITimer>();
            _display = Substitute.For<IDisplay>();
            _output = Substitute.For<IOutput>();
            _powerTube = new PowerTube(_output);
            _ui = Substitute.For<IUserInterface>();


            _cookController = new CookController(_timer, _display, _powerTube);
            _cookController.UI = _ui;

        }

        // Test 1 - StartCooking metode 50 Watts
        [Test]
        public void Start_StartCooking_50WPower()
        {
            _cookController.StartCooking(50,60);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("50")));
        }
        // Test 2 - StartCooking metode testcases exception lavet ud fra en Boundary Value Analysis med grænseværderne 50 og 700 
        [TestCase(49, 60)]
        [TestCase(701, 60)]
        public void Start_StartCooking_RangeExceptions(int power, int timer)
        {
            Assert.Throws<System.ArgumentOutOfRangeException>(() => _cookController.StartCooking(power, timer));
        }

        // Test 3 - Stop Power på StartCooking metode

        [Test]
        public void Stop_StartCooking_OffPower()
        {
            _cookController.StartCooking(50, 60);
            _cookController.Stop();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("Off")));
        }
    }
}
