using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePSAV.Models.PSAVCrud
{
    public class MonedaModel
    {
        public class MonedaModelTabla
        {
            public int tc_id { get; set; }
            public string tc_name { get; set; }
            public double tc_change { get; set; }
            public int tc_userin { get; set; }
            public string tc_userupd { get; set; }
            public string tc_dateupd { get; set; }
            public int activo { get; set; }
        }
    }
}
