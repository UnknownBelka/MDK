using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TelephoneSpravochnik
{
    static class Views
    {
        public static CollectionViewSource AbonentsView { get; set; }
        public static CollectionViewSource DistrictsView { get; set; }
        public static CollectionViewSource Lgotnaya_categoryView { get; set; }
        public static CollectionViewSource Phone_categoryView { get; set; }
    }
}
