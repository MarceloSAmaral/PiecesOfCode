using DummyCommercialApp.Data;
using DummyCommercialApp.Models;
using System;
using System.Collections.Generic;

namespace DummyCommercialApp.ApportionmentStrategies
{
    public interface IApportionmentStrategy
    {
        List<Apportionment> GetApportionments(OperationsDBContext dBContext, Guid orderID);
    }
}
