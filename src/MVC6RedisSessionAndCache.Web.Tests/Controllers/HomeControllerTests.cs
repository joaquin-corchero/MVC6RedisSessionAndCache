using Microsoft.AspNet.Mvc;
using MVC6RedisSessionAndCache.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MVC6RedisSessionAndCache.Web.Tests.Controllers
{
    public class when_working_with_the_home_controller
    {
        protected HomeController _controller;
        protected IActionResult _actionResult;

        public when_working_with_the_home_controller()
        {
            _controller = new HomeController();
        }

        /*[Theory]
        [InlineData(21, 1)]
        [InlineData(23, 3)]
        [InlineData(25, 5)]
        public void CheckingSomething(int value, int result)
        {
            Assert.Equal((int)Calculate(value), result);
        }

        private int Calculate(int value)
        {
            return value - 20;
        }*/
    }

    public class and_executing_the_index_action : when_working_with_the_home_controller
    {
        private void Execute()
        {
            base._actionResult = base._controller.Index();
        }

        [Fact]
        public void then_the_view_is_returned()
        {
            this.Execute();

            Assert.Equal(base._actionResult.GetType(), typeof(ViewResult));
        }

        [Fact]
        public void then_the_view_data_message_is_set()
        {
            var expected = "This is the index page.";
            this.Execute();

            var actual = _controller.ViewData["Message"];
            Assert.Equal(expected, actual);
        }
    }
}
