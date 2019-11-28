using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace MicrowaveOven.Test.Integration
{
    [TestFixture]
    public class Step1CookControllerPowerTubeTest
    {
        private ICookController _cookController;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IDisplay _display;
        private IOutput _output;
        private IUserInterface _userInterface;


        public void SetUp()
        {
            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);
            _powerTube = new PowerTube(_output);
            
            // Laver en substitute instans for følgende fake modulerne
            _timer = Substitute.For<ITimer>();
            _display = Substitute.For<IDisplay>();
            _output = Substitute.For<IOutput>();
        }

        [Test]
        public void Stop_StartCooking_OffPower()
        {
            _cookController.StartCooking(50, 60);
            _cookController.Stop();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("Off")));
        }
    }
}
