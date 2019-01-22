using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Team7ADProject.Entities;

namespace Team7ADProject.Service
{
    public class CommonService
    {
        #region Singleton Design Pattern

        private static readonly CommonService instance = new CommonService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static CommonService()
        {
        }

        private CommonService()
        {
        }

        public static CommonService Instance
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