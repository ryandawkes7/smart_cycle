﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCycle.Data
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
