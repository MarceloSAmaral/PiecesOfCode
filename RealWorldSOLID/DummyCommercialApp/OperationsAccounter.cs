using DummyCommercialApp.AccountingStrategies;
using DummyCommercialApp.ApportionmentStrategies;
using DummyCommercialApp.Data;
using DummyCommercialApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DummyCommercialApp
{
    public class OperationsAccounter
    {
        private IApportionmentStrategyFactory _ApportionmentStrategyFactory { get; }
        private IAccountingStrategyFactory _AccountingStrategyFactory { get; }

        public OperationsAccounter(IApportionmentStrategyFactory apportionmentStrategyFactory, IAccountingStrategyFactory accountingStrategyFactory)
        {
            _ApportionmentStrategyFactory = apportionmentStrategyFactory;
            _AccountingStrategyFactory = accountingStrategyFactory;
        }

        public async Task AccountOperationAsync(Guid orderID)
        {
            using (SqlConnection sqlConnection = CreateConnection())
            using (SqlTransaction transaction = CreateTransaction(sqlConnection))
            using (OperationsDBContext dbContext = OperationsDBContext.Factory(transaction))
            {
                try
                {
                    var operationsAccounterServiceUserID = GetServiceUserID();
                    var serverDatetime = GetServerDatetime();
                    var order = await LoadOrderAsync(orderID, transaction);

                    var plannedAmmounts = await GetPlannedAmmountsAsync(dbContext, orderID);
                    var executedOperations = await GetOperations(dbContext, orderID);

                    IApportionmentStrategy apportionmentStrategy = _ApportionmentStrategyFactory.GetStrategy(order.TransportationModal, order.Movement);
                    var apportionments = apportionmentStrategy.GetApportionments(dbContext, orderID);

                    IAccountingStrategy accountingStrategy = _AccountingStrategyFactory.GetStrategy(order.TransportationModal, order.Movement);
                    var accountEntries = accountingStrategy.CreateAccountingOperations(serverDatetime, operationsAccounterServiceUserID, order.Movement, plannedAmmounts, executedOperations, apportionments);

                    foreach (var accountEntry in accountEntries)
                    {
                        if(accountEntry.EntryType == AccountEntryType.Credit)
                        {
                            await CreditOperationAsync(accountEntry, transaction);
                        }
                        else
                        {
                            await DebitAsync(accountEntry, transaction);
                        }
                    }

                    await dbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private Task DebitAsync(AccountEntry accountEntry, SqlTransaction transaction)
        {
            throw new NotImplementedException();
        }

        private Task CreditOperationAsync(AccountEntry accountEntry, SqlTransaction transaction)
        {
            throw new NotImplementedException();
        }

        private Task<List<ProductMovementsOperations>> GetOperations(OperationsDBContext dbContext, Guid orderID)
        {
            throw new NotImplementedException();
        }

        private Task<List<PlannedAmmounts>> GetPlannedAmmountsAsync(OperationsDBContext dbContext, Guid orderID)
        {
            throw new NotImplementedException();
        }

        private Task<Order> LoadOrderAsync(Guid operationID, SqlTransaction transaction)
        {
            throw new NotImplementedException();
        }

        private DateTime GetServerDatetime()
        {
            throw new NotImplementedException();
        }

        private Guid GetServiceUserID()
        {
            throw new NotImplementedException();
        }

        private SqlTransaction CreateTransaction(SqlConnection sqlConnection)
        {
            throw new NotImplementedException();
        }

        private SqlConnection CreateConnection()
        {
            throw new NotImplementedException();
        }
    }
}
