﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ClassExamples
{
    public class Repository : IRepository
    {
        int x;
        public Repository()
        {
            x = 5;
        }

        public int dzen()
        {
            return 34;
        }
    }
}
