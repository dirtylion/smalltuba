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

        private List<ClientLog> log; 

        public Model()
        {
            this.log = new List<ClientLog>();
        }

        public string Name { get; set; }

        public List<ClientLog> Log
        {
            get { return this.log; }
        }
    }
}
