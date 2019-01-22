using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.Service
{
    public sealed class DisbursementService
    {
        #region Singleton Design Pattern

        private static readonly DisbursementService instance = new DisbursementService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static DisbursementService()
        {
        }

        private DisbursementService()
        {
        }

        public static DisbursementService Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        LogicDB context = new LogicDB();



    }
}