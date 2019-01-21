using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Service
{
    public sealed class InventoryMgmtService
    {
        #region Singleton Design Pattern

        private static readonly InventoryMgmtService instance = new InventoryMgmtService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static InventoryMgmtService()
        {
        }

        private InventoryMgmtService()
        {
        }

        public static InventoryMgmtService Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
    }
}