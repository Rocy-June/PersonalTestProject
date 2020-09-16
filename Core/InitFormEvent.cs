using System;
using System.Collections.Generic;
using System.Linq;
using SqlHandler;
using SqlHandler.Model;

namespace EventHandler
{
    public class InitFormEvent
    {
        public void InitSQL()
        {
            SQLite.Init(Config.DATABASE_FILE_PATH);
            SQLite.CheckTables();
        }
    }
}
