using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7ADProjectApi.ViewModels
{
    public class SetStationeryRetrievalApiModel
    {
        public string UserId { get; set; }
        public List<StationeryRetrievalApiModel> ApiModelList { get; set; }
    }
}