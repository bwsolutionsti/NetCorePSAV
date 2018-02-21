using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models.PSAVCrud
{
    public class Comvta
    {
      public class Comvtatabla
        {
            public int tc_cvid { get; set; }
            public string tc_cvtext { get; set; }
            public string tc_cvfee { get; set; }
        }
    }
}
