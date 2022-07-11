using DummyCommercialApp.AccountingStrategies;
using DummyCommercialApp.ApportionmentStrategies;
using System;
using System.Threading.Tasks;

namespace DummyCommercialApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            /*
             * This program does not perform any real action. It just illustrates how the SOLID principles are applied to solve an intricate business functionality of a real commercial application (TAS - Terminal Automation System)
             * There are no commercial secrets here. Anyone who knows how logistic terminals operate can draw a similar generic design.
             */
            OperationsAccounter operationsAccounter = new OperationsAccounter(new ApportionmentStrategyFactory(), new AccountingStrategyFactory());
            var orderID = Guid.NewGuid();
            await operationsAccounter.AccountOperationAsync(orderID);
        }
    }
}
