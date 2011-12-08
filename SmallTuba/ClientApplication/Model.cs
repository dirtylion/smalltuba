using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmallTuba.Entities;

namespace ClientApplication
{
    class Model
    {
        private string name;

        private List<LogState> log; 

        public Model()
        {
            this.log = new List<LogState>();
        }

        public string Name { get; set; }

        public List<LogState> Log
        {
            get { return this.log; }
        }
    }
}
