using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Service
{
    public sealed class ProcurementService
    {
        #region Singleton Design Pattern

        private static readonly ProcurementService instance = new ProcurementService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ProcurementService()
        {
        }

        private ProcurementService()
        {
        }

        public static ProcurementService Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
    }
}