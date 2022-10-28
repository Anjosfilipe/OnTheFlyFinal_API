using System.Collections.Generic;

namespace ClassLibrary
{ 
    public class Sales
    {
        public Flight Flight { get; set; }
        public List<Passenger> Passagers { get; set; }
        public bool Reserved { get; set; }
        public bool Sold { get; set; }  
    }
}
