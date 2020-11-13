using System;
using System.Collections.Generic;
using System.Text;

namespace ClassExamples
{
    public class Foo<TRepository> : IFoo<TRepository> where TRepository: IRepository
    {
        TRepository repository;
        public Foo(TRepository repository)
        {
            this.repository = repository;
        }
        public TRepository GetRepository()
        {
            return repository;
        }
    }
}
