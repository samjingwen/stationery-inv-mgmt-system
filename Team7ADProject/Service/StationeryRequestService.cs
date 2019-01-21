using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProject.Service
{
    public sealed class StationeryRequestService
    {
        #region Singleton Design Pattern

        private static readonly StationeryRequestService instance = new StationeryRequestService();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static StationeryRequestService()
        {
        }

        private StationeryRequestService()
        {
        }

        public static StationeryRequestService Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion


    }
}