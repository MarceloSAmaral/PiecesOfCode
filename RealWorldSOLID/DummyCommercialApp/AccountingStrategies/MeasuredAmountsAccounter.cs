using DummyCommercialApp.Models;
using System;
using System.Collections.Generic;

namespace DummyCommercialApp.AccountingStrategies
{
    internal class MeasuredAmountsAccounter : IAccountingStrategy
    {
        public List<AccountEntry> CreateAccountingOperations(DateTime serverDatetime, Guid userID, MovementTypes operationMovementType, List<PlannedAmmounts> plannedAmmounts, List<ProductMovementsOperations> operations, List<Apportionment> apportionments)
        {
            throw new NotImplementedException();
        }
    }
}
