using DummyCommercialApp.Data;
using DummyCommercialApp.Models;
using System;
using System.Collections.Generic;

namespace DummyCommercialApp.ApportionmentStrategies
{
    internal class RoadTransportedApportionment : IApportionmentStrategy
    {
        public List<Apportionment> GetApportionments(OperationsDBContext dBContext, Guid orderID)
        {
            throw new NotImplementedException();
        }
    }
}
