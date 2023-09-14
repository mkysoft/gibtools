using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mkysoft.gib.tester.types
{
    public class Zarf
    {
        public string GB { get; set; }
        public string GonderenVKN_TCKN { get; set; }
        public string GonderenUnvan { get; set; }
        public string PK { get; set; }
        public string AliciVKN_TCKN { get; set; }
        public string AliciUnvan { get; set; }
        public string ZarfEttn { get; set; }
        public string ZarfTarih { get; set; }
        public Enums.EnvType EnvType { get; set; }
        public Enums.ElmntType ElmntType { get; set; }
        public string ZarfKodu { get; set; }
        public string ZarfMesaji { get; set; }

        public byte[] Data { get; set; }

        internal Zarf Clone()
        {
            return new Zarf()
            {
                GB = this.GB,
                GonderenVKN_TCKN = this.GonderenVKN_TCKN,
                GonderenUnvan = this.GonderenUnvan,
                PK = this.PK,
                AliciVKN_TCKN = this.AliciVKN_TCKN,
                AliciUnvan = this.AliciUnvan,
                ZarfEttn = this.ZarfEttn,
                ZarfTarih = this.ZarfTarih,
                EnvType = this.EnvType,
                ElmntType = this.ElmntType,
                ZarfKodu = this.ZarfKodu
            };
        }
    }
}
