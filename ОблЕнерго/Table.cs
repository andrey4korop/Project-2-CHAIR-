using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ОблЕнерго
{
    class Table
    {

        public Table(string number, string name, string timeend, string money)
        {
            this.Enable = false;
            this.Number = number;
            this.Name = name;
            this.TimeEnd = timeend;
            this.Money = money;

            this.Minimal = "0";
            this.myPrice = 0;

        }
        public bool Enable { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string TimeEnd { get; set; }
        public string Money { get; set; }

        public string Minimal { get; set; }
        public int myPrice { get; set; }



    }
}
