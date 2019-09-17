using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests.TestInfrastructure
{
    public class BaseControllerTest<T> : TestContainer where T : Controller
    {
        public T GetController()
        {
            return Get<T>();
        }
    }
}
