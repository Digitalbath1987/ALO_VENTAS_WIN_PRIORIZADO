using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ALO.Entidades
{
    [Serializable()]
    public class EServiceRestFulException : System.Exception
    {

        public EServiceRestFulException() : base() { }
        public EServiceRestFulException(string message) : base(message) { }
        public EServiceRestFulException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected EServiceRestFulException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }


    }
}
