using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Service
{
    public sealed class GeneralMgmtService
    {
        #region Singleton Design Pattern

        private static readonly GeneralMgmtService instance = new GeneralMgmtService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static GeneralMgmtService()
        {
        }

        private GeneralMgmtService()
        {
        }

        public static GeneralMgmtService Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion


    }
}