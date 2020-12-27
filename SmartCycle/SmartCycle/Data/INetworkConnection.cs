using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCycle.Data
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckInternetConnection();
    }
}
