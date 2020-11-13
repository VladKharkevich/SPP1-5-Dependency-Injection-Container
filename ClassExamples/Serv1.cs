using System;
using System.Collections.Generic;
using System.Text;

namespace ClassExamples
{
    
    public class Serv1 : AbstractServ
    {
        public int elem;
        public Serv1()
        {
            elem = 5;
        }

        public override int GetInt()
        {
            return 1;
        }
    }
}
