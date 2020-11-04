using System;
using System.Collections.Generic;
using System.Text;

namespace ClassExamples
{
    public class Service: IService
    {
        public int elem;
        public IRepository repository;
        public Service(IRepository repository)
        {
            elem = 6;
            this.repository = repository;
        }
    }
}
