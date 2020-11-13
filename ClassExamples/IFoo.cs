using System;
using System.Collections.Generic;
using System.Text;

namespace ClassExamples
{
    public interface IFoo<TRepository> where TRepository : IRepository
    {
        TRepository GetRepository();
    }
}
