using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Services;

namespace UnitTestProject
{
    internal class MockConnectivityService : IConnectivityService
    {
        public bool Connected { get; set; } = true;

        public bool IsConnectedToInternet() => Connected;
    }
}
