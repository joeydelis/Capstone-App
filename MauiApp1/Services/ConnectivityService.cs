using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Services
{
    class ConnectivityService : IConnectivityService
    {
        public bool IsConnectedToInternet()
        {
            return Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
