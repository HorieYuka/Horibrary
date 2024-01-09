using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTemplate.Base
{
    internal class Helper
    {
        public const int MaxPlotBuffLength = 100000;

        public static EventHandler EvtBelowStatus;

        public static void UpdateBelowStatus(bool? _BelowCircle = null, string? _BelowText = null, int? _Belowbar = null)
        {
            BelowStatusModel Md = new BelowStatusModel();

            Md.BelowCircle = _BelowCircle;
            Md.BelowText = _BelowText;
            Md.BelowBar = _Belowbar;

            if (EvtBelowStatus != null)
                EvtBelowStatus.Invoke(Md, new EventArgs());
        }

    }
}
