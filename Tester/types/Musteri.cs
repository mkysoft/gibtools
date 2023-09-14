using com.mkysoft.gib.signer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mkysoft.gib.tester.types
{
    public class Musteri
    {
        public string VKN { get; set; }
        public string Unvan { get; set; }
        public List<Etiket> Etiketler { get; set; }
        public string ID { get; set; }
        public signer.enums.Device TokenDevice { get; set; }
        public string TokenName { get; set; }
        public string TokenSerial { get; set; }
        public string TokenPin { get; set; }
        public int FaturaSay { get; set; }
        public int ZarfSay { get; set; }
    }

    public class Etiket
    {
        public string GB { get; set; }
        public string PK { get; set; }
    }
}
