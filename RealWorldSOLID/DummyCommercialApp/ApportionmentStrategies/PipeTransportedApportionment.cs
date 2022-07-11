using DummyCommercialApp.Data;
using DummyCommercialApp.Models;
using System;
using System.Collections.Generic;

namespace DummyCommercialApp.ApportionmentStrategies
{
    internal class PipeTransportedApportionment : IApportionmentStrategy
    {
        public List<Apportionment> GetApportionments(OperationsDBContext dBContext, Guid orderID)
        {
            throw new NotImplementedException();
        }
    }
}
