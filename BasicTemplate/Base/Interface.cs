using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicTemplate.Base
{
    interface IPage
    {
        PackIconKind PageIcon { get; }
        string PageName { get; }
        short PageNum { get; }
    }

    interface IExample
    {
        string ExampleName { get; }
        short ExampleNum { get; }
    }

    interface ISlot
    { }
}
