using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CsvDataProcessor.Infra
{
    public class DispatcherUtils
    {
        private static Dispatcher _instance;
        public static Dispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = App.Current.Dispatcher;
                }
                return _instance;
            }
        }
    }
}
