using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace XUnitTests.Utils
{
    public class TestCase<T>
    {
        public T Payload { get; set; }
        public bool ExpectedSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
