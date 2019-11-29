using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace MicrowaveOven.Test.Integration
{
    [TestFixture]
    public class Step2CookControllerTest
    {
        private CookController _cookController;
        private ITimer _timer; 
        private IPowerTube _powerTube;
        private IDisplay _display;
        private IOutput _output;
        private IUserInterface _ui;
        

        [SetUp]
        public void SetUp()
        {
            // Laver en substitute instans for følgende fake modulerne
            _display = Substitute.For<IDisplay>();
            _output = Substitute.For<IOutput>();
            _ui = Substitute.For<IUserInterface>();

            _powerTube = new PowerTube(_output);
            _timer = new Timer();

            _cookController = new CookController(_timer, _display, _powerTube);
            _cookController.UI = _ui;
        }

        // Test 1 - Fra Timer til CookController
        [Test]
        public void Start_StartCooking_Power()
        {
            int Watts = 60;
            _cookController.StartCooking(Watts, 60);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains(Watts.ToString())));
        }

        [Test]
        public void Start_StartCooking_StopOff()
        {
            _cookController.StartCooking(60, 60);
            Thread.Sleep(2000);
            _cookController.Stop();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void Start_StartCooking_ExpireOff()
        {
            int time = 1;
            _cookController.StartCooking(60, time);
            Thread.Sleep(1500);
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        // Test 2 - Fra CookController til Timer
        [TestCase(610, 610)]
        [TestCase(0, 0)]
        [TestCase(100, 100)]
        public void Start_StartCooking_RemainingTime(int time, int remainingTime)
        {
            _cookController.StartCooking(50, time);
            Assert.That(_timer.TimeRemaining, Is.EqualTo(remainingTime));
        }




    }
}
